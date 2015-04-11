using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using STPS.SIRCE.Negocio;
using System.Collections.Generic;
using STPS.SIRCE.Entidades;

namespace STPS.SIRCE.WEB.Tests.Trabajadores
{
    [TestClass]
    public class UnitTestTrabajadores
    {
        [TestMethod]
        public void ConsultarInstitucionesEducativas_HappyPath()
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            UtileriaNeg utileria = new UtileriaNeg();
            List<CatalogoPOCO> catalogoPOCO = new List<CatalogoPOCO>();

            timer.Start();
            try
            {
                catalogoPOCO = utileria.EnumeradorALista<Enumeradores.InstitucionEducativa>();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            timer.Stop();
            Debug.WriteLine("ConsultarInstitucionEducativa :: Minutes: " + timer.Elapsed.Minutes + " | Seconds: " + timer.Elapsed.Seconds + " | Milliseconds: " + timer.Elapsed.Milliseconds);

            timer.Start();
            foreach (var item in catalogoPOCO)
            {
                Debug.WriteLine("ConsultarInstitucionEducativa :: ID: " + item.catalogoID + " | Descripcion: " + item.catalogoDescripcion);
            }
            timer.Stop();
            Debug.WriteLine("Foreach ConsultarInstitucionEducativa :: Minutes: " + timer.Elapsed.Minutes + " | Seconds: " + timer.Elapsed.Seconds + " | Milliseconds: " + timer.Elapsed.Milliseconds);

            Assert.AreNotEqual(0, catalogoPOCO.Count);
        }

        [TestMethod]
        public void CrearTrabajador_HappyPath()
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            TrabajadorNEG objecto = new TrabajadorNEG();
            Entidades.TrabajadorPOCO trabajadores = new TrabajadorPOCO();
            List<NormaTrabajadorPOCO> listNormaTrabajador = new List<NormaTrabajadorPOCO>();
            bool resultado = false;

            trabajadores.TrabajadorID = 0;
            trabajadores.CURP = "CURP";
            trabajadores.Nombre = "Nombre";
            trabajadores.ApellidoMaterno = "Paterno";
            trabajadores.ApellidoPaterno = "Materno";
            trabajadores.OcupacionID = 1;
            trabajadores.EntidadFederativaID = 1;
            trabajadores.MunicipioID = 1;
            trabajadores.EscolaridadID = 1;
            trabajadores.InstitucionesEducativasID = 1;
            trabajadores.EscolaridadID = 1;
            trabajadores.CentroTrabajoSIRCEID = 1;
            trabajadores.Genero = 1;
            trabajadores.FechaNacimiento = DateTime.Now.ToShortDateString();
            trabajadores.Eliminado = false;

            NormaTrabajadorPOCO normaTrabajador = new NormaTrabajadorPOCO();
            //Poner le listado para recuperar los valores de la normas de competencia.
            normaTrabajador.TrabajadorID = 1;
            normaTrabajador.NormaCompetenciaID = 1;
            normaTrabajador.FechaEmision = DateTime.Now.ToShortDateString();
            trabajadores.NormaTrabajador.Add(normaTrabajador);

            timer.Start();
            try
            {
                //resultado = objecto.CrearTrabajador(trabajadores);
                Debug.WriteLine("CreaTrabajador :: resultado: " + resultado);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            timer.Stop();
            Debug.WriteLine("CreaTrabajador :: Minutes: " + timer.Elapsed.Minutes + " | Seconds: " + timer.Elapsed.Seconds + " | Milliseconds: " + timer.Elapsed.Milliseconds);

            Assert.IsFalse(resultado, "Registro exitoso");
        }

        //[TestMethod]
        //public void CrearNormaEstandar_HappyPath()
        //{
        //    System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
        //    TrabajadorNEG objecto = new TrabajadorNEG();
        //    Entidades.NormaTrabajador norma = new NormaTrabajador();
        //    bool resultado = false;

        //    norma.TrabajadorID = 1;
        //    norma.NormaCompetenciaID = 1;
        //    norma.FechaEmision = DateTime.Now;

        //    timer.Start();
        //    try
        //    {
        //        resultado = objecto.CrearNormasTrabajador(norma);
        //        Debug.WriteLine("CreaTrabajador :: resultado: " + resultado);
        //    }
        //    catch (Exception ex)
        //    {
        //        Assert.Fail(ex.Message);
        //    }
        //    timer.Stop();
        //    Debug.WriteLine("CreaTrabajador :: Minutes: " + timer.Elapsed.Minutes + " | Seconds: " + timer.Elapsed.Seconds + " | Milliseconds: " + timer.Elapsed.Milliseconds);

        //    Assert.IsFalse(resultado, "Registro exitoso");
        //}

        [TestMethod]
        public void ConsultarTrabajadores_HappyPath()
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            List<TrabajadorPOCO> listTrabajadores = new List<TrabajadorPOCO>();
            TrabajadorNEG objecto = new TrabajadorNEG();

            timer.Start();
            try
            {
                //listTrabajadores = objecto.ConsultarTrabajador(1);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            timer.Stop();
            Debug.WriteLine("ConsultarTrabajadores :: Minutes: " + timer.Elapsed.Minutes + " | Seconds: " + timer.Elapsed.Seconds + " | Milliseconds: " + timer.Elapsed.Milliseconds);

            timer.Start();
            foreach (var item in listTrabajadores)
            {
                Debug.WriteLine("ConsultarTrabajadores :: ID: " + item.TrabajadorID + " | Nombre: " + item.Nombre);
            }
            timer.Stop();
            Debug.WriteLine("Foreach ConsultarTrabajadores :: Minutes: " + timer.Elapsed.Minutes + " | Seconds: " + timer.Elapsed.Seconds + " | Milliseconds: " + timer.Elapsed.Milliseconds);

            Assert.AreNotEqual(0, listTrabajadores.Count);
        }
    }
}
