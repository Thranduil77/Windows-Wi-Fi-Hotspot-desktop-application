using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WifiDemoApp_1
{
    using System.IO;

    /// <summary>
    /// Interaction logic for HowToUseProgramWindow.xaml
    /// </summary>
    public partial class HowToUseProgramWindow : Window
    {
        private List<BitmapImage> _bitmapImages;
        private int _index;

        public List<BitmapImage> BitmapImages
        {
            get { return _bitmapImages; }
            set { _bitmapImages = value; }
        }

        public int Index
        {
            get { return _index; }
            set
            {
                if (value != _index)
                    _index = value;
            }
        }

        public HowToUseProgramWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Index = 0;
            LoadImagesAndFillDictionary();
            SetImage();
        }

        private void SetImage()
        {
            FinalImage.Source = BitmapImages[Index];
            ImageNumber.Text = $"{Index+1} / {BitmapImages.Count}";
            ImageTextBlock.Text = Path.GetFileName(BitmapImages[Index].UriSource.AbsolutePath);
        }

        private void LoadImagesAndFillDictionary()
        {
            var files = Directory.GetFiles("Images/HowToUse/", "*.jpg", SearchOption.AllDirectories);

            BitmapImages = new List<BitmapImage>();

            foreach (var file in files)
            {
                var image = new BitmapImage(new Uri("pack://application:,,,/" + file));
                BitmapImages.Add(image);
            }
        }

        private void Button_RightClick(object sender, RoutedEventArgs e)
        {
            if(Index +1 >= BitmapImages.Count)
                return;

            Index++;
            SetImage();
        }

        private void Button_LeftClick(object sender, RoutedEventArgs e)
        {
            if(Index <= 0)
                return;

            Index--;
            SetImage();
        }

        private void Button_StartClick(object sender, RoutedEventArgs e)
        {
            Index = 0;
            SetImage();
        }

        private void Button_EndClick(object sender, RoutedEventArgs e)
        {
            Index = BitmapImages.Count-1;
            SetImage();
        }
    }
}
