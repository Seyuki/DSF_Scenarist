using DSF_Scenarist.Classes.Tools;
using DSF_Scenarist.Views.SubViews;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DSF_Scenarist.Classes.StoryItems
{
    /// <summary>
    /// Classe représentant la conséquence d'une action
    /// </summary>
    public class StoryActionConsequence : StoryItemTemplate
    {
        public enum eConsequenceType
        {
            Success,
            Failure
        }

        #region Propriétés

        /// <summary>
        /// Icone pour le TreeView
        /// </summary>
        private PackIconKind _icon;

        /// <summary>
        /// Message à afficher
        /// </summary>
        private string _message = string.Empty;

        /// <summary>
        /// Chaine contenant les évolutions de jauges
        /// A parser !
        /// </summary>
        private string _gauges = string.Empty;

        /// <summary>
        /// Chaine contenant les évolutions de compétences
        /// A parser !
        /// </summary>
        private string _skills = string.Empty;

        /// <summary>
        /// Chaine contenant les effets
        /// A parser !
        /// </summary>
        private string _effects = string.Empty;

        #endregion Propriétés

        #region Accesseurs

        /// <summary>
        /// Message à afficher
        /// </summary>
        public PackIconKind Icon
        {
            get => _icon;
            set
            {
                _icon = value;
                OnPropertyChanged("Icon");
            }
        }

        /// <summary>
        /// Icone pour le TreeView
        /// </summary>
        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged("Message");
            }
        }

        /// <summary>
        /// Chaine contenant les évolutions de jauges
        /// A parser !
        /// </summary>
        public string Gauges
        {
            get => _gauges;
            set
            {
                _gauges = value;
                OnPropertyChanged("Gauges");
            }
        }

        /// <summary>
        /// Chaine contenant les évolutions de compétences
        /// A parser !
        /// </summary>
        public string Skills
        {
            get => _skills;
            set
            {
                _skills = value;
                OnPropertyChanged("Skills");
            }
        }

        /// <summary>
        /// Chaine contenant les effets
        /// A parser !
        /// </summary>
        public string Effects
        {
            get => _effects;
            set
            {
                _effects = value;
                OnPropertyChanged("Effects");
            }
        }

        #endregion Accesseurs

        #region Constructeurs

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        /// <param name="parent">Parent de l'élément</param>
        /// <param name="type">Type de conséquence</param>
        public StoryActionConsequence(StoryItemTemplate parent, eConsequenceType type)
        {
            this.Parent = parent;
            this.Icon = type == eConsequenceType.Success ? PackIconKind.CheckboxMarked : PackIconKind.CloseBox;
        }

        /// <summary>
        /// Constructeur avec informations
        /// </summary>
        /// <param name="parent">Parent de l'élément</param>
        /// <param name="type">Type de conséquence</param>
        /// <param name="message">Message à afficher</param>
        /// <param name="gauges">Chaine contenant les évolutions de jauges</param>
        /// <param name="skills">Chaine contenant les évolutions de compétences</param>
        /// <param name="effects">Chaine contenant les effets</param>
        public StoryActionConsequence(StoryItemTemplate parent, eConsequenceType type, string message, string gauges, string skills, string effects)
        {
            this.Parent = parent;
            this.Icon = type == eConsequenceType.Success ? PackIconKind.CheckboxMarked : PackIconKind.CloseBox;
            this.Message = message;
            this.Gauges = gauges;
            this.Skills = skills;
            this.Effects = effects;
        }

        #endregion Constructeurs

        #region Methodes

        /// <summary>
        /// Ouvre l'objet
        /// </summary>
        public override async void OpenItem()
        {
            StoryActionConsequenceView consequenceView = new StoryActionConsequenceView();
            consequenceView.DataContext = this;
            await DialogHost.Show(consequenceView, "RootDialog");
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
            JSON += Environment.NewLine + indent + "\"Message\": \"" + this.Message + "\"";
            JSON += ",";
            JSON += Environment.NewLine + indent + "\"Gauges\": \"" + this.Gauges + "\"";
            JSON += ",";
            JSON += Environment.NewLine + indent + "\"Skills\": \"" + this.Skills + "\"";
            JSON += ",";
            JSON += Environment.NewLine + indent + "\"Effects\": \"" + this.Effects + "\"";
            JSON += Environment.NewLine + indentWrap + "}";

            return JSON;
        }

        /// <summary>
        /// Enleve le flag de modification sur l'objet et ses enfants
        /// </summary>
        public void SetNotChanged()
        {
            this.IsChanged = false;
        }

        #endregion Methodes
    }
}
