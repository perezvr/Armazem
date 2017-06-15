using ArmazemModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmazemController
{
    public static class Requisicao_Controller
    {
        static RequisicaoDAL dal = new RequisicaoDAL();

        public static Requisicao BuscaPorCodigo(int codigo)
        {
            try
            {
                return dal.FindById(codigo);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
