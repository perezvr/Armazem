using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ArmazemUIs
{
    static class Util
    {
        #region Mensagens personalizadas

        const string TITULO = "Controle de Armazém";

        public static void MensagemDeInformacao(string msg)
        {
            MessageBox.Show(msg, TITULO, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static void MensagemDeErro(Exception ex)
        {
            string msg = ex.InnerException != null
                                        ? ex.InnerException.Message
                                        : ex.Message;

            MessageBox.Show(msg, TITULO, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void MensagemDeAtencao(string msg)
        {
            MessageBox.Show(msg, TITULO, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        public static bool MensagemDeConfirmacao(string msg)
        {
            if (MessageBox.Show(msg, TITULO, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}
