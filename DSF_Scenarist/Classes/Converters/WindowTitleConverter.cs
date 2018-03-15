using System;
using System.Windows.Data;

namespace DSF_Scenarist.Classes.Converters
{
    /// <summary>
    /// Converter ajoutant la chaine de caractère au titre de la fenêtre si non nulle ou vide
    /// </summary>
    class WindowTitleConverter : IValueConverter
    {
        /// <summary>
        /// Conversion
        /// </summary>
        /// <param name="value">Valeur à convertir</param>
        /// <param name="targetType">Type de cible</param>
        /// <param name="parameter">Paramètre</param>
        /// <param name="culture">Culture de langue</param>
        /// <returns>Brush</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || value.GetType() != typeof(Scenario))
                return "";
            else if (string.IsNullOrEmpty((value as Scenario).FullPath))
                return " [Nouveau scénario non enregistré]";
            else
                return " [" + (value as Scenario).FullPath + "]";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}