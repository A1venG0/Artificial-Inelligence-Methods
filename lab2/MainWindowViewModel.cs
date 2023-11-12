using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace lab2
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string currentPosition;

        public DelegateCommand StartButtonPressedCommand { get; private set; }
        public DelegateCommand ExitButtonPressedCommand { get; private set; }

        public MainWindowViewModel()
        {
            StartButtonPressedCommand = new DelegateCommand((parameter) => UpdateDialog());
            ExitButtonPressedCommand = new DelegateCommand((parameter) => Environment.Exit(0));
            currentPosition = "";
            Mediator.Instance.Register("CurrentPositionUpdated", (parameter) => OnCurrentPositionChanged(parameter));
        }

        private void UpdateDialog()
        {
            string question;
            string firstOption;
            string secondOption;
            string thirdOption;
            bool isTextBoxActivated;
            bool isFinal;
            if (!string.IsNullOrWhiteSpace(currentPosition))
            {
                Mediator.Instance.Send("CloseDialog");
            }
            GetQuestion(out question, out firstOption, out secondOption, out thirdOption, out isTextBoxActivated, out isFinal);
            DialogLogic.OpenDialog(question, firstOption, secondOption, isTextBoxActivated, thirdOption, isFinal);
        }

        private void OnCurrentPositionChanged(object parameter)
        {
            object[] args = (object[])parameter;
            if (Convert.ToInt32(args[0]) == 0) // input text
            {
                var result = Convert.ToDouble(args[1]);
                if (string.IsNullOrWhiteSpace(currentPosition)) // temperature
                {
                    if (result < 37.5)
                        currentPosition += "1";
                    else
                        currentPosition += "2";
                }
                else // symptoms duration
                {
                    if (result < 3)
                        currentPosition += "1";
                    else if (result >= 3 && result <= 7)
                        currentPosition += "2";
                    else
                        currentPosition += "3";
                }
                UpdateDialog();
            }
            else if (Convert.ToInt32(args[0]) == -1) // invalid state
            {
                MessageBox.Show("An error has occured. Please select a valid option", "Expert system", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else // options selected
            {
                currentPosition += args[0].ToString();
                UpdateDialog();
            }
            
        }

        private void GetQuestion(out string question, out string firstOption, out string secondOption, out string thirdOption, out bool isTextBoxActivated, out bool isFinal)
        {
            if (string.IsNullOrWhiteSpace(currentPosition))
            {
                question = "What's your body temperature?";
                firstOption = "";
                secondOption = "";
                thirdOption = "";
                isTextBoxActivated = true;
                isFinal = false;
            }
            else if (currentPosition == "1")
            {
                question = "Are you coughing?";
                firstOption = "Yes";
                secondOption = "No";
                thirdOption = "";
                isTextBoxActivated = false;
                isFinal = false;
            }
            else if (currentPosition == "2")
            {
                question = "Do you have body aches?";
                firstOption = "Yes";
                secondOption = "No";
                thirdOption = "";
                isTextBoxActivated = false;
                isFinal = false;
            }
            else if (currentPosition == "11")
            {
                question = "What's the cough type?";
                firstOption = "Dry";
                secondOption = "Productive";
                thirdOption = "";
                isTextBoxActivated = false;
                isFinal = false;
            }
            else if (currentPosition == "12")
            {
                question = "Do you have sore throat?";
                firstOption = "Yes";
                secondOption = "No";
                thirdOption = "";
                isTextBoxActivated = false;
                isFinal = false;
            }
            else if (currentPosition == "21" || currentPosition == "122")
            {
                question = "How would you describe your fatigue level?";
                firstOption = "Mild";
                secondOption = "Moderate";
                thirdOption = "Severe";
                isTextBoxActivated = false;
                isFinal = false;
            }
            else if (currentPosition == "22")
            {
                question = "What's the duration of your symptoms in days?";
                isTextBoxActivated = true;
                firstOption = "";
                secondOption = "";
                thirdOption = "";
                isFinal = false;
            }
            else if (currentPosition == "221" || currentPosition == "223")
            {
                question = "Do you have a congestion?";
                firstOption = "Yes";
                secondOption = "No";
                thirdOption = "";
                isTextBoxActivated = false;
                isFinal = false;
            }
            else if (currentPosition == "111")
            {
                question = "You're likely to have a viral infection";
                firstOption = "";
                secondOption = "";
                thirdOption = "";
                isTextBoxActivated = false;
                isFinal = true;
            }
            else if (currentPosition == "112")
            {
                question = "You're likely to have a bronchitis";
                firstOption = "";
                secondOption = "";
                thirdOption = "";
                isTextBoxActivated = false;
                isFinal = true;
            }
            else if (currentPosition == "121")
            {
                question = "You're likely to have a common cold";
                firstOption = "";
                secondOption = "";
                thirdOption = "";
                isTextBoxActivated = false;
                isFinal = true;
            }
            else if (currentPosition == "211" || currentPosition == "1221")
            {
                question = "You're likely to have a common cold";
                firstOption = "";
                secondOption = "";
                thirdOption = "";
                isTextBoxActivated = false;
                isFinal = true;
            }
            else if (currentPosition == "212" || currentPosition == "1222")
            {
                question = "You're likely to have a flu";
                firstOption = "";
                secondOption = "";
                thirdOption = "";
                isTextBoxActivated = false;
                isFinal = true;
            }
            else if (currentPosition == "213" || currentPosition == "1223")
            {
                question = "You're likely to have a flu or a viral infection";
                firstOption = "";
                secondOption = "";
                thirdOption = "";
                isTextBoxActivated = false;
                isFinal = true;
            }
            else if (currentPosition == "2211")
            {
                question = "You're likely to have an upper respiratory infection";
                firstOption = "";
                secondOption = "";
                thirdOption = "";
                isTextBoxActivated = false;
                isFinal = true;
            }
            else if (currentPosition == "2232")
            {
                question = "You're likely to have a lower respiratory infection";
                firstOption = "";
                secondOption = "";
                thirdOption = "";
                isTextBoxActivated = false;
                isFinal = true;
            }
            else if (currentPosition == "222")
            {
                question = "You're likely to have a viral infection";
                firstOption = "";
                secondOption = "";
                thirdOption = "";
                isTextBoxActivated = false;
                isFinal = true;
            }
            else
            {
                question = "";
                firstOption = "";
                secondOption = "";
                thirdOption = "";
                isTextBoxActivated = false;
                isFinal = false;
            }
            
        }
    }
}