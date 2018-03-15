using DSF_Scenarist.Classes.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSF_Scenarist.Classes.StoryItems
{
    /// <summary>
    /// Classe représentant toutes les conséquences potentielles d'une action
    /// </summary>
    public class StoryActionConsequences : StoryItemTemplate
    {
        #region Propriétés

        /// <summary>
        /// Collection de potentiels succès
        /// </summary>
        private ObservableCollection<StoryActionConsequence> _successes = new ObservableCollection<StoryActionConsequence>();

        /// <summary>
        /// Collection de potentiels échecs
        /// </summary>
        private ObservableCollection<StoryActionConsequence> _failures = new ObservableCollection<StoryActionConsequence>();

        #endregion Propriétés

        #region Accesseurs

        /// <summary>
        /// Collection de potentiels succès
        /// </summary>
        public ObservableCollection<StoryActionConsequence> Successes
        {
            get => _successes;
            set
            {
                _successes = value;
                OnPropertyChanged("Successes");
            }
        }

        /// <summary>
        /// Collection de potentiels échecs
        /// </summary>
        public ObservableCollection<StoryActionConsequence> Failures
        {
            get => _failures;
            set
            {
                _failures = value;
                OnPropertyChanged("Failures");
            }
        }

        #endregion Accesseurs

        #region Constructeurs

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        /// <param name="parent">Parent de l'élément</param>
        public StoryActionConsequences(StoryItemTemplate parent)
        {
            this.Parent = parent;

            this.Successes.CollectionChanged += new NotifyCollectionChangedEventHandler((sender, e) => StoryCollectionChanged(sender, e, "Successes"));
            this.Failures.CollectionChanged += new NotifyCollectionChangedEventHandler((sender, e) => StoryCollectionChanged(sender, e, "Failures"));
        }

        /// <summary>
        /// Constructeur avec un succès et un échec
        /// </summary>
        /// <param name="parent">Parent de l'élément</param>
        /// <param name="success">Succès</param>
        /// <param name="failure">Echec</param>
        public StoryActionConsequences(StoryItemTemplate parent, StoryActionConsequence success, StoryActionConsequence failure)
        {
            this.Parent = parent;
            this.Successes.Add(success);
            this.Failures.Add(failure);

            this.Successes.CollectionChanged += new NotifyCollectionChangedEventHandler((sender, e) => StoryCollectionChanged(sender, e, "Successes"));
            this.Failures.CollectionChanged += new NotifyCollectionChangedEventHandler((sender, e) => StoryCollectionChanged(sender, e, "Failures"));
        }

        /// <summary>
        /// Constructeur avec collections de potentiels conséquences
        /// </summary>
        /// <param name="parent">Parent de l'élément</param>
        /// <param name="successes">Collection de potentiels succès</param>
        /// <param name="failures">Collection de potentiels échecs</param>
        public StoryActionConsequences(StoryItemTemplate parent, ObservableCollection<StoryActionConsequence> successes, ObservableCollection<StoryActionConsequence> failures)
        {
            this.Parent = parent;
            this.Successes = successes;
            this.Failures = failures;

            this.Successes.CollectionChanged += new NotifyCollectionChangedEventHandler((sender, e) => StoryCollectionChanged(sender, e, "Successes"));
            this.Failures.CollectionChanged += new NotifyCollectionChangedEventHandler((sender, e) => StoryCollectionChanged(sender, e, "Failures"));
        }

        #endregion Constructeurs

        #region Methodes

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
            JSON += Environment.NewLine + indent + "\"Success\":";
            JSON += Environment.NewLine + indent + "[";
            JSON += Environment.NewLine;

            // Tableau des succès potentiels
            for (int i = 0, max = this.Successes.Count - 1; i <= max; ++i)
            {
                // Transformation en JSON
                string JSONtmp = this.Successes[i].ToJSON(lvl + 2);

                // Si ce n'est pas le premier élement
                if (i > 0) JSONtmp = Environment.NewLine + JSONtmp;
                // Si ce n'est pas le dernier élément
                if (i < max) JSONtmp += ",";

                JSON += JSONtmp;
            }

            JSON += Environment.NewLine + indent + "],";
            JSON += Environment.NewLine + indent + "\"Failure\":";
            JSON += Environment.NewLine + indent + "[";
            JSON += Environment.NewLine;

            // Tableau des échecs potentiels
            for (int i = 0, max = this.Failures.Count - 1; i <= max; ++i)
            {
                // Transformation en JSON
                string JSONtmp = this.Failures[i].ToJSON(lvl + 2);

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

            foreach (StoryActionConsequence sucess in this.Successes)
            {
                sucess.SetNotChanged();
            }
            foreach (StoryActionConsequence failure in this.Failures)
            {
                failure.SetNotChanged();
            }
        }

        #endregion Methodes
    }
}
