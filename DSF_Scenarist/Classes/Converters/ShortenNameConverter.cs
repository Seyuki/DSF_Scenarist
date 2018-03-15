using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DSF_Scenarist.Classes.Converters
{
    /// <summary>
    /// Converter permettant de raccourcis une chaine de caractère (pour le TreeView)
    /// </summary>
    public class ShortenNameConverter : IValueConverter
    {
        /// <summary>
        /// Conversion
        /// </summary>
        /// <param name="value">Chaine à raccourcis</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int maxLength = 33;

            string name = value.ToString();
            if (name.Length >= maxLength)
            {
                name = name.Substring(0, maxLength - 3) + "...";
            }

            return name;
        }

        /// <summary>
        /// Rétablissement de la conversion (NON IMPLEMENTEE)
        /// </summary>
        /// <param name="value">Chaine convertie</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
