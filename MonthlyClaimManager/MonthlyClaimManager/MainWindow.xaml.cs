using Microsoft.Win32;
using System.IO;
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
            openFileDialog.Filter = "PDF Files|*.pdf|Word Documents|*.docx|Excel Sheets|*.xlsx";
            openFileDialog.Title = "Select a Supporting Document";

            if (openFileDialog.ShowDialog() == true)
            {
                var fileInfo = new FileInfo(openFileDialog.FileName);

                if (fileInfo.Length > 5 * 1024 * 1024) // 5 MB size limit
                {
                    MessageBox.Show("File size should be less than 5MB.");
                    return;
                }

                MessageBox.Show("File Uploaded: " + fileInfo.Name);
                // Store the file path or handle file processing here
            }
        }


        // Submit Claim Button
        private void SubmitClaim_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(LecturerIDTextBox.Text) || string.IsNullOrWhiteSpace(LecturerNameTextBox.Text) ||
                    string.IsNullOrWhiteSpace(HoursWorkedTextBox.Text) || string.IsNullOrWhiteSpace(HourlyRateTextBox.Text))
                {
                    throw new InvalidOperationException("All fields are required.");
                }

                int lecturerID = int.Parse(LecturerIDTextBox.Text);
                string lecturerName = LecturerNameTextBox.Text;
                int hoursWorked = int.Parse(HoursWorkedTextBox.Text);
                decimal hourlyRate = decimal.Parse(HourlyRateTextBox.Text);

                // Calculate claim amount
                decimal claimAmount = hoursWorked * hourlyRate;

                // Create new claim object and add to the list
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

                LoadPendingClaims();
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter valid numeric values for Hours Worked and Hourly Rate.");
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }


        // Load pending claims for coordinator/manager
        private void LoadPendingClaims()
        {
            PendingClaimsListBox.ItemsSource = null;
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