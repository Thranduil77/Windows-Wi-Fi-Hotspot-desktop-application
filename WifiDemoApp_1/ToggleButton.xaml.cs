#region Using

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

#endregion

namespace WifiDemoApp_1
{
    /// <summary>
    ///     Interaction logic for ToggleButton.xaml
    /// </summary>
    public partial class ToggleButton : UserControl
    {
        private readonly Thickness LeftSide = new Thickness(-39, 0, 0, 0);

        private readonly SolidColorBrush Off = new SolidColorBrush(Color.FromRgb(160, 160, 160));
        private readonly SolidColorBrush On = new SolidColorBrush(Color.FromRgb(130, 190, 125));
        private readonly Thickness RightSide = new Thickness(0, 0, -31, 0);
        private bool _toogledBoolean;

        /// <summary>
        ///     Init method - poziva se prilikom kreiranja user controle
        /// </summary>
        public ToggleButton()
        {
            InitializeComponent();

            Back.Fill = Off;
            ToogledBooleanPublic = false;
            Dot.Margin = LeftSide;
        }

        public bool ToogledBooleanPublic
        {
            get => _toogledBoolean;
            set
            {
                if (_toogledBoolean != value)
                {
                    _toogledBoolean = value;

                    if (value == false)
                    {
                        Back.Fill = Off;
                        Dot.Margin = LeftSide;
                    }
                }
                ;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Dot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UserClicked();
        }

        private void Back_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UserClicked();
        }


        private void UserClicked()
        {
            if (ToogledBooleanPublic == false)
            {
                Back.Fill = On;
                ToogledBooleanPublic = true;
                Dot.Margin = RightSide;
            }
            else
            {
                Back.Fill = Off;
                ToogledBooleanPublic = false;
                Dot.Margin = LeftSide;
            }
        }
    }
}