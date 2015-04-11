using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using STPS.SIRCE.Datos;
using STPS.SIRCE.Entidades;

namespace STPS.SIRCE.Negocio
{
    //[System.Diagnostics.DebuggerNonUserCode()]
    public class WorkUnit : IDisposable
    {
        protected SIRCEEntities contextoSIRCE { get; set; }

        public WorkUnit()
        {
            contextoSIRCE = new SIRCEEntities();
        }

        public WorkUnit(SIRCEEntities scC)
        {
            contextoSIRCE = scC;
        }

        public void Dispose()
        {
            contextoSIRCE.Dispose();
        }
        public void Save()
        {
            contextoSIRCE.SaveChanges();
        }
    }
}
