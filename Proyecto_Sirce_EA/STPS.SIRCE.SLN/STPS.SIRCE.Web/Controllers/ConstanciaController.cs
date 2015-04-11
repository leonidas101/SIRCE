using System;
using System.Web.Mvc;
using STPS.SIRCE.Web.Models;
using STPS.SIRCE.Entidades.POCOS;
using System.Web.Script.Serialization;

namespace STPS.SIRCE.WEB.Controllers
{
    public class ConstanciaController : Controller
    {
        //
        // GET: Constancia
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ConsultarDatosView(int listaID)
        {
            ConstanciaModel modelo = new ConstanciaModel();            
            modelo.listaID = listaID;
            modelo.centroTrabajoSIRCEID = ((SesionPOCO)Session["SesionPOCO"]).centroTrabajo.centroTrabajoSirceID;
            modelo.EmpresaSIRCEID = ((SesionPOCO)Session["SesionPOCO"]).empresa.empresaSIRCEID;

            modelo.ConsultarTrabajadores(modelo.EmpresaSIRCEID , modelo.centroTrabajoSIRCEID);
            modelo.ConsultarCursos(modelo.EmpresaSIRCEID);
            modelo.ConsultarLista();

            modelo.ConsultarConstancias();

            return PartialView("Datos", modelo);
        }

        [HttpPost]
        public ActionResult CrearConstancia(string rawModel)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var modelo = js.Deserialize<ConstanciaModel>(rawModel);

            modelo.centroTrabajoSIRCEID = ((SesionPOCO)Session["SesionPOCO"]).centroTrabajo.centroTrabajoSirceID;
            modelo.EmpresaSIRCEID = ((SesionPOCO)Session["SesionPOCO"]).empresa.empresaSIRCEID;
            modelo.CrearConstancia();

            modelo.ConsultarTrabajadores(modelo.EmpresaSIRCEID, modelo.centroTrabajoSIRCEID);
            modelo.ConsultarCursos(modelo.EmpresaSIRCEID);
            modelo.ConsultarLista();
            modelo.constancia = null;
            modelo.ConsultarConstancias();            

            return PartialView("Datos", modelo);
        }        
    }
}
