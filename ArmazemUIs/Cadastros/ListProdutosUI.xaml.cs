﻿using ArmazemController;
using ArmazemModel;
using ArmazemModel.Entities;
using System;
using System.Data.Entity.Infrastructure;
using System.Windows;
using System.Windows.Input;

namespace ArmazemUIs.Cadastros
{
    /// <summary>
    /// Interaction logic for ListProdutosUI.xaml
    /// </summary>
    public partial class ListProdutosUI : Window
    {
        ProdutoController Produto_Controller { get; set; }

        public ListProdutosUI()
        {
            InitializeComponent();
            Produto_Controller = new ProdutoController();
        }

        private void AtualizaListaDeProdutos()
        {
            gridProdutos.ItemsSource = Produto_Controller.ListarTodos();
        }

        #region Operações

        private void IncluirNovoRegistro()
        {
            statusBar.Text = string.Empty;

            try
            {
                CadastroProdutoUI cadastroProdutosUI = new CadastroProdutoUI();
                cadastroProdutosUI.Owner = this;
                cadastroProdutosUI.ShowDialog();
                AtualizaListaDeProdutos();
            }
            catch (Exception ex)
            {
                statusBar.Text = ex.InnerException != null
                                 ? ex.InnerException.Message
                                 : ex.Message;
            }
        }

        private void ExcluirRegistro()
        {
            statusBar.Text = string.Empty;
            Produto produto = (Produto)gridProdutos.SelectedItem;

            try
            {
                if (produto != null)
                {

                    if (Util.MensagemDeConfirmacao($"Deseja realmente excluir o produto {produto.Codigo}?"))
                    {
                        Produto_Controller.Deletar(produto.Codigo);
                        AtualizaListaDeProdutos();
                        statusBar.Text = "Produto excluído com sucesso.";
                    }
                }

                else
                    throw new ValidationException("Selecione um registo.");
            }
            catch (ValidationException ex)
            {
                statusBar.Text = ex.Message;
            }
            catch (DbUpdateException)
            {
                Util.MensagemDeAtencao($"O produto {produto.Codigo} não pode ser excluído pois está relacionado a uma ou mais requisições de saída!");

            }
            catch (Exception ex)
            {
                statusBar.Text = ex.InnerException != null
                                        ? ex.InnerException.Message
                                        : ex.Message;
            }
        }

        private void SelecionarRegistroParaEdicao()
        {
            statusBar.Text = string.Empty;
            try
            {
                if (gridProdutos.SelectedItem != null)
                {

                    CadastroProdutoUI cadastroProdutoUI = new CadastroProdutoUI(((Produto)gridProdutos.SelectedItem));
                    cadastroProdutoUI.Owner = this;
                    cadastroProdutoUI.ShowDialog();
                    AtualizaListaDeProdutos();

                }
                else
                    throw new ValidationException("Selecione um registo.");

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

        #endregion

        #region Eventos

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AtualizaListaDeProdutos();
        }
        private void gridProdutos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelecionarRegistroParaEdicao();
        }


        #region Button_Click

        private void btnSelecionar_Click(object sender, RoutedEventArgs e)
        {
            SelecionarRegistroParaEdicao();
        }

        private void btnNovo_Click(object sender, RoutedEventArgs e)
        {
            IncluirNovoRegistro();
        }

        private void btnExcluir_Click(object sender, RoutedEventArgs e)
        {
            ExcluirRegistro();
        }

        #endregion

        #endregion
    }
}

//TODO arrumar o GetPrecoCusto para contemplar o Produto Composto

