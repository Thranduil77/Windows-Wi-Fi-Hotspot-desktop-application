namespace WifiDemoApp_1
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows;
    using System.Windows.Media.Imaging;

    #endregion

    /// <summary>
    ///     Interaction logic for HowToUseProgramWindow.xaml
    /// </summary>
    public partial class HowToUseProgramWindow : Window
    {
        private int _index;

        public HowToUseProgramWindow()
        {
            InitializeComponent();
        }

        public List<BitmapImage> BitmapImages { get; set; }

        public int Index
        {
            get => _index;
            set
            {
                if (value != _index)
                    _index = value;
            }
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
            ImageNumber.Text = $"{Index + 1} / {BitmapImages.Count}";
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
            if (Index + 1 >= BitmapImages.Count)
                return;

            Index++;
            SetImage();
        }

        private void Button_LeftClick(object sender, RoutedEventArgs e)
        {
            if (Index <= 0)
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
            Index = BitmapImages.Count - 1;
            SetImage();
        }
    }
}