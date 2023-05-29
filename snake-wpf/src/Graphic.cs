using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace snake_wpf.src
{
    public static class Graphic
    {
        public readonly static ImageSource EmptyImage = LoadImage("Empty.png");
        public readonly static ImageSource BodyImage = LoadImage("Body.png");
        public readonly static ImageSource HeadImage = LoadImage("Head.png");
        public readonly static ImageSource FoodImage = LoadImage("Food.png");
        public readonly static ImageSource DeadHeadImage = LoadImage("DeadHead.png");
        public readonly static ImageSource DeadBodyImage = LoadImage("DeadBody.png");

        private static ImageSource LoadImage(string fileName)
        {
            return new BitmapImage(new Uri($"Assets/{fileName}", UriKind.Relative));
        }
    }
}
