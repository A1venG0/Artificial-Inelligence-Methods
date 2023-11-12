using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    public static class DialogLogic
    {
        public static IDialogFactory dialogFactory;

        public static short OpenDialog(string question, string firstOption, string secondOption, bool isTextBoxActivated = false, string thirdOption = "", bool isFinal = false)
        {
            IDialog dialog = dialogFactory.CreateDialog();
            return dialog.OpenDialog(question, firstOption, secondOption, isTextBoxActivated, thirdOption, isFinal);
        }
    }
}
