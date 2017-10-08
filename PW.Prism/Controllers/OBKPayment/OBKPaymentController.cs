using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PW.Ncels.Database.Models;
using PW.Ncels.Database.Repository.DirectionToPay;

namespace PW.Prism.Controllers.OBKPayment
{
    public class OBKPaymentController : Controller
    {
        // GET: OBKPayment
        public ActionResult Index()
        {
            var guid = Guid.NewGuid();
            DirectionToPayRepository repository = new DirectionToPayRepository();

            ViewBag.DirectionToPayStatuses =
                repository.GetDirectionToPayStatuses().ToList().OrderBy(o => o.Name)
                    .Select(o => new Item() { Id = o.Id.ToString(), Name = o.Name }).ToList();
            return PartialView(guid);
        }
    }
}