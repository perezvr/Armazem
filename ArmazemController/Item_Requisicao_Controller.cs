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
    public class Item_Requisicao_Controller
    {
        ItemRequisicaoDAL item_RequisicaoDAL = null;

        public Item_Requisicao_Controller()
        {
            item_RequisicaoDAL = new ItemRequisicaoDAL(); 
        }

        public ItemRequisicao BuscaPorCodigo(int id)
        {
            try
            {
                return item_RequisicaoDAL.FindById(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SetContext(ArmazemEntities contexto)
        {
            item_RequisicaoDAL.Contexto = contexto;
        }
    }
}
