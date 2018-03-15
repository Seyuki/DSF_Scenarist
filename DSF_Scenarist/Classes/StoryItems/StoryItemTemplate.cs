using DSF_Scenarist.Classes.Tools;
using DSF_Scenarist.Windows;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DSF_Scenarist.Classes.StoryItems
{
    /// <summary>
    /// Template d'éléments de scénario
    /// </summary>
    public class StoryItemTemplate : INotifyPropertyChanged
    {
        #region Propriétés

        /// <summary>
        /// Flag indiquant si l'élément est ouvert dans le TreeView
        /// </summary>
        private bool _isExpanded = false;

        /// <summary>
        /// Flag indiquant si l'élément est sélectionné dans le TreeView
        /// </summary>
        private bool _isSelected = false;

        /// <summary>
        /// Flag indiquant si l'élement a été modifié
        /// </summary>
        private bool _isChanged = false;

        /// <summary>
        /// Parent de l'élément
        /// </summary>
        private StoryItemTemplate _parent = null;

        /// <summary>
        /// Commande 'Ouvrir'
        /// </summary>
        private ICommand _openCmd;

        /// <summary>
        /// Commande 'Supprimer'
        /// </summary>
        private ICommand _deleteCmd;

        /// <summary>
        /// Commande d'ajout d'un enfant
        /// </summary>
        private ICommand _addChildCmd;

        #endregion Propriétés

        #region Evenements

        /// <summary>
        /// Evenement changement de valeur d'une propriété
        /// </summary>
        /// <param name="propertyName">Nom de la propriété</param>
        protected void OnPropertyChanged(string propertyName)
        {
            var evt = this.PropertyChanged;
            if (evt != null)
                evt(this, new PropertyChangedEventArgs(propertyName));

            if (propertyName != "IsChanged" && propertyName != "IsSelected" && propertyName != "IsExpanded")
                this.IsChanged = true;
        }

        /// <summary>
        /// Handler de l'evenement changement de valeur d'une propriété
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Evenements

        #region Accesseurs

        /// <summary>
        /// Flag indiquant si l'élément est ouvert dans le TreeView
        /// </summary>
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                _isExpanded = value;
                OnPropertyChanged("IsExpanded");

                // On ouvre le parent
                if (Parent != null) Parent.IsExpanded = true;
            }
        }

        /// <summary>
        /// Flag indiquant si l'élément est sélectionné
        /// </summary>
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged("IsSelected");

                // On ouvre le parent
                if (Parent != null) Parent.IsExpanded = true;
            }
        }

        /// <summary>
        /// Flag indiquant si l'élement a été modifié
        /// </summary>
        public bool IsChanged
        {
            get => _isChanged;
            set
            {
                _isChanged = value;
                OnPropertyChanged("IsChanged");

                // On flag aussi le parent
                if (value && Parent != null) Parent.IsChanged = true;
            }
        }

        /// <summary>
        /// Parent de l'élément
        /// </summary>
        public StoryItemTemplate Parent { get => _parent; set { _parent = value; } }

        /// <summary>
        /// Commande 'Ouvrir'
        /// </summary>
        public ICommand OpenCmd { get => _openCmd; }

        /// <summary>
        /// Commande 'Supprimer'
        /// </summary>
        public ICommand DeleteCmd { get => _deleteCmd; }

        /// <summary>
        /// Commande d'ajout d'un enfant
        /// </summary>
        public ICommand AddChildCmd { get => _addChildCmd; }

        /// <summary>
        /// Prévisualisation au format JSON
        /// </summary>
        public string PreviewJSON { get => this.ToJSON(); }

        /// <summary>
        /// ContextMenu de l'objet
        /// </summary>
        public ContextMenu ItemMenu { get => (ContextMenu)Application.Current.MainWindow.Resources[this.GetType().Name + "Menu"]; }

        #endregion Accesseurs

        #region Constructeur

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        /// <param name="parent">Parent de l'élément</param>
        public StoryItemTemplate()
        {
            // Commandes
            this._openCmd = new CommandHandler(param => OpenItem(), true);
            this._deleteCmd = new CommandHandler(param => DeleteItem(), true);
            this._addChildCmd = new CommandHandler(param => AddChild(param), true);
        }

        #endregion Constructeur

        #region Methodes

        /// <summary>
        /// Evenement changement de la collection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void StoryCollectionChanged(object sender, NotifyCollectionChangedEventArgs e, string CollectionName)
        {
            this.IsChanged = true;
            OnPropertyChanged(CollectionName);
        }

        /// <summary>
        /// Ouvre (sélectionne) l'item
        /// </summary>
        public virtual void OpenItem()
        {
            this.IsSelected = true;
        }

        /// <summary>
        /// Supprime l'item
        /// </summary>
        public void DeleteItem()
        {
            this.Parent.DeleteChild(this);
        }

        /// <summary>
        /// Transforme l'objet en JSON
        /// </summary>
        /// <param name="lvl">Niveau d'indentation</param>
        /// <returns>Objet au format JSON</returns>
        public virtual string ToJSON(int lvl = 0)
        {
            return string.Empty;
        }

        /// <summary>
        /// Ajout d'un item
        /// </summary>
        public virtual void AddChild(object param = null) { }

        /// <summary>
        /// Supprime l'item en paramètre des enfants
        /// </summary>
        /// <param name="child">item à supprimer</param>
        public virtual void DeleteChild(StoryItemTemplate child) { }

        /// <summary>
        /// Demande confirmation à l'utilisateur
        /// </summary>
        /// <returns>vrai si l'élément doit être supprimé, sinon false</returns>
        public bool DeleteConfirmation()
        {
            bool? answer = null;

            // Fenêtre de demande
            ConfirmationWindow askWindow = new ConfirmationWindow()
            {
                Message = { Text = "L'élément va être supprimé." }
            };
            askWindow.Owner = Application.Current.MainWindow;

            if (askWindow.ShowDialog() == false)
                answer = askWindow.Rep;

            return answer ?? false;
        }

        #endregion Methodes
    }
}
