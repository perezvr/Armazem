using ArmazemController;
using ArmazemModel;
using ArmazemModel.Entities;
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
    /// Interaction logic for ComposicaoUI.xaml
    /// </summary>
    public partial class ComposicaoUI : Window
    {
        ProdutoController Produto_Controller { get; set; }
        ComposicaoController ComposicaoController { get; set; }
        Composicao composicao = new Composicao();
        Produto itemComposicaoSelecionado = new Produto();
        Produto produtoSelecionado = new Produto();

        #region Construtores

        public ComposicaoUI()
        {
            InitializeComponent();
            Produto_Controller = new ProdutoController();
            ComposicaoController = new ComposicaoController();
        }

        public ComposicaoUI(Composicao composicao)
        {
            InitializeComponent();
            Produto_Controller = new ProdutoController();
            ComposicaoController = new ComposicaoController();

            this.composicao = ComposicaoController.PesquisaPorId(composicao.Id);
            produtoSelecionado = composicao.Produto;
            PreencherFormulario();
        }

        #endregion

        #region Validações

        private void ValidaSalvarFormulario()
        {
            if (string.IsNullOrWhiteSpace(txtDescricao.Text))
                throw new ValidationException("A descrição do produto é obrigatória!");
            if (composicao.ItensComposcicao.Count.Equals(0))
                throw new ValidationException("Insira ao menos um item para a composição!");
        }

        #endregion

        #region Operacoes


        /// <summary>
        /// Atualiza a exibição do custo total dos itens da composicao
        /// /// </summary>
        private void AtualizaCustoTotal()
        {
            txtCustoTotal.Text = composicao.ItensComposcicao.Sum(x => x.Qtde * x.Produto.PrecoCusto).Value.ToString("n2");
        }

        private void AtualizaListaDeItens()
        {
            gridInsumos.ItemsSource = composicao.ItensComposcicao;
            gridInsumos.Items.Refresh();
        }

        private void PreencherFormulario()
        {
            txtCodigo.Text = produtoSelecionado.Codigo.ToString();
            PreencheProduto(produtoSelecionado);
            AtualizaListaDeItens();
            AtualizaCustoTotal();
        }

        /// <summary>
        /// Acessa a tela de busca de produtos
        /// </summary>
        private void BuscaDeProdutos(TIPO_PRODUTO tipoProduto)
        {
            try
            {
                BuscaProdutosUI buscaProdutosUI = new BuscaProdutosUI(tipoProduto);
                buscaProdutosUI.Owner = this;
                buscaProdutosUI.ShowDialog();

                if (buscaProdutosUI.ProdutoSelecionado.Codigo > 0)
                {
                    switch (tipoProduto)
                    {
                        case TIPO_PRODUTO.SIMPLES:
                            itemComposicaoSelecionado = buscaProdutosUI.ProdutoSelecionado;
                            PreencheItemComposicao(itemComposicaoSelecionado);
                            break;
                        case TIPO_PRODUTO.COMPOSTO:
                            produtoSelecionado = buscaProdutosUI.ProdutoSelecionado;
                            PreencheProduto(produtoSelecionado);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Util.MensagemDeErro(ex);
            }
        }

        private void LimparItemComposicao()
        {
            itemComposicaoSelecionado = new Produto();

            txtCodigoInsumo.Text =
                txtDescricaoInsumo.Text =
                txtQtdeInsumo.Text = string.Empty;
        }

        private void LimparProduto()
        {
            produtoSelecionado = new Produto();

            txtCodigo.Text =
                txtDescricao.Text = string.Empty;
        }

        private void RemoverItem()
        {
            statusBar.Text = string.Empty;

            try
            {
                if (gridInsumos.SelectedItem != null)
                {
                    composicao.ItensComposcicao.Remove((ItemComposicao)gridInsumos.SelectedItem);
                    AtualizaListaDeItens();
                    AtualizaCustoTotal();
                }
                else
                    throw new ValidationException("Selecione um item.");

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

        private void PreencheItemComposicao(Produto item)
        {
            txtCodigoInsumo.Text = item.Codigo.ToString();
            txtDescricaoInsumo.Text = item.Descricao;
        }

        private void PreencheProduto(Produto produto)
        {
            txtCodigo.Text = produto.Codigo.ToString();
            txtDescricao.Text = produto.Descricao;
            txtPrecoVenda.Text = produto.PrecoVenda.Value.ToString("n2");
        }

        private void CarregaProdutoPorCodigo(int codigo, TIPO_PRODUTO tipo)
        {
            statusBar.Text = string.Empty;

            try
            {
                switch (tipo)
                {
                    case TIPO_PRODUTO.SIMPLES:
                        if (!string.IsNullOrWhiteSpace(txtCodigoInsumo.Text))
                        {

                            itemComposicaoSelecionado = Produto_Controller.PesquisaProduto(codigo, tipo);
                            if (!itemComposicaoSelecionado.Codigo.Equals(0))
                                PreencheItemComposicao(itemComposicaoSelecionado);
                            else
                                LimparItemComposicao();
                        }
                        break;
                    case TIPO_PRODUTO.COMPOSTO:
                        if (!string.IsNullOrWhiteSpace(txtCodigo.Text))
                        {

                            produtoSelecionado = Produto_Controller.PesquisaProduto(codigo, tipo);
                            if (!produtoSelecionado.Codigo.Equals(0))
                            {
                                composicao = ComposicaoController.PesquisaPorProdutoCodigo(int.Parse(txtCodigo.Text));
                                if (composicao.Id > 0)
                                    PreencherFormulario();
                                else
                                    PreencheProduto(produtoSelecionado);
                            }
                            else
                                LimparProduto();
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {

                throw;
            }

            if (!string.IsNullOrWhiteSpace(txtCodigoInsumo.Text))
            {
                try
                {
                    itemComposicaoSelecionado = Produto_Controller.PesquisaProduto(codigo, tipo);
                    if (!itemComposicaoSelecionado.Codigo.Equals(0))
                        PreencheItemComposicao(itemComposicaoSelecionado);
                    else
                        LimparItemComposicao();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        /// <summary>
        /// Adiciona um item na composição
        /// </summary>
        private void AdicionarItem()
        {
            statusBar.Text = string.Empty;

            try
            {
                ItemComposicao itemComposicao = new ItemComposicao()
                {
                    Produto = itemComposicaoSelecionado,
                    Qtde = int.Parse(txtQtdeInsumo.Text),
                };

                ComposicaoController.AdidionarItem(composicao, itemComposicao);
                AtualizaListaDeItens();
                AtualizaCustoTotal();
                LimparItemComposicao();
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

        /// <summary>
        /// Limpa o formulário
        /// </summary>
        private void LimparFormulario()
        {
            statusBar.Text = string.Empty;

            produtoSelecionado = new Produto();
            itemComposicaoSelecionado = new Produto();
            composicao = new Composicao();

            txtCodigo.Text =
            txtDescricao.Text =
            txtPrecoVenda.Text =
            txtCodigoInsumo.Text =
            txtDescricaoInsumo.Text =
            txtQtdeInsumo.Text = string.Empty;

            txtCodigo.Focus();
        }

        /// <summary>
        /// Salva o formulário
        /// </summary>
        private void SalvarFormulario()
        {
            statusBar.Text = string.Empty;

            try
            {
                ValidaSalvarFormulario();

                composicao.Produto = produtoSelecionado;
                ComposicaoController.Salvar(composicao);

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

        /// <summary>
        /// Exclui uma composição
        /// </summary>
        private void ExcluirRegistro()
        {
            statusBar.Text = string.Empty;

            try
            {
                if (Util.MensagemDeConfirmacao($"Deseja realmente excluir a composicao do produto {composicao.Produto.Codigo}?"))
                {
                    ComposicaoController.Deletar(composicao);
                    LimparFormulario();
                    statusBar.Text = statusBar.Text = "Composição excluída com sucesso.";
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Eventos

        private void gridInsumos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //TODO atualizar insumos
        }

        private void txtCodigoInsumo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtCodigoInsumo.Text))
                CarregaProdutoPorCodigo(int.Parse(txtCodigoInsumo.Text), TIPO_PRODUTO.SIMPLES);
        }

        private void txtCodigoInsumo_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = Util.TextoSomenteNumerico(e.Text);
        }

        private void txtCodigoInsumo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.F3))
                BuscaDeProdutos(TIPO_PRODUTO.SIMPLES);
        }

        private void btnRemoverInsumo_Click(object sender, RoutedEventArgs e)
        {
            RemoverItem();
        }

        private void btnAddInsumo_Click(object sender, RoutedEventArgs e)
        {
            AdicionarItem();
        }

        private void btnNovo_Click(object sender, RoutedEventArgs e)
        {
            LimparFormulario();
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            SalvarFormulario();
        }

        private void txtCodigo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtCodigo.Text))
                CarregaProdutoPorCodigo(int.Parse(txtCodigo.Text), TIPO_PRODUTO.COMPOSTO);
        }

        private void txtCodigo_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = Util.TextoSomenteNumerico(e.Text);
        }

        private void txtCodigo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.F3))
                BuscaDeProdutos(TIPO_PRODUTO.COMPOSTO);
        }

        private void btnExcluir_Click(object sender, RoutedEventArgs e)
        {
            ExcluirRegistro();
        }

        #endregion

    }
}


//TODO arrumar tabIndex de todas as telas
//TODO encapsular as mensagens de erro Exception
//TODO mudar as cores da grid