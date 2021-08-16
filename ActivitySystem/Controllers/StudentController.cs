using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ActivitySystem.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActivitySystem.Controllers
{
   [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        
        UsersRepository UsersInformation = new UsersRepository();
        ActivityRepository ActivityInformation = new ActivityRepository();
        public IActionResult Index(string msgsuccsess, string msgfail)
        {
            ViewData["Successful"] = msgsuccsess;
            ViewData["Falied"] = msgfail;
            ViewData["StudentName"] = User.FindFirst(ClaimTypes.GivenName).Value;
            return View(ActivityInformation.GetAllStudentRegisterations(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)));
        }

        public IActionResult ActivityBrowse()
        {

            return View(ActivityInformation.GetAllOpenActivities(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)));
        }

        public IActionResult Registration(Guid? id)
        {
            ViewBag.StudentCount = ActivityInformation.StudentNumber(id);
            ViewBag.CheckNumber = ActivityInformation.CheckNumber(id);
            ViewBag.CheckHistory = ActivityInformation.StudentRegisterHistory(id, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            return View(ActivityInformation.GetActivityByGuId(id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Registration(Guid? id, string none)
        {
            try
            {
                int checkResult = ActivityInformation.StudentRegister(id, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
                if (checkResult == 1)
                    ViewData["Successful"] = "Registerd Successfully";
                else
                {
                    ViewData["Falied"] = "You already registerd in this activity";
                    ViewData["NoRedirect"] = "";
                }
            }
            catch
            {
                ViewData["Falied"] = "An Error Occurred while processing your request, please try again Later";
            }
            ViewBag.CheckNumber = ActivityInformation.CheckNumber(id);
            ViewBag.StudentCount = ActivityInformation.StudentNumber(id);
            return View(ActivityInformation.GetActivityByGuId(id));
        }

        public IActionResult CancelRegisteration(string msgsuccsess, string msgfail)
        {
            ViewData["Successful"] = msgsuccsess;
            ViewData["Falied"] = msgfail;
            return View(ActivityInformation.GetAllWaitingActivitiesForStudent(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)));
        }
        public IActionResult Cancelation(Guid? id)
        {
            try
            {
                int checkResult = ActivityInformation.CancelRegister(id, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
                if (checkResult == 1)
                    return RedirectToAction("Index", "Student", new { msgsuccsess = "You canceled your registeration successfully", msgfail = "" });
                else
                    return RedirectToAction("Index", "Student", new { msgsuccsess = "", msgfail = "An Error Occurred while processing your request, please try again Later" });
            }
            catch
            {
                return RedirectToAction("Index", "Student", new { msgsuccsess = "", msgfail = "An Error Occurred while processing your request, please try again Later" });
            }
        }
        public IActionResult ViewActivtyInfo(Guid? id)
        {
            
            return View(ActivityInformation.GetActivityByRegisterGuId(id));
            
        }
        
    }
}
