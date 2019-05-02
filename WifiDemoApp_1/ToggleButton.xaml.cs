using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WifiDemoApp_1
{
    #region Using

    #endregion

    /// <summary>
    ///     Interaction logic for ToggleButton.xaml
    /// </summary>
    public partial class ToggleButton : UserControl
    {
        private bool _toggledBoolean;

        public readonly SolidColorBrush Off = new SolidColorBrush(Color.FromRgb(160, 160, 160));
        public readonly SolidColorBrush On = new SolidColorBrush(Color.FromRgb(130, 190, 125));

        public readonly Thickness LeftSide = new Thickness(-39, 0, 0, 0);
        public readonly Thickness RightSide = new Thickness(0, 0, -31, 0);

        /// <summary>
        ///     Init method - poziva se prilikom kreiranja user controle
        /// </summary>
        public ToggleButton()
        {
            InitializeComponent();

            Back.Fill = Off;
            ToggledBooleanPublic = false;
            Dot.Margin = LeftSide;
        }

        public bool ToggledBooleanPublic
        {
            get => _toggledBoolean;

            set
            {
                if (_toggledBoolean != value)
                {
                    _toggledBoolean = value;

                    if (value == false)
                    {
                        Back.Fill = Off;
                        Dot.Margin = LeftSide;
                    }
                }
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
            //if (ToggledBooleanPublic == false)
            //{
            //    Back.Fill = _on;
            //    ToggledBooleanPublic = true;
            //    Dot.Margin = _rightSide;
            //}
            //else
            //{
            //    Back.Fill = _off;
            //    ToggledBooleanPublic = false;
            //    Dot.Margin = _leftSide;
            //}
        }
    }
}