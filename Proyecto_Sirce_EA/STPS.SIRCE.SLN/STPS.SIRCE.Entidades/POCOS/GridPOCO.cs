using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STPS.SIRCE.Entidades
{
    public class GridPOCO<T>
    {
        public List<String> encabezados { get; set; }
        public List<String> columnas { get; set; }

        public List<IconosGridPOCO> iconosGrid { get; set; }

        public List<T> datos { get; set; }

        public string columnaID { get; set; }
    }
}