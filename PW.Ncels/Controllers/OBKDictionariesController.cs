using PW.Ncels.Database.DataModel;
using PW.Ncels.Database.Helpers;
using System;
using System.Linq;
using System.Web.Mvc;

namespace PW.Ncels.Controllers
{
    public class OBKDictionariesController : ACommonController
    {
        private ncelsEntities db = UserHelper.GetCn();

        [Authorize()]
        [HttpGet]
        public ActionResult GetObkContractTypes()
        {
            var contractTypes = db.OBK_Ref_Type.OrderBy(x => x.Id).Select(o => new { o.Id, Name = o.NameRu, o.Code, o.NameKz });
            return Json(contractTypes.ToList(), JsonRequestBehavior.AllowGet);
        }

        [Authorize()]
        [HttpGet]
        public ActionResult GetObkOrganizations()
        {
            //var obkOrganizations = db.OBK_Organization.Select(o => new { o.Id, Name = o.NameRu, o.NameKz });
            //return Json(obkOrganizations, JsonRequestBehavior.AllowGet);

            var obkDeclarants = db.OBK_Declarant.Where(o => o.IsConfirmed == true).Select(o => new { o.Id, Name = o.NameKz, o.NameKz }).ToList();
            var noData = new { Id = Guid.Empty, Name = "Нет данных", NameKz = "Нет данных" };
            var list = new[] { noData }.ToList().Concat(obkDeclarants);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [Authorize()]
        [HttpGet]
        public ActionResult GetMeasureDictionary()
        {
            var srMeasures = db.sr_measures.Select(x => new { Id = x.id, Name = x.short_name, NameKz = x.short_name_kz }); 
            return Json(srMeasures, JsonRequestBehavior.AllowGet);
        }
    }
}