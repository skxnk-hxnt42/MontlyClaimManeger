namespace MonthlyClaimManager.Models
{
    public class Claim
    {
        public int LecturerID { get; set; }
        public string LecturerName { get; set; }
        public int HoursWorked { get; set; }
        public decimal HourlyRate { get; set; }
        public decimal ClaimAmount { get; set; }
        public string ClaimStatus { get; set; } = "Pending";
        public string? DocumentPath { get; set; } // Nullable string
                                                  // For the uploaded document
    }
}
