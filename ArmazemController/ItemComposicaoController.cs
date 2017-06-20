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

        #region Operações

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

        #endregion

        public void SetContext(ArmazemEntities contexto)
        {
            itemComposicaoDAL.Contexto = contexto;
        }
    }
}
