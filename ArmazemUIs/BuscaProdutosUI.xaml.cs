using ArmazemController;
using ArmazemModel;
using ArmazemModel.Entities;
using System;
using System.Windows;
using System.Windows.Input;
using static ArmazemModel.Util;

namespace ArmazemUIs
{
    /// <summary>
    /// Interaction logic for BuscaProdutosUI.xaml
    /// </summary>
    public partial class BuscaProdutosUI : Window
    {
        ProdutoController ProdutoController { get; set; }
        public Produto ProdutoSelecionado { get; set; }
        TIPO_PRODUTO tipoProduto;

        public BuscaProdutosUI(TIPO_PRODUTO tipoProduto)
        {
            InitializeComponent();
            this.tipoProduto = tipoProduto;
            ProdutoController = new ProdutoController();
        }

        private void SelecionarProduto()
        {
            try
            {
                if (gridProdutos.SelectedItem != null)
                {
                    ProdutoSelecionado = (Produto)gridProdutos.SelectedItem;
                    Close();
                }
                else
                    throw new ValidationException("Selecione um produto.");

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

        private void BuscaProduto()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txtDescricao.Text))
                {
                    if (tipoProduto.Equals(TIPO_PRODUTO.TODOS))
                        gridProdutos.ItemsSource = ProdutoController.ListarPorDescricao(txtDescricao.Text);
                    else
                        gridProdutos.ItemsSource = ProdutoController.ListarPorDescricaoETipo(txtDescricao.Text, tipoProduto);
                }
            }
            catch (Exception ex)
            {
                Util.MensagemDeErro(ex);
            }
        }

        #region Button_Click

        private void gridProdutos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelecionarProduto();
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            BuscaProduto();
        }

        #endregion

        private void btnSelecionar_Click(object sender, RoutedEventArgs e)
        {
            SelecionarProduto();
        }
    }
}
//TODO colocar icone no sistema