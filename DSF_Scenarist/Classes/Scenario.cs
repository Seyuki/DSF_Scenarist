using DSF_Scenarist.Classes.StoryItems;
using DSF_Scenarist.Classes.Tools;
using DSF_Scenarist.Windows;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static DSF_Scenarist.Classes.StoryItems.StoryActionConsequence;

namespace DSF_Scenarist.Classes
{
    /// <summary>
    /// Enumerateur de niveau JSON
    /// </summary>
    enum eJSONlevel
    {
        Path,
        Level,
        Block,
        Action,
        Conditions,
        Consequences,
        Success,
        Failure
    }

    /// <summary>
    /// Class représentant un scénario DSF
    /// </summary>
    public class Scenario : StoryItemTemplate
    {
        #region Propriétés

        /// <summary>
        /// Chemin complet vers le scénario
        /// </summary>
        private string _fullPath;

        /// <summary>
        /// Collection de cursus / path
        /// </summary>
        private ObservableCollection<StoryPath> _paths = new ObservableCollection<StoryPath>();

        #endregion Propriétés

        #region Accesseurs

        /// <summary>
        /// Chemin complet vers le scénario
        /// </summary>
        public string FullPath
        {
            get => _fullPath;
            set
            {
                _fullPath = value;
                OnPropertyChanged("FullPath");
            }
        }

        /// <summary>
        /// Collection de cursus / path
        /// </summary>
        public ObservableCollection<StoryPath> Paths
        {
            get => _paths;
            set
            {
                _paths = value;
                OnPropertyChanged("Paths");
            }
        }

        #endregion Accesseurs

        #region Constructeurs

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public Scenario()
        {
            this.Paths.CollectionChanged += new NotifyCollectionChangedEventHandler((sender, e) => StoryCollectionChanged(sender, e, "Paths"));
        }

        /// <summary>
        /// Constructeur avec chemin vers le fichier JSON
        /// </summary>
        /// <param name="JSONPath">Chemin vers le fichier JSON contenant le scénario</param>
        public Scenario(string JSONPath)
        {
            this.FullPath = JSONPath;
            this.Load();

            this.Paths.CollectionChanged += new NotifyCollectionChangedEventHandler((sender, e) => StoryCollectionChanged(sender, e, "Paths"));
        }

        #endregion Constructeurs

        #region Methodes

        /// <summary>
        /// Ajoute un nouveau path au scénario
        /// </summary>
        public override void AddChild(object param = null)
        {
            // Création d'un item
            StoryPath newPath = new StoryPath(this);
            this.Paths.Add(newPath);

            // On sélectionne le nouvel item et le flag comme modifié
            newPath.IsSelected = true;
            newPath.IsChanged = true;
        }

        /// <summary>
        /// Supprime l'item en paramètre des enfants
        /// </summary>
        /// <param name="child">item à supprimer</param>
        public override void DeleteChild(StoryItemTemplate child)
        {
            if (!this.DeleteConfirmation())
                return;

            for (int i = 0; i < this.Paths.Count; ++i)
            {
                if (this.Paths[i] == child)
                    this.Paths.RemoveAt(i);
            }
        }

        /// <summary>
        /// Parse le fichier JSON pour le stocker dans le modèle
        /// </summary>
        public void Load()
        {
            try
            {
                // On vide le scénario chargé en mémoire
                this.Clear();

                // Charge le contenu du fichier en mémoire
                string[] ScenarioFile = File.ReadAllLines(this.FullPath, Encoding.UTF8);

                // On stock le level dans lequel on se trouve (par défaut : path)
                eJSONlevel lvl = eJSONlevel.Path;

                // Parcours du fichier
                foreach (string line in ScenarioFile)
                {
                    string[] JSON = SplitJSONline(line);

                    switch (lvl)
                    {
                        case eJSONlevel.Path:
                            lvl = ParseFromPath(JSON);
                            break;

                        case eJSONlevel.Level:
                            lvl = ParseFromLevel(JSON);
                            break;

                        case eJSONlevel.Block:
                            lvl = ParseFromBlock(JSON);
                            break;

                        case eJSONlevel.Action:
                            lvl = ParseFromAction(JSON);
                            break;

                        case eJSONlevel.Conditions:
                            lvl = ParseFromConditions(JSON);
                            break;

                        case eJSONlevel.Consequences:
                            lvl = ParseFromConsequences(JSON);
                            break;

                        case eJSONlevel.Success:
                            lvl = ParseFromSuccessOrFailure(JSON, lvl);
                            break;

                        case eJSONlevel.Failure:
                            lvl = ParseFromSuccessOrFailure(JSON, lvl);
                            break;
                    }
                }

                // On enlève le flag de modification
                this.SetNotChanged();
            }
            catch { }
        }

