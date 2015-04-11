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
    public class TableroControlController : Controller
    {
        // GET: TableroControl
        public ActionResult Index()
        {
            return View("index");
        }

        public ActionResult ConsultarListas()
        {
            TableroControlModel model = new TableroControlModel();
            model.empresaSIRCEID = ((SesionPOCO)Session["SesionPOCO"]).empresa.empresaSIRCEID;
            model.centroTrabajoDNE = ((SesionPOCO)Session["SesionPOCO"]).centroTrabajo ;
            model.consultarListas();
            return PartialView("Grid", model);
        }

        public ActionResult ConsultarDatosView(string rawModel)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var model = js.Deserialize<TableroControlModel>(rawModel);
            if (model.accion != (from a in model.acciones where a.catalogoDescripcion.Equals("Crear") select a.catalogoID).FirstOrDefault())
            {
                model.ConsultarLista();
            }
            return View("Datos", model);
        }

        [HttpPost]
        public JsonResult postLista(string rawModel)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var model = js.Deserialize<TableroControlModel>(rawModel);
            model.unidadResponsableID =  ((SesionPOCO)Session["SesionPOCO"]).usuario.unidadResponsable;
            model.usuarioCreacion =  ((SesionPOCO)Session["SesionPOCO"]).usuario.usuarioActiveDirectoryNombre;
            model.centroTrabajoSIRCEID = ((SesionPOCO)Session["SesionPOCO"]).centroTrabajo.centroTrabajoSirceID;
            bool success = model.postLista();
            return new JsonResult() { Data = new { success } };
        }

    }
}