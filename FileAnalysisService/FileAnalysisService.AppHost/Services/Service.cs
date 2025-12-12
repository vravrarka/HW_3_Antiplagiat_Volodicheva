using FileAnalysisService.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAnalysisService.Services
{
    public class AnalysisAppService
    {
        private readonly ConcurrentBag<WorkSubmission> _submissions = new();

        public WorkReport Analyze(AnalysisRequest request)
        {
            var now = DateTime.UtcNow;

            //обработка плагиата
            var isPlagiat = _submissions.Any(s =>
                s.FileHash == request.FileHash &&
                s.StudentId != request.StudentId &&
                s.SubmittedAt < now
            );

            var workId = IdGenerator.NextId();

            var submission = new WorkSubmission
            {
                WorkId = workId,
                StudentId = request.StudentId,
                AssignmentId = request.AssignmentId,
                FileId = request.FileId,
                FileHash = request.FileHash,
                IsPlagiat = isPlagiat,
                SubmittedAt = now
            };

            _submissions.Add(submission);

            return new WorkReport(
                submission.WorkId,
                submission.StudentId,
                submission.AssignmentId,
                submission.IsPlagiat,
                submission.SubmittedAt
            );
        }

        public List<WorkReport> GetReportsByAssignment(string assignmentId)
        {
            return _submissions
                .Where(s => s.AssignmentId == assignmentId)
                .Select(s => new WorkReport(
                    s.WorkId,
                    s.StudentId,
                    s.AssignmentId,
                    s.IsPlagiat,
                    s.SubmittedAt
                ))
                .ToList();
        }
    }
}
