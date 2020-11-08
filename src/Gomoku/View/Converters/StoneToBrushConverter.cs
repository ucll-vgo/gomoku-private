using Model.Gomoku;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace View.Converters
{
    public class StoneConverter : IValueConverter
    {
        public object White { get; set; }

        public object Black { get; set; }

        public object Empty { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stone = (Stone)value;

            if ( stone == Stone.WHITE )
            {
                return White;
            }
            else if ( stone == Stone.BLACK )
            {
                return Black;
            }
            else
            {
                return Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
