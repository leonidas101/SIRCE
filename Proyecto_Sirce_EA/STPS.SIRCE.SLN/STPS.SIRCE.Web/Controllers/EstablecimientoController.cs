using System;
using System.Web.Mvc;
using STPS.SIRCE.Web.Models;
using STPS.SIRCE.Entidades.POCOS;
using System.Web.Script.Serialization;

namespace STPS.SIRCE.WEB.Controllers
{
    public class EstablecimientoController : Controller
    {
        // GET: Establecimientos
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ConsultarDatosView(int listaID)
        {
            EstablecimientoModel modelo = new EstablecimientoModel();
            modelo.listaID = listaID;
            modelo.centroTrabajoSIRCEID = ((SesionPOCO)Session["SesionPOCO"]).centroTrabajo.centroTrabajoSirceID;
            modelo.EmpresaSIRCEID = ((SesionPOCO)Session["SesionPOCO"]).empresa.empresaSIRCEID;

            modelo.ConsultarTrabajadores(modelo.EmpresaSIRCEID, modelo.centroTrabajoSIRCEID);
            modelo.ConsultarCursos(modelo.EmpresaSIRCEID);
            modelo.ConsultarLista();

            modelo.ConsultarEstablecimientos();

            return PartialView("Datos", modelo);
        }

        [HttpPost]
        public ActionResult CrearEstablecimiento(string rawModel)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var modelo = js.Deserialize<EstablecimientoModel>(rawModel);

            modelo.centroTrabajoSIRCEID = ((SesionPOCO)Session["SesionPOCO"]).centroTrabajo.centroTrabajoSirceID;
            modelo.EmpresaSIRCEID = ((SesionPOCO)Session["SesionPOCO"]).empresa.empresaSIRCEID;
            modelo.CrearEstablecimiento(modelo.centroTrabajoSIRCEID);

            modelo.ConsultarTrabajadores(modelo.EmpresaSIRCEID, modelo.centroTrabajoSIRCEID);
            modelo.ConsultarCursos(modelo.EmpresaSIRCEID);
            modelo.ConsultarLista();
            modelo.establecimiento = null;
            modelo.ConsultarEstablecimientos();

            return PartialView("Datos", modelo);
        }    
    }
}
