using EmailOtp.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmailOtp.Controllers
{
    public class EmailOtpController : Controller
    {
        private readonly EmailOtpService _otpService;

        public EmailOtpController(EmailOtpService otpService)
        {
            _otpService = otpService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GenerateOtp(string email)
        {
            var status = _otpService.generate_OTP_Email(email);
            ViewBag.Status = status;
            return View("Index");
        }

        [HttpPost]
        public IActionResult ValidateOtp(string otp)
        {
            var status = _otpService.check_OTP(otp);
            ViewBag.Status = status;
            return View("Index");
        }
    }
}
