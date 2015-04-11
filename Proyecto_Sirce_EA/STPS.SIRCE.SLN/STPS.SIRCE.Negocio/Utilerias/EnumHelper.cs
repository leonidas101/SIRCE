using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;


namespace STPS.SIRCE.Negocio
{
    public static class EnumHelper
    {
        public static string ToDescription(Enum value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            if (!Enum.IsDefined(value.GetType(), value))
            {
                return string.Empty;
            }

            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
            if (fieldInfo != null)
            {
                DescriptionAttribute[] attributes =
                    fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
                if (attributes != null && attributes.Length > 0)
                {
                    return attributes[0].Description;
                }
            }

            return StringHelper.ToFriendlyName(value.ToString());
        }

        public static string ClaveEnumerador<T>(string str) where T : struct
        {
            Type enumType = typeof(T);
            int valor = 0;

            // Validamos que sea de tipo Enum
            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T must be of type System.Enum");

            // Obtenemos los elementos del enumerador
            var resultado = Enum.GetName(enumType,(object)str);

            var otro = Enum.Parse(enumType, str);


            //// Para cada elemento del enumerador, creamos su CatalogoPOCO
            //foreach (var enumerador in elementos)
            //{
            //    object obj = Enum.ToObject(enumType, (int)enumerador);
            //    // Obtenemos la descripcion del elemento
            //    var descripcion = EnumHelper.ToDescription((Enum)obj);
            //    // Agregamos a la lista el nuevo POCO
            //    catalogo.Add(new CatalogoPOCO { catalogoID = (int)enumerador, catalogoDescripcion = descripcion });
            //}

            return valor.ToString();
        }
    }
}