using ArmazemController;
using ArmazemModel;
using ArmazemModel.DAL;
using ArmazemModel.Entities;
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
        ProdutoDAL pDAL = new ProdutoDAL();

        public MainWindow()
        {
            InitializeComponent();

            //cria o banco de dados caso ainda não exista
            ArmazemEntities db = new ArmazemEntities();
            db.Database.CreateIfNotExists();
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

        #region Chamadas do menu

        private void menuProdutos_Click(object sender, RoutedEventArgs e)
        {
            AcessaCadastroProdutos();

        }

        private void menuComposicao_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ListComposicoesUI listComposicoesUI = new ListComposicoesUI();
                listComposicoesUI.Owner = this;
                listComposicoesUI.ShowDialog();
            }
            catch (Exception ex)
            {
                statusBar.Text = ex.InnerException != null
                    ? ex.InnerException.Message
                    : ex.Message;
            }
        }

        #endregion

    }
}
