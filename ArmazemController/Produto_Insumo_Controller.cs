using ArmazemModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmazemController
{
    public class Produto_Insumo_Controller
    {
        Produto_InsumoDAL produtoInsumoDAL = null;

        public Produto_Insumo_Controller()
        {
            produtoInsumoDAL = new Produto_InsumoDAL();
        }
        public void Salvar(Produto_Insumo produtoInsumo)
        {
            try
            {
                produtoInsumo.Produto_Codigo = produtoInsumo.Produto.Codigo;
                produtoInsumo.Produto = null;

                if (produtoInsumo.Id.Equals(0))
                    produtoInsumoDAL.Add(produtoInsumo);
                else
                    produtoInsumoDAL.Update(produtoInsumo);
            }
            catch (Exception)
            {
                throw;
            }
        } 
        
        public void SetContext(ArmazemEntities contexto)
        {
            produtoInsumoDAL.Contexto = contexto;
        }
    }
}
