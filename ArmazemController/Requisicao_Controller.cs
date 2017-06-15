using ArmazemModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmazemController
{
    public class Requisicao_Controller
    {
        RequisicaoDAL requisicaoDAL = null;

        public Requisicao_Controller()
        {
            requisicaoDAL = new RequisicaoDAL();
        }

        public  void Salvar(Requisicao requisicao)
        {
            try
            {

                if (requisicao.Codigo.Equals(0))
                    requisicaoDAL.Add(requisicao);
                else
                    requisicaoDAL.Update(requisicao);
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

        public void SetContext(ArmazemEntities contexto)
        {
            requisicaoDAL.Contexto = contexto;
        }
    }
}
