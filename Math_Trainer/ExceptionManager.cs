using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathTrainerVC19
{
    class ExceptionManager
    {
        
        public class Control : System.Windows.Controls.Label
        {
            protected override void OnPreviewTextInput(System.Windows.Input.TextCompositionEventArgs e)
            {
                
            }

            //System.Collections.Generic.List<string> ErrorText = new System.Collections.Generic.List<string>();
            //ErrorText.Add("Error: Only numerical input is allowed");

            public static string[] ErrorArgument = new string[]
            {
                "Error: Only numerical input is allowed",
                "Exeption thrown in " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Invalid input.\nResetting variable...",
                "Warning: Input value of MAX_RESULT_PER_EXERCISE is 0"
            };

            public static int AdvancedInputCheck(string input)
            {
                int tData = 0;
                try
                {
                    bool convertSuccess = int.TryParse(input, out tData);
                    if (!convertSuccess)
                        throw new FormatException();
                    if (tData == 0)
                        throw new NotFiniteNumberException();
                }
                catch (FormatException)
                {
                    Console.WriteLine(ExceptionManager.Control.ErrorArgument[1] + "\nValue of tData: " + tData);
                   // mWindow.ErrorTextLabelSet(ExceptionManager.Control.ErrorArgument[0]);
                   // mWindow.SetStartButtonState(false);
                    //tData = 0;
                }
                catch (NotFiniteNumberException)
                {
                    Console.WriteLine(ExceptionManager.Control.ErrorArgument[1] + "\nValue of tData: " + tData);
                }

                return tData;
            }
        }
    }
}
