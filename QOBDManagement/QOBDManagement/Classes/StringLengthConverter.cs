using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace QOBDManagement.Classes
{
    public class StringLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string valueToProcess = value as string;
            string output = "";
            if (valueToProcess != null)
            {
                int nbCharToDisplay = 40;
                if (valueToProcess.Length > nbCharToDisplay)
                {
                    int index = 0;
                    string newContent = "";
                    var stringTable = valueToProcess.Split(' ').ToList();
                    while (index < stringTable.Count)
                    {
                        newContent += stringTable[index] + " ";
                        if (newContent.Length >= nbCharToDisplay)
                        {
                            output += newContent + Environment.NewLine;
                            newContent = "";
                        }     
                        index++;
                    }
                    output += newContent;
                }
                else
                    output = valueToProcess;

                return output;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
