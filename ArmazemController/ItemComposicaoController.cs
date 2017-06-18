using ArmazemModel;
using ArmazemModel.DAL;
using ArmazemModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmazemController
{
    public class ItemComposicaoController
    {
        ItemComposicaoDAL itemComposicaoDAL = null;

        public ItemComposicaoController()
        {
            itemComposicaoDAL = new ItemComposicaoDAL();
        }
        public void Salvar(ItemComposicao itemComposicao)
        {
            try
            {
                Produto produto = itemComposicao.Produto;
                itemComposicao.ProdutoCodigo = itemComposicao.Produto.Codigo;
                itemComposicao.Produto = null;

                if (itemComposicao.Id.Equals(0))
                    itemComposicaoDAL.Add(itemComposicao);
                else
                    itemComposicaoDAL.Update(itemComposicao);

                itemComposicao.Produto = produto;

            }
            catch (Exception)
            {
                throw;
            }
        } 

        public void DeletaPorComposicao(int id)
        {
            try
            {
                itemComposicaoDAL.Delete(x => x.ComposicaoId.Equals(id));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Excluir(ItemComposicao itemComposicao)
        {
            try
            {
                itemComposicaoDAL.Delete(x => x.Id.Equals(itemComposicao.Id));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void SetContext(ArmazemEntities contexto)
        {
            itemComposicaoDAL.Contexto = contexto;
        }
    }
}
