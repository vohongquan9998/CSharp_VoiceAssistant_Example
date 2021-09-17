using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AssistantBotV1
{
    class Core
    {
        public string time()
        {
            return DateTime.Now.ToString(@"hh") + " hours and " + DateTime.Now.ToString(@"mm") + " minute";
        }
        public string day()
        {
            return DateTime.Now.ToString(@"dd MM yyyy");
        }
        public void showCommands(TextBox text,string[] grammarFile)
        {
            text.Visible = true;
            for (int i = 0; i < grammarFile.Length; i++)
            {

                text.Text += grammarFile[i];
                text.Text += Environment.NewLine;
            }
        }
    }
}
