using ArmazemController.Relatorios;
using ArmazemModel;
using ArmazemModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static ArmazemModel.Util;

namespace ArmazemUIs.Relatórios
{
    /// <summary>
    /// Interaction logic for RelatorioRequisicoesUI.xaml
    /// </summary>
    public partial class RelatoriosUI : Window
    {
        RelatorioRequisicoesController RelatorioRequisicoesController { get; set; }
        RelatorioSaidasController RelatorioSaidasController { get; set; }
        TIPO_RELATORIO tipoRelatorio;

        public RelatoriosUI(TIPO_RELATORIO tipoRelatorio)
        {
            InitializeComponent();
            RelatorioRequisicoesController = new RelatorioRequisicoesController();
            RelatorioSaidasController = new RelatorioSaidasController();
            this.tipoRelatorio = tipoRelatorio;
            Title = $"Relatório de {tipoRelatorio.getDescription()}";
            LimparFormulario();
        }

        #region Operações

        private void LimparFormulario()
        {
            statusBar.Text = string.Empty;
            dtpDataInicial.SelectedDate = DateTime.Now;
            dtpDataFinal.SelectedDate = DateTime.Now;
            gridRequisicoes.ItemsSource = null;

            switch (tipoRelatorio)
            {
                case TIPO_RELATORIO.REQUISICOES:
                    lblObs.Visibility = Visibility.Hidden;
                    break;
                case TIPO_RELATORIO.SAIDAS:
                    lblObs.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        private void ValidaPesquisa()
        {
            if (dtpDataInicial.SelectedDate == null)
                throw new ValidationException("Informe a data inicial!");
            if (dtpDataFinal.SelectedDate == null)
                throw new ValidationException("Informe a data final!");
            if (dtpDataInicial.SelectedDate.Value > dtpDataFinal.SelectedDate.Value)
                throw new ValidationException("A data final nao pode ser menor que a data inicial!");
        }

        private void Pesquisar()
        {
            statusBar.Text = string.Empty;

            try
            {
                ValidaPesquisa();
                List<ItemRequisicao> lista = new List<ItemRequisicao>();

                switch (tipoRelatorio)
                {
                    case TIPO_RELATORIO.REQUISICOES:
                        lista = RelatorioRequisicoesController.Pesquisa(dtpDataInicial.SelectedDate.Value, dtpDataFinal.SelectedDate.Value);
                        break;
                    case TIPO_RELATORIO.SAIDAS:
                        lista = RelatorioSaidasController.Pesquisa(dtpDataInicial.SelectedDate.Value, dtpDataFinal.SelectedDate.Value);
                        break;
                    default:
                        break;
                }


                gridRequisicoes.ItemsSource = lista;
                txtCustoTotal.Text = lista.Sum(x => x.Qtde * x.PrecoCusto).ToString("n2");
                txtVendaTotal.Text = lista.Sum(x => x.Qtde * x.Produto.PrecoVenda).Value.ToString("n2");
            }
            catch (ValidationException ex)
            {
                statusBar.Text = ex.Message;
            }
            catch (Exception ex)
            {
                Util.MensagemDeErro(ex);
            }
        }

        #endregion

        #region Eventos

        private void btnLimpar_Click(object sender, RoutedEventArgs e)
        {
            LimparFormulario();
        }
      private void btnVisualizar_Click(object sender, RoutedEventArgs e)
        {
            Pesquisar();
        }

        #endregion

  
    }
}
