namespace Claiming_System.Models
{
    public class Claim
    {
        public int Id { get; set; }
        public string? LecturerName { get; set; } = string.Empty;
        public string? AdditionalNotes { get; set; } = string.Empty;
        public string LecturerDepartment { get; set; } = string.Empty;
        public string Month { get; set; } = string.Empty;
        public string? SupportingDocument { get; set; }
        public decimal HoursWorked { get; set; }
        public decimal HourlyRate { get; set; }
        public decimal TotalClaim { get; set; }
        public string Status { get; set; } = string.Empty;
    }

}
