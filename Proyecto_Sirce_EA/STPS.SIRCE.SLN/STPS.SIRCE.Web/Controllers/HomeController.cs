using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using STPS.SIRCE.Entidades;
using STPS.SIRCE.Negocio;
using System.Text.RegularExpressions;
using STPS.SIRCE.Entidades.POCOS;

namespace STPS.SIRCE.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            SesionPOCO objSesion = new SesionPOCO();
            //Hardcode COCHINO
            objSesion.usuario = new UsuarioPOCO();
            objSesion.usuario.unidadResponsable = 111;
            objSesion.usuario.usuarioActiveDirectoryID = 1;
            objSesion.usuario.usuarioActiveDirectoryCorreo = "cibarra@stps.com.mx";
            objSesion.usuario.usuarioActiveDirectoryNombre = "Christopher Ibarra Stone";
            Session["SesionPOCO"] = objSesion;
            Session.Timeout = 30;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}