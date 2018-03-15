using DSF_Scenarist.Classes.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSF_Scenarist.Classes.StoryItems
{
    /// <summary>
    /// Classe représentant les conditions d'une action
    /// </summary>
    public class StoryActionConditions : StoryItemTemplate
    {
        #region Propriétés

        /// <summary>
        /// Condition de visibilité
        /// </summary>
        private string _visibility = string.Empty;

        /// <summary>
        /// Condition de réussite
        /// </summary>
        private string _success = string.Empty;

        #endregion Propriétés

        #region Accesseurs

        /// <summary>
        /// Condition de visibilité
        /// </summary>
        public string Visibility
        {
            get => _visibility;
            set
            {
                _visibility = value;
                OnPropertyChanged("Visibility");
            }
        }

        /// <summary>
        /// Condition de réussite
        /// </summary>
        public string Success
        {
            get => _success;
            set
            {
                _success = value;
                OnPropertyChanged("Success");
            }
        }

        #endregion Accesseurs

        #region Constructeurs

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        /// <param name="parent">Parent de l'élément</param>
        public StoryActionConditions(StoryItemTemplate parent) => this.Parent = parent;

        /// <summary>
        /// Constructeur avec informations
        /// </summary>
        /// <param name="parent">Parent de l'élément</param>
        /// <param name="visibility">Condition de visibilité</param>
        /// <param name="success">Condition de réussite</param>
        public StoryActionConditions(StoryItemTemplate parent, string visibility, string success)
        {
            this.Parent = parent;
            this.Visibility = visibility;
            this.Success = success;
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
            JSON += Environment.NewLine + indent + "\"Visibility\": \"" + this.Visibility + "\"";
            JSON += ",";
            JSON += Environment.NewLine + indent + "\"Success\": \"" + this.Success + "\"";
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