        #region Parsing JSON

        /// <summary>
        /// Split la ligne JSON
        /// </summary>
        /// <param name="line">Ligne JSON à splitter</param>
        /// <returns>
        /// Tableau de strings sans guillemets
        /// Le premier élement est forcé en minuscule
        /// </returns>
        private string[] SplitJSONline(string line)
        {
            // Trim de la ligne
            line = line.Trim();

            // Suppression d'une potentielle virgule
            if (line.Substring(0, 1) == ",") line = line.Substring(1).Trim();
            else if (line.Last() == ',') line = line.Substring(0, line.Length - 1).Trim();

            // Création d'un tableau à deux éléments
            string[] pair = new string[] { string.Empty, string.Empty };
            int IndexSplit = line.IndexOf(':');

            // Si pas de séparateur (:)
            if (IndexSplit == -1)
            {
                pair[0] = line.Trim();
            }
            // Si séparateur
            else
            {
                pair[0] = line.Substring(0, IndexSplit).Trim().ToLower();
                pair[1] = line.Substring(IndexSplit + 1).Trim();
            }

            // On enlève les qualifiers si présents
            if (pair[0].Length > 0 && (pair[0].Substring(0, 1) == "\"" || pair[0].Substring(0, 1) == "'")) pair[0] = pair[0].Substring(1).Trim();
            if (pair[0].Length > 0 && (pair[0].Last() == '"' || pair[0].Last() == '\'')) pair[0] = pair[0].Substring(0, pair[0].Length - 1).Trim();

            if (pair[1].Length > 0 && (pair[1].Substring(0, 1) == "\"" || pair[1].Substring(0, 1) == "'")) pair[1] = pair[1].Substring(1).Trim();
            if (pair[1].Length > 0 && (pair[1].Last() == '"' || pair[1].Last() == '\'')) pair[1] = pair[1].Substring(0, pair[1].Length - 1).Trim();

            return pair;
        }

        /// <summary>
        /// Parse le JSON à partir du niveau 'path'
        /// </summary>
        /// <param name="JSON">Paire JSON à parser</param>
        /// <returns>Nouveau niveau JSON</returns>
        private eJSONlevel ParseFromPath(string[] JSON)
        {
            switch(JSON[0])
            {
                case "{":
                    this.Paths.Add(new StoryPath(this));
                    break;

                case "path":
                    this.Paths.Last().Name = JSON[1];
                    break;

                case "levels":
                    return eJSONlevel.Level;
            }

            return eJSONlevel.Path;
        }

        /// <summary>
        /// Parse le JSON à partir du niveau 'level'
        /// </summary>
        /// <param name="JSON">Paire JSON à parser</param>
        /// <returns>Nouveau niveau JSON</returns>
        private eJSONlevel ParseFromLevel(string[] JSON)
        {
            switch (JSON[0])
            {
                case "{":
                    this.Paths.Last().Levels.Add(new StoryLevel(this.Paths.Last()));
                    break;

                case "id":
                    Int32.TryParse(JSON[1], out int tmp);
                    this.Paths.Last().Levels.Last().ID = tmp;
                    break;

                case "name":
                    this.Paths.Last().Levels.Last().Name = JSON[1];
                    break;

                case "transition":
                    this.Paths.Last().Levels.Last().Transition = JSON[1];
                    break;

                case "blocks":
                    return eJSONlevel.Block;

                case "]":
                    return eJSONlevel.Path;
            }

            return eJSONlevel.Level;
        }

