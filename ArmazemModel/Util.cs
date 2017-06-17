using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

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

        public static void ToMoney(this TextBox txInput)
        {
            txInput.TextWrapping = System.Windows.TextWrapping.NoWrap;
            txInput.AcceptsReturn = false;
            txInput.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Right;
            txInput.PreviewTextInput += TxInput_PreviewTextInput;
            txInput.GotFocus += TxInput_GotFocus;
            txInput.LostFocus += TxInput_LostFocus;

            if (string.IsNullOrEmpty(txInput.Text))
                txInput.Text = "0,00";
        }

        private static void TxInput_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                TextBox txInput = (sender as TextBox);
                txInput.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Right;
                txInput.LostFocus += TxInput_LostFocus;

                Regex rgxNumbers = new Regex("[^0-9]+");
                e.Handled = (rgxNumbers.IsMatch(e.Text) || (e.Text.Equals(",")));
                if (e.Text.Equals(",") || e.Text.Equals("."))
                {
                    if (e.Text.Equals(",") && txInput.Text.Contains(","))
                        return;

                    if (e.Text.Equals(".") && txInput.Text.Contains(","))
                        return;

                    if (e.Text.Equals(",") && txInput.Text.Contains(","))
                        return;

                    if (e.Text.Equals(".") && (txInput.Text.Last().Equals('.') || txInput.Text.Last().Equals(',')))
                        return;

                    if (e.Text.Equals(",") && (txInput.Text.Last().Equals('.') || txInput.Text.Last().Equals(',')))
                        return;

                    txInput.Text += e.Text;
                    txInput.SelectionStart = txInput.Text.Length; // add some logic if length is 0
                    txInput.SelectionLength = 0;

                }
                if (e.Text.Equals("-"))
                {
                    if (txInput.Text.Contains("-"))
                        return;

                    txInput.Text = "-";
                    txInput.SelectionStart = txInput.Text.Length; // add some logic if length is 0
                    txInput.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private static void TxInput_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            TextBox tx = (TextBox)sender;
            tx.SelectAll();
        }

        private static void TxInput_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            TextBox textBox = (sender as TextBox);
            if (string.IsNullOrWhiteSpace(textBox.Text))
                textBox.Text = "0,00";
            else
                textBox.Text = decimal.Parse(textBox.Text).ToString("n2");
        }

        public static void ToNumeric(this TextBox txInput)
        {
            txInput.PreviewTextInput += TxInput_PreviewTextInput1;

            if (string.IsNullOrEmpty(txInput.Text))
                txInput.Text = "0";
            txInput.LostFocus += TxInput_LostFocus2;
            txInput.GotFocus += TxInput_GotFocus;
        }

        private static void TxInput_PreviewTextInput1(object sender, TextCompositionEventArgs e)
        {
            Regex rgxNumbers = new Regex("[^0-9]+");
            e.Handled = (rgxNumbers.IsMatch(e.Text));
        }

        private static void TxInput_LostFocus2(object sender, System.Windows.RoutedEventArgs e)
        {
            TextBox tx = (sender as TextBox);
            if (string.IsNullOrWhiteSpace(tx.Text))
                tx.Text = "0";
            if (string.IsNullOrEmpty(tx.Text))
                tx.Text = "0";
        }

    }

    public class ValidationException : Exception
    {
        public ValidationException(string mensagem) : base(mensagem) { }
    }


}
