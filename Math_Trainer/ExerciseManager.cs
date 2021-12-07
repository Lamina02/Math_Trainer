using System;

namespace MathTrainerVC19
{
    class ExerciseManager
    {
        public static float left_operand;
        public static float right_operand;
        public static float result;
        protected const int max_exercises = 30000;
        protected const int max_per_operand = 99;
        protected static int target_operand;        


        public static float Generate_Exercise(float left_operand_input, float right_operand_input)
        {
            int t_max_operands = Handler.MaxOperands();
            left_operand = left_operand_input;
            right_operand = right_operand_input;

            Random rnd = new Random();
            target_operand = rnd.Next(1, t_max_operands);
            
            if (left_operand + right_operand >= Handler._MaxExerciseResult)
                return 0;

            switch (target_operand)
            {
                case 1:
                    result = left_operand + right_operand;
                    break;
                case 2:
                    result = left_operand - right_operand;
                    break;
                case 3:
                    result = left_operand * right_operand;
                    break;
                case 4:
                    result = left_operand / right_operand;
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}
