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
    [Authorize(Roles = "Instructor")]
    public class InstructorController : Controller
    {
        ActivityRepository ActivityInformation = new ActivityRepository();

        public IActionResult ActivateActivty(Guid? id)
        {
            try
            {
                int checkResult = ActivityInformation.ActivateActivity(id);
                if (checkResult == 1) {
                    TempData["Success"] = "Activity has been Activated Successfully";
                    TempData.Keep("Success");
                    return RedirectToAction("Index", "Instructor");
                }
                else {
                    TempData["Failled"] = "An Error Occurred while processing your request, please try again Later";
                    TempData.Keep("Failled");
                    return RedirectToAction("Index", "Instructor");
                }
            }
            catch
            {
                TempData["Failled"] = "An Error Occurred while processing your request, please try again Later";
                TempData.Keep("Failled");
                return RedirectToAction("Index", "Instructor");
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
                    return RedirectToAction("Index", "Instructor");
                }
                else
                {
                    TempData["Failled"] = "An Error Occurred while processing your request, please try again Later";
                    TempData.Keep("Failled");
                    return RedirectToAction("Index", "Instructor");
                }
            }
            catch
            {
                TempData["Failled"] = "An Error Occurred while processing your request, please try again Later";
                TempData.Keep("Failled");
                return RedirectToAction("Index", "Instructor");
            }
        }

        public IActionResult UpdateActivityInfo(Guid? id)
        {
            var ActivityInfo = ActivityInformation.GetActivityByGuId(id);
            ViewData["SemesterId"] = new SelectList(new ActivityRepository().GetAllSemestersForRegisteration(), "Id", "SemesterName");
            return View(ActivityInfo);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateActivityInfo(tblActivities ActivityInfo)
        {
            try
            {
                int checkResult = ActivityInformation.UpdateActivityInstructor(ActivityInfo);
                if (checkResult == 1)
                ViewData["Successful"] = "Activity Updated Successfully";
            }
            catch
            {
                ViewData["SemesterId"] = new SelectList(new ActivityRepository().GetAllSemestersForRegisteration(), "Id", "SemesterName");
                ViewData["Falied"] = "An Error Occurred while processing your request, please try again Later";
            }
            return View();
        }
        public IActionResult RegisterApproval()
        {
            ViewData["Successful"] = TempData["Success"];
            ViewData["Failled"] = TempData["Failled"];
            return View(ActivityInformation.GetAllWaitingActivitiesForInstructor(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)));
        }
        public IActionResult ChangeStatus(Guid? id, string status)
        {
            try
            {
                int statusid = 0;
                if (status == "Approve")
                    statusid = 2;
                else
                    statusid = 3;
                int checkResult = ActivityInformation.ActivityApproval(id, statusid);
                if (checkResult == 1)
                {
                    TempData["Success"] = "You approved the student successfully";
                    TempData.Keep("Success");
                    return RedirectToAction("RegisterApproval", "Instructor");
                }
                else if (checkResult == 2)
                {
                    TempData["Success"] = "You denied the student successfully";
                    TempData.Keep("Success");
                    return RedirectToAction("RegisterApproval", "Instructor");
                }
                else
                {
                    TempData["Failled"] = "An Error Occurred while processing your request, please try again Later";
                    TempData.Keep("Failled");
                    return RedirectToAction("RegisterApproval", "Instructor");
                }
            }
            catch
            {
                TempData["Failled"] = "An Error Occurred while processing your request, please try again Later";
                TempData.Keep("Failled");
                return RedirectToAction("RegisterApproval", "Instructor");
            }
        }
        public IActionResult index ()
        {
            ViewData["Successful"] = TempData["Success"];
            ViewData["Failled"] = TempData["Failled"];
            return View(ActivityInformation.GetAllActiveActivities(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)));
        }
        public IActionResult OpenActivty(Guid? id)
        {
            try
            {
                int checkResult = ActivityInformation.OpenActivty(id);
                if (checkResult == 1) {
                    TempData["Success"] = "Activity has been opened successfully";
                    TempData.Keep("Success");
                    return RedirectToAction("index", "Instructor");}
                else
                {
                    TempData["Failled"] = "An Error Occurred while processing your request, please try again Later";
                    TempData.Keep("Failled");
                    return RedirectToAction("index", "Instructor");
                }
            }
            catch
            {
                TempData["Failled"] = "An Error Occurred while processing your request, please try again Later";
                TempData.Keep("Failled");
                return RedirectToAction("index", "Instructor");
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
                    return RedirectToAction("index", "Instructor");
                }
                else
                {
                    TempData["Failled"] = "An Error Occurred while processing your request, please try again Later";
                    TempData.Keep("Failled");
                    return RedirectToAction("index", "Instructor");
                }
            }
            catch
            {
                TempData["Failled"] = "An Error Occurred while processing your request, please try again Later";
                TempData.Keep("Failled");
                return RedirectToAction("index", "Instructor");
            }
        }
        public IActionResult ViewStudents(Guid? id)
        {
            return View(ActivityInformation.GetAllRegisteredStudents(id));
        }
    }
}
