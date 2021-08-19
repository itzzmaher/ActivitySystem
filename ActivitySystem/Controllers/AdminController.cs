using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ActivitySystem.Models;
using ActivitySystem.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ActivitySystem.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class AdminController : Controller
    {
        UsersRepository UsersInformation = new UsersRepository();
        ActivityRepository ActivityInformation = new ActivityRepository();
        public IActionResult Index()
        {
            ViewData["Successful"] = TempData["Success"];
            return View(UsersInformation.GetAllUsers());

        }
        public IActionResult Dashboard()
        {
            ViewData["TodayName"] = DateTime.Now.DayOfWeek.ToString();
            ViewData["TodayDate"] = DateTime.Now.ToString("dd/MM/yyy");
            ViewData["CurrentSem"] = ActivityInformation.GetCurrentSemester();
            ViewData["NumberOfActivites"] = ActivityInformation.GetAllOpenActivites().Count();
            ViewData["TotalStudents"] = UsersInformation.GetAllStudents().Count();
            ViewData["NumberOfInstructors"] = UsersInformation.GetAllInstuctors().Count();
            ViewData["NumberOfAdmins"] = UsersInformation.GetAllAdmins().Count();
            return View();
        }
        public IActionResult ViewSemesters()
        {
            ViewData["Successful"] = TempData["Success"];
            ViewData["Failled"] = TempData["Failled"];
            return View(ActivityInformation.GetAllSemesters());
        }
        public IActionResult AddSemester()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddSemester(tblSemesters SemInfo)
        {
            int CheckResult = ActivityInformation.AddSemester(SemInfo);
            if (CheckResult == 1)
                ViewData["Successful"] = "Semester Added Successfully";
            else if (CheckResult == 2)
            {
                ViewData["Falied"] = "This Semester already Exist";
                ViewData["NoRedirect"] = "No Redirect";
            }
            else
                ViewData["Falied"] = "An Error Occurred while processing your request, please try again Later";
            return View();
        }
        public IActionResult ViewActivities()
        {
            ViewData["Successful"] = TempData["Success"];
            ViewData["Failled"] = TempData["Failled"];
            return View(ActivityInformation.GetAllActivities());
        }
        public IActionResult AddActivity()
        {
            ViewData["UserId"] = new SelectList(new UsersRepository().GetAllInstuctors(), "Id", "Name");
            ViewData["SemesterId"] = new SelectList(new ActivityRepository().GetAllSemestersForRegisteration(), "Id", "SemesterName");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddActivity(tblActivities ActivityInfo)
        {
            try
            {

                int checkResult = ActivityInformation.ActivityInsert(ActivityInfo, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
                if (checkResult == 1)
                {
                    ViewData["Successful"] = "Activity Added Successfully";
                }
            }
            catch
            {
                ViewData["Falied"] = "An Error Occurred while processing your request, please try again Later";
            }
            return View(ActivityInfo);
        }
        public IActionResult InsertAccount()
        {
            ViewData["CollegeId"] = new SelectList(UsersInformation.GetAllColleges(), "Id", "CollegeName");
            if (User.IsInRole("SuperAdmin"))
            {
                ViewData["RoleId"] = new SelectList(UsersInformation.GetAllRoles(), "Id", "RoleName");
            }
            if (User.IsInRole("Admin"))
            {
                ViewData["RoleId"] = new SelectList(UsersInformation.GetAllRolesForAdmin(), "Id", "RoleName");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InsertAccount(tblUsers UserInfo)
        {
            try
            {
                int checkResult = UsersInformation.UserInsert(UserInfo);
                if (checkResult == 1)
                    ViewData["Successful"] = "User Added Successfully";
                else if (checkResult == 2)
                {
                    ViewData["Falied"] = "This user already exists";
                    ViewData["NoRedirect"] = "No Redirect";
                }
                else
                    ViewData["Falied"] = "An Error Occurred while processing your request, please try again Later";
            }
            catch
            {
                ViewData["Falied"] = "An Error Occurred while processing your request, please try again Later";
            }
            ViewData["RoleId"] = new SelectList(UsersInformation.GetAllRoles(), "Id", "RoleName", UserInfo.RoleId);
            ViewData["CollegeId"] = new SelectList(UsersInformation.GetAllColleges(), "Id", "CollegeName", UserInfo.CollegeId);
            return View(UserInfo);
        }
        public IActionResult SemesterState(Guid? id, string status)
        {
            try
            {
                int CheckResult = ActivityInformation.SemesterActivation(id, status);
                if (CheckResult == 1)
                {
                    TempData["Success"] = "Semeter has been activated Successfully";
                    TempData.Keep("Success");
                    return RedirectToAction("ViewSemesters", "Admin");

                }
                else if (CheckResult == 2)
                {
                    TempData["Success"] = "Semeter has been deactivated Successfully";
                    TempData.Keep("Success");
                    return RedirectToAction("ViewSemesters", "Admin");
                }
                else
                {
                    TempData["Failled"] = "An Error Occurred while processing your request, please try again Later";
                    TempData.Keep("Failled");
                    return RedirectToAction("ViewSemesters", "Admin");
                }
            }
            catch
            {
                TempData["Failled"] = "An Error Occurred while processing your request, please try again Later";
                TempData.Keep("Failled");
                return RedirectToAction("ViewSemesters", "Admin");
            }

        }

        public IActionResult ActivateActivty(Guid? id)
        {
            try
            {
                int checkResult = ActivityInformation.ActivateActivity(id);
                if (checkResult == 1)
                {
                    TempData["Success"] = "Activity has been Activated Successfully";
                    TempData.Keep("Success");
                    return RedirectToAction("ViewActivities", "Admin");
                }
                else
                {
                    TempData["Failled"] = "An Error Occurred while processing your request, please try again Later";
                    TempData.Keep("Failled");
                    return RedirectToAction("ViewActivities", "Admin");
                }
            }
            catch
            {
                TempData["Failled"] = "An Error Occurred while processing your request, please try again Later";
                TempData.Keep("Failled");
                return RedirectToAction("ViewActivities", "Admin");
            }
        }
        public IActionResult DeactivateActivty(Guid? id)
        {
            try
            {
                int checkResult = ActivityInformation.DeactivateActivity(id);
                if (checkResult == 1)
                {
                    TempData["Success"] = "Activity has been deactivated Successfully";
                    TempData.Keep("Success");
                    return RedirectToAction("ViewActivities", "Admin");
                }
                else
                {
                    TempData["Failled"] = "An Error Occurred while processing your request, please try again Later";
                    TempData.Keep("Failled");
                    return RedirectToAction("ViewActivities", "Admin");
                }
            }
            catch
            {
                TempData["Failled"] = "An Error Occurred while processing your request, please try again Later";
                TempData.Keep("Failled");
                return RedirectToAction("ViewActivities", "Admin");
            }
        }
        public IActionResult AdminUpdateActivity(Guid? id)
        {
            var ActivityInfo = ActivityInformation.GetActivityByGuId(id);
            ViewData["UserId"] = new SelectList(new UsersRepository().GetAllInstuctors(), "Id", "Name");
            ViewData["SemesterId"] = new SelectList(new ActivityRepository().GetAllSemestersForRegisteration(), "Id", "SemesterName");
            return View(ActivityInfo);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdminUpdateActivity(tblActivities ActivityInfo)
        {
            try
            {

                int checkResult = ActivityInformation.UpdateActivityAdmin(ActivityInfo);
                if (checkResult == 1)
                    ViewData["Successful"] = "Activity Updated Successfully";
            }
            catch
            {
                ViewData["Falied"] = "An Error Occurred while processing your request, please try again Later";
            }
            return View();
        }
        public IActionResult OpenActivty(Guid? id)
        {
            try
            {
                int checkResult = ActivityInformation.OpenActivty(id);
                if (checkResult == 1)
                {
                    TempData["Success"] = "Activity has been opened successfully";
                    TempData.Keep("Success");
                    return RedirectToAction("ViewActivities", "Admin");
                }
                else
                {
                    TempData["Failled"] = "An Error Occurred while processing your request, please try again Later";
                    TempData.Keep("Failled");
                    return RedirectToAction("ViewActivities", "Admin");
                }
            }
            catch
            {
                TempData["Failled"] = "An Error Occurred while processing your request, please try again Later";
                TempData.Keep("Failled");
                return RedirectToAction("ViewActivities", "Admin");
            }
        }
        public IActionResult CloseActivty(Guid? id)
        {
            try
            {
                int checkResult = ActivityInformation.CloseActivty(id);
                if (checkResult == 1)
                {
                    TempData["Success"] = "Activity has been closed successfully";
                    TempData.Keep("Success");
                    return RedirectToAction("ViewActivities", "Admin");
                }
                else
                {
                    TempData["Failled"] = "An Error Occurred while processing your request, please try again Later";
                    TempData.Keep("Failled");
                    return RedirectToAction("ViewActivities", "Admin");
                }
            }
            catch
            {
                TempData["Failled"] = "An Error Occurred while processing your request, please try again Later";
                TempData.Keep("Failled");
                return RedirectToAction("ViewActivities", "Admin");
            }
        }
        public IActionResult ModifyUserInfo(Guid id)
        {

            ViewData["CollegeId"] = new SelectList(UsersInformation.GetAllColleges(), "Id", "CollegeName");
            if (User.IsInRole("SuperAdmin"))
            {
                ViewData["RoleId"] = new SelectList(UsersInformation.GetAllRoles(), "Id", "RoleName");
            }
            if (User.IsInRole("Admin"))
            {
                ViewData["RoleId"] = new SelectList(UsersInformation.GetAllRolesForAdmin(), "Id", "RoleName");
            }
            return View(UsersInformation.GetUserByGuId(id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ModifyUserInfo(tblUsers UserInfo)
        {
            try
            {
                int CheckResult = UsersInformation.UpdateUserInfoByAdmin(UserInfo);
                if (CheckResult == 1)
                {
                    ViewData["Successful"] = "User information was modified successfully";
                }
            }
            catch
            {
                ViewData["Falied"] = "An Error Occurred while processing your request, please try again Later";
            }
            return View();
        }
        public IActionResult RemoveSemester(Guid id)
        {
            try
            {
                int CheckResult = ActivityInformation.SemesterDelete(id);
                if (CheckResult == 1)
                {
                    TempData["Success"] = "Semeter has been Deleted Successfully";
                    TempData.Keep("Success");
                    return RedirectToAction("ViewSemesters", "Admin");
                }
                else
                {
                    TempData["Failled"] = "An Error Occurred while processing your request, please try again Later";
                    TempData.Keep("Failled");
                    return RedirectToAction("ViewSemesters", "Admin");
                }
            }
            catch
            {
                TempData["Failled"] = "An Error Occurred while processing your request, please try again Later";
                TempData.Keep("Failled");
                return RedirectToAction("ViewSemesters", "Admin");
            }
        }
        public IActionResult DeactivateUser(Guid id)
        {
            tblUsers UserInfo = UsersInformation.GetUserByGuId(id);

            int CheckResult = UsersInformation.DeactivateUser(UserInfo);
            if (CheckResult == 1)
            {
                TempData["Success"] = "Account has been deactivated successfully";
                TempData.Keep("Success");
                return RedirectToAction("Index", "Admin");
            }

            else
            {
                TempData["Failled"] = "An Error Occurred while processing your request, please try again Later";
                TempData.Keep("Failled");
                return RedirectToAction("Index", "Admin");
            }
        }
        public IActionResult AdminActivate(Guid id)
        {

            tblUsers userInfo = UsersInformation.GetUserByGuId(id);
            ViewData["RoleId"] = new SelectList(UsersInformation.GetAllRoles(), "Id", "RoleName");
            ViewData["CollegeId"] = new SelectList(UsersInformation.GetAllColleges(), "Id", "CollegeName");
            return View(userInfo);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdminActivate(tblUsers UserInfo)
        {
            try
            {
                string EncryptedPassword = new AccountController().Encrypt(UserInfo.Password);
                UsersInformation.ActivateUser(UserInfo, EncryptedPassword);
                ViewData["Successful"] = "User Activated Successfully";
            }
            catch
            {
                ViewData["Falied"] = "An Error Occurred while processing your request, please try again Later";
            }
            return View();
        }
        public IActionResult DeleteAccount(Guid? Id)
        {

            int CheckResult = UsersInformation.DeleteAccount(Id);
            if (CheckResult == 1)
            {
                TempData["Success"] = "Account has been deleted successfully";
                TempData.Keep("Success");
                return RedirectToAction("Index", "Admin");
            }

            else
            {
                TempData["Failled"] = "An Error Occurred while processing your request, please try again Later";
                TempData.Keep("Failled");
                return RedirectToAction("Index", "Admin");
            }
        }
    }
}


