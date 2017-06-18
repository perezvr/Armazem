using ArmazemController;
using ArmazemModel;
using ArmazemModel.Entities;
using System;
using System.Data.Entity.Infrastructure;
using System.Windows;
using System.Windows.Controls;
using static ArmazemModel.Util;

namespace ArmazemUIs.Cadastros
{
    /// <summary>
    /// Interaction logic for CadastroProdutoUI.xaml
    /// </summary>
    public partial class CadastroProdutoUI : Window
    {
        ProdutoController ProdutoController { get; set; }
        Produto produtoSelecionado = new Produto();

        #region Construtores

        public CadastroProdutoUI()
        {
            IniciaJanela();
        }

        public CadastroProdutoUI(Produto produto)
        {
            IniciaJanela();

            produtoSelecionado = ProdutoController.PesquisaPorCodigo(produto.Codigo);
            PreencherFormulario();
            radsimples.IsEnabled = false;
            radComposto.IsEnabled = false;
        }

        #endregion

        #region Validações

        private bool ValidaPrecoInserido(TextBox txt)
        {
            return decimal.TryParse(txt.Text, out decimal preco);
        }

        private bool ValidaEstoqueAtual()
        {
            return int.TryParse(txtEstoqueAtual.Text, out int estoque);
        }

        private void ValidaSalvarFormulario()
        {
            if (string.IsNullOrWhiteSpace(txtDescricao.Text))
                throw new ValidationException("A descrição do produto é obrigatória!");
            if (!string.IsNullOrWhiteSpace(txtPrecoCusto.Text) && !ValidaPrecoInserido(txtPrecoCusto))
                throw new ValidationException("O preço de custo do produto é inválido!");
            if (!string.IsNullOrWhiteSpace(txtPrecoVenda.Text) && !ValidaPrecoInserido(txtPrecoVenda))
                throw new ValidationException("O preço de venda do produto é inválido!");
            if (!string.IsNullOrWhiteSpace(txtEstoqueAtual.Text) && !ValidaEstoqueAtual())
                throw new ValidationException("O estoque atual do produto é inválido!");
        }

        #endregion

        #region Operações

        private void IniciaJanela()
        {
            InitializeComponent();
            ProdutoController = new ProdutoController();
            ConfiguraTextBoxes();
        }

        private void ConfiguraTextBoxes()
        {
            txtPrecoCusto.ToMoney();
            txtPrecoVenda.ToMoney();
            txtEstoqueAtual.ToNumeric();
        }

        private void PreencherFormulario()
        {
            try
            {
                if (produtoSelecionado.Tipo.Equals((int)TIPO_PRODUTO.SIMPLES))
                    radsimples.IsChecked = true;
                else
                    radComposto.IsChecked = true;

                txtCodigo.Text = produtoSelecionado.Codigo.ToString();
                txtDescricao.Text = produtoSelecionado.Descricao;
                txtPrecoCusto.Text = produtoSelecionado.PrecoCusto.HasValue
                    ? produtoSelecionado.PrecoCusto.Value.ToString("n2")
                    : string.Empty;
                txtPrecoVenda.Text = produtoSelecionado.PrecoVenda.HasValue
                    ? produtoSelecionado.PrecoVenda.Value.ToString("n2")
                    : string.Empty;
                txtEstoqueAtual.Text = produtoSelecionado.EstoqueAtual.HasValue
                    ? produtoSelecionado.EstoqueAtual.Value.ToString()
                    : string.Empty;

            }
            catch (Exception ex)
            {
                Util.MensagemDeErro(ex);
            }
        }

        private void LimparFormulario()
        {
            statusBar.Text = string.Empty;

            produtoSelecionado = new Produto();

            txtCodigo.Text =
            txtDescricao.Text =
            txtPrecoCusto.Text =
            txtPrecoVenda.Text =
            txtEstoqueAtual.Text = string.Empty;

            radsimples.IsChecked = true;

            txtDescricao.Focus();
        }

        private void SalvarFormulario()
        {
            statusBar.Text = string.Empty;

            try
            {
                ValidaSalvarFormulario();

                produtoSelecionado.Descricao = txtDescricao.Text;
                produtoSelecionado.Tipo = radsimples.IsChecked.HasValue && radsimples.IsChecked.Value
                    ? (int)TIPO_PRODUTO.SIMPLES
                    : (int)TIPO_PRODUTO.COMPOSTO;

                produtoSelecionado.PrecoCusto = !string.IsNullOrWhiteSpace(txtPrecoCusto.Text)
                    ? decimal.Parse(txtPrecoCusto.Text)
                    : 0;
                produtoSelecionado.PrecoVenda = !string.IsNullOrWhiteSpace(txtPrecoVenda.Text)
                     ? decimal.Parse(txtPrecoVenda.Text)
                     : 0;
                produtoSelecionado.EstoqueAtual = !string.IsNullOrWhiteSpace(txtEstoqueAtual.Text)
                    ? int.Parse(txtEstoqueAtual.Text)
                    : 0;


                ProdutoController.Salvar(produtoSelecionado);

                txtCodigo.Text = produtoSelecionado.Codigo.ToString();
                statusBar.Text = "Produto gravado com sucesso";
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

        private void ExcluirRegistro()
        {
            statusBar.Text = string.Empty;

            try
            {
                if (Util.MensagemDeConfirmacao($"Deseja realmente excluir o produto {produtoSelecionado.Codigo}?"))
                {
                    ProdutoController.Deletar(produtoSelecionado.Codigo);
                    LimparFormulario();
                    statusBar.Text = statusBar.Text = "Produto excluído com sucesso.";
                }
            }
            catch (DbUpdateException)
            {
                Util.MensagemDeAtencao($"O produto {produtoSelecionado.Codigo} não pode ser excluído pois está relacionado a uma ou mais composições ou requisições de saída!");
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
            txtDescricao.Focus();
        }

        #region Button_Click

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            SalvarFormulario();
        }

        private void btnExluir_Click(object sender, RoutedEventArgs e)
        {
            ExcluirRegistro();
        }

        private void btnNovo_Click(object sender, RoutedEventArgs e)
        {
            produtoSelecionado = new Produto();
            LimparFormulario();
        }

        private void radsimples_Checked(object sender, RoutedEventArgs e)
        {
            txtPrecoCusto.IsEnabled =
                txtPrecoVenda.IsEnabled =
                txtEstoqueAtual.IsEnabled = true;
        }

        private void radComposto_Checked(object sender, RoutedEventArgs e)
        {
            txtPrecoCusto.Text =
                txtPrecoVenda.Text =
                txtEstoqueAtual.Text = string.Empty;
            txtPrecoCusto.IsEnabled = 
                txtPrecoVenda.IsEnabled = 
                txtEstoqueAtual.IsEnabled = false;
        }

        #endregion

        #endregion
    }
}


//TODO arrumar os namespaces das UIs