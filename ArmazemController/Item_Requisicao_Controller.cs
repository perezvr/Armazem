using ArmazemModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmazemController
{
    public static class Item_Requisicao_Controller
    {
        static Item_RequisicaoDAL item_RequisicaoDAL = new Item_RequisicaoDAL();

        public static Item_Requisicao BuscaPorCodigo(int id)
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
    }
}
