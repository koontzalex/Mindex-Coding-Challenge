using System;

namespace CodeChallenge.Models
{
    public class ReportingStructure
    {
        public String ReportingStructureId { get; set; }
        public virtual Employee Employee { get; set; }
        public int NumberOfReports { get; set; }
    }
}
