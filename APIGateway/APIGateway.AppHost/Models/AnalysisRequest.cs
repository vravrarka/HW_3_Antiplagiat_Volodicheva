using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIGateway.Models
{
    public record AnalysisRequest(
    string StudentId,
    string AssignmentId,
    int FileId,
    string FileHash
    );
}