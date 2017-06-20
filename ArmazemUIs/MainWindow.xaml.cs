using ArmazemController;
using ArmazemModel;
using ArmazemModel.DAL;
using ArmazemModel.Entities;
using ArmazemUIs.Cadastros;
using ArmazemUIs.Relatórios;
using ArmazemUIs.Requisicoes;
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
        ComposicaoController con = new ComposicaoController();
        ProdutoController con1 = new ProdutoController();

        public MainWindow()
        {
            InitializeComponent();

            //cria o banco de dados caso ainda não exista
            ArmazemEntities db = new ArmazemEntities();
            db.Database.CreateIfNotExists();
        }

        #region Operações

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

        private void AcessaCadastroComposicao()
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

        private void AcessaCadastroRequisicao()
        {
            try
            {
                ListRequisicoesUI listRequisicoessUI = new ListRequisicoesUI();
                listRequisicoessUI.Owner = this;
                listRequisicoessUI.ShowDialog();
            }
            catch (Exception ex)
            {
                statusBar.Text = ex.InnerException != null
                    ? ex.InnerException.Message
                    : ex.Message;
            }
        }

        private void AcessaRelatorioRequisicoes()
        {
            try
            {
                RelatoriosUI relatoriosUI = new RelatoriosUI(TIPO_RELATORIO.REQUISICOES);
                relatoriosUI.Owner = this;
                relatoriosUI.ShowDialog();
            }
            catch (Exception ex)
            {
                statusBar.Text = ex.InnerException != null
                    ? ex.InnerException.Message
                    : ex.Message;
            }
        }

        private void AcessaRelatorioSaidas()
        {
            try
            {
                RelatoriosUI relatoriosUI = new RelatoriosUI(TIPO_RELATORIO.SAIDAS);
                relatoriosUI.Owner = this;
                relatoriosUI.ShowDialog();
            }
            catch (Exception ex)
            {
                statusBar.Text = ex.InnerException != null
                    ? ex.InnerException.Message
                    : ex.Message;
            }
        }

        #endregion

        #region Chamadas do menu

        private void menuProdutos_Click(object sender, RoutedEventArgs e)
        {
            AcessaCadastroProdutos();
        }

        private void menuComposicao_Click(object sender, RoutedEventArgs e)
        {
            AcessaCadastroComposicao();
        }

        private void menuRequisicao_Click(object sender, RoutedEventArgs e)
        {
            AcessaCadastroRequisicao();
        }

        private void manuRelatorioRequisicoes_Click(object sender, RoutedEventArgs e)
        {
            AcessaRelatorioRequisicoes();
        }

        private void manuRelatorioSaidasEstoque_Click(object sender, RoutedEventArgs e)
        {
            AcessaRelatorioSaidas();
        }

        #endregion
    }
}
