using DSF_Scenarist.Classes.StoryItems;
using DSF_Scenarist.Classes.Tools;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DSF_Scenarist.Classes.ViewModels
{
    /// <summary>
    /// ViewModel de la fenêtre principale
    /// </summary>
    public class MainViewModel : NotifyClassTemplate
    {
        #region Propriétés

        /// <summary>
        /// Scenario chargé
        /// </summary>
        private Scenario _scenario;

        /// <summary>
        /// Commande sur évènement d'ouverture de l'application
        /// </summary>
        private ICommand _appLoadedCmd;

        /// <summary>
        /// Commande 'Nouveau'
        /// </summary>
        private ICommand _newCmd;

        /// <summary>
        /// Commande 'Ouvrir'
        /// </summary>
        private ICommand _openCmd;

        /// <summary>
        /// Commande 'Enregistrer'
        /// </summary>
        private ICommand _saveCmd;

        /// <summary>
        /// Commande 'Enregistrer sous...'
        /// </summary>
        private ICommand _saveAsCmd;

        /// <summary>
        /// Commande de suppression pour le TreeView
        /// </summary>
        private ICommand _deleteSelectedCmd;

        #endregion Propriétés

        #region Accesseurs

        /// <summary>
        /// Scenario chargé
        /// </summary>
        public Scenario Scenario
        {
            get => _scenario;
            set
            {
                _scenario = value;
                OnPropertyChanged("Scenario");
                OnPropertyChanged("IsScenario");
            }
        }

        /// <summary>
        /// Flag indiquant si un scénario est ouvert
        /// </summary>
        public bool IsScenario
        {
            get => _scenario != null;
        }

        /// <summary>
        /// Commande sur évènement d'ouverture de l'application
        /// </summary>
        public ICommand AppLoadedCmd { get => _appLoadedCmd; }

        /// <summary>
        /// Commande 'Nouveau'
        /// </summary>
        public ICommand NewCmd { get => _newCmd; }

        /// <summary>
        /// Commande 'Ouvrir'
        /// </summary>
        public ICommand OpenCmd { get => _openCmd; }

        /// <summary>
        /// Commande 'Enregistrer'
        /// </summary>
        public ICommand SaveCmd { get => _saveCmd; }

        /// <summary>
        /// Commande 'Enregistrer sous...'
        /// </summary>
        public ICommand SaveAsCmd { get => _saveAsCmd; }

        /// <summary>
        /// Commande de suppression pour le TreeView
        /// </summary>
        public ICommand DeleteSelectedCmd { get => _deleteSelectedCmd; }

        #endregion Accesseurs

        #region Constructeurs

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public MainViewModel()
        {
            // Commandes
            this._appLoadedCmd = new CommandHandler(param => LoadApp(), true);

            this._newCmd = new CommandHandler(param => NewScenario(), true);
            this._openCmd = new CommandHandler(param => OpenScenario(), true);
            this._saveCmd = new CommandHandler(param => SaveScenario(), true);
            this._saveAsCmd = new CommandHandler(param => SaveAsScenario(), true);

            this._deleteSelectedCmd = new CommandHandler(param => DeleteSelectedItem(param), true);
        }

        #endregion Constructeurs

        #region Methodes

        /// <summary>
        /// Ouverture de l'application
        /// </summary>
        private void LoadApp()
        {
            // Vérification de l'existence d'arguments
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
                this.Scenario = new Scenario(args[1]);
        }

        /// <summary>
        /// Crée un nouveau scénario
        /// </summary>
        private void NewScenario()
        {
            // On vérifie si le scénario actuel a été modifié
            if (this.Scenario != null && this.Scenario?.AskForSaving() == null)
                return;

            this.Scenario = new Scenario();
        }

        /// <summary>
        /// Ouvre un scénario
        /// </summary>
        private void OpenScenario()
        {
            // Sélection du fichier
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Scénario DSF (*.json)| *.json";

            // Si rien n'a été sélectionné, on ne fait rien
            if (openFileDialog.ShowDialog() != true)
                return;

            // Suite de l'ouverture
            this.OpenScenario(openFileDialog.FileName);
        }

        /// <summary>
        /// Ouvre un scénario dans le chemin est renseigné en paramètre
        /// </summary>
        /// <param name="path">Chemin du scénario à ouvrir</param>
        public void OpenScenario(string path)
        {
            // On vérifie si le scénario actuel a été modifié
            if (this.Scenario != null && this.Scenario?.AskForSaving() == null)
                return;

            // Ouverture du scénario
            this.Scenario = new Scenario(path);
        }

        /// <summary>
        /// Enregistre le scénario
        /// </summary>
        private void SaveScenario()
        {
            this.Scenario?.Save();
            OnPropertyChanged("Scenario");
        }

        /// <summary>
        /// Enregistre sous le scénario
        /// </summary>
        private void SaveAsScenario()
        {
            this.Scenario?.SaveAs();
            OnPropertyChanged("Scenario");
        }

        /// <summary>
        /// Supprime l'élément sélectionné du TreeView
        /// </summary>
        private void DeleteSelectedItem(Object item)
        {
            // On vérifie le type de l'objet
            if (!(item is StoryItemTemplate))
                return;

            // Suppression de l'élement
            (item as StoryItemTemplate).DeleteItem();
        }

        #endregion Methodes
    }
}
