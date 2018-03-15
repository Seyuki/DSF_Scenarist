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
using System.Windows.Input;

namespace DSF_Scenarist.Classes.StoryItems
{
    /// <summary>
    /// Classe représentant un cursus (story path)
    /// </summary>
    public class StoryPath : StoryItemTemplate
    {
        #region Propriétés

        /// <summary>
        /// Icone pour le TreeView
        /// </summary>
        private PackIconKind _icon = PackIconKind.SourceBranch;

        /// <summary>
        /// Vue à afficher
        /// </summary>
        private UserControl _view = new StoryPathView();

        /// <summary>
        /// Nom du cursus / path
        /// </summary>
        private string _name = string.Empty;

        /// <summary>
        /// Collection de paliers / levels
        /// </summary>
        private ObservableCollection<StoryLevel> _levels = new ObservableCollection<StoryLevel>();

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
        /// Nom du cursus / path
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// Collection de paliers / levels
        /// </summary>
        public ObservableCollection<StoryLevel> Levels
        {
            get => _levels;
            set
            {
                _levels = value;
                OnPropertyChanged("Levels");
                OnPropertyChanged("Children");
            }
        }

        /// <summary>
        /// Enfants pour le TreeView
        /// </summary>
        public ObservableCollection<StoryLevel> Children { get => _levels; }

        #endregion Accesseurs

        #region Constructeurs

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        /// <param name="parent">Parent de l'élément</param>
        public StoryPath(StoryItemTemplate parent)
        {
            this.Parent = parent;
            this.View.DataContext = this;

            this.Levels.CollectionChanged += new NotifyCollectionChangedEventHandler((sender, e) => StoryCollectionChanged(sender, e, "Levels"));
        }

        /// <summary>
        /// Constructeur avec nom
        /// </summary>
        /// <param name="parent">Parent de l'élément</param> 
        /// <param name="name">Nom du cursus / path</param>
        public StoryPath(StoryItemTemplate parent, string name)
        {
            this.Parent = parent;
            this.Name = name;

            this.Levels.CollectionChanged += new NotifyCollectionChangedEventHandler((sender, e) => StoryCollectionChanged(sender, e, "Levels"));
        }

        /// <summary>
        /// Constructeur avec nom et levels
        /// </summary>
        /// <param name="parent">Parent de l'élément</param> 
        /// <param name="name">Nom du cursus / path</param>
        /// <param name="levels">Collection de paliers / levels</param>
        public StoryPath(StoryItemTemplate parent, string name, ObservableCollection<StoryLevel> levels)
        {
            this.Parent = parent;
            this.Name = name;
            this.Levels = levels;
            this.View.DataContext = this;

            this.Levels.CollectionChanged += new NotifyCollectionChangedEventHandler((sender, e) => StoryCollectionChanged(sender, e, "Levels"));
        }

        #endregion Constructeurs

        #region Methodes

        /// <summary>
        /// Ajoute un nouveau level au path
        /// </summary>
        public override void AddChild(object param = null)
        {
            // Création d'un item
            StoryLevel newLevel = new StoryLevel(this);
            this.Levels.Add(newLevel);

            // Incrément de l'ID
            int maxID = 0;
            foreach (StoryLevel level in this.Levels)
            {
                if (level.ID > maxID)
                    maxID = level.ID;
            }
            newLevel.ID = maxID + 1;

            // On sélectionne le nouvel item et le flag comme modifié
            newLevel.IsSelected = true;
            newLevel.IsChanged = true;
        }

        /// <summary>
        /// Supprime l'item en paramètre des enfants
        /// </summary>
        /// <param name="child">item à supprimer</param>
        public override void DeleteChild(StoryItemTemplate child)
        {
            if (!this.DeleteConfirmation())
                return;

            for (int i = 0; i < this.Levels.Count; ++i)
            {
                if (this.Levels[i] == child)
                    this.Levels.RemoveAt(i);
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
            JSON += Environment.NewLine + indent + "\"Path\": \"" + this.Name + "\"";
            JSON += ",";
            JSON += Environment.NewLine + indent + "\"Levels\":";
            JSON += Environment.NewLine + indent + "[";
            JSON += Environment.NewLine;

            // Tableau des actions
            for (int i = 0, max = this.Levels.Count - 1; i <= max; ++i)
            {
                // Transformation en JSON
                string JSONtmp = this.Levels[i].ToJSON(lvl + 2);

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

            foreach (StoryLevel level in this.Levels)
            {
                level.SetNotChanged();
            }
        }

        #endregion Methodes
    }
}
