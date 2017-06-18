using ArmazemController;
using ArmazemModel;
using ArmazemModel.Entities;
using System;
using System.Windows;
using System.Windows.Input;

namespace ArmazemUIs.Cadastros
{
    /// <summary>
    /// Interaction logic for ListComposicoesUI.xaml
    /// </summary>
    public partial class ListComposicoesUI : Window
    {
        ComposicaoController ComposicaoController { get; set; }

        public ListComposicoesUI()
        {
            InitializeComponent();
            ComposicaoController = new ComposicaoController();
        }


        #region Operações

        private void AtualizaListaDeComposicoes()
        {
            gridComposicoes.ItemsSource = ComposicaoController.ListarTodas();
        }

        private void IncluirNovoRegistro()
        {
            statusBar.Text = string.Empty;

            try
            {
                ComposicaoUI composicaoUI = new ComposicaoUI();
                composicaoUI.Owner = this;
                composicaoUI.ShowDialog();
                AtualizaListaDeComposicoes();
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
            Composicao composicao = (Composicao)gridComposicoes.SelectedItem;

            try
            {
                if (composicao != null)
                {

                    if (Util.MensagemDeConfirmacao($"Deseja realmente excluir a composicao {composicao.Id}?"))
                    {
                        ComposicaoController.Deletar(composicao);
                        AtualizaListaDeComposicoes();
                        statusBar.Text = "Composição excluída com sucesso.";
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
                if (gridComposicoes.SelectedItem != null)
                {

                    ComposicaoUI composicaoUI = new ComposicaoUI(((Composicao)gridComposicoes.SelectedItem));
                    composicaoUI.Owner = this;
                    composicaoUI.ShowDialog();
                    AtualizaListaDeComposicoes();

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
            AtualizaListaDeComposicoes();
        }

        private void gridProdutos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
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
