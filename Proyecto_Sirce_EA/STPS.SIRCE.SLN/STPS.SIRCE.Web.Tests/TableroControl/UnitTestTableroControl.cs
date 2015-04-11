using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using STPS.SIRCE.Negocio;
using System.Collections.Generic;
using STPS.SIRCE.Entidades;

namespace STPS.SIRCE.WEB.Tests.TableroControl
{
    [TestClass]
    public class UnitTestTableroControl
    {
        [TestMethod]
        public void ObtenerConsecutivos_HappyPath()
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            TableroControlNEG objecto = new TableroControlNEG();
            Entidades.ConsecutivoPOCO consecutivo = new ConsecutivoPOCO();

            
            bool resultado = false;

            consecutivo.EmpresaSIRCEID = 1;
            consecutivo.ConsecutivoURID = 1;

            timer.Start();
            try
            {
                resultado = objecto.obtenerConsecutivo(consecutivo);

                Debug.WriteLine("ConsecutivoLista :: " + consecutivo.ConsecutivoEmpresa + " | ConsecutivoDC4 :: " + consecutivo.ConsecutivoDC4);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            timer.Stop();
            Debug.WriteLine("CreaTrabajador :: Minutes: " + timer.Elapsed.Minutes + " | Seconds: " + timer.Elapsed.Seconds + " | Milliseconds: " + timer.Elapsed.Milliseconds);

            Assert.IsTrue(resultado, "Registro exitoso");
        }
    }
}
