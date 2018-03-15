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
    /// Classe représentant un palier (story level)
    /// </summary>
    public class StoryLevel : StoryItemTemplate
    {
        #region Propriétés

        /// <summary>
        /// Icone pour le TreeView
        /// </summary>
        private PackIconKind _icon = PackIconKind.Animation;

        /// <summary>
        /// Vue à afficher
        /// </summary>
        private UserControl _view = new StoryLevelView();

        /// <summary>
        /// ID du palier / level
        /// </summary>
        private int _ID = 0;

        /// <summary>
        /// Nom du palier / level
        /// </summary>
        private string _name = string.Empty;

        /// <summary>
        /// Phrase de transition
        /// </summary>
        private string _transition = string.Empty;

        /// <summary>
        /// Collection de blocs
        /// </summary>
        private ObservableCollection<StoryBlock> _blocks = new ObservableCollection<StoryBlock>();

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
        /// ID du palier / level
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
        /// Nom du palier / level
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
        /// Phrase de transition
        /// </summary>
        public string Transition
        {
            get => _transition;
            set
            {
                _transition = value;
                OnPropertyChanged("Transition");
            }
        }

        /// <summary>
        /// Collection de blocs
        /// </summary>
        public ObservableCollection<StoryBlock> Blocks
        {
            get => _blocks;
            set
            {
                _blocks = value;
                OnPropertyChanged("Blocks");
                OnPropertyChanged("Children");
            }
        }

        /// <summary>
        /// Enfants pour le TreeView
        /// </summary>
        public ObservableCollection<StoryBlock> Children { get => _blocks; }

        #endregion Accesseurs

        #region Constructeurs

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        /// <param name="parent">Parent de l'élément</param>
        public StoryLevel(StoryItemTemplate parent)
        {
            this.Parent = parent;
            this.View.DataContext = this;

            this.Blocks.CollectionChanged += new NotifyCollectionChangedEventHandler((sender, e) => StoryCollectionChanged(sender, e, "Blocks"));
        }

        /// <summary>
        /// Constructeur avec informations
        /// </summary>
        /// <param name="parent">Parent de l'élément</param>
        /// <param name="id">ID du bloc</param>
        /// <param name="name">Nom du bloc / level</param>
        /// <param name="transition">Phrase de transition</param>
        public StoryLevel(StoryItemTemplate parent, int id, string name, string transition)
        {
            this.Parent = parent;
            this.ID = id;
            this.Name = name;
            this.Transition = transition;
            this.View.DataContext = this;

            this.Blocks.CollectionChanged += new NotifyCollectionChangedEventHandler((sender, e) => StoryCollectionChanged(sender, e, "Blocks"));
        }

        /// <summary>
        /// Constructeur avec informations et blocs
        /// </summary>
        /// <param name="parent">Parent de l'élément</param>
        /// <param name="id">ID du bloc</param>
        /// <param name="name">Nom du bloc / level</param>
        /// <param name="transition">Phrase de transition</param>
        /// <param name="blocks">Collection de blocs</param>
        public StoryLevel(StoryItemTemplate parent, int id, string name, string transition, ObservableCollection<StoryBlock> blocks)
        {
            this.Parent = parent;
            this.ID = id;
            this.Name = name;
            this.Transition = transition;
            this.Blocks = blocks;
            this.View.DataContext = this;

            this.Blocks.CollectionChanged += new NotifyCollectionChangedEventHandler((sender, e) => StoryCollectionChanged(sender, e, "Blocks"));
        }

        #endregion Constructeurs

        #region Methodes

        /// <summary>
        /// Ajoute un nouveau block au level
        /// </summary>
        public override void AddChild(object param = null)
        {
            // Création d'un item
            StoryBlock newBlock = new StoryBlock(this);
            this.Blocks.Add(newBlock);

            // Incrément de l'ID
            int maxID = 0;
            foreach (StoryBlock block in this.Blocks)
            {
                if (block.ID > maxID)
                    maxID = block.ID;
            }
            newBlock.ID = maxID + 1;

            // On sélectionne le nouvel item et le flag comme modifié
            newBlock.IsSelected = true;
            newBlock.IsChanged = true;
        }

        /// <summary>
        /// Supprime l'item en paramètre des enfants
        /// </summary>
        /// <param name="child">item à supprimer</param>
        public override void DeleteChild(StoryItemTemplate child)
        {
            if (!this.DeleteConfirmation())
                return;

            for (int i = 0; i < this.Blocks.Count; ++i)
            {
                if (this.Blocks[i] == child)
                    this.Blocks.RemoveAt(i);
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
            JSON += Environment.NewLine + indent + "\"Name\": \"" + this.Name + "\"";
            JSON += ",";
            JSON += Environment.NewLine + indent + "\"Transition\": \"" + this.Transition + "\"";
            JSON += ",";
            JSON += Environment.NewLine + indent + "\"Blocks\":";
            JSON += Environment.NewLine + indent + "[";
            JSON += Environment.NewLine;

            // Tableau des actions
            for (int i = 0, max = this.Blocks.Count - 1; i <= max; ++i)
            {
                // Transformation en JSON
                string JSONtmp = this.Blocks[i].ToJSON(lvl + 2);

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

            foreach (StoryBlock block in this.Blocks)
            {
                block.SetNotChanged();
            }
        }

        #endregion Methodes
    }
}
