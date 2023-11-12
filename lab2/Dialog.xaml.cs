using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace lab2
{
    /// <summary>
    /// Interaction logic for Dialog.xaml
    /// </summary>
    public partial class Dialog : Window, IDialog
    {
        public Dialog()
        {
            DataContext = new DialogViewModel();
            InitializeComponent();

            Mediator.Instance.Register("CloseDialog", (parameter) => Close());
        }

        public short OpenDialog(string question, string firstOption, string secondOption, bool isTextBoxActivated = false, string thirdOption = "", bool isFinal = false)
        {
            var viewModel = (DialogViewModel)DataContext;
            viewModel.Question = question;
            viewModel.isFinal = isFinal;
            if (!isTextBoxActivated && !string.IsNullOrWhiteSpace(firstOption))
            {
                viewModel.FirstOptionVisibility = Visibility.Visible;
                viewModel.SecondOptionVisibility = Visibility.Visible;
                viewModel.FirstOption = firstOption;
                viewModel.SecondOption = secondOption;
                if (!string.IsNullOrWhiteSpace(thirdOption))
                {
                    viewModel.ThirdOption = thirdOption;
                    viewModel.ThirdOptionVisibility = Visibility.Visible;
                }
            }
            viewModel.InputTextVisibility = isTextBoxActivated ? Visibility.Visible : Visibility.Hidden;

            ShowInTaskbar = false;
            Show();
            if (viewModel.IsFirstOptionChecked)
            {
                return 1;
            }
            else if (viewModel.IsSecondOptionChecked)
            {
                return 2;
            }
            else if (viewModel.IsThirdOptionChecked)
            {
                return 3;
            }
            else
            {
                return 0;
            }
        }
    }
}
