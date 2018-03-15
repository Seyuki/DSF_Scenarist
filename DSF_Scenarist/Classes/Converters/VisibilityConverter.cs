using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace DSF_Scenarist.Classes.Converters
{
    /// <summary>
    /// Converter transformant un bool en objet Visibility
    /// </summary>
    class VisibilityConverter : IValueConverter
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
            if (value == null)
                return Visibility.Collapsed;
            else if (value.GetType() != typeof(bool))
                return Visibility.Visible;
            else if ((bool)value)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}