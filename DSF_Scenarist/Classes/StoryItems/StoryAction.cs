using DSF_Scenarist.Classes.Tools;
using DSF_Scenarist.Views;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using static DSF_Scenarist.Classes.StoryItems.StoryActionConsequence;

namespace DSF_Scenarist.Classes.StoryItems
{
    /// <summary>
    /// Classe représentant une action
    /// </summary>
    public class StoryAction : StoryItemTemplate
    {
        #region Propriétés

        /// <summary>
        /// Icone pour le TreeView
        /// </summary>
        private PackIconKind _icon = PackIconKind.Label;

        /// <summary>
        /// Vue à afficher
        /// </summary>
        private UserControl _view = new StoryActionView();

        /// <summary>
        /// Nom de l'action (Texte du bouton)
        /// </summary>
        private string _name = string.Empty;

        /// <summary>
        /// Type d'action (Standard, Special)
        /// </summary>
        private string _type = string.Empty;

        /// <summary>
        /// Conditions (Visibilité et réussite)
        /// </summary>
        private StoryActionConditions _conditions = new StoryActionConditions(null);

        /// <summary>
        /// Conséquences de l'action
        /// </summary>
        private StoryActionConsequences _consequences = new StoryActionConsequences(null);

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
        /// Nom de l'action (Texte du bouton)
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
        /// Type d'action (Standard, Special)
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
        /// Conditions (Visibilité et réussite)
        /// </summary>
        public StoryActionConditions Conditions
        {
            get => _conditions;
            set
            {
                _conditions = value;
                OnPropertyChanged("Conditions");
            }
        }

        /// <summary>
        /// Conséquences de l'action
        /// </summary>
        public StoryActionConsequences Consequences
        {
            get => _consequences;
            set
            {
                _consequences = value;
                OnPropertyChanged("Consequences");
            }
        }

        /// <summary>
        /// Enfants pour le TreeView
        /// Pour éviter une erreur de Binding, car les actions n'ont pas d'enfants
        /// </summary>
        public ObservableCollection<StoryAction> Children { get => null; }

        #endregion Accesseurs

        #region Constructeurs

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        /// <param name="parent">Parent de l'élément</param>
        public StoryAction(StoryItemTemplate parent)
        {
            this.Parent = parent;
            this.Conditions.Parent = this;
            this.Consequences.Parent = this;
            this.View.DataContext = this;
        }

        /// <summary>
        /// Constructeur avec informations
        /// </summary>
        /// <param name="parent">Parent de l'élément</param>
        /// <param name="name">Nom de l'action (Texte du bouton)</param>
        /// <param name="type">Type d'action (Standard, Special)</param>
        public StoryAction(StoryItemTemplate parent, string name, string type)
        {
            this.Parent = parent;
            this.Conditions.Parent = this;
            this.Consequences.Parent = this;
            this.Name = name;
            this.Type = type;
            this.View.DataContext = this;
        }

        /// <summary>
        /// Constructeur avec informations, conditions et conséquences
        /// </summary>
        /// <param name="parent">Parent de l'élément</param>
        /// <param name="name">Nom de l'action (Texte du bouton)</param>
        /// <param name="type">Type d'action (Standard, Special)</param>
        /// <param name="conditions">Conditions (Visibilité et réussite)</param>
        /// <param name="consequences">Conséquences de l'action</param>
        public StoryAction(StoryItemTemplate parent, string name, string type, StoryActionConditions conditions, StoryActionConsequences consequences)
        {
            this.Parent = parent;
            this.Conditions.Parent = this;
            this.Consequences.Parent = this;
            this.Name = name;
            this.Type = type;
            this.Conditions = conditions;
            this.Consequences = consequences;
            this.View.DataContext = this;
        }

        #endregion Constructeurs

        #region Methodes

        /// <summary>
        /// Ajoute une nouvelle conséquence
        /// </summary>
        public override void AddChild(object param = null)
        {
            // Récupération de la collection à populer et de l'icone en fonction du type de conséquence
            ObservableCollection<StoryActionConsequence> consequences;
            eConsequenceType type;

            if (param?.ToString().ToLower() == "success")
            {
                consequences = this.Consequences.Successes;
                type = eConsequenceType.Success;
            }
            else if (param?.ToString().ToLower() == "failure")
            {
                consequences = this.Consequences.Failures;
                type = eConsequenceType.Failure;
            }
            else
                return;

            // Création d'un item
            StoryActionConsequence newConsequence = new StoryActionConsequence(this, type);
            consequences.Add(newConsequence);

            // On flag le nouvel item comme modifié
            newConsequence.IsChanged = true;

            // On ouvre le nouvel item
            newConsequence.OpenItem();
        }

        /// <summary>
        /// Supprime l'item en paramètre des enfants
        /// </summary>
        /// <param name="child">item à supprimer</param>
        public override void DeleteChild(StoryItemTemplate child)
        {
            if (!this.DeleteConfirmation())
                return;

            // Vérification dans les succès
            for (int i = 0; i < this.Consequences.Successes.Count; ++i)
            {
                if (this.Consequences.Successes[i] == child)
                {
                    this.Consequences.Successes.RemoveAt(i);
                    return;
                }
            }

            // Vérification dans les échecs
            for (int i = 0; i < this.Consequences.Failures.Count; ++i)
            {
                if (this.Consequences.Failures[i] == child)
                {
                    this.Consequences.Failures.RemoveAt(i);
                    return;
                }
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
            JSON += Environment.NewLine + indent + "\"Name\": \"" + this.Name + "\"";
            JSON += ",";
            JSON += Environment.NewLine + indent + "\"Type\": \"" + this.Type + "\"";
            JSON += ",";
            JSON += Environment.NewLine + indent + "\"Conditions\":";
            JSON += Environment.NewLine + this.Conditions.ToJSON(lvl + 1);
            JSON += ",";
            JSON += Environment.NewLine + indent + "\"Consequences\":";
            JSON += Environment.NewLine + this.Consequences.ToJSON(lvl + 1);
            JSON += Environment.NewLine + indentWrap + "}";

            return JSON;
        }

        /// <summary>
        /// Enleve le flag de modification sur l'objet et ses enfants
        /// </summary>
        public void SetNotChanged()
        {
            this.IsChanged = false;
            this.Conditions.SetNotChanged();
            this.Consequences.SetNotChanged();
        }

        #endregion Methodes
    }
}
