using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityTestAssistant.Library.Models.Profile
{
    public interface IKnowledge
    {
        string KnowledgeID { get; }
        string Knowledge { get; }
    }
}
