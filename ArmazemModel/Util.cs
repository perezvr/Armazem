using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ArmazemModel
{
    public class Util
    {
        public static object Clone(object obj)
        {
            object new_obj = Activator.CreateInstance(obj.GetType());
            foreach (PropertyInfo pi in obj.GetType().GetProperties())
            {
                if (pi.CanRead && pi.CanWrite)
                {
                    pi.SetValue(new_obj, pi.GetValue(obj, null), null);
                }
            }
            return new_obj;
        }

        public enum TIPO_PRODUTO : int
        {
            [Description("Produtos Simples")]
            SIMPLES = 1,
            [Description("Produtos Compostos")]
            COMPOSTO = 2,
        }
    }

    public static class Extensions
    {
        /// <summary>
        /// Retorna a descrição setada para o Enum em questão
        /// </summary>
        /// <param name="val">Enum</param>
        /// <returns>Descrição do Enum
        /// </returns>
        public static string getDescription(this Enum val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }

    public class ValidationException : Exception
    {
        public ValidationException(string mensagem) : base(mensagem) { }
    }


}
