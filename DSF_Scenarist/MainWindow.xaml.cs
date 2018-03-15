using DSF_Scenarist.Classes.ViewModels;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DSF_Scenarist
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        /// <summary>
        /// Constructeur de la fenêtre
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Evenement Closing de la fenêtre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainViewModel vm = (MainViewModel)this.DataContext;

            // Si pas de scénario ouvert, ou qu'il n'a pas été modifié
            if (vm.Scenario == null || !vm.Scenario.IsChanged)
            {
                return;
            }

            bool? isSaved = vm.Scenario.AskForSaving();
            if (isSaved == null)
            {
                e.Cancel = true;
            }
        }

        #region Gestion Drag & Drop

        /// <summary>
        /// Entrée de l'élément dans la fenêtre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppDragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop) ||
                sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        /// <summary>
        /// Drop de l'élément dans la fenêtre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                (this.DataContext as MainViewModel).OpenScenario(files[0]);
            }
        }

        #endregion Gestion Drag & Drop
    }
}
