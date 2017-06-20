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

namespace ArmazemUIs.Requisicoes
{
    /// <summary>
    /// Interaction logic for SaidaMercadoriaUI.xaml
    /// </summary>
    public partial class SaidaMercadoriaUI : Window
    {
        RequisicaoController RequisicaoController { get; set; }
        ProdutoController ProdutoController { get; set; }
        Produto itemRequisicaoSelecionado = new Produto();

        #region Construtores

        public SaidaMercadoriaUI()
        {
            IniciaJanela();
        }

        public SaidaMercadoriaUI(Requisicao requisicao)
        {
            IniciaJanela();

            RequisicaoController.Requisicao = RequisicaoController.PesquisaPorId(requisicao.Id);
            PreencherFormulario();
        }

        #endregion

        #region Validações

        private void ValidaAdicionarItem()
        {
            if (itemRequisicaoSelecionado.Codigo.Equals(0))
                throw new ValidationException("Selecione um item.");
            if (string.IsNullOrWhiteSpace(txtQtdeItem.Text) || int.Parse(txtQtdeItem.Text).Equals(0))
                throw new ValidationException("Informe a quantidade do item.");
        }

        private void ValidaSalvarFormulario()
        {
            if (string.IsNullOrWhiteSpace(txtResponsavel.Text))
                throw new ValidationException("Selecione o responsável pela requisição!");
            if (dtpAbertura.SelectedDate == null)
                throw new ValidationException("Informe a data da requisição!");
            if (RequisicaoController.Requisicao.Id.Equals(0) && dtpAbertura.SelectedDate.Value.Date < DateTime.Now.Date)
                throw new ValidationException("A data da requisição não pode ser menor que a data atual!");
            if (RequisicaoController.Requisicao.ItensRequisicao.Count.Equals(0))
                throw new ValidationException("Insira ao menos um item para a requisição!");
        }

        #endregion

        #region Operações

        private void PreencherFormulario()
        {
            txtCodigo.Text = RequisicaoController.Requisicao.Id.ToString();
            txtResponsavel.Text = RequisicaoController.Requisicao.Responsavel;
            dtpAbertura.SelectedDate = RequisicaoController.Requisicao.DataAbertura;
            AtualizaListaDeItens();
            AtualizaCustoTotal();
        }

        private void IniciaJanela()
        {
            InitializeComponent();
            RequisicaoController = new RequisicaoController();
            ProdutoController = new ProdutoController();
            ConfiguraTextBoxes();
            dtpAbertura.SelectedDate = DateTime.Now;
        }

        private void ConfiguraTextBoxes()
        {
            txtCodigoItem.ToNumeric();
            txtQtdeItem.ToNumeric();
        }
        
        private void LimparFormulario()
        {
            statusBar.Text = string.Empty;

            RequisicaoController.Requisicao = new Requisicao();

            txtCodigo.Text =
            txtResponsavel.Text =
            txtCodigoItem.Text =
            txtDescricaoItem.Text =
            txtQtdeItem.Text =
            txtCustoTotal.Text = string.Empty;

            dtpAbertura.SelectedDate = DateTime.Now;

            AtualizaListaDeItens();
            AtualizaCustoTotal();

            txtCodigo.Focus();
        }

        private void AtualizaListaDeItens()
        {
            gridItens.ItemsSource = RequisicaoController.Requisicao.ItensRequisicao;
            gridItens.Items.Refresh();
        }
        
        private void AtualizaCustoTotal()
        {
            txtCustoTotal.Text = RequisicaoController.Requisicao.ItensRequisicao.Sum(x => x.Qtde * x.Produto.PrecoCusto).Value.ToString("n2");
        }

        private void CarregaProdutoPorCodigo(int codigo)
        {
            statusBar.Text = string.Empty;

            try
            {

                if (!string.IsNullOrWhiteSpace(txtCodigoItem.Text))
                {

                    itemRequisicaoSelecionado = ProdutoController.PesquisaPorCodigo(codigo);
                    if (!itemRequisicaoSelecionado.Codigo.Equals(0))
                        PreencheItemRequisicao(itemRequisicaoSelecionado);
                    else
                        LimparItemRequisicao();
                }


            }
            catch (Exception ex)
            {
                Util.MensagemDeErro(ex);
            }
        }

        private void PreencheItemRequisicao(Produto item)
        {
            txtCodigoItem.Text = item.Codigo.ToString();
            txtDescricaoItem.Text = item.Descricao;
        }

        private void LimparItemRequisicao()
        {
            itemRequisicaoSelecionado = new Produto();

            txtCodigoItem.Text =
                txtDescricaoItem.Text =
                txtQtdeItem.Text = string.Empty;
        }
        
        private void BuscaDeProdutos()
        {
            try
            {
                BuscaProdutosUI buscaProdutosUI = new BuscaProdutosUI(TIPO_PRODUTO.TODOS);
                buscaProdutosUI.Owner = this;
                buscaProdutosUI.ShowDialog();

                if (buscaProdutosUI.ProdutoSelecionado.Codigo > 0)
                {
                    itemRequisicaoSelecionado = buscaProdutosUI.ProdutoSelecionado;
                    PreencheItemRequisicao(itemRequisicaoSelecionado);
                }
            }
            catch (Exception ex)
            {
                Util.MensagemDeErro(ex);
            }
        }

        private void AdicionarItem()
        {
            statusBar.Text = string.Empty;

            try
            {
                ValidaAdicionarItem();

                if (RequisicaoController.Requisicao.ItensRequisicao.Any(x => x.Produto.Codigo.Equals(itemRequisicaoSelecionado.Codigo)))
                    RequisicaoController.Requisicao.ItensRequisicao.Where(x => x.Produto.Codigo.Equals(itemRequisicaoSelecionado.Codigo)).FirstOrDefault().Qtde = int.Parse(txtQtdeItem.Text);
                else
                    RequisicaoController.AdidionarItem(itemRequisicaoSelecionado, int.Parse(txtQtdeItem.Text));

                AtualizaListaDeItens();
                AtualizaCustoTotal();
                LimparItemRequisicao();
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

        private void RemoverItem()
        {
            statusBar.Text = string.Empty;

            try
            {
                if (gridItens.SelectedItem != null)
                {
                    RequisicaoController.RemoverItem((ItemRequisicao)gridItens.SelectedItem);
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

        private void EfetivarRequisicao()
        {
            statusBar.Text = string.Empty;

            try
            {
                RequisicaoController.Efetivar();
                statusBar.Text = "Requisição efetivada com sucesso";
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
                ValidaSalvarFormulario();

                RequisicaoController.Requisicao.Responsavel = txtResponsavel.Text;
                RequisicaoController.Requisicao.DataAbertura = dtpAbertura.SelectedDate.Value;
                RequisicaoController.Salvar();

                txtCodigo.Text = RequisicaoController.Requisicao.Id.ToString();
                statusBar.Text = "Requisição gravada com sucesso";
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
                if (Util.MensagemDeConfirmacao($"Deseja realmente excluir a requisição do produto {RequisicaoController.Requisicao.Id}?"))
                {
                    RequisicaoController.Deletar(RequisicaoController.Requisicao);
                    LimparFormulario();
                    statusBar.Text = statusBar.Text = "Requisição excluída com sucesso.";
                }

            }
            catch (Exception ex)
            {
                Util.MensagemDeErro(ex);
            }
        }

        #endregion

        #region Eventos

        private void btnNovo_Click(object sender, RoutedEventArgs e)
        {
            LimparFormulario();
        }

        private void txtCodigoItem_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtCodigoItem.Text))
                CarregaProdutoPorCodigo(int.Parse(txtCodigoItem.Text));
        }

        private void txtCodigoItem_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.F3))
                BuscaDeProdutos();
        }

        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            AdicionarItem();
        }

        private void btnRemoverItem_Click(object sender, RoutedEventArgs e)
        {
            RemoverItem();
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            SalvarFormulario();
        }

        private void btnExcluir_Click(object sender, RoutedEventArgs e)
        {
            ExcluirRegistro();
        }

        private void btnEfetivar_Click(object sender, RoutedEventArgs e)
        {
            EfetivarRequisicao();
        }

        #endregion
    }
}
