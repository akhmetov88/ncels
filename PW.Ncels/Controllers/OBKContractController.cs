using PW.Ncels.Database.Repository.OBK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PW.Ncels.Controllers
{
    public class OBKContractController : Controller
    {
        OBKContractRepository obkRepo;
        public OBKContractController()
        {
            obkRepo = new OBKContractRepository();
        }

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

        public ActionResult SearchDrug(int regType, string drugNumber, string drugTradeName, string drugManufacturer, string drugMnn)
        {
            var list = obkRepo.GetSearchReestr(regType, drugNumber, drugTradeName, drugManufacturer, drugMnn);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}