using SecurityTestAssistant.Library.Models.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityTestAssistant.Library.Models.Events
{
    public class QuestionAskedEventAgrs : EventArgs
    {
        public QuestionAskedEventAgrs(IQuestion question)
        {
            this.Question = question;
        }

        public IQuestion Question { get; private set; }
    }
}
