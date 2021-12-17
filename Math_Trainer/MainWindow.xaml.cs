using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;

namespace MathTrainerVC19
{
    public partial class MainWindow : Window
    {
        private string configFileName = "TrainerConfig.xml";

        public MainWindow()
        {
            InitializeComponent();

            ReadConfig(configFileName);
            ApplyChanges();
            ResizeMode = System.Windows.ResizeMode.CanMinimize;
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        START:

            Trainer mTrainer = new Trainer();
            Results mResults = new Results();

            mTrainer.Owner = this;
            mResults.Owner = this;

            mTrainer.ShowDialog();

            if (mTrainer.SetTrainerWindowState == true)
            {
                mResults.ShowDialog();
                if (mResults.SetResultsWindowState == true)
                {
                    goto START;
                }
            }
            this.Show();
        }

        private void IsAdditionChecked(object sender, RoutedEventArgs e)
        {
            Handler.UsingAddition = true;
        }

        private void IsSubtractionChecked(object sender, RoutedEventArgs e)
        {
            Handler.UsingSubtraction = true;
        }

        private void IsMultiplicationChecked(object sender, RoutedEventArgs e)
        {
            Handler.UsingMultiplication = true;
        }

        private void IsDivisionChecked(object sender, RoutedEventArgs e)
        {
            Handler.UsingDivision = true;
        }

        private void ConfigCalcTChkBoxAddition_Unchecked(object sender, RoutedEventArgs e)
        {
            Handler.UsingAddition = false;
        }

        private void ConfigCalcTChkBoxSubtraction_Unchecked(object sender, RoutedEventArgs e)
        {
            Handler.UsingSubtraction = false;
        }

        private void ConfigCalcTChkBoxMultiplication_Unchecked(object sender, RoutedEventArgs e)
        {
            Handler.UsingMultiplication = false;
        }

        private void ConfigCalcTChkBoxDivision_Unchecked(object sender, RoutedEventArgs e)
        {
            Handler.UsingDivision = false;
        }

        private void ConfigDisableTimer_Checked(object sender, RoutedEventArgs e)
        {
            Handler.DisableTimer = true;
            ConfigDifficultyRdBtnEasy.IsEnabled = false;
            ConfigDifficultyRdBtnMedium.IsEnabled = false;
            ConfigDifficultyRdBtnHard.IsEnabled = false;
            ConfigExtrasChkBoxTimelimit.IsEnabled = false;
        }

        private void ConfigDisableTimer_Unchecked(object sender, RoutedEventArgs e)
        {
            Handler.DisableTimer = false;
            ConfigDifficultyRdBtnEasy.IsEnabled = true;
            ConfigDifficultyRdBtnMedium.IsEnabled = true;
            ConfigDifficultyRdBtnHard.IsEnabled = true;
            ConfigExtrasChkBoxTimelimit.IsEnabled = true;
        }

        private void Radio_BtnCheck(object sender, RoutedEventArgs e)
        {
            RadioButton radio = (RadioButton)sender;

            if (radio != null)
            {
                string input = radio.Tag.ToString(); // waaaata code xd
                switch (input)
                {
                    case "Easy":
                        Handler.InitTime = 15;
                        Handler.StartDifficulty = 1;
                        break;
                    case "Medium":
                        Handler.InitTime = 10;
                        Handler.StartDifficulty = 2;
                        break;
                    case "Hard":
                        Handler.InitTime = 5;
                        Handler.StartDifficulty = 3;
                        break;
                    default:
                        break;
                }
            }
        }

        private void TxtBox_HighestResult_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs arg)
        {
            while (ExceptionManager.Control.AdvancedInputCheck(TxtBox_HighestResult.Text) <= 10 && ExceptionManager.Control.AdvancedInputCheck(TxtBox_HighestResult.Text) >= 1000)
            {
                Console.WriteLine("\nHighest Result reached");
            }
        }

