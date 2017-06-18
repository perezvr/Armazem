using ArmazemModel;
using ArmazemModel.DAL;
using ArmazemModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmazemController.Relatorios
{
    public class RelatorioRequisicoesController
    {
        ItemRequisicaoDAL itemRequisicaoDAL;

        public RelatorioRequisicoesController()
        {
            itemRequisicaoDAL = new ItemRequisicaoDAL();
        }

        private void ValidaPesquisa(DateTime dataInicial, DateTime dataFinal)
        {
            if (dataInicial == null)
                throw new ValidationException("Informe a data inicial!");
            if (dataFinal == null)
                throw new ValidationException("Informe a data final!");
            if (dataInicial > dataFinal)
                throw new ValidationException("A data final nao pode ser menor que a data inicial!");

        }


        public List<ItemRequisicao> Pesquisa(DateTime dataInicial, DateTime dataFinal)
        {
            try
            {
                ValidaPesquisa(dataInicial, dataFinal);
                return itemRequisicaoDAL.ListReportRequisicoes(dataInicial, dataFinal);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
