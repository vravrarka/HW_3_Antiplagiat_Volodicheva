using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAnalysisService.Models
{
    public record AnalysisRequest(
    int WorkId,
    string StudentId,
    string AssignmentId,
    int FileId,
    string FileHash
    );
}