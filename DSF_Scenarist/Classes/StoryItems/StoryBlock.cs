using DSF_Scenarist.Classes.Tools;
using DSF_Scenarist.Views;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DSF_Scenarist.Classes.StoryItems
{
    /// <summary>
    /// Classe représentant un bloc
    /// </summary>
    public class StoryBlock : StoryItemTemplate
    {
        #region Propriétés

        /// <summary>
        /// Icone pour le TreeView
        /// </summary>
        private PackIconKind _icon = PackIconKind.CheckboxBlank;

        /// <summary>
        /// Vue à afficher
        /// </summary>
        private UserControl _view = new StoryBlockView();

        /// <summary>
        /// ID du bloc
        /// </summary>
        private int _ID = 0;

        /// <summary>
        /// Phrase de description
        /// </summary>
        private string _description = string.Empty;

        /// <summary>
        /// Type de bloc (Standard / Special)
        /// </summary>
        private string _type = string.Empty;

        /// <summary>
        /// Collection d'actions
        /// </summary>
        private ObservableCollection<StoryAction> _actions = new ObservableCollection<StoryAction>();

        #endregion Propriétés

        #region Accesseurs

        /// <summary>
        /// Icone pour le TreeView
        /// </summary>
        public PackIconKind Icon { get => _icon; }

        /// <summary>
        /// Vue à afficher
        /// </summary>
        public UserControl View { get => _view; }

        /// <summary>
        /// ID du bloc
        /// </summary>
        public int ID
        {
            get => _ID;
            set
            {
                _ID = value;
                OnPropertyChanged("ID");
            }
        }

        /// <summary>
        /// Nom du cursus / path
        /// </summary>
        public string Name
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged("Description");
            }
        }

        /// <summary>
        /// Phrase de description
        /// </summary>
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged("Description");
            }
        }

        /// <summary>
        /// Type de bloc (Standard / Special)
        /// </summary>
        public string Type
        {
            get => _type;
            set
            {
                _type = value;
                OnPropertyChanged("Type");
            }
        }

        /// <summary>
        /// Collection de blocs / levels
        /// </summary>
        public ObservableCollection<StoryAction> Actions
        {
            get => _actions;
            set
            {
                _actions = value;
                OnPropertyChanged("Actions");
                OnPropertyChanged("Children");
            }
        }

        /// <summary>
        /// Enfants pour le TreeView
        /// </summary>
        public ObservableCollection<StoryAction> Children { get => _actions; }

        #endregion Accesseurs

        #region Constructeurs

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        /// <param name="parent">Parent de l'élément</param>
        public StoryBlock(StoryItemTemplate parent)
        {
            this.Parent = parent;
            this.View.DataContext = this;

            this.Actions.CollectionChanged += new NotifyCollectionChangedEventHandler((sender, e) => StoryCollectionChanged(sender, e, "Actions"));
        }

        /// <summary>
        /// Constructeur avec informations
        /// </summary>
        /// <param name="parent">Parent de l'élément</param>
        /// <param name="id">ID du bloc</param>
        /// <param name="description">Phrase de description</param>
        /// <param name="type">Type de bloc (Standard / Special)</param>
        public StoryBlock(StoryItemTemplate parent, int id, string description, string type)
        {
            this.Parent = parent;
            this.ID = id;
            this.Description = description;
            this.Type = type;
            this.View.DataContext = this;

            this.Actions.CollectionChanged += new NotifyCollectionChangedEventHandler((sender, e) => StoryCollectionChanged(sender, e, "Actions"));
        }

        /// <summary>
        /// Constructeur avec informations et actions
        /// </summary>
        /// <param name="parent">Parent de l'élément</param>
        /// <param name="id">ID du bloc</param>
        /// <param name="description">Phrase de description</param>
        /// <param name="type">Type de bloc (Standard / Special)</param>
        /// <param name="actions">Collection d'actions</param>
        public StoryBlock(StoryItemTemplate parent, int id, string description, string type, ObservableCollection<StoryAction> actions)
        {
            this.Parent = parent;
            this.ID = id;
            this.Description = description;
            this.Type = type;
            this.Actions = actions;
            this.View.DataContext = this;

            this.Actions.CollectionChanged += new NotifyCollectionChangedEventHandler((sender, e) => StoryCollectionChanged(sender, e, "Actions"));
        }

        #endregion Constructeurs

        #region Methodes

        /// <summary>
        /// Ajoute une nouvelle action au block
        /// </summary>
        public override void AddChild(object param = null)
        {
            // Création d'un item
            StoryAction newAction = new StoryAction(this);
            this.Actions.Add(newAction);

            // On sélectionne le nouvel item et le flag comme modifié
            newAction.IsSelected = true;
            newAction.IsChanged = true;
        }

        /// <summary>
        /// Supprime l'item en paramètre des enfants
        /// </summary>
        /// <param name="child">item à supprimer</param>
        public override void DeleteChild(StoryItemTemplate child)
        {
            if (!this.DeleteConfirmation())
                return;

            for (int i = 0; i < this.Actions.Count; ++i)
            {
                if (this.Actions[i] == child)
                    this.Actions.RemoveAt(i);
            }
        }

        /// <summary>
        /// Transforme l'objet en JSON
        /// </summary>
        /// <param name="lvl">Niveau d'indentation</param>
        /// <returns>Objet au format JSON</returns>
        public override string ToJSON(int lvl = 0)
        {
            // Strings d'indentation
            string indentWrap = new String('\t', lvl);
            string indent = new String('\t', lvl + 1);

            // Transfromation en JSON
            string JSON;

            JSON = indentWrap + "{";
            JSON += Environment.NewLine + indent + "\"ID\": " + this.ID.ToString();
            JSON += ",";
            JSON += Environment.NewLine + indent + "\"Description\": \"" + this.Description + "\"";
            JSON += ",";
            JSON += Environment.NewLine + indent + "\"Type\": \"" + this.Type + "\"";
            JSON += ",";
            JSON += Environment.NewLine + indent + "\"Actions\":";
            JSON += Environment.NewLine + indent + "[";
            JSON += Environment.NewLine;

            // Tableau des actions
            for (int i = 0, max = this.Actions.Count - 1; i <= max; ++i)
            {
                // Transformation en JSON
                string JSONtmp = this.Actions[i].ToJSON(lvl + 2);

                // Si ce n'est pas le premier élement
                if (i > 0) JSONtmp = Environment.NewLine + JSONtmp;
                // Si ce n'est pas le dernier élément
                if (i < max) JSONtmp += ",";

                JSON += JSONtmp;
            }

            JSON += Environment.NewLine + indent + "]";
            JSON += Environment.NewLine + indentWrap + "}";

            return JSON;
        }

        /// <summary>
        /// Enleve le flag de modification sur l'objet et ses enfants
        /// </summary>
        public void SetNotChanged()
        {
            this.IsChanged = false;

            foreach (StoryAction action in this.Actions)
            {
                action.SetNotChanged();
            }
        }

        #endregion Methodes
    }
}
