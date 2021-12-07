using System;

namespace MathTrainerVC19
{
    class Handler
    {

        // Settings
        private static bool AdditionFlag;
        private static bool SubtractionFlag;
        private static bool MultiplicationFlag;
        private static bool DivisionFlag;
        private static bool tAllowRetry;
        private static int tInitTime;
        private static int tStartDifficulty;
        private static int tMaxExerciseResult;
        private static int tMaxExercises;

        // Results
        private static int tScore;
        private static int tCorrectAnswers;
        private static int tFalseAnswers;
        private static int tMaxTimeTaken;

        // Trainer Data
        private static int tResult;
        private static int tInput;
        private static int tNumExercises;
        private static TimeSpan passed_time;


        /// GETTERS and SETTERS
        public static bool UsingAddition { get => AdditionFlag; set => AdditionFlag = value; }

        public static bool UsingSubtraction { get => SubtractionFlag; set => SubtractionFlag = value; }

        public static bool UsingMultiplication { get => MultiplicationFlag; set => MultiplicationFlag = value; }

        public static bool UsingDivision { get => DivisionFlag; set => DivisionFlag = value; }

        public static bool AllowRetry { get => tAllowRetry; set => tAllowRetry = value; }

        public static int MaxExerciseResult { get => tMaxExerciseResult; set => tMaxExerciseResult = value; }

        public static int MaxExercises { get => tMaxExercises; set => tMaxExercises = value; }

        public static int NumExercises { get => tNumExercises; set => tNumExercises = value; }

        public static int Result { get => tResult; set => tResult = value; }

        public static int Input { get => tInput; set => tInput = value; }

        public static int Score { get => tScore; set => tScore = value; }

        public static int CorrectAnswers { get => tCorrectAnswers; set => tCorrectAnswers = value; }

        public static int FalseAnswers { get => tFalseAnswers; set => tFalseAnswers = value; }

        public static int InitTime { get => tInitTime; set => tInitTime = value; }

        public static TimeSpan PassedTime { get => passed_time; set => passed_time = value; }

        public static int MaxTime { get => tMaxTimeTaken; set => tMaxTimeTaken = value; }
        
        public static int StartDifficulty { get => tStartDifficulty; set => tStartDifficulty = value; }
    }
}