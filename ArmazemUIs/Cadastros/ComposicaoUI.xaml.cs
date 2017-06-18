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

namespace ArmazemUIs.Cadastros
{
    /// <summary>
    /// Interaction logic for ComposicaoUI.xaml
    /// </summary>
    public partial class ComposicaoUI : Window
    {
        ProdutoController ProdutoController { get; set; }
        ComposicaoController ComposicaoController { get; set; }
        Composicao composicao = new Composicao();
        Produto itemComposicaoSelecionado = new Produto();

        #region Construtores

        public ComposicaoUI()
        {
            IniciaJanela();
        }

        public ComposicaoUI(Composicao composicao)
        {
            IniciaJanela();

            this.composicao = ComposicaoController.PesquisaPorId(composicao.Id);
            PreencherFormulario();
            txtCodigo.IsEnabled = false;
        }

        #endregion

        #region Validações

        private void ValidaAdicionarItem()
        {
            if (itemComposicaoSelecionado.Codigo.Equals(0))
                throw new ValidationException("Selecione um item.");
            if (string.IsNullOrWhiteSpace(txtQtdeInsumo.Text) || int.Parse(txtQtdeInsumo.Text).Equals(0))
                throw new ValidationException("Informe a quantidade do item.");
        }

        private void ValidaSalvarFormulario()
        {
            if (composicao.Produto == null || composicao.Produto.Codigo.Equals(0))
                throw new ValidationException("Selecione o produto principal da composição!");
            if (composicao.ItensComposcicao.Count.Equals(0))
                throw new ValidationException("Insira ao menos um item para a composição!");
        }

        #endregion

        #region Operacoes

        private void IniciaJanela()
        {
            InitializeComponent();
            ProdutoController = new ProdutoController();
            ComposicaoController = new ComposicaoController();
            ConfiguraTextBoxes();
        }

        private void ConfiguraTextBoxes()
        {
            txtCodigoInsumo.ToNumeric();
            txtQtdeInsumo.ToNumeric();
        }


        /// <summary>
        /// Atualiza a exibição do custo total dos itens da composicao
        /// /// </summary>
        private void AtualizaCustoTotal()
        {
            txtCustoTotal.Text = composicao.ItensComposcicao.Sum(x => x.Qtde * x.Produto.PrecoCusto).Value.ToString("n2");
            txtPrecoVenda.Text = composicao.ItensComposcicao.Sum(x => x.Qtde * x.Produto.PrecoVenda).Value.ToString("n2");

        }

        private void AtualizaListaDeItens()
        {
            gridInsumos.ItemsSource = composicao.ItensComposcicao;
            gridInsumos.Items.Refresh();
        }

        private void PreencherFormulario()
        {
            txtCodigo.Text = composicao.Produto.Codigo.ToString();
            PreencheProduto(composicao.Produto);
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
                            composicao.Produto = buscaProdutosUI.ProdutoSelecionado;
                            PreencheProduto(composicao.Produto);
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
            composicao.Produto = new Produto();

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
                    ComposicaoController.RemoverItem(composicao,(ItemComposicao)gridInsumos.SelectedItem);
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
            txtPrecoVenda.Text = composicao.ItensComposcicao.Sum(x => x.Qtde * x.Produto.PrecoVenda).Value.ToString("n2");
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

                            itemComposicaoSelecionado = ProdutoController.PesquisaProduto(codigo, tipo);
                            if (!itemComposicaoSelecionado.Codigo.Equals(0))
                                PreencheItemComposicao(itemComposicaoSelecionado);
                            else
                                LimparItemComposicao();
                        }
                        break;
                    case TIPO_PRODUTO.COMPOSTO:
                        if (!string.IsNullOrWhiteSpace(txtCodigo.Text))
                        {
                            composicao = ComposicaoController.PesquisaPorProdutoCodigo(int.Parse(txtCodigo.Text));

                            if (composicao.Id > 0)
                            {
                                Util.MensagemDeInformacao("Já existe uma composição para o produto selecionado.");
                                LimparFormulario();
                            }
                            else
                            {
                                composicao = new Composicao();
                                composicao.Produto = ProdutoController.PesquisaProduto(int.Parse(txtCodigo.Text), tipo);

                                if (composicao.Produto.Codigo > 0)
                                    PreencheProduto(composicao.Produto);
                                else
                                    LimparFormulario();
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Util.MensagemDeErro(ex);
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

                ValidaAdicionarItem();
                ItemComposicao itemComposicao = new ItemComposicao()
                {
                    Produto = itemComposicaoSelecionado,
                    Qtde = int.Parse(txtQtdeInsumo.Text),
                };

                if (composicao.ItensComposcicao.Any(x => x.Produto.Codigo.Equals(itemComposicaoSelecionado.Codigo)))
                    composicao.ItensComposcicao.Where(x => x.Produto.Codigo.Equals(itemComposicaoSelecionado.Codigo)).FirstOrDefault().Qtde = int.Parse(txtQtdeInsumo.Text);
                else
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

            itemComposicaoSelecionado = new Produto();
            composicao = new Composicao();

            txtCodigo.Text =
            txtDescricao.Text =
            txtPrecoVenda.Text =
            txtCodigoInsumo.Text =
            txtDescricaoInsumo.Text =
            txtQtdeInsumo.Text = string.Empty;

            AtualizaListaDeItens();
            AtualizaCustoTotal();

            txtCodigo.IsEnabled = true;
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

                composicao.Produto.PrecoCusto = composicao.ItensComposcicao.Sum(x => x.Produto.PrecoCusto * x.Qtde);
                composicao.Produto.PrecoVenda = composicao.ItensComposcicao.Sum(x => x.Produto.PrecoVenda * x.Qtde);
                ComposicaoController.Salvar(composicao);

                txtCodigo.Text = composicao.Produto.Codigo.ToString();
                statusBar.Text = "Composição gravada com sucesso";
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
            catch (Exception ex)
            {
                Util.MensagemDeErro(ex);
            }
        }

        #endregion

        #region Eventos

        private void txtCodigoInsumo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtCodigoInsumo.Text))
                CarregaProdutoPorCodigo(int.Parse(txtCodigoInsumo.Text), TIPO_PRODUTO.SIMPLES);
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