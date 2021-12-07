using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MathTrainerVC19
{
    public class NumericTextBox : System.Windows.Controls.TextBox
    {
        private static string pattern = "[0-9]";// 
        private static readonly System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        protected override void OnPreviewTextInput(System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }
//        protected override void OnPreviewKeyDown(System.Windows.Input.KeyEventArgs keyData)
//        {
            //if ()
//                keyData.Handled = true;
//            base.OnPreviewKeyDown(keyData);
//        }


    } // end of class NumericTextBox

    public class RegexTextBox : System.Windows.Controls.TextBox
    {
        public System.Text.RegularExpressions.Regex Regex { get; set; } = null;


        ///////////////////////////////////////////////////////////////////////
        // MEMBERS

        protected override void OnPreviewTextInput(System.Windows.Input.TextCompositionEventArgs e)
        {
            string currentText = this.Text;
            string candidateText = currentText + e.Text;

            // If we have a set regex, and the current text fails,
            // mark as handled so the text is not processed.
            if (Regex != null && !Regex.IsMatch(candidateText))
            {
                e.Handled = true;
            }// ok

            base.OnPreviewTextInput(e);
        }

    } // end of class RegexTextbox
}
