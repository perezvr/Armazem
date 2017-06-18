using ArmazemModel;
using ArmazemModel.DAL;
using ArmazemModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArmazemController
{
    public class ComposicaoController
    {
        ComposicaoDAL composicaoDAL = null;
        ItemComposicaoController itemComposicaoController;


        public ComposicaoController()
        {
            itemComposicaoController = new ItemComposicaoController();
            composicaoDAL = new ComposicaoDAL();
        }

        /// <summary>
        /// Valida inclusão de itens para uma composição
        /// </summary>
        private void ValidaAdicionarItem(ItemComposicao itemComposicao)
        {
            if (itemComposicao.Produto.Codigo.Equals(0))
                throw new ValidationException("Selecione um item.");
            if (itemComposicao.Qtde.Equals(0))
                throw new ValidationException("Informe a quantidade do item.");

        }

        public void AdidionarItem(Composicao composicao, ItemComposicao itemComposicao)
        {
            try
            {
                ValidaAdicionarItem(itemComposicao);
                composicao.ItensComposcicao.Add(itemComposicao);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void RemoverItem(Composicao composicao, ItemComposicao item)
        {
            try
            {
                composicao.ItensComposcicao.Remove(item);
            }
            catch (Exception)
            {

                throw;
            }
        }


        public void Salvar(Composicao composicao)
        {
            try
            {
                //TODO validar
                if (composicaoDAL.FindById(composicao.Id) == null)
                    composicaoDAL.Add(composicao);
                else
                    composicaoDAL.Update(composicao);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SetContext(ArmazemEntities contexto)
        {
            composicaoDAL.Contexto = contexto;
        }

        public List<Composicao> ListarTodas()
        {
            try
            {
                return composicaoDAL.GetAll().ToList() ?? new List<Composicao>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Deletar(Composicao composicao)
        {
            UnitOfWork unitOfWork = null;
            composicao = composicaoDAL.GetDB(x => x.Id.Equals( composicao.Id));

            try
            {
                unitOfWork = new UnitOfWork();
                unitOfWork.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                composicaoDAL.SetContext(unitOfWork.Context);
                itemComposicaoController.SetContext(unitOfWork.Context);

                composicao.ItensComposcicao.ForEach(x => itemComposicaoController.Excluir(x));
                composicaoDAL.Delete(x => x.Id.Equals(composicao.Id));

                unitOfWork.Commit();

            }
            catch (Exception)
            {
                if (unitOfWork != null)
                    unitOfWork.RollBack();
                throw;
            }
        }

        public Composicao PesquisaPorProdutoCodigo(int codigo)
        {
            try
            {
                return composicaoDAL.Get(x => x.ProdutoCodigo.Equals(codigo)) ?? new Composicao();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Composicao PesquisaPorId(int id)
        {
            try
            {
                return composicaoDAL.FindById(id) ?? new Composicao();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
