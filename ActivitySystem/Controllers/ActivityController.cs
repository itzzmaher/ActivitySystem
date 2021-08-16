using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActivitySystem.Models;
using ActivitySystem.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ActivitySystem.Controllers
{
    public class ActivityController : Controller
    {
        ActivityRepository ActivityInformation = new ActivityRepository();

    }

}