        private void textboxTxtInLimitExercises_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs arg)
        {
            Handler.MaxExercises = ExceptionManager.Control.AdvancedInputCheck(textboxTxtInLimitExercises.Text);
        }

        private void ConfigTimeLimit_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs arg)
        {
            Handler.MaxTime = ExceptionManager.Control.AdvancedInputCheck(textboxTxtInLimitExercises.Text);
        }

        private void TxtBox_HighestResult_Initialized(object sender, EventArgs e)
        {
            TxtBox_HighestResult.Text = "50";

            Handler.MaxExerciseResult = ExceptionManager.Control.AdvancedInputCheck(TxtBox_HighestResult.Text);
        }

        private void ConfigMainWindow_Closed(object sender, EventArgs e)
        {
            App.Current.Shutdown();
        }

        private void ConfigMainWindow_StateChanged(object sender, EventArgs e)
        {
            if (!(Handler.UsingAddition || Handler.UsingSubtraction || Handler.UsingMultiplication || Handler.UsingDivision))
            {
                this.ButtonStart.IsEnabled = false;
            }
            else
            {
                this.ButtonStart.IsEnabled = true;
            }
        }

        private void ConfigExtrasChkBoxAllowRetry_Checked(object sender, RoutedEventArgs e)
        {
            Handler.AllowRetry = true;
        }

        private void SaveConfig_Clicked(object sender, RoutedEventArgs e)
        {
            WriteConfig(configFileName);
        }

        private void BtnResetConfig_Click(object sender, RoutedEventArgs e)
        {
            RestoreDefaultSettings();
        }

        private void ReadConfig(string filename)
        {
            try
            {
                // Make sure the file exists
                if (File.Exists(filename) == true)
                {
                    // Load XML Document
                    var xDoc = XDocument.Load(filename);

                    // Loop trough Elements for every XML Attribute
                    foreach (var userxml in xDoc.Root.Elements("Setting"))
                    {
                        Handler.UsingAddition = (bool)userxml.Attribute("UsingAddition");
                        Handler.UsingSubtraction = (bool)userxml.Attribute("UsingSubtraction");
                        Handler.UsingMultiplication = (bool)userxml.Attribute("UsingMultiplication");
                        Handler.UsingDivision = (bool)userxml.Attribute("UsingDivision");
                        Handler.MaxExerciseResult = (int)userxml.Attribute("Spectrum");
                        Handler.StartDifficulty = (int)userxml.Attribute("StartDifficulty");

                    }
                    foreach (var userxml2 in xDoc.Root.Elements("OptionalSettings"))
                    {
                        Handler.MaxTime = (int)userxml2.Attribute("TimeLimit");
                        Handler.MaxExercises = (int)userxml2.Attribute("ExerciseLimit");
                        Handler.AllowRetry = (bool)userxml2.Attribute("AllowRetry");
                    }
                }
            }
            catch (System.Xml.XmlException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void WriteConfig(string filename)
        {
            try
            {
                // Build our XML Structure
                var xDoc = new XDocument(
                            new XElement("Settings",
                            new XElement("Setting",
                                new XAttribute("UsingAddition", Handler.UsingAddition),
                                new XAttribute("UsingSubtraction", Handler.UsingSubtraction),
                                new XAttribute("UsingMultiplication", Handler.UsingMultiplication),
                                new XAttribute("UsingDivision", Handler.UsingDivision),
                                new XAttribute("Spectrum", Handler.MaxExerciseResult),
                                new XAttribute("StartDifficulty", Handler.StartDifficulty)),
                            new XElement("OptionalSettings",
                                new XAttribute("TimeLimit", Handler.MaxTime),
                                new XAttribute("ExerciseLimit", Handler.MaxExercises),
                                new XAttribute("AllowRetry", Handler.AllowRetry)
                                )
                            )
                        );

                // Save to XML Document in current directory
                xDoc.Save(filename);
            }
            catch (System.Xml.XmlException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void ApplyChanges()
        {
            if (Handler.UsingAddition)
                ConfigCalcTChkBoxAddition.IsChecked = true;
            if (Handler.UsingSubtraction)
                ConfigCalcTChkBoxSubtraction.IsChecked = true;
            if (Handler.UsingMultiplication)
                ConfigCalcTChkBoxMultiplication.IsChecked = true;
            if (Handler.UsingDivision)
                ConfigCalcTChkBoxDivision.IsChecked = true;
            if (Handler.AllowRetry)
                ConfigExtrasChkBoxAllowRetry.IsChecked = true;

            if (Handler.MaxExerciseResult != 0)
                TxtBox_HighestResult.Text = Handler.MaxExerciseResult.ToString();
            if (Handler.MaxExercises != 0)
            {
                ConfigExtrasChkBoxLimitExersices.IsChecked = false;
                textboxTxtInLimitExercises.Text = Handler.MaxExercises.ToString();
            }
            if(Handler.MaxTime != 0)
            {
                ConfigExtrasChkBoxTimelimit.IsChecked = true;
                ConfigTimeLimit.Text = Handler.MaxTime.ToString();
            }

            if (Handler.StartDifficulty == 1)
                ConfigDifficultyRdBtnEasy.IsChecked = true;
            if (Handler.StartDifficulty == 2)
                ConfigDifficultyRdBtnMedium.IsChecked = true;
            if (Handler.StartDifficulty == 3)
                ConfigDifficultyRdBtnHard.IsChecked = true;
        }

        private void RestoreDefaultSettings()
        {
            ConfigCalcTChkBoxAddition.IsChecked = true;
            Handler.UsingAddition = true;

            ConfigCalcTChkBoxSubtraction.IsChecked = false;
            Handler.UsingSubtraction = false;

            ConfigCalcTChkBoxMultiplication.IsChecked = false;
            Handler.UsingMultiplication = false;

            ConfigCalcTChkBoxDivision.IsChecked = false;
            Handler.UsingDivision = false;

            ConfigExtrasChkBoxAllowRetry.IsChecked = false;
            Handler.AllowRetry = false;

            if (TxtBox_HighestResult.Text == "" || TxtBox_HighestResult.Text != "")
            {
                Handler.MaxExerciseResult = 10;
                TxtBox_HighestResult.Text = "10";
                
            }
            
            ConfigExtrasChkBoxTimelimit.IsChecked = false;
            ConfigTimeLimit.Text = "";
            Handler.MaxTime = 0;

            ConfigExtrasChkBoxLimitExersices.IsChecked = false;
            TxtBox_HighestResult.Text = "";
            Handler.MaxExercises = 0;

            if (Handler.StartDifficulty != 1)
                Handler.StartDifficulty = 1;

            RadioButton radioButton;

            if (Handler.StartDifficulty == 1)
                ConfigDifficultyRdBtnEasy.IsChecked = true;
        }
    }
}
