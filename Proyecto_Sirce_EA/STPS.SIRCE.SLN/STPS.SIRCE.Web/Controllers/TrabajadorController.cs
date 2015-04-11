using System;
using System.Web.Mvc;
using STPS.SIRCE.Web.Models;
using STPS.SIRCE.Entidades;
using STPS.SIRCE.Negocio;
using System.Linq;
using System.Web.Script.Serialization;
using STPS.SIRCE.Entidades.POCOS;

namespace STPS.SIRCE.WEB.Controllers
{
    public class TrabajadorController : Controller
    {
        //
        // GET: /Trabajador/
        public ActionResult Index()
        {
            return PartialView();
        }

        //
        // GET: /Trabajador/Details/5
        public ActionResult Grid()
        {
            TrabajadorModel modelo = new TrabajadorModel();
            modelo.ConsultarCatalogos();
            modelo.ConsultarTrabajadores(((SesionPOCO)Session["SesionPOCO"]).centroTrabajo.centroTrabajoSirceID);
            return View("Grid", modelo);
        }

        //
        // GET: /Trabajador/Create
        public ActionResult ConsultarDatosView(string rawModel)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var model = js.Deserialize<TrabajadorModel>(rawModel);
            model.ConsultarCatalogos();
            //Aqui creamos el entitity
            if (model.accion != (from a in model.acciones where a.catalogoDescripcion.Equals("Crear") select a.catalogoID).FirstOrDefault())
            {
                //Se busca el trabajador al modelo
                model.ConsultaTrabajador();
                //Se asigna los valores al modelo
            }
            return View("Datos", model);
        }

        public ActionResult PostTrabajador(string rawModel)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var model = js.Deserialize<TrabajadorModel>(rawModel);
            model.centroTrabajoSIRCEID = ((SesionPOCO)Session["SesionPOCO"]).centroTrabajo.centroTrabajoSirceID;
            bool success = model.PostTrabajador();
            return new JsonResult() { Data = new { success } };
        }

        [HttpPost]
        public JsonResult ConsultarMunicipiosTrabajador(string rawModel)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var model = js.Deserialize<TrabajadorModel>(rawModel);
            model.ConsultarMunicipiosTrabajador();
            return new JsonResult() { Data = model, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult VerificaCURP(string rawModel)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var model = js.Deserialize<TrabajadorModel>(rawModel);
            model.ValidaCURP(((SesionPOCO)Session["SesionPOCO"]).centroTrabajo.centroTrabajoSirceID);
            return new JsonResult() { Data = model, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}
