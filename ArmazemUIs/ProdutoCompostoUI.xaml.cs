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
using static ArmazemModel.Util;

namespace ArmazemUIs
{
    /// <summary>
    /// Interaction logic for ProdutoCompostoUI.xaml
    /// </summary>
    public partial class ProdutoCompostoUI : Window
    {
        Produto insumoSelecionado = new Produto();
        Produto produtoSelecionado = new Produto();


        public ProdutoCompostoUI()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Atualiza a exibição do custo total dos insumos para o produto composto
        /// </summary>
        private void AtualizaCustoTotal()
        {
            txtCustoTotal.Text = produtoSelecionado.Produto_Insumo.Sum(x => x.Qtde * x.Produto.Preco_Custo).Value.ToString("n2");
        }

        private void AtualizaListaDeInsumos()
        {
            //gridInsumos.ItemsSource = null;
            gridInsumos.ItemsSource = produtoSelecionado.Produto_Insumo;
            gridInsumos.Items.Refresh();
        }

        /// <summary>
        /// Acessa a tela de busca de produtos para insumo
        /// </summary>
        private void BuscaDeProdutos()
        {
            try
            {
                BuscaProdutosUI buscaProdutosUI = new BuscaProdutosUI();
                buscaProdutosUI.Owner = this;
                buscaProdutosUI.ShowDialog();

                if (buscaProdutosUI.ProdutoSelecionado.Codigo > 0)
                {
                    insumoSelecionado = buscaProdutosUI.ProdutoSelecionado;
                    PreencheInsumo(insumoSelecionado);

                }
            }
            catch (Exception ex)
            {
                Util.MensagemDeErro(ex);
            }


        }

        private void LimparInsumo()
        {
            insumoSelecionado = new Produto();

            txtCodigoInsumo.Text = 
                txtDescricaoInsumo.Text =
                txtQtdeInsumo.Text = string.Empty;
        }

        private void RemoverInsumo()
        {
            statusBar.Text = string.Empty;

            try
            {
                if (gridInsumos.SelectedItem != null)
                {
                    produtoSelecionado.Produto_Insumo.Remove((Produto_Insumo)gridInsumos.SelectedItem);
                    AtualizaListaDeInsumos();
                    AtualizaCustoTotal();
                }
                else
                    throw new ValidationException("Selecione um insumo.");
                
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

        private void PreencheInsumo(Produto insumo)
        {
            txtCodigoInsumo.Text = insumo.Codigo.ToString();
            txtDescricaoInsumo.Text = insumo.Descricao;
        }

        private void CarregaInsumoPorCodigo()
        {
            statusBar.Text = string.Empty;

            if (!string.IsNullOrWhiteSpace(txtCodigoInsumo.Text))
            {
                try
                {
                    insumoSelecionado = Produto_Controller.PesquisaProdutoSimplesPorCodigo(int.Parse(txtCodigoInsumo.Text));
                    if (!insumoSelecionado.Codigo.Equals(0))
                        PreencheInsumo(insumoSelecionado);
                    else
                        LimparInsumo();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        private bool ValidaQtdeInsumo()
        {
            return int.TryParse(txtQtdeInsumo.Text, out int estoque);
        }

        /// <summary>
        /// Valida inclusão de insumo para um produto composto
        /// </summary>
        private void ValidaAdicionarInsumo()
        {
            if (insumoSelecionado.Codigo.Equals(0))
                throw new ValidationException("Selecione um insumo.");
            if (!ValidaQtdeInsumo() || int.Parse(txtQtdeInsumo.Text).Equals(0))
                throw new ValidationException("Informe a quantidade do insumo.");

        }

        private bool ValidaPrecoInserido(TextBox txt)
        {
            return decimal.TryParse(txt.Text, out decimal preco);
        }

        private void ValidaSalvarFormulario()
        {
            if (string.IsNullOrWhiteSpace(txtDescricao.Text))
                throw new ValidationException("A descrição do produto é obrigatória!");
            if (!string.IsNullOrWhiteSpace(txtPrecoVenda.Text) && !ValidaPrecoInserido(txtPrecoVenda))
                throw new ValidationException("O preço de venda do produto é inválido!");
            if (produtoSelecionado.Produto_Insumo.Count.Equals(0))
                throw new ValidationException("Insira ao menos um insumo para o produto composto!");
        }

        /// <summary>
        /// Adiciona um insumo para um produto composto
        /// </summary>
        private void AdicionarInsumo()
        {
            statusBar.Text = string.Empty;

            try
            {
                ValidaAdicionarInsumo();

                Produto_Insumo insumo = new Produto_Insumo()
                {
                    Produto = insumoSelecionado,
                    Produto_Codigo = insumoSelecionado.Codigo,
                    Produto1 = produtoSelecionado,
                //    Qtde = int.Parse(txtQtdeInsumo.Text),
                };

                produtoSelecionado.Produto_Insumo.Add(insumo);
                AtualizaListaDeInsumos();
                AtualizaCustoTotal();
                LimparInsumo();

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

        private void SalvarFormulario()
        {
            statusBar.Text = string.Empty;

            try
            {
               // ValidaSalvarFormulario();

                produtoSelecionado.Descricao = txtDescricao.Text;
                produtoSelecionado.Tipo = (int)TIPO_PRODUTO.COMPOSTO;
                //produtoSelecionado.Preco_Custo = produtoSelecionado.Produto_Insumo.Sum(x => x.Qtde * x.Produto.Preco_Custo).Value;
                produtoSelecionado.Preco_Venda = !string.IsNullOrWhiteSpace(txtPrecoVenda.Text)
                     ? decimal.Parse(txtPrecoVenda.Text)
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

        #region Eventos

        private void gridInsumos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
        

        private void txtCodigoInsumo_LostFocus(object sender, RoutedEventArgs e)
        {
            CarregaInsumoPorCodigo();
        }

        #endregion

        private void txtCodigoInsumo_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = Util.TextoSomenteNumerico(e.Text);
        }

        private void txtCodigoInsumo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.F3))
                BuscaDeProdutos();
        }

        private void btnRemoverInsumo_Click(object sender, RoutedEventArgs e)
        {
            RemoverInsumo();
        }

        private void btnAddInsumo_Click(object sender, RoutedEventArgs e)
        {
            AdicionarInsumo();
        }

        private void btnNovo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            SalvarFormulario();
        }
    }
}


//TODO arrumar tabIndex de todas as telas
//TODO encapsular as mensagens de erro Exception
//TODO mudar as cores da grid