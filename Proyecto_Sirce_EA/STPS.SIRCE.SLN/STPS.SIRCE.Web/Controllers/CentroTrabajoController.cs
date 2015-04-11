using STPS.SIRCE.Entidades.POCOS;
using STPS.SIRCE.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace STPS.SIRCE.Web.Controllers
{
    public class CentroTrabajoController : Controller
    {
        // GET: CentroTrabajo
        public ActionResult Index()
        {
            CentroTrabajoModel model = new CentroTrabajoModel();
            model.empresaID = ((SesionPOCO)Session["SesionPOCO"]).empresa.empresaID;
            return PartialView("Index", model);
        }


        public ActionResult ConsultarVistaFiltros(string rawModel)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var model = js.Deserialize<CentroTrabajoModel>(rawModel);
            model.ConsultarCatalogos();
            return View("Filtros", model);
        }



        [HttpPost]
        public JsonResult ConsultarMunicipios(string rawModel)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var model = js.Deserialize<CentroTrabajoModel>(rawModel);
            model.ConsultarMunicipios();
            return new JsonResult() { Data = model, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult ConsultarCentrosTrabajo(string rawModel)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var model = js.Deserialize<CentroTrabajoModel>(rawModel);
            model.ConsultarCentrosTrabajo();
            return new JsonResult() { Data = model, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult SeleccionCentroTrabajo(string rawModel)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var model = js.Deserialize<CentroTrabajoModel>(rawModel);
            model.centroTrabajo.empresaSIRCEID = ((SesionPOCO)Session["SesionPOCO"]).empresa.empresaSIRCEID;
            model.ValidaCentroTrabajoSIRCE();
            SesionPOCO objSesion = (SesionPOCO)Session["SesionPOCO"];
            objSesion.centroTrabajo = model.centroTrabajo;
            Session["SessionPOCO"] = objSesion;
            return PartialView("~/Views/TableroControl/index.cshtml");
        }
    }
}