using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIGateway.Models
{
    public record WorkReport(
    int WorkId,
    string StudentId,
    string AssignmentId,
    bool IsPlagiat,
    DateTime SubmittedAt
    );
}