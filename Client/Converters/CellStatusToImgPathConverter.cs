using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace Client.Converters
{
    [ValueConversion(typeof(CellStatus), typeof(string))]
    public class CellStatusToImgPathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            switch ((CellStatus)value) {
                case CellStatus.EMPTY:
                    return "/Resources/Images/cell_empty.png";
                case CellStatus.USER_O:
                    return "/Resources/Images/cell_o_blue.png";
                case CellStatus.USER_X:
                    return "/Resources/Images/cell_x_blue.png";
                case CellStatus.OPPONENT_O:
                    return "/Resources/Images/cell_o_red.png";
                case CellStatus.OPPONENT_X:
                    return "/Resources/Images/cell_x_red.png";
                default:
                    throw new Exception("WRONG_CELL_STATUS_ERROR");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            switch ((string)value) {
                case "/Resources/Images/cell_empty.png":
                    return CellStatus.EMPTY;
                case "/Resources/Images/o_blue.png":
                    return CellStatus.USER_O;
                case "/Resources/Images/x_blue.png":
                    return CellStatus.USER_X;
                case "/Resources/Images/o_red.png":
                    return CellStatus.OPPONENT_O;
                case "/Resources/Images/x_red.png":
                    return CellStatus.OPPONENT_X;
                default:
                    throw new Exception("WRONG_CELL_STATUS_ERROR");
            }
        }
    }
}
