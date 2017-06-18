using ArmazemController;
using ArmazemModel;
using ArmazemModel.Entities;
using System;
using System.Windows;
using System.Windows.Input;

namespace ArmazemUIs.Requisicoes
{
    /// <summary>
    /// Interaction logic for ListRequisicoesUI.xaml
    /// </summary>
    public partial class ListRequisicoesUI : Window
    {
        RequisicaoController RequisicaoController { get; set; }

        public ListRequisicoesUI()
        {
            InitializeComponent();
            RequisicaoController = new RequisicaoController();
        }

        #region Operações

        private void AtualizaListaDeRequisicoes()
        {
            gridRequisicoes.ItemsSource = RequisicaoController.ListarTodas();
        }

        private void IncluirNovoRegistro()
        {
            statusBar.Text = string.Empty;

            try
            {
                SaidaMercadoriaUI saidaMercadoriaUI = new SaidaMercadoriaUI();
                saidaMercadoriaUI.Owner = this;
                saidaMercadoriaUI.ShowDialog();
                AtualizaListaDeRequisicoes();
            }
            catch (Exception ex)
            {
                statusBar.Text = ex.InnerException != null
                                 ? ex.InnerException.Message
                                 : ex.Message;
            }
        }

        private void ExcluirRegistro()
        {
            statusBar.Text = string.Empty;
            Requisicao requisicao = (Requisicao)gridRequisicoes.SelectedItem;

            try
            {
                if (requisicao != null)
                {

                    if (Util.MensagemDeConfirmacao($"Deseja realmente excluir a requisicao {requisicao.Id}?"))
                    {
                        RequisicaoController.Deletar(requisicao);
                        AtualizaListaDeRequisicoes();
                        statusBar.Text = "Requisição excluída com sucesso.";
                    }
                }

                else
                    throw new ValidationException("Selecione um registo.");
            }
            catch (ValidationException ex)
            {
                statusBar.Text = ex.Message;
            }
            catch (Exception ex)
            {
                statusBar.Text = ex.InnerException != null
                                        ? ex.InnerException.Message
                                        : ex.Message;
            }
        }

        private void SelecionarRegistroParaEdicao()
        {
            statusBar.Text = string.Empty;
            try
            {
                if (gridRequisicoes.SelectedItem != null)
                {

                    SaidaMercadoriaUI saidaMercadoriaUI = new SaidaMercadoriaUI(((Requisicao)gridRequisicoes.SelectedItem));
                    saidaMercadoriaUI.Owner = this;
                    saidaMercadoriaUI.ShowDialog();
                    AtualizaListaDeRequisicoes();

                }
                else
                    throw new ValidationException("Selecione um registo.");

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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AtualizaListaDeRequisicoes();
        }

        private void gridRequisicoes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelecionarRegistroParaEdicao();
        }

        private void btnSelecionar_Click(object sender, RoutedEventArgs e)
        {
            SelecionarRegistroParaEdicao();
        }

        private void btnExcluir_Click(object sender, RoutedEventArgs e)
        {
            ExcluirRegistro();
        }

        private void btnNovo_Click(object sender, RoutedEventArgs e)
        {
            IncluirNovoRegistro();
        }

        #endregion

    }
}
