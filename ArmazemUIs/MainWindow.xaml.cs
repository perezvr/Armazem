using ArmazemController;
using ArmazemModel;
using System;
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
        }
        private void AcessaCadastroProdutos(TIPO_PRODUTO tipoCadastro)
        {
            try
            {
                ListProdutosUI listProdutosUI = new ListProdutosUI(tipoCadastro);
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
            AcessaCadastroProdutos(TIPO_PRODUTO.SIMPLES);
        }
        
        private void menuProdutosCompostos_Click(object sender, RoutedEventArgs e)
        {
            AcessaCadastroProdutos(TIPO_PRODUTO.COMPOSTO);

        }

        #endregion

    }
}
