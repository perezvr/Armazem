using ArmazemController;
using ArmazemModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
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

namespace ArmazemUIs
{
    /// <summary>
    /// Interaction logic for ListProdutosUI.xaml
    /// </summary>
    public partial class ListProdutosUI : Window
    {
        TIPO_PRODUTO tipoCadastro;

        public ListProdutosUI(TIPO_PRODUTO tipoCadastro)
        {
            InitializeComponent();
            this.tipoCadastro = tipoCadastro;
            Title = $"Lista de {tipoCadastro.getDescription()}";
        }

        private void HabilitaComponentes()
        {
            switch (tipoCadastro)
            {
                case TIPO_PRODUTO.SIMPLES:
                    break;
                case TIPO_PRODUTO.COMPOSTO:
                colEstoqueAtual.Visibility = Visibility.Hidden;
                    break;
                default:
                    break;
            }

            //TODO arrumar o GetPrecoCusto para contemplar o Produto Composto
        }

        private void AtualizaListaDeProdutos()
        {
            gridProdutos.ItemsSource = Produto_Controller.Listar(i => i.Tipo.Equals((int)tipoCadastro));          
        }

        #region Operações

        private void IncluirNovoRegistro()
        {
            statusBar.Text = string.Empty;

            try
            {
                switch (tipoCadastro)
                {
                    case TIPO_PRODUTO.SIMPLES:
                        ProdutoSimplesUI produtoSimplesUI = new ProdutoSimplesUI();
                        produtoSimplesUI.Owner = this;
                        produtoSimplesUI.ShowDialog();
                        AtualizaListaDeProdutos();
                        break;
                    case TIPO_PRODUTO.COMPOSTO:
                        ProdutoCompostoUI produtoCompostoUI = new ProdutoCompostoUI();
                        produtoCompostoUI.Owner = this;
                        produtoCompostoUI.ShowDialog();
                        AtualizaListaDeProdutos();
                        break;
                    default:
                        break;

                }
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
            Produto produto = (Produto)gridProdutos.SelectedItem;

            try
            {
                if (produto != null)
                {

                    if (Util.MensagemDeConfirmacao($"Deseja realmente excluir o produto {produto.Codigo}?"))
                    {
                        Produto_Controller.Deletar(produto.Codigo);
                        AtualizaListaDeProdutos();
                        statusBar.Text = "Produto excluído com sucesso.";
                    }
                }

                else
                    throw new ValidationException("Selecione um registo.");
            }
            catch (ValidationException ex)
            {
                statusBar.Text = ex.Message;
            }
            catch (DbUpdateException)
            {
                Util.MensagemDeAtencao($"O produto {produto.Codigo} não pode ser excluído pois está relacionado a uma ou mais requisições de saída!");

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
                if (gridProdutos.SelectedItem != null)
                {
                    switch (tipoCadastro)
                    {
                        case TIPO_PRODUTO.SIMPLES:
                            ProdutoSimplesUI produtoSimplesUI = new ProdutoSimplesUI(((Produto)gridProdutos.SelectedItem));
                            produtoSimplesUI.Owner = this;
                            produtoSimplesUI.ShowDialog();
                            AtualizaListaDeProdutos();
                            break;
                        case TIPO_PRODUTO.COMPOSTO:
                            break;
                        default:
                            break;
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
                Util.MensagemDeErro(ex);
            }
        }

        #endregion

        #region Eventos

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AtualizaListaDeProdutos();
        }
        private void gridProdutos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelecionarRegistroParaEdicao();
        }


        #region Button_Click

        private void btnSelecionar_Click(object sender, RoutedEventArgs e)
        {
            SelecionarRegistroParaEdicao();
        }

        private void btnNovo_Click(object sender, RoutedEventArgs e)
        {
            IncluirNovoRegistro();
        }

        private void btnExcluir_Click(object sender, RoutedEventArgs e)
        {
            ExcluirRegistro();
        }

        #endregion

        #endregion
    }
}
