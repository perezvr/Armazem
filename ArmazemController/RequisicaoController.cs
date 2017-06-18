using ArmazemModel;
using ArmazemModel.DAL;
using ArmazemModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using static ArmazemModel.Util;

namespace ArmazemController
{
    public class RequisicaoController
    {
        RequisicaoDAL requisicaoDAL = null;
        public Requisicao Requisicao { get; set; }
        ItemRequisicaoController itemRequisicaoController;
        ComposicaoController composicaoController;
        ProdutoController produtoController;


        public RequisicaoController()
        {
            requisicaoDAL = new RequisicaoDAL();
            Requisicao = new Requisicao();
            itemRequisicaoController = new ItemRequisicaoController();
            composicaoController = new ComposicaoController();
            produtoController = new ProdutoController();
        }

        private void ValidaSalvarRequisicao()
        {

            if (Requisicao.Efetivado)
                throw new ValidationException("Requisição já efetivada, não pode ser alterada!");
            if (string.IsNullOrWhiteSpace(Requisicao.Responsavel))
                throw new ValidationException("Selecione o responsável pela requisição!");
            if (Requisicao.DataAbertura == null)
                throw new ValidationException("informe a data da requisição!");
            if (Requisicao.Id.Equals(0) && Requisicao.DataAbertura.Date < DateTime.Now.Date)
                throw new ValidationException("A data da requisição não pode ser menor que a data atual!");
            if (Requisicao.ItensRequisicao.Count.Equals(0))
                throw new ValidationException("Insira ao menos um item para a requisição!");
        }

        public void Salvar()
        {
            try
            {
                ValidaSalvarRequisicao();

                if (Requisicao.DataEfetivacao < new DateTime(1900, 1, 1))
                    Requisicao.DataEfetivacao = new DateTime(1900, 1, 1);

                if (Requisicao.Id.Equals(0))
                    requisicaoDAL.Add(Requisicao);
                else
                    requisicaoDAL.Update(Requisicao);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Requisicao BuscaPorCodigo(int codigo)
        {
            try
            {
                return requisicaoDAL.FindById(codigo);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Requisicao> ListarTodas()
        {
            try
            {
                return requisicaoDAL.GetAllDB().ToList() ?? new List<Requisicao>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ValidaDeletarRequisicao()
        {
            if (Requisicao.Efetivado)
                throw new ValidationException("Requisição já efetivada, não pode ser excluída!");
        }

        public void Deletar(Requisicao requisicao)
        {
            ValidaDeletarRequisicao();

            UnitOfWork unitOfWork = null;
            requisicao = requisicaoDAL.GetDB(x => x.Id.Equals(requisicao.Id));

            try
            {
                unitOfWork = new UnitOfWork();
                unitOfWork.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                requisicaoDAL.SetContext(unitOfWork.Context);
                itemRequisicaoController.SetContext(unitOfWork.Context);

                requisicao.ItensRequisicao.ForEach(x => itemRequisicaoController.Excluir(x));
                requisicaoDAL.Delete(x => x.Id.Equals(requisicao.Id));

                unitOfWork.Commit();

            }
            catch (Exception)
            {
                if (unitOfWork != null)
                    unitOfWork.RollBack();
                throw;
            }
        }

        public Requisicao PesquisaPorId(int id)
        {
            try
            {
                return requisicaoDAL.FindById(id) ?? new Requisicao();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Valida inclusão de itens para uma composição
        /// </summary>
        private void ValidaAdicionarItem(ItemRequisicao itemRequisicao)
        {
            if (itemRequisicao.Produto.Codigo.Equals(0))
                throw new ValidationException("Selecione um item.");
            if (itemRequisicao.Qtde.Equals(0))
                throw new ValidationException("Informe a quantidade do item.");

        }

        public void AdidionarItem(Produto produto, int qtde)
        {
            try
            {
                ItemRequisicao itemRequisicao = new ItemRequisicao()
                {
                    Produto = produto,
                    Qtde = qtde,
                    PrecoCusto = produto.PrecoCusto.Value
                };

                ValidaAdicionarItem(itemRequisicao);
                Requisicao.ItensRequisicao.Add(itemRequisicao);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void RemoverItem(ItemRequisicao item)
        {
            try
            {
                Requisicao.ItensRequisicao.Remove(item);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void ValidabaixaDeEstoque()
        {
            Requisicao.ItensRequisicao.ForEach(x =>
            {
                if (x.Produto.Tipo.Equals((int)TIPO_PRODUTO.SIMPLES) && x.Produto.EstoqueAtual - x.Qtde < 0)
                    throw new ValidationException($"Não há estoque suficiente para o produto {x.Produto.Codigo}!");
                else if (x.Produto.Tipo.Equals((int)TIPO_PRODUTO.COMPOSTO))
                {
                    Composicao composicao = composicaoController.PesquisaPorProdutoCodigo(x.Produto.Codigo);

                    composicao.ItensComposcicao.ForEach(y =>
                    {
                        if (y.Produto.EstoqueAtual - (x.Qtde * y.Qtde) < 0)
                            throw new ValidationException($"Não há estoque suficiente para o produto {y.Produto.Codigo}!");
                    });

                }

            });
        }


        private void ValidaEfetivarRequisicao()
        {
            if (Requisicao.Id.Equals(0))
                throw new ValidationException("Selecione ou grave a requisição antes de efetivá-la!");
            if (Requisicao.Efetivado)
                throw new ValidationException("A requisicação já está efetivada!");
            ValidabaixaDeEstoque();

        }

        private void BaixarEstoque()
        {
            Requisicao.ItensRequisicao.ForEach(x =>
            {
                if (x.Produto.Tipo.Equals((int)TIPO_PRODUTO.SIMPLES))
                {
                    x.Produto.EstoqueAtual -= x.Qtde;
                    produtoController.Salvar(x.Produto);
                }
                else
                {
                    Composicao composicao = composicaoController.PesquisaPorProdutoCodigo(x.Produto.Codigo);

                    composicao.ItensComposcicao.ForEach(y =>
                    {
                        y.Produto.EstoqueAtual -= x.Qtde * y.Qtde;
                        produtoController.Salvar(y.Produto);
                    });
                }
            });
        }

        public void Efetivar()
        {
            UnitOfWork unitOfWork = null;
            Requisicao = requisicaoDAL.GetDB(x => x.Id.Equals(Requisicao.Id));

            try
            {
                ValidaEfetivarRequisicao();

                unitOfWork = new UnitOfWork();
                unitOfWork.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);

                requisicaoDAL.SetContext(unitOfWork.Context);
                produtoController.SetContext(unitOfWork.Context);
                composicaoController.SetContext(unitOfWork.Context);

                Requisicao.Efetivado = true;
                Requisicao.DataEfetivacao = DateTime.Now;

                requisicaoDAL.Update(Requisicao);
                BaixarEstoque();

                unitOfWork.Commit();

            }
            catch (Exception)
            {
                if (unitOfWork != null)
                    unitOfWork.RollBack();
                throw;
            }

        }
    }
}
