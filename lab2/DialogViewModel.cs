using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace lab2
{
    public class DialogViewModel : ViewModelBase
    {
        public bool isFinal = false;

        private string _question;
        public string Question
        {
            get => _question;
            set
            {
                _question = value;
                OnPropertyChanged(nameof(Question));
            }
        }

        private string _firstOption;
        public string FirstOption
        {
            get => _firstOption;
            set
            {
                _firstOption = value;
                OnPropertyChanged(nameof(FirstOption));
            }
        }

        private string _secondOption;
        public string SecondOption
        {
            get => _secondOption;
            set
            {
                _secondOption = value;
                OnPropertyChanged(nameof(SecondOption));
            }
        }

        private string _thirdOption;
        public string ThirdOption
        {
            get => _thirdOption;
            set
            {
                _thirdOption = value;
                OnPropertyChanged(nameof(ThirdOption));
            }
        }

        private bool _isFirstOptionChecked;
        public bool IsFirstOptionChecked
        {
            get => _isFirstOptionChecked;
            set
            {
                _isFirstOptionChecked = value;
                OnPropertyChanged(nameof(IsFirstOptionChecked));
            }
        }

        private Visibility _firstOptionVisibility;
        public Visibility FirstOptionVisibility
        {
            get => _firstOptionVisibility;
            set
            {
                _firstOptionVisibility = value;
                OnPropertyChanged(nameof(FirstOptionVisibility));
            }
        }

        private bool _isSecondOptionChecked;
        public bool IsSecondOptionChecked
        {
            get => _isSecondOptionChecked;
            set
            {
                _isSecondOptionChecked = value;
                OnPropertyChanged(nameof(IsSecondOptionChecked));
            }
        }

        private Visibility _secondOptionVisibility;
        public Visibility SecondOptionVisibility
        {
            get => _secondOptionVisibility;
            set
            {
                _secondOptionVisibility = value;
                OnPropertyChanged(nameof(SecondOptionVisibility));
            }
        }

        private bool _isThirdOptionChecked;
        public bool IsThirdOptionChecked
        {
            get => _isThirdOptionChecked;
            set
            {
                _isThirdOptionChecked = value;
                OnPropertyChanged(nameof(IsThirdOptionChecked));
            }
        }

        private Visibility _thirdOptionVisibility;
        public Visibility ThirdOptionVisibility
        {
            get => _thirdOptionVisibility;
            set
            {
                _thirdOptionVisibility = value;
                OnPropertyChanged(nameof(ThirdOptionVisibility));
            }
        }

        private string _inputText;
        public string InputText
        {
            get => _inputText;
            set
            {
                _inputText = value;
                OnPropertyChanged(nameof(InputText));
            }
        }

        private Visibility _inputTextVisibility;
        public Visibility InputTextVisibility
        {
            get => _inputTextVisibility;
            set
            {
                _inputTextVisibility = value;
                OnPropertyChanged(nameof(InputTextVisibility));
            }
        }

        public DelegateCommand ConfirmButtonPressedCommand { get; private set; }

        public DialogViewModel()
        {
            FirstOptionVisibility = Visibility.Hidden;
            SecondOptionVisibility = Visibility.Hidden;
            ThirdOptionVisibility = Visibility.Hidden;

            ConfirmButtonPressedCommand = new DelegateCommand((parameter) => OnConfirmButtonPressed());
        }

        private void OnConfirmButtonPressed()
        {
            if (isFinal)
            {
                Environment.Exit(0);
            }
            var choice = new object[2];
            if (IsFirstOptionChecked)
                choice[0] = 1;
            else if (IsSecondOptionChecked)
                choice[0] = 2;
            else if (IsThirdOptionChecked)
                choice[0] = 3;
            else if (!string.IsNullOrWhiteSpace(InputText))
            {
                choice[0] = 0;
                choice[1] = InputText;
            }
              
            Mediator.Instance.Send("CurrentPositionUpdated", choice);
        }
    }
}
