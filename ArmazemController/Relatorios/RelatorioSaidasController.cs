using ArmazemModel;
using ArmazemModel.DAL;
using ArmazemModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ArmazemModel.Util;

namespace ArmazemController.Relatorios
{
    public class RelatorioSaidasController
    {
        ItemRequisicaoDAL itemRequisicaoDAL;
        ComposicaoController composicaoController;

        public RelatorioSaidasController()
        {
            itemRequisicaoDAL = new ItemRequisicaoDAL();
            composicaoController = new ComposicaoController();
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
                List<ItemRequisicao> itensOriginais = itemRequisicaoDAL.ListReportSaidas(dataInicial, dataFinal);
                List<ItemRequisicao> listaFinal = new List<ItemRequisicao>();
                itensOriginais.ForEach(x =>
                {
                    if (x.Produto.Tipo.Equals((int)TIPO_PRODUTO.SIMPLES))
                        listaFinal.Add(x);
                    else
                    {
                        Composicao composicao = composicaoController.PesquisaPorProdutoCodigo(x.Produto.Codigo);

                        composicao.ItensComposcicao.ForEach(y =>
                        {
                                ItemRequisicao itemDecomposto = new ItemRequisicao()
                                {
                                    Produto = y.Produto,
                                    Qtde = x.Qtde * y.Qtde,
                                    RequisicaoId = x.Requisicao.Id,
                                    PrecoCusto = y.Produto.PrecoCusto.Value,
                                    PrecoVenda = y.Produto.PrecoVenda.Value
                                };

                                listaFinal.Add(itemDecomposto);
                        });
                    }
                });

                return listaFinal;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
