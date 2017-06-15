using ArmazemModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmazemController
{
    public class Item_Requisicao_Controller
    {
        Item_RequisicaoDAL item_RequisicaoDAL = null;

        public Item_Requisicao_Controller()
        {
            item_RequisicaoDAL = new Item_RequisicaoDAL(); 
        }

        public Item_Requisicao BuscaPorCodigo(int id)
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
