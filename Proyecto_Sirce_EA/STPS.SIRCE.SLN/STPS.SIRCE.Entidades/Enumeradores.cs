using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace STPS.SIRCE.Entidades
{
    public class Enumeradores
    {
        public enum EstatusLista
        {
            /// <summary>
            /// Enumerador para el estatus de la lista
            /// </summary>
            [Description("En Proceso")]
            Enproceso = 1,
            [Description("Lista Enviada")]
            Enviada = 2
        }

        public enum DirectorioOrigen
        {
            [Description("Portal de Servicios Electrónicos")]
            PSE = 1,
            [Description("Directorio Nacional de Empresas")]
            DNE = 2
        }
        public enum TipoAgenteCapacitador
        {
            [Description("Interno")]
            Interno = 1,
            [Description("Externo")]
            Externo = 2,
            [Description("Otro")]
            Otro =3
        }

        public enum ModalidadCapacitacion
        {
            [Description("Presencial")]
            Presencial = 1,
            [Description("En línea")]
            EnLinea = 2,
            [Description("Mixta")]
            Mixta = 3
        }

        public enum ConexionesSatelitales
        {
            /// <summary>
            /// Enumerador para asociar conexiones satelitales.
            /// </summary>
            [Description("Directorio Nacional de Empresas")]
            DNE = 1,
            [Description("Catalogos Institucionales")]
            CI = 2,
            [Description("Portal de Servicios Electronicos")]
            PSE = 3
        }

        public enum InstitucionEducativa
        { 
            /// <summary>
            /// Enumerador para asociar las instituciones educativas.
            /// </summary>
            [ Description("Pública")]
            Publica = 1,
            [Description("Privada")]
            Privada = 2
        }

        public enum Genero
        { 
            /// <summary>
            /// Enumerador para asociar los generos
            /// </summary>
            [Description("Hombre")]
            H = 1,
            [Description("Mujer")]
            M = 2
        }

        public enum TipoCRUD
        {
            /// <summary>
            /// Enumerador para asociar el tipo de operación que se realizará en el CRUD
            /// </summary>
            [Description("Consultar")]
            Consultar = 1,
            [Description("Crear")]
            Crear = 2,
            [Description("Editar")]
            Editar = 3,
            [Description("Eliminar")]
            Eliminar = 4
        }


        public enum OrigenSIRCE
        {
            /// <summary>
            /// Enumerador para asociar el tipo de operación que se realizará en el CRUD
            /// </summary>
            [Description("Internet")]
            Internet = 1,
            [Description("Ventanilla")]
            Ventanilla = 2
        }

        public static string GetDescription(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute attribute
                    = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))
                        as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }


    }
}
