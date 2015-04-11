using System;
using System.Web.Mvc;
using STPS.SIRCE.Web.Models;
using STPS.SIRCE.Entidades;
using STPS.SIRCE.Negocio;
using System.Web.Script.Serialization;
using STPS.SIRCE.Entidades.POCOS;

namespace STPS.SIRCE.Web.Controllers
{
    public class CursoController : Controller
    {
        //
        // GET: /Curso/
        public ActionResult Index()
        {
            return PartialView();
        }

        //
        // GET: /Curso/Details/5
        public ActionResult Grid()
        {

            CursoModel modelo = new CursoModel();
            modelo.empresaSirceId = ((SesionPOCO)Session["SesionPOCO"]).empresa.empresaSIRCEID;
            modelo.ConsultarCursos();

            return View("Grid", modelo);
        }

        //
        // POST: /Curso/Create
        public ActionResult GuardarEntidad(string rawModel)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var modelo = js.Deserialize<CursoModel>(rawModel);
            modelo.ConsultarCatalogos();

            if (modelo.Accion == modelo.ConsultarAccion("Crear"))
            {
                modelo.Crear();
            }
            else if (modelo.Accion == modelo.ConsultarAccion("Editar"))
            {
                modelo.Editar();
            }

            return View("Grid", modelo);
        }

        public ActionResult AccionarCurso(string rawModel)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var modelo = js.Deserialize<CursoModel>(rawModel);

            modelo.curso = null;
            modelo.ConsultarCatalogos();
            if (modelo.Accion == modelo.ConsultarAccion("Editar") || modelo.Accion == modelo.ConsultarAccion("Consultar"))
                modelo.Consultar();
            else if (modelo.Accion == modelo.ConsultarAccion("Eliminar"))
            {
                modelo.Eliminar();
                return View("Grid", modelo);
            }

            return View("Datos", modelo);
        }

    }
}
