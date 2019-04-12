namespace WifiDemoApp_1
{
    #region Using

    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Threading;
    using System.Xml;
    using System.Xml.Serialization;
    using Microsoft.Win32;

    #endregion

    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        //private readonly SolidColorBrush _off = new SolidColorBrush(Color.FromRgb(160, 160, 160));
        //private readonly SolidColorBrush _on = new SolidColorBrush(Color.FromRgb(130, 190, 125));

        public MainWindow()
        {
            InitializeComponent();
            CreateDispatcherTimer();
        }

        private void CreateDispatcherTimer()
        {
            var timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal,
                                            delegate { DateTimeTextBlock.Text = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss"); }, Dispatcher);
        }

        private void ToggleButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OutputOfaProgram.Text = "";
            var hotspotData = new HotspotData
                              {
                                  Name = NetworkNameInput.Text,
                                  Password = WirelessPasswordInput.Text
                              };

            if (SwitchUserControlButton.ToggledBooleanPublic)
            {
                TurnWiFiOff();
                return;
            }

            if (ValidateHotspotData(hotspotData) == ValidationResult.Invalid) return;

            TurnWifiOn();

            var startHotspotCmdWithParams = string.Format(WifiDemoApp_1.Resources.StartHotspotWithParameters, WirelessPasswordInput.Text,
                                                          NetworkNameInput.Text);
            RunCmd(startHotspotCmdWithParams);
            RunCmd(WifiDemoApp_1.Resources.StopHotspot);
        }

        /// <summary>
        ///     Logic for turning Wifi on
        /// </summary>
        private void TurnWifiOn()
        {
            LightEllipse.Fill = SwitchUserControlButton.On;
            DisplayTextBlock.Text = "ON";
            SwitchUserControlButton.ToggledBooleanPublic = true;
            SwitchUserControlButton.Back.Fill = SwitchUserControlButton.On;
            SwitchUserControlButton.ToggledBooleanPublic = true;
            SwitchUserControlButton.Dot.Margin = SwitchUserControlButton.RightSide;
        }

        /// <summary>
        ///     Logic for turning Wifi off
        /// </summary>
        private void TurnWiFiOff()
        {
            LightEllipse.Fill = SwitchUserControlButton.Off;
            DisplayTextBlock.Text = "OFF";
            SwitchUserControlButton.ToggledBooleanPublic = false;
            SwitchUserControlButton.Back.Fill = SwitchUserControlButton.Off;
            SwitchUserControlButton.ToggledBooleanPublic = false;
            SwitchUserControlButton.Dot.Margin = SwitchUserControlButton.LeftSide;
        }

        /// <summary>
        ///     Generic method for running cmd commands
        /// </summary>
        private void RunCmd(string command)
        {
            //clear output text
            OutputOfaProgram.Text = "";

            // Start the child process.
            var process = new Process
                          {
                              StartInfo =
                              {
                                  UseShellExecute = false,
                                  RedirectStandardOutput = true,
                                  RedirectStandardError = true,
                                  FileName = "cmd.exe",
                                  Arguments = $"/c call {command}",
                                  CreateNoWindow = true,
                                  WindowStyle = ProcessWindowStyle.Hidden
                              }
                          };

            // Redirect the output stream of the child process.
            // Do not create the black window.
            process.OutputDataReceived += OutputHandler;
            process.ErrorDataReceived += OutputHandler;
            process.Start();

            // Do not wait for the child process to exit before
            // reading to the end of its redirected stream.
            // p.WaitForExit();
            // Read the output stream first and then wait.
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            process.OutputDataReceived -= OutputHandler;
            process.ErrorDataReceived -= OutputHandler;
        }

        private void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            //This is a common problem with people getting started.Whenever you 
            //update your UI elements from a thread other than the main thread, 
            //you need to use:

            //Using Dispatcher.Invoke is a synchronous call, so it is effectively 
            //the same as doing this on the UI thread, because the call to Invoke 
            //    blocks until the UI performs the requested task. If you're doing
            //    that too much in a short time span you're effectively blocking the 
            //    UI thread.
            Dispatcher.BeginInvoke(new Action(() => { OutputOfaProgram.Text += outLine.Data + "\n"; }),
                                   DispatcherPriority.ApplicationIdle);
        }

        #region Network section

        private void OpenNetworkConnections_OnClick(object sender, RoutedEventArgs e)
        {
            var startInfo = new ProcessStartInfo("NCPA.cpl") { UseShellExecute = true };

            Process.Start(startInfo);
        }

        #endregion

        private void Button_CheckConnectedDevicesClick(object sender, RoutedEventArgs e)
        {
            RunCmd(WifiDemoApp_1.Resources.ShowHotspots);
        }

        #region Main section

        private void OpenConfiguration_OnClick(object sender, RoutedEventArgs e)
        {
            // Displays an OpenFileDialog so the user can select a Cursor.  
            var openFileDialog = new OpenFileDialog
                                 {
                                     Filter = ".xml Files|*.xml",
                                     Title = "Select a .xml config File",
                                     Multiselect = false,
                                     RestoreDirectory = true
                                 };

            // Show the Dialog.  
            // If the user clicked OK in the dialog and  
            // a .CUR file was selected, open it.  
            if (openFileDialog.ShowDialog() != true)
                return;

            //Get the path of specified file
            var filePath = openFileDialog.FileName;

            try
            {
                using (TextReader reader = new StreamReader(filePath))
                {
                    var serializer = new XmlSerializer(typeof(HotspotData));
                    var hotspotData = (HotspotData) serializer.Deserialize(reader);
                    if (ValidateHotspotData(hotspotData) == ValidationResult.Valid)
                    {
                        WirelessPasswordInput.Text = hotspotData.Password;
                        NetworkNameInput.Text = hotspotData.Name;
                    }
                    else
                    {
                        MessageBox.Show($"wifi name and password in '{filePath}' are invalid!", "ERROR",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        ///     Validate deserialized Hotspot Data
        /// </summary>
        /// <param name="test"></param>
        /// <returns></returns>
        private static ValidationResult ValidateHotspotData(HotspotData test)
        {
            if (string.IsNullOrEmpty(test.Name) || string.IsNullOrEmpty(test.Password))
            {
                var outputMessage = $"Hotspot data is {ValidationResult.Invalid}";
                Console.WriteLine(outputMessage);
                MessageBox.Show(outputMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                return ValidationResult.Invalid;
            }

            return ValidationResult.Valid;
        }

        private void SaveConfiguration_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(WirelessPasswordInput?.Text) || string.IsNullOrEmpty(NetworkNameInput?.Text))
            {
                MessageBox.Show("Wifi name and password missing!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                var xsSubmit = new XmlSerializer(typeof(HotspotData));
                var subReq = new HotspotData
                             {
                                 Name = NetworkNameInput.Text,
                                 Password = WirelessPasswordInput.Text
                             };

                using (var stringWriter = new StringWriter())
                {
                    try
                    {
                        using (var writer = XmlWriter.Create(stringWriter))
                        {
                            xsSubmit.Serialize(writer, subReq);

                            var save = new SaveFileDialog
                                       {
                                           FileName = "HotspotData.xml",
                                           Filter = "Xml File | *.xml",
                                           InitialDirectory = "D:\\",
                                           RestoreDirectory = true
                                       };

                            if (save.ShowDialog() != true)
                                return;

                            using (var streamWriter = new StreamWriter(save.FileName))
                            {
                                streamWriter.Write(stringWriter);
                            }
                        }

                        MessageBox.Show("Configuration saved successfully!", "Information", MessageBoxButton.OK,
                                        MessageBoxImage.Information);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        #endregion

        #region About section

        private void AboutAuthor_OnClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Author: Ivan Zagar, Junior Software Engineer \nCheckout my GitHub link ?", "About me", MessageBoxButton.YesNo,
                                MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                Process.Start("https://github.com/Thranduil77/");
        }

        private void HowToUse_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new HowToUseProgramWindow();
            dialog.Show();
        }

        #endregion

        private void WirelessNetworkNameInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox) sender;
            if (textBox.Text.Length <= GlobalParameters.MinNetworkNameLength)
            {
                //add a tooltip for text box
                textBox.ToolTip = string.Format(WifiDemoApp_1.Resources.NetworkNameLengthInvalid, textBox.Text.Length);
                NetworkNameInput.BorderThickness = new Thickness(2);
                NetworkNameInput.BorderBrush = Brushes.Red;
            }
            else
            {
                //reset defaults
                NetworkNameInput.BorderThickness = new Thickness(1);
                NetworkNameInput.BorderBrush = Brushes.Black;
                textBox.ToolTip = null;
            }
        }

        private void WirelessPasswordInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox) sender;
            if (textBox.Text.Length <= GlobalParameters.MinPasswordLength || textBox.Text.Length >= GlobalParameters.MaxPasswordLength)
            {
                //add a tooltip for text box
                textBox.ToolTip = string.Format(WifiDemoApp_1.Resources.PasswordLengthInvalid, textBox.Text.Length);
                WirelessPasswordInput.BorderThickness = new Thickness(2);
                WirelessPasswordInput.BorderBrush = Brushes.Red;
            }
            else
            {
                //reset defaults
                WirelessPasswordInput.BorderThickness = new Thickness(1);
                WirelessPasswordInput.BorderBrush = Brushes.Black;
                textBox.ToolTip = null;
            }
        }
    }
}