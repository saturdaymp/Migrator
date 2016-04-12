using System;
using System.Windows.Forms;

namespace MigratorGUI
{
    /// <summary>
    /// Code borrowed from:
    /// http://www.reflectionit.nl/Articles/InputBox.aspx
    /// </summary>
    public partial class InputBox : Form
    {
        /// <summary>
        /// Delegate used to validate the object
        /// </summary>
        private InputBoxValidatingHandler _validator;

        private InputBox()
        {
            // Required for Windows Form Designer support
            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, System.EventArgs e)
        {
            this.Validator = null;
            this.Close();
        }

        private void buttonOK_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Displays a prompt in a dialog box, waits for the user to input text or click a button.
        /// </summary>
        /// <param name="prompt">String expression displayed as the message in the dialog box</param>
        /// <param name="title">String expression displayed in the title bar of the dialog box</param>
        /// <param name="defaultResponse">String expression displayed in the text box as the default response</param>
        /// <param name="validator">Delegate used to validate the text</param>
        /// <param name="xpos">Numeric expression that specifies the distance of the left edge of the dialog box from the left edge of the screen.</param>
        /// <param name="ypos">Numeric expression that specifies the distance of the upper edge of the dialog box from the top of the screen</param>
        /// <returns>An InputBoxResult object with the Text and the OK property set to true when OK was clicked.</returns>
        public static InputBoxResult Show(string prompt, string title, string defaultResponse, InputBoxValidatingHandler validator, int xpos, int ypos)
        {
            using (InputBox form = new InputBox())
            {
                form.labelPrompt.Text = prompt;
                form.Text = title;
                form.textBoxText.Text = defaultResponse;
                if (xpos >= 0 && ypos >= 0)
                {
                    form.StartPosition = FormStartPosition.Manual;
                    form.Left = xpos;
                    form.Top = ypos;
                }
                form.Validator = validator;

                DialogResult result = form.ShowDialog();

                InputBoxResult retval = new InputBoxResult();
                if (result == DialogResult.OK)
                {
                    retval.Text = form.textBoxText.Text;
                    retval.OK = true;
                }
                return retval;
            }
        }

        /// <summary>
        /// Displays a prompt in a dialog box, waits for the user to input text or click a button.
        /// </summary>
        /// <param name="prompt">String expression displayed as the message in the dialog box</param>
        /// <param name="title">String expression displayed in the title bar of the dialog box</param>
        /// <param name="defaultResponse">String expression displayed in the text box as the default response</param>
        /// <param name="validator">Delegate used to validate the text</param>
        /// <returns>An InputBoxResult object with the Text and the OK property set to true when OK was clicked.</returns>
        public static InputBoxResult Show(string prompt, string title, string defaultText, InputBoxValidatingHandler validator)
        {
            return Show(prompt, title, defaultText, validator, -1, -1);
        }


        /// <summary>
        /// Reset the ErrorProvider
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxText_TextChanged(object sender, System.EventArgs e)
        {
            errorProviderText.SetError(textBoxText, "");
        }

        /// <summary>
        /// Validate the Text using the Validator
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxText_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Validator != null)
            {
                InputBoxValidatingArgs args = new InputBoxValidatingArgs();
                args.Text = textBoxText.Text;
                Validator(this, args);
                if (args.Cancel)
                {
                    e.Cancel = true;
                    errorProviderText.SetError(textBoxText, args.Message);
                }
            }
        }

        protected InputBoxValidatingHandler Validator
        {
            get
            {
                return (this._validator);
            }
            set
            {
                this._validator = value;
            }
        }


        /// <summary>
        /// Class used to store the result of an InputBox.Show message.
        /// </summary>
        public class InputBoxResult
        {
            public bool OK;
            public string Text;
        }

        /// <summary>
        /// EventArgs used to Validate an InputBox
        /// </summary>
        public class InputBoxValidatingArgs : EventArgs
        {
            public string Text;
            public string Message;
            public bool Cancel;
        }

        /// <summary>
        /// Delegate used to Validate an InputBox
        /// </summary>
        public delegate void InputBoxValidatingHandler(object sender, InputBoxValidatingArgs e);

    }
}
