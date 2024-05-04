using CS451_Team_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CS451_Team_Project.Pages
{
    public class DashboardModel : PageModel
    {
        [FromQuery]
        [FromRoute]
        public string email { get; set; }
        [FromQuery]
        [FromRoute]
        public string token { get; set; }


        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<NewLoginModel> _logger;

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DashboardModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<NewLoginModel> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult OnGet([FromServices] AppDbContext db, [FromQuery] string token, [FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                // Handle the case where email or token is missing in the URL query parameters
                // You may want to redirect to an error page or show a message
                return RedirectToPage("/Error");
            }

            string decryptedEmail = EncryptionHelper.Decrypt(email);

            // Validate the token
            if (ValidateTokenForDashboard(decryptedEmail, token))
            {
                // Token is valid, proceed with your logic

                // For demonstration, let's assume we're fetching the user from the database
                var user = db.Users.FirstOrDefault(u => u.Email == decryptedEmail);

                if (user != null)
                {
                    // Proceed with your logic using the user and token
                    // For example, update the user's last login time, or perform any other action
                    FirstName = user.FirstName; // Assuming these properties exist in your user model
                    LastName = user.LastName;

                    return Page(); // Return the Dashboard page
                }
                else
                {
                    // Handle the case where the user is not found in the database
                    return RedirectToPage("/Error");
                }
            }
            else
            {
                // Token is invalid, handle accordingly
                // You may want to redirect to an error page or show a message
                return RedirectToPage("/Error");
            }
        }

        private bool ValidateTokenForDashboard(string email, string token)
        {
            // Validate the token
            // let's assume the token is valid if it's not null or empty
            return !string.IsNullOrEmpty(token);
        }
    }
}
