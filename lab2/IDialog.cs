using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace lab2
{
    public interface IDialog
    {
        short OpenDialog(string question, string firstOption, string secondOption, bool isTextBoxActivated = false, string thirdOption = "", bool isFinal = false);
    }
}
