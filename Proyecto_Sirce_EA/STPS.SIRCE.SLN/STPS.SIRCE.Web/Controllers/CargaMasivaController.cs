using STPS.SIRCE.Entidades.POCOS;
using STPS.SIRCE.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace STPS.SIRCE.Web.Controllers
{
    public class CargaMasivaController : Controller
    {
        //
        // GET: /CargaMasiva/
        public ActionResult Index()
        {
            int listaID = 1; //enrae
            CargaMasivaModel modelo = new CargaMasivaModel();
            modelo.ListaID = listaID;
            
            return View(modelo);
        }

        public ActionResult CargaMasiva(string rawModel)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var modelo = js.Deserialize<CargaMasivaModel>(rawModel);

            modelo.ListaID = modelo.ListaID;
            modelo.CentroTrabajoSIRCEID = 1;//((SesionPOCO)Session["SesionPOCO"]).centroTrabajo.centroTrabajoSirceID;
            modelo.EmpresaID = 1;// ((SesionPOCO)Session["SesionPOCO"]).empresa.empresaSIRCEID;
            modelo.NombreEmpresa = "Nombre de la empresa";
            modelo.RFCEmpresa = "AABA100101A11";
            modelo.FolioDC = "123456";
            modelo.ListaCentroTrabajoID = 3;

            return View("CargaMasiva", modelo);
        }

        public ActionResult SubirArchivo()
        {
            var archivo = Request.Files["Filedata"];

            var nombreArchivo = new CargaMasivaModel().GurardarArchivo(archivo);

            return Content(nombreArchivo);
        }

        public ActionResult CargarArchivo(string rawModel)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var modelo = js.Deserialize<CargaMasivaModel>(rawModel);

            modelo.ProcesarArchivo(modelo.NombreArchivo);

            return View("Grid", modelo);
        }
    }
}