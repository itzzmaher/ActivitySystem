using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ActivitySystem.Models;
using ActivitySystem.Repository;
using DocuSign.eSign.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ActivitySystem.Controllers
{

    public class AccountController : Controller
    {
        UsersRepository UsersInformation = new UsersRepository();
       
        #region Login/Logout
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            try
            {
                string UserName = User.FindFirst(ClaimTypes.Name).Value;
                if (UserName == "" || UserName == null)
                    return View();
                if(User.IsInRole("SuperAdmin") || User.IsInRole("Admin"))
                    return RedirectToAction("Index", "Admin");
                else if (User.IsInRole("Student"))
                    return RedirectToAction("Index", "Student");
                else if (User.IsInRole("Instructor"))
                    return RedirectToAction("Index", "Instructor");
                else
                return View();
            }
            catch
            {
                return View();
            }
        }
        #region Encryption
        public string Encrypt(string password)
        {
            string salt = "Activities";
            string GenPass = password + salt;
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                UTF8Encoding utf8 = new UTF8Encoding();
                byte[] data = md5.ComputeHash(utf8.GetBytes(GenPass));
                return Convert.ToBase64String(data);
            }
        }
        #endregion Encryption
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(tblUsers accountInfo)
        {
            try
            {
                string EncryptedPassword = Encrypt(accountInfo.Password);
                tblUsers Account = UsersInformation.GetAccountsForLogin(accountInfo, EncryptedPassword);
                if (Account == null)
                {
                    ViewData["Login_error"] = "Login Failed";
                    //ViewData["NoRedirect"] = "No Redirect";
                }
                else if (Account.IsGraduate == true)
                {
                    ViewData["NotAutherozied"] = "Your account has been suspended because you're no longer in KFU";
                    return View();
                }
                else
                {
                    var identity = new ClaimsIdentity(new[]
                    {
                    new Claim(ClaimTypes.Email, Account.KfuEmail),
                    new Claim(ClaimTypes.Role, Account.Role.RoleName),
                    new Claim(ClaimTypes.NameIdentifier, Account.Id.ToString()),
                    new Claim(ClaimTypes.Name, Account.GuId.ToString()),
                    new Claim(ClaimTypes.GivenName, Account.Name),
                    new Claim(ClaimTypes.Sid, Account.CollegeId.ToString())


                    }, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        principal);
                    return RedirectToAction("Index", "Home");
                }
            }
            catch
            {
                ViewData["Login_error"] = "Login Failed";
                return View();
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(AccountController.Login), "Account");
        }
        #endregion Login/Logout
        public IActionResult LogIn()
        {
            return View();
        }
        public IActionResult ActivateAccount()
        {
            TempData.Keep("GuId");
            Guid guid = Guid.Parse(TempData["GuId"].ToString());
            tblUsers userInfo = UsersInformation.GetUserByGuId(guid);
            return View(userInfo);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ActivateAccount(tblUsers UserInfo)
        {
            try {

                string EncryptedPassword = Encrypt(UserInfo.Password);
                UsersInformation.ActivateUser(UserInfo, EncryptedPassword);
                ViewData["Successful"] = "User Activated Successfully";}
              
            catch
            {
                ViewData["Falied"] = "An Error Occurred while processing your request, please try again Later";
            }
            return View();
        }
        public IActionResult VerifyAccount()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult VerifyAccount(string KfuEmail)
        {
            tblUsers StudentEmailFound = UsersInformation.checkemailuserforRecoverPassword(KfuEmail);
            if (StudentEmailFound != null)
            {
                if(StudentEmailFound.IsActive == true) {
                    ViewData["Verified"] = "Your account is verified";
                }
                else { 
                Random random = new Random();
                string code = random.Next(100000, 999999).ToString();
                int check = emailSending(StudentEmailFound.KfuEmail, code);
                if (check == 1)
                {
                    TempData["Code"] = code;
                    TempData.Keep("Code");
                    TempData["Page"] = "ActivateAccount";
                    TempData.Keep("Page");
                    TempData["GuId"] = StudentEmailFound.GuId;
                    TempData.Keep("GuId");
                    return RedirectToAction(nameof(Code));
                            }
                        
                else
                    ViewData["Falied"] = "The code is not sent succussfuly";
            }
            }
            else
            {
                ViewData["Falied"] = "Problem in processing request";
                ViewData["NoRedirect"] = "No Redirect";
            }
            return View();
        }
        public IActionResult Code()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Code(string codeInput)
        {
            
            if (codeInput == TempData["Code"].ToString())
                return RedirectToAction(TempData["Page"].ToString(), "Account");
            else
            {
                TempData.Keep("Code");
                ViewData["Falied"] = "Invalid Code";
            }
            return View();
        }
        public IActionResult EmailCheck()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EmailCheck(string email)
        {
            tblUsers StudentEmailFound = UsersInformation.checkemailuserforRecoverPassword(email);
            if (StudentEmailFound != null)
            {
                Random random = new Random();
                string code = random.Next(100000, 999999).ToString();
                int check = emailSending(StudentEmailFound.KfuEmail, code);
                if (check == 1)
                {
                    ViewData["Successful"] = "The Code is sent to your Email ";
                    TempData["Code"] = code;
                    TempData.Keep("Code");
                    TempData["Page"] = "ForgatPassword";
                    TempData.Keep("Page");
                    TempData["GuId"] = StudentEmailFound.GuId;
                    TempData.Keep("GuId");

                    return RedirectToAction(nameof(Code));
                }
                else
                    ViewData["Falied"] = "The Email is not sent succussfuly";
            }
            else
                ViewData["WrongEmail"] = "This email was not found";
            return View();
        }
        public IActionResult ForgatPassword()
        {
            Guid guid = Guid.Parse(TempData["GuId"].ToString());
            tblUsers userInfo = UsersInformation.GetUserByGuId(guid);
            return View(userInfo);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ForgatPassword(tblUsers UserInfo,string ConfirmedPassword)
        {
            try 
            {
                    string EncryptedPassword = Encrypt(UserInfo.Password);
                    int CheckResult = UsersInformation.ForgatPassword(UserInfo, EncryptedPassword);
                    if (CheckResult == 1) {
                    ViewData["Successful"] = "Your password has been changed successfully";
                    return View();
                }
            }
            catch
            {
                ViewData["Falied"] = "An Error Occurred while processing your request, please try again Later";
            }
            return View();
        }
        public IActionResult Index()
        {
            if(User.Identity.IsAuthenticated == true)
            return View(UsersInformation.GetUserById(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)));
            else
                return RedirectToAction("LogIn", "Account");
        }
        
        public IActionResult ChangeUserPassword()
        {

            return View(UsersInformation.GetUserById(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeUserPassword(tblUsers UserInfo, string NewPassword)
        {
            try
            {
                int id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                string OldEncryptedPassword = Encrypt(UserInfo.Password);
                int CheckResult = UsersInformation.CheckUserPassword(id, OldEncryptedPassword);
                if (CheckResult == 0)
                {
                    ViewBag.Error = 2;
                    return View();
                }
                else
                {
                    string EncryptedPassword = Encrypt(NewPassword);
                    int check = UsersInformation.ChangePassword(id, EncryptedPassword);
                    if (check == 1)
                        ViewData["Successful"] = "Your Password has been changed. You will be logged out";
                    else
                        ViewData["Falied"] = "An Error Occurred while processing your request, please try again Later";
                }
            }
            catch
            {
                ViewData["Falied"] = "An Error Occurred while processing your request, please try again Later";
            }
            return View();
        }
        public IActionResult ChangeName(int AccountId)
        {
            return View(UsersInformation.GetUserById(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeName(tblUsers UserInfo)
        {
            try
            {
                int CheckResult = UsersInformation.ChangeUserName(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value), UserInfo.Name);
                if (CheckResult == 1)
                    ViewData["Success"] = "Your Name modifed Successfully";
                else
                    ViewData["Failed"] = "An Error Occurred while processing your request, please try again Later";
            }
            catch
            {
                ViewData["Failed"] = "An Error Occurred while processing your request, please try again Later";
            }
            return View();
        }
        #region email
        public int emailSending(string emailTo, string code)
        {
            try
            {
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.UseDefaultCredentials = true;
                client.EnableSsl = true;
                
                client.Credentials = new NetworkCredential("ActivitySystemCCSIT@gmail.com", "CCSIT1234");

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("ActivitySystemCCSIT@gmail.com");
                //mailMessage.To.Add(emailTo);
                mailMessage.To.Add(emailTo);
                mailMessage.IsBodyHtml = true;

                //mailMessage.Body = "This is the code for verification: " + code + " please enter it in the code verification page";
                mailMessage.Body = "<div class=\"row\"> <h3>This is the code for verification:</h3> <h3 style = \"color:red\" > "+code+" </h3><br/><h3>please enter it in the code verification page</h3></div>";


                mailMessage.Subject = "Email Verfication";
                client.Send(mailMessage);
                return 1;
            }
            catch 
            {
                ModelState.AddModelError("Exception", "an Error has accured please contact the administration");
                return 0;
            }
        }
        #endregion email

    }
}
