using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAnalysisService.Models
{
    public class WorkSubmission
    {
        public int WorkId { get; set; }
        public string StudentId { get; set; } = default!;
        public string AssignmentId { get; set; } = default!;
        public int FileId { get; set; }
        public string FileHash { get; set; } = default!;
        public bool IsPlagiat { get; set; }
        public DateTime SubmittedAt { get; set; }
    }
}