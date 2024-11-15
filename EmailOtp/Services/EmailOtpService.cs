using System.Text.RegularExpressions;

namespace EmailOtp.Services
{
    public class EmailOtpService
    {
        private const string Domain = "dso.org.sg";
        private const int OTPValiditySeconds = 60; // 1 minute
        private const int MaxTries = 10;
        private string? currentOTP;
        private DateTime? otpExpiry;
        private int attempts = 0;


        public void Start()
        {
            // Initialize resources if needed
        }

        public void Close()
        {
            // Clean up resources if needed
        }

        public string generate_OTP_Email(string userEmail)
        {
            if (string.IsNullOrWhiteSpace(userEmail) || !isValidEmail(userEmail))
                return "STATUS_EMAIL_INVALID";

            if (!userEmail.EndsWith($"@{Domain}", StringComparison.OrdinalIgnoreCase))
                return "STATUS_EMAIL_INVALID";

            currentOTP = generateRandomOTP();
            otpExpiry = DateTime.UtcNow.AddSeconds(OTPValiditySeconds);

            string emailBody = $"Your OTP Code is {currentOTP}. The code is valid for 1 minute.";

            // Assume send_email(email_address, email_body) is implemented
            bool emailSent = sendEmail(userEmail, emailBody);
            attempts = 0;
            return emailSent ? "STATUS_EMAIL_OK" : "STATUS_EMAIL_FAIL";

        }

        public string check_OTP(string userInput)
        {
            if (currentOTP == null || otpExpiry == null || DateTime.UtcNow > otpExpiry)
                return "STATUS_OTP_TIMEOUT";

            if (attempts < MaxTries)
            {
                if (DateTime.UtcNow > otpExpiry)
                    return "STATUS_OTP_TIMEOUT";

                if (userInput == currentOTP) {
                    attempts = 0;
                    return "STATUS_OTP_OK";
                }

                attempts++;
            }

            return "STATUS_OTP_FAIL";
        }

        private bool isValidEmail(string email)
        {
            const string emailPattern =
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$"; // Basic email regex
            return Regex.IsMatch(email, emailPattern);
        }

        private string generateRandomOTP()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        private bool sendEmail(string emailAddress, string emailBody)
        {
            // Mocked email function for demonstration
            System.Diagnostics.Debug.WriteLine($"Sending email to {emailAddress}: {emailBody}");
            return true;
        }

    }
}
