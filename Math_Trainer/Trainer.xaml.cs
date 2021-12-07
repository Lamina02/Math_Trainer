using System;
using System.Windows;
using System.Windows.Threading;

namespace MathTrainerVC19
{

    public partial class Trainer : Window
    {
        private DispatcherTimer _timer;
        private readonly DispatcherTimer _passed_time_timer;
        private TimeSpan _time;
        private TimeSpan _passed_time;
        private readonly Random rnd = new Random();
        private bool setTrainerWindowState;
        public bool SetTrainerWindowState { get => setTrainerWindowState; set => setTrainerWindowState = value; }

        public Trainer()
        {
            InitializeComponent();
            SetTrainerWindowState = false;
            
            /// Timer
            _time = TimeSpan.FromSeconds(Handler.InitTime);
            _passed_time = TimeSpan.FromSeconds(0);

            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                lblTime.Content = _time.ToString("c");
                if (_time == TimeSpan.Zero)
                {
                    _timer.Stop();

                    SetTrainerWindowState = true;
                    this.Close();
                }
                _time = _time.Add(TimeSpan.FromSeconds(-1));
            }, Application.Current.Dispatcher);

            _passed_time_timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                if (_time == TimeSpan.Zero)
                {
                    _passed_time_timer.Stop();
                }
                _passed_time = _passed_time.Add(TimeSpan.FromSeconds(1));
                Handler.PassedTime = _passed_time; // save timer intervall in our "database" so we can access it later
            }, Application.Current.Dispatcher);

            _timer.Start();
            _passed_time_timer.Start();
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            lblTime.Content = DateTime.Now.ToLongTimeString();

        }

        private void BtnShowResults_Click(object sender, RoutedEventArgs e)
        {
            // Switch back to saved training session
            SetTrainerWindowState = true;
            this.Close(); 
        }

        private void BtnNewTraining_Click(object sender, RoutedEventArgs e)
        {
            if(_time != TimeSpan.Zero && _time.TotalSeconds >= 4)
            _time = _time.Add(TimeSpan.FromSeconds(-3));
            lbl_Exercise_Initialized(0, e);
            Result_Input_Box.Clear();
        }

        private void lbl_Exercise_Initialized(object sender, EventArgs e)
        {

            string exercise_caption = string.Empty;
            int exerciseResultPerSide = (Handler.MaxExerciseResult / 2);
            int left_operand = rnd.Next(1, exerciseResultPerSide);
            int right_operand = rnd.Next(1, exerciseResultPerSide);

            if (Handler.MaxExercises == 0)
                Handler.MaxExercises = 3899492;

            while (left_operand / right_operand <= 0.0)
            {
                left_operand = rnd.Next(1, exerciseResultPerSide);
            }
            while (left_operand < right_operand)
            {
                left_operand = rnd.Next(1, exerciseResultPerSide);
            }

            bool ok = false;
            while (ok != true)
            {
                int target_operand = rnd.Next(0,4);
                switch (target_operand)
                {
                    case 0:
                        if (Handler.UsingAddition)
                        {
                            Handler.Result = left_operand + right_operand;
                            exercise_caption = left_operand.ToString() + " + " + right_operand.ToString();
                            ok = true;
                        }break;
                    case 1:
                        if (Handler.UsingSubtraction)
                        {
                            Handler.Result = left_operand - right_operand;
                            exercise_caption = left_operand.ToString() + " - " + right_operand.ToString();
                            ok = true;
                        }break;
                    case 2:
                        if (Handler.UsingMultiplication)
                        {
                            Handler.Result = left_operand * right_operand;
                            exercise_caption = left_operand.ToString() + " x " + right_operand.ToString();
                            ok = true;
                        }break;
                    case 3:
                        if (Handler.UsingDivision)
                        {
                            Handler.Result = left_operand / right_operand;
                            exercise_caption = left_operand.ToString() + " : " + right_operand.ToString();
                            ok = true;
                        }break;
                    default:
                        {
                        }
                        break;
                }
            }
            lbl_Exercise.Content = exercise_caption;
            Handler.MaxExercises -= 1;
        }

        private void Result_Input_Box_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)  // check ever key
        {
            if(Handler.MaxExercises == Handler.NumExercises)
            {
                if ((Handler.Score >= 10000 || Handler.Score <= 0) && _time == TimeSpan.Zero)
                {
                    _timer.Stop();
                    SetTrainerWindowState = true;
                    this.Close();
                }
            }

            if ( e.Key == System.Windows.Input.Key.Enter)
            {
                if (Handler.Result == Handler.Input) // check if result is right after pressing enter 
                {
                    if (Handler.Score <= 9999 && _time != TimeSpan.Zero)
                    {
                        lblScore.Content = Handler.Score += 1;
                        _time = _time.Add(TimeSpan.FromSeconds(10)); // add 10 sec to timer
                        lbl_Exercise_Initialized(0,e); // new exercise
                        Result_Input_Box.Clear(); // flush input from result text box
                        Handler.CorrectAnswers += 1;
                    }
                }
                else
                {
                    if (Handler.Score >= 1 && (_time.TotalSeconds >= 10 || _time == TimeSpan.Zero))
                    {
                        if(Handler.AllowRetry)
                        {
                            lblScore.Content = Handler.Score -= 1;
                            _time = _time.Add(TimeSpan.FromSeconds(-10)); // take away 10 sec from timer
                            Handler.FalseAnswers += 1;
                        }
                        else
                        {
                            lblScore.Content = Handler.Score -= 1;
                            _time = _time.Add(TimeSpan.FromSeconds(-10)); // take away 10 sec from timer
                            Handler.FalseAnswers += 1;
                            lbl_Exercise_Initialized(0, e); // new exercise
                            Result_Input_Box.Clear(); // flush input from result text box
                        }
                    }
                }
                Handler.NumExercises += 1;
                e.Handled = true;
            }
        }

        private void Result_Input_Box_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            Handler.Input = ExceptionManager.Control.AdvancedInputCheck(Result_Input_Box.Text);
        }

        // not enter
    }
}