        /// <summary>
        /// Parse le JSON à partir du niveau 'block'
        /// </summary>
        /// <param name="JSON">Paire JSON à parser</param>
        /// <returns>Nouveau niveau JSON</returns>
        private eJSONlevel ParseFromBlock(string[] JSON)
        {
            switch (JSON[0])
            {
                case "{":
                    this.Paths.Last().Levels.Last().Blocks.Add(new StoryBlock(this.Paths.Last().Levels.Last()));
                    break;

                case "id":
                    Int32.TryParse(JSON[1], out int tmp);
                    this.Paths.Last().Levels.Last().Blocks.Last().ID = tmp;
                    break;

                case "description":
                    this.Paths.Last().Levels.Last().Blocks.Last().Description = JSON[1];
                    break;

                case "type":
                    this.Paths.Last().Levels.Last().Blocks.Last().Type = JSON[1];
                    break;

                case "actions":
                    return eJSONlevel.Action;

                case "]":
                    return eJSONlevel.Level;
            }

            return eJSONlevel.Block;
        }

        /// <summary>
        /// Parse le JSON à partir du niveau 'action'
        /// </summary>
        /// <param name="JSON">Paire JSON à parser</param>
        /// <returns>Nouveau niveau JSON</returns>
        private eJSONlevel ParseFromAction(string[] JSON)
        {
            switch (JSON[0])
            {
                case "{":
                    this.Paths.Last().Levels.Last().Blocks.Last().Actions.Add(new StoryAction(this.Paths.Last().Levels.Last().Blocks.Last()));
                    break;

                case "name":
                    this.Paths.Last().Levels.Last().Blocks.Last().Actions.Last().Name = JSON[1];
                    break;

                case "type":
                    this.Paths.Last().Levels.Last().Blocks.Last().Actions.Last().Type = JSON[1];
                    break;

                case "conditions":
                    return eJSONlevel.Conditions;

                case "consequences":
                    return eJSONlevel.Consequences;

                case "]":
                    return eJSONlevel.Block;
            }

            return eJSONlevel.Action;
        }

        /// <summary>
        /// Parse le JSON à partir du niveau 'conditions'
        /// </summary>
        /// <param name="JSON">Paire JSON à parser</param>
        /// <returns>Nouveau niveau JSON</returns>
        private eJSONlevel ParseFromConditions(string[] JSON)
        {
            switch (JSON[0])
            {
                case "visibility":
                    this.Paths.Last().Levels.Last().Blocks.Last().Actions.Last().Conditions.Visibility = JSON[1];
                    break;

                case "success":
                    this.Paths.Last().Levels.Last().Blocks.Last().Actions.Last().Conditions.Success = JSON[1];
                    break;

                case "}":
                    return eJSONlevel.Action;
            }

            return eJSONlevel.Conditions;
        }

        /// <summary>
        /// Parse le JSON à partir du niveau 'consequences'
        /// </summary>
        /// <param name="JSON">Paire JSON à parser</param>
        /// <returns>Nouveau niveau JSON</returns>
        private eJSONlevel ParseFromConsequences(string[] JSON)
        {
            switch (JSON[0])
            {
                case "success":
                    return eJSONlevel.Success;

                case "failure":
                    return eJSONlevel.Failure;

                case "}":
                    return eJSONlevel.Action;
            }

            return eJSONlevel.Consequences;
        }

        /// <summary>
        /// Parse le JSON à partir du niveau 'success' ou 'failure'
        /// </summary>
        /// <param name="JSON">Paire JSON à parser</param>
        /// <param name="lvl">niveau actuel du JSON</param>
        /// <returns>Nouveau niveau JSON</returns>
        private eJSONlevel ParseFromSuccessOrFailure(string[] JSON, eJSONlevel lvl)
        {
            ObservableCollection<StoryActionConsequence> ConsequenceTypeCollection = 
                lvl == eJSONlevel.Success ? this.Paths.Last().Levels.Last().Blocks.Last().Actions.Last().Consequences.Successes
                                          : this.Paths.Last().Levels.Last().Blocks.Last().Actions.Last().Consequences.Failures;

            eConsequenceType consequenceType = lvl == eJSONlevel.Success ? eConsequenceType.Success : eConsequenceType.Failure;

            switch (JSON[0])
            {
                case "{":
                    ConsequenceTypeCollection.Add(new StoryActionConsequence(this.Paths.Last().Levels.Last().Blocks.Last().Actions.Last(), consequenceType));
                    break;

                case "message":
                    ConsequenceTypeCollection.Last().Message = JSON[1];
                    break;

                case "gauges":
                    ConsequenceTypeCollection.Last().Gauges = JSON[1];
                    break;

                case "skills":
                    ConsequenceTypeCollection.Last().Skills = JSON[1];
                    break;

                case "effects":
                    ConsequenceTypeCollection.Last().Effects = JSON[1];
                    break;

                case "]":
                    return eJSONlevel.Consequences;
            }

            return lvl;
        }

        #endregion Parsing JSON

        /// <summary>
        /// Si le scénario a été modifié, ouvre une fenêtre demandé s'il doit être enregistré
        /// </summary>
        public bool? AskForSaving()
        {
            bool? answer = null;

            // Si pas de changement, on renvoie vrai
            if (!this.IsChanged)
                return true;

            // Fenêtre de demande
            AskForSavingWindow askWindow = new AskForSavingWindow();
            askWindow.Owner = Application.Current.MainWindow;

            if (askWindow.ShowDialog() == false)
                answer = askWindow.Rep;

            // Si true, on enregistre
            if (answer != null && (bool)answer)
                this.Save();

            return answer;
        }

        /// <summary>
        /// Enregistre sous le scénario
        /// </summary>
        public void SaveAs()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = string.IsNullOrEmpty(this.FullPath) ? "Nouveau scénario DSF" : Path.GetFileNameWithoutExtension(FullPath);
            saveFileDialog.DefaultExt = ".json";
            saveFileDialog.Filter = "Scénario DSF|*.json";

            bool? result = saveFileDialog.ShowDialog();

            if (result != true)
                return;

            this.FullPath = saveFileDialog.FileName;
            this.Save();
        }

        /// <summary>
        /// Enregistre le scénario dans le fichier spécifié en paramètre
        /// </summary>
        /// <param name="Path">Nouveau chemin pour le fichier</param>
        public void Save(string Path)
        {
            this.FullPath = Path;
            this.Save();
        }

        /// <summary>
        /// Enregistre le scénario présent dans le modèle au format JSON
        /// </summary>
        public void Save()
        {
            // Si le scénario n'a pas de chemin (donc un nouveau scénario), on redirige vers le SaveAs
            if (string.IsNullOrEmpty(this.FullPath))
            {
                this.SaveAs();
                return;
            }

            // Si le fichier de résultat existe déjà, on le supprime
            if (File.Exists(this.FullPath)) File.Delete(this.FullPath);

            // On écrit dans le fichier
            using (StreamWriter sw = new StreamWriter(this.FullPath))
            {
                string JSON = string.Empty;

                // Paths
                for (int i = 0, max = this.Paths.Count - 1; i <= max; ++i)
                {
                    // Transformation en JSON
                    string JSONtmp = this.Paths[i].ToJSON();

                    // Si ce n'est pas le premier élement
                    if (i > 0) JSONtmp = Environment.NewLine + JSONtmp;
                    // Si ce n'est pas le dernier élément
                    if (i < max) JSONtmp += ",";

                    JSON += JSONtmp;
                }

                // Ecriture dans le fichier
                sw.WriteLine(JSON);

                // Retablissement du flag de modification
                this.SetNotChanged();
            }
        }

        /// <summary>
        /// Enleve le flag de modification sur l'objet et ses enfants
        /// </summary>
        private void SetNotChanged()
        {
            this.IsChanged = false;

            foreach (StoryPath path in this.Paths)
            {
                path.SetNotChanged();
            }
        }

        /// <summary>
        /// Vide le scénario chargé en mémoire
        /// </summary>
        public void Clear()
        {
            this.Paths.Clear();
        }

        #endregion Methodes
    }
}
