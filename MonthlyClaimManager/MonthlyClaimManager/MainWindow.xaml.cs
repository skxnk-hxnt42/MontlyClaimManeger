using Microsoft.Win32;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MonthlyClaimManager
{
    public partial class MainWindow : Window
    {
        private List<Claim> _claims = new List<Claim>(); // Simulating database of claims

        public MainWindow()
        {
            InitializeComponent();
            // Load initial data for pending claims
            LoadPendingClaims();
        }

        // Event for file upload (dummy for now)
        private void UploadDocument_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                MessageBox.Show("File Uploaded: " + openFileDialog.FileName);
            }
        }

        // Submit Claim Button
        private void SubmitClaim_Click(object sender, RoutedEventArgs e)
        {
            int lecturerID = int.Parse(LecturerIDTextBox.Text);
            string lecturerName = LecturerNameTextBox.Text;
            int hoursWorked = int.Parse(HoursWorkedTextBox.Text);
            decimal hourlyRate = decimal.Parse(HourlyRateTextBox.Text);

            // Calculate claim amount
            decimal claimAmount = hoursWorked * hourlyRate;

            // Create new claim object and add to the list (this would be saved to a database in real scenario)
            var claim = new Claim
            {
                LecturerID = lecturerID,
                LecturerName = lecturerName,
                HoursWorked = hoursWorked,
                ClaimAmount = claimAmount,
                ClaimStatus = "Pending"
            };

            _claims.Add(claim);
            MessageBox.Show("Claim Submitted!");

            // Refresh lecturer claims grid
            LecturerClaimsDataGrid.ItemsSource = null;
            LecturerClaimsDataGrid.ItemsSource = _claims;
        }

        // Load pending claims for coordinator/manager
        private void LoadPendingClaims()
        {
            // Normally, this data would be fetched from the database
            PendingClaimsListBox.ItemsSource = _claims.FindAll(c => c.ClaimStatus == "Pending");
        }

        // When a pending claim is selected, show details
        private void PendingClaimsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedClaim = PendingClaimsListBox.SelectedItem as Claim;
            if (selectedClaim != null)
            {
                ClaimLecturerIDTextBox.Text = selectedClaim.LecturerID.ToString();
                ClaimLecturerNameTextBox.Text = selectedClaim.LecturerName;
                ClaimHoursWorkedTextBox.Text = selectedClaim.HoursWorked.ToString();
                ClaimAmountTextBox.Text = selectedClaim.ClaimAmount.ToString("C");
                ClaimStatusTextBox.Text = selectedClaim.ClaimStatus;
            }
        }

        // Approve Claim
        private void ApproveClaim_Click(object sender, RoutedEventArgs e)
        {
            var selectedClaim = PendingClaimsListBox.SelectedItem as Claim;
            if (selectedClaim != null)
            {
                selectedClaim.ClaimStatus = "Approved";
                MessageBox.Show("Claim Approved!");
                LoadPendingClaims(); // Refresh list
            }
        }

        // Reject Claim
        private void RejectClaim_Click(object sender, RoutedEventArgs e)
        {
            var selectedClaim = PendingClaimsListBox.SelectedItem as Claim;
            if (selectedClaim != null)
            {
                selectedClaim.ClaimStatus = "Rejected";
                MessageBox.Show("Claim Rejected!");
                LoadPendingClaims(); // Refresh list
            }
        }
    }

    // Simple Claim class to represent claim data
    public class Claim
    {
        public int LecturerID { get; set; }
        public string LecturerName { get; set; }
        public int HoursWorked { get; set; }
        public decimal ClaimAmount { get; set; }
        public string ClaimStatus { get; set; }
    }
}