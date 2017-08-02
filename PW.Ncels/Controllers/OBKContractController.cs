using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PW.Ncels.Controllers
{
    public class OBKContractController : Controller
    {
        // GET: OBKContract
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Contract(Guid? id, string listAction)
        {
            ViewBag.ListAction = listAction;
            return View(id ?? Guid.NewGuid());
        }
    }
}