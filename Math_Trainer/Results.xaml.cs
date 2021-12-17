using System;
using System.Threading.Tasks;
using System.Windows;

namespace MathTrainerVC19
{
    /// <summary>
    /// Interaction logic for Results.xaml
    /// </summary>
    public partial class Results : Window
    {
        private bool resultsFlag = false;
        private static int percentage = 0;
        private string resultsFileName = "Results_" + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";

        public bool SetResultsWindowState { get => resultsFlag; set => resultsFlag = value; }

        public Results()
        {
            InitializeComponent();
            //SetResultsWindowState = false;
            ResizeMode = System.Windows.ResizeMode.CanMinimize;
        }

        // Switch to configuration window
        private void BtnNewTraining_Click(object sender, RoutedEventArgs e)
        {
            Handler.Score = 0;
            Handler.CorrectAnswers = 0;
            Handler.FalseAnswers = 0;
            Handler.MaxTime = 0;
            Handler.MaxExercises = 0;

            // Check for null and close current Window
            if (System.Windows.Application.Current.MainWindow != null)
                this.Close();
        }

        private void BtnSaveSettings_Click(object sender, RoutedEventArgs e)
        {

            // Write file using StreamWriter  
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(resultsFileName))
            {
                writer.WriteLine("Results per " + DateTime.Today.ToString("dd.MM.yyyy\n"));
                writer.WriteLine("\nScore: " + Handler.Score);
                writer.WriteLine("\nCorrect Answers: " + Handler.CorrectAnswers);
                writer.WriteLine("\nFalse Answers: " + Handler.FalseAnswers);
                writer.WriteLine("\nTime Taken: "  + Handler.MaxTime);
            }

            // Notify user
            MessageBox.Show("Results saved!");
        }

        private void TimeTakenValue_Initialized(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Handler.MaxTime.ToString())) { TimeTakenValue.Content = Handler.MaxTime; }
        }

        async private void CorrectAnswersValue_Initialized(object sender, EventArgs e)
        {
            if (Handler.NumExercises != 0)
            {
                CorrectAnswersValue.Content = Handler.CorrectAnswers;
                bool handled = await Task.Run(() => ExecuteLongTask()).ConfigureAwait(true);
            }
            else { Handler.CorrectAnswers = 0; }
        }

        async private void FalseAnswersValue_Initialized(object sender, EventArgs e)
        {
            if (Handler.NumExercises != 0)
            {
                FalseAnswersValue.Content = Handler.FalseAnswers;
                bool handled = await Task.Run(() => ExecuteLongTask()).ConfigureAwait(true);
            }
            else { Handler.FalseAnswers = 0; }
        }

        private bool ExecuteLongTask()
        {
            try
            {
                FalseAnswersValue.Content = Handler.FalseAnswers;
                CorrectAnswersValue.Content = Handler.CorrectAnswers;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private void PercentageValue_Initialized(object sender, EventArgs e)
        {
            if (Handler.NumExercises != 0)
            {
                percentage = Handler.CorrectAnswers * 100 / Handler.NumExercises;
                PercentageValue.Content = percentage.ToString() + "%";
            }
        }

        private void lbl_Exercise_Initialized(object sender, EventArgs e)
        {
            string[] FeedbackCaption = new string[]
            {
                "Awesome!\nYou've answered them all!",
                "You rock!\nKeep up the great work!",
                "You've got more than the half correct. Keep on practicing!",
                "Your accurancy is 50%\nKeep on practicing in order to get better",
                "You've got less than the half correct.\nTry to keep on practicing frequently in order to at least get more than 50% correctly.",
                "You are seemingly struggling to answer the exercises correctly.\nI recommend looking over the topic again. You might as well want to give yourself more time for the start and build up from there.",
                "You are struggling a lot to answer the questions.\nTry to begin with an easier maximum results range and some more time. Also look over the theory of the topic once again and maybe do some exercises by hand step-by-step on paper",
                "You have very few or none of the exercises correct. I highly recommend going over the topic again before you come back."
            };

            if (percentage == 100)
            {
                lbl_Exercise.Text = FeedbackCaption[0];
            }
            else if (percentage >= 70)
            {
                lbl_Exercise.Text = FeedbackCaption[1];
            }
            else if (percentage >= 50)
            {
                lbl_Exercise.Text = FeedbackCaption[2];
            }
            else if (percentage == 50)
            {
                lbl_Exercise.Text = FeedbackCaption[3];
            }
            else if (percentage >= 40)
            {
                lbl_Exercise.Text = FeedbackCaption[4];
            }
            else if (percentage >= 26)
            {
                lbl_Exercise.Text = FeedbackCaption[5];
            }
            else if (percentage >= 10)
            {
                lbl_Exercise.Text = FeedbackCaption[6];
            }
            else if (percentage >= 0 || percentage == 0)
            {
                lbl_Exercise.Text = FeedbackCaption[7];
            }
        }

        private void TotalExercisesValue_Initialized(object sender, EventArgs e)
        {
            if (Handler.NumExercises != 0) { TotalExercisesValue.Content = Handler.NumExercises; }
            else { Handler.NumExercises = 0; }
        }
    }
}
