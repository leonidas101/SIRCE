using STPS.SIRCE.Entidades.POCOS;
using STPS.SIRCE.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace STPS.SIRCE.WEB.Controllers
{
    public class EmpresaController : Controller
    {
        // GET: Empresa
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ConsultarVistaFiltros()
        {
            EmpresaModel model = new EmpresaModel();
            model.ConsultarCatalogos();
            return View("Filtros", model);
        }

        [HttpPost]
        public JsonResult ConsultarMunicipios(string rawModel)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var model = js.Deserialize<EmpresaModel>(rawModel);
            model.ConsultarMunicipios();
            return new JsonResult() { Data = model, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public ActionResult ConsultarVistaGrid(string rawModel)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var model = js.Deserialize<EmpresaModel>(rawModel);
            model.ConsultarEmpresas();
            return View("Grid", model);
        }

        [HttpPost]
        public JsonResult ConsultarEmpresas(string rawModel)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var model = js.Deserialize<EmpresaModel>(rawModel);
            model.ConsultarEmpresas();
            return new JsonResult() { Data = model, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult SeleccionaEmpresa(string rawModel)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var model = js.Deserialize<EmpresaModel>(rawModel);
            model.ValidaEmpresaSIRCE();
            SesionPOCO objSesion = (SesionPOCO)Session["SesionPOCO"];
            objSesion.empresa = model.empresa;
            Session["SesionPOCO"] = objSesion;
            CentroTrabajoModel centroTrabajoModel = new CentroTrabajoModel();
            centroTrabajoModel.empresaID = model.empresa.empresaID;
            return PartialView("~/Views/CentroTrabajo/index.cshtml", centroTrabajoModel);
        }
    }
}