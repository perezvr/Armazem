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
    public class ItemRequisicaoController
    {
        ItemRequisicaoDAL itemRequisicaoDAL = null;

        public ItemRequisicaoController()
        {
            itemRequisicaoDAL = new ItemRequisicaoDAL(); 
        }

        public ItemRequisicao BuscaPorCodigo(int id)
        {
            try
            {
                return itemRequisicaoDAL.FindById(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Excluir(ItemRequisicao itemRequisicao)
        {
            try
            {
                itemRequisicaoDAL.Delete(x => x.Id.Equals(itemRequisicao.Id));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void SetContext(ArmazemEntities contexto)
        {
            itemRequisicaoDAL.Contexto = contexto;
        }
    }
}
