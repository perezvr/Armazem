using ArmazemController;
using ArmazemModel;
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

namespace ArmazemUIs
{
    /// <summary>
    /// Interaction logic for BuscaProdutosUI.xaml
    /// </summary>
    public partial class BuscaProdutosUI : Window
    {
        Produto_Controller Produto_Controller { get; set; }

        public Produto ProdutoSelecionado { get; set; }

        public BuscaProdutosUI()
        {
            InitializeComponent();
            Produto_Controller = new Produto_Controller();
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
                    gridProdutos.ItemsSource = Produto_Controller.ListarPorDescricao(txtDescricao.Text);
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