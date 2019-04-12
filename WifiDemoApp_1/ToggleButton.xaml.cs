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
        private readonly Thickness _leftSide = new Thickness(-39, 0, 0, 0);
        private readonly SolidColorBrush _off = new SolidColorBrush(Color.FromRgb(160, 160, 160));
        private readonly SolidColorBrush _on = new SolidColorBrush(Color.FromRgb(130, 190, 125));
        private readonly Thickness _rightSide = new Thickness(0, 0, -31, 0);
        private bool _toggledBoolean;

        /// <summary>
        ///     Init method - poziva se prilikom kreiranja user controle
        /// </summary>
        public ToggleButton()
        {
            InitializeComponent();

            Back.Fill = _off;
            ToggledBooleanPublic = false;
            Dot.Margin = _leftSide;
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
                        Back.Fill = _off;
                        Dot.Margin = _leftSide;
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
            if (ToggledBooleanPublic == false)
            {
                Back.Fill = _on;
                ToggledBooleanPublic = true;
                Dot.Margin = _rightSide;
            }
            else
            {
                Back.Fill = _off;
                ToggledBooleanPublic = false;
                Dot.Margin = _leftSide;
            }
        }
    }
}