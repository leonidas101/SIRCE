using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using STPS.SIRCE.Entidades;

namespace STPS.SIRCE.Datos
{
    public class AccesableContext
    {
        protected SIRCEEntities contexto;

        public AccesableContext(SIRCEEntities pContexto)
        {
            this.contexto = pContexto;
        }
    }
}
