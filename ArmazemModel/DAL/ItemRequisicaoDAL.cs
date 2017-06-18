using ArmazemModel.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmazemModel.DAL
{
    public class ItemRequisicaoDAL : DAL<ItemRequisicao>
    {
        public List<ItemRequisicao> ListReportRequisicoes(DateTime dataInicial, DateTime dataFinal)
        {
            var lista = (from ir in Contexto.ItemRequisicao
                         join r in Contexto.Requisicao on ir.RequisicaoId equals r.Id
                         where DbFunctions.TruncateTime(r.DataAbertura) >= dataInicial && DbFunctions.TruncateTime(r.DataAbertura) <= dataFinal
                         select ir);

            return lista.ToList();
        }

        public List<ItemRequisicao> ListReportSaidas(DateTime dataInicial, DateTime dataFinal)
        {
            var lista = (from ir in Contexto.ItemRequisicao
                         join r in Contexto.Requisicao on ir.RequisicaoId equals r.Id
                         where DbFunctions.TruncateTime(r.DataAbertura) >= dataInicial 
                            && DbFunctions.TruncateTime(r.DataAbertura) <= dataFinal
                            && r.Efetivado
                         select ir);

            return lista.ToList();
        }
    }
}
