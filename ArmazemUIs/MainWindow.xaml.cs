using ArmazemController;
using ArmazemModel;
using System;
using System.Linq;
using System.Windows;
using static ArmazemModel.Util;

namespace ArmazemUIs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //teste();
        }

        private void teste()
        {
            Produto_Controller pc = new Produto_Controller();
            Produto p1 = pc.PesquisaPorCodigo(20);


        }
        private void AcessaCadastroProdutos(TIPO_PRODUTO tipoCadastro)
        {
            try
            {
                ListProdutosUI listProdutosUI = new ListProdutosUI();
                listProdutosUI.Owner = this;
                listProdutosUI.ShowDialog();
            }
            catch (Exception ex)
            {
                statusBar.Text = ex.InnerException != null
                    ? ex.InnerException.Message
                    : ex.Message;
            }
        }

        private void AcessaCadastroProdutos()
        {
            try
            {
                ListProdutosUI listProdutosUI = new ListProdutosUI();
                listProdutosUI.Owner = this;
                listProdutosUI.ShowDialog();
            }
            catch (Exception ex)
            {
                statusBar.Text = ex.InnerException != null
                    ? ex.InnerException.Message
                    : ex.Message;
            }
        }

        #region Chamadas do manu

        private void menuProdutosSimples_Click(object sender, RoutedEventArgs e)
        {
        }
        
        private void menuProdutosCompostos_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        private void menuProdutos_Click(object sender, RoutedEventArgs e)
        {
            AcessaCadastroProdutos();

        }

        private void menuComposicao_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
