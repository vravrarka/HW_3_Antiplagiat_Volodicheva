namespace APIGateway.Models {
    public class WorkSubmitRequest
    {
        public IFormFile File { get; set; }
        public string StudentId { get; set; }
        public string AssignmentId { get; set; }
    }
}