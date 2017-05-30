using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace QOBDManagement.Classes
{
    class ImageConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null 
                || value as InfoManager.Display == null 
                || (value as InfoManager.Display != null 
                        && string.IsNullOrEmpty((value as InfoManager.Display).TxtFileFullPath)))
                return new BitmapImage();

            var display = (InfoManager.Display)value;

            var task = Task.Run(() =>
            {
                FileStream imageStream = new FileStream(display.TxtFileFullPath, FileMode.Open, FileAccess.Read, FileShare.Read | FileShare.Delete);
                BitmapImage imageSource = new BitmapImage();
                imageSource.BeginInit();
                imageSource.UriSource = null;
                imageSource.StreamSource = imageStream;
                imageSource.CacheOption = BitmapCacheOption.OnLoad;
                imageSource.EndInit();
                imageSource.Freeze();

                return imageSource;
            });

            return new QOBDCommon.Classes.NotifyTaskCompletion<BitmapImage>(task);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
