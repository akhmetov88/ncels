using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PW.Ncels.Controllers
{
    public class SafetyAssessmentController : Controller
    {
        // GET: SafetyAssessment
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Delete(string id)
        {
            return RedirectToAction("RegisterSafetyAssessmentList");
        }

        public ActionResult RegisterSafetyAssessmentList()
        {
            return View();
        }
    }
}