using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DSF_Scenarist.Classes.Converters
{
    /// <summary>
    /// Renvoie le nombre d'élements dans une liste
    /// </summary>
    public class CollectionCountConverter : IValueConverter
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
            try
            {
                return (value as ICollection).Count;
            }
            catch
            {
                return 0;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}