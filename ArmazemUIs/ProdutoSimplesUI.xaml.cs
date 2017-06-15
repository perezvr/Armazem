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
    /// Interaction logic for ProdutoSimplesUI.xaml
    /// </summary>
    public partial class ProdutoSimplesUI : Window
    {
        Produto produtoSelecionado = new Produto();

        #region Construtores

        public ProdutoSimplesUI()
        {
            InitializeComponent();
        }

        public ProdutoSimplesUI(Produto produto)
        {
            InitializeComponent();
            produtoSelecionado = produto;
            PreencherFormulario();
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
        
        private void PreencherFormulario()
        {
            try
            {
                txtCodigo.Text = produtoSelecionado.Codigo.ToString();
                txtDescricao.Text = produtoSelecionado.Descricao;
                txtPrecoCusto.Text = produtoSelecionado.Preco_Custo.HasValue
                    ? produtoSelecionado.Preco_Custo.Value.ToString("n2")
                    : string.Empty;
                txtPrecoVenda.Text = produtoSelecionado.Preco_Venda.HasValue
                    ? produtoSelecionado.Preco_Venda.Value.ToString("n2")
                    : string.Empty;
                txtEstoqueAtual.Text = produtoSelecionado.Estoque_Atual.HasValue
                    ? produtoSelecionado.Estoque_Atual.Value.ToString()
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

            txtDescricao.Focus();
        }

        private void SalvarFormulario()
        {
            statusBar.Text = string.Empty;

            try
            {
                ValidaSalvarFormulario();

                produtoSelecionado.Descricao = txtDescricao.Text;
                produtoSelecionado.Tipo = (int)TIPO_PRODUTO.SIMPLES;
                produtoSelecionado.Preco_Custo = !string.IsNullOrWhiteSpace(txtPrecoCusto.Text)
                    ? decimal.Parse(txtPrecoCusto.Text)
                    : 0;
                produtoSelecionado.Preco_Venda = !string.IsNullOrWhiteSpace(txtPrecoVenda.Text)
                     ? decimal.Parse(txtPrecoVenda.Text)
                     : 0;
                produtoSelecionado.Estoque_Atual = !string.IsNullOrWhiteSpace(txtEstoqueAtual.Text)
                    ? int.Parse(txtEstoqueAtual.Text)
                    : 0;


                Produto_Controller.Salvar(produtoSelecionado);

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
                    Produto_Controller.Deletar(produtoSelecionado.Codigo);
                    LimparFormulario();
                    statusBar.Text = statusBar.Text = "Produto excluído com sucesso.";
                    produtoSelecionado = new Produto();
                }
            }
            catch (DbUpdateException)
            {
                Util.MensagemDeAtencao($"O produto {produtoSelecionado.Codigo} não pode ser excluído pois está relacionado a uma ou mais requisições de saída!");
            }
            catch (Exception ex)
            {
                Util.MensagemDeErro(ex);
            }
        }

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

        #endregion

        #endregion

    }
}


//TODO arrumar os namespaces das UIs