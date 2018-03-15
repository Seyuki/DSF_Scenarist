using System.ComponentModel;

namespace DSF_Scenarist.Classes.Tools
{
    /// <summary>
    ///  Classe contenant les éléments de INotifyPropertyChanged
    /// </summary>
    public class NotifyClassTemplate : INotifyPropertyChanged
    {
        /// <summary>
        /// Evenement changement de valeur d'une propriété
        /// </summary>
        /// <param name="propertyName">Nom de la propriété</param>
        protected void OnPropertyChanged(string propertyName)
        {
            var evt = this.PropertyChanged;
            if (evt != null)
                evt(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Handler de l'evenement changement de valeur d'une propriété
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
