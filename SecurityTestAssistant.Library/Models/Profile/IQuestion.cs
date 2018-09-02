using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityTestAssistant.Library.Models.Profile
{
    public interface IQuestion
    {
        string QuestionText { get; }
        string QuestionID { get; }
        QuestionType QuestionType { get; }
        IList<string> AnswerOptions { get; }
    }
}
