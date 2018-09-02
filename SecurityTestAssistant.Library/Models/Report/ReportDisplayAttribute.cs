using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityTestAssistant.Library.Models.Report
{
    public class ReportDisplayAttribute : Attribute
    {
        public ReportDisplayAttribute(string value)
        {
            this.Value = value;
        }
        public ReportDisplayAttribute() { }


        public string Value { get; set; }
    }
}
