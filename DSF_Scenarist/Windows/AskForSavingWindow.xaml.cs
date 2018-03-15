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
using System.Windows.Shapes;

namespace DSF_Scenarist.Windows
{
    /// <summary>
    /// Interaction logic for AskForSavingWindow.xaml
    /// </summary>
    public partial class AskForSavingWindow : MetroWindow
    {
        /// <summary>
        /// Constructeur de la fenêtre
        /// </summary>
        public AskForSavingWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Réponse à retourner
        /// </summary>
        private bool? _rep = null;

        /// <summary>
        /// Réponse à retourner
        /// </summary>
        public bool? Rep { get => _rep; }

        /// <summary>
        /// Click sur le bouton 'Oui'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendTrue(object sender, RoutedEventArgs e)
        {
            this._rep = true;
            this.Close();
        }

        /// <summary>
        /// Click sur le bouton 'Non'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendFalse(object sender, RoutedEventArgs e)
        {
            this._rep = false;
            this.Close();
        }

        /// <summary>
        /// Click sur le bouton 'Oui'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendNull(object sender, RoutedEventArgs e)
        {
            this._rep = null;
            this.Close();
        }
    }
}
