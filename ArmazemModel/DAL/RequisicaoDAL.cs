using ArmazemModel.Entities;
using System.Data.Entity;
using System.Linq;

namespace ArmazemModel.DAL
{
    public class RequisicaoDAL : DAL<Requisicao>
    {
        /// <summary>
        /// Adiciona um objeto no banco de dados
        /// </summary>
        /// <param name="objeto">Objeto a ser incluído no banco de dados</param>
        public void Add(Requisicao objeto)
        {
            foreach (var item in objeto.ItensRequisicao)
            {
                Contexto.Entry(item.Produto).State = EntityState.Unchanged;
            }

            Contexto.Requisicao.Add(objeto);
            Contexto.SaveChanges();
        }

        /// <summary>
        /// Atualiza um registro no banco de dados
        /// </summary>
        /// <param name="objeto">Objeto a ser atualizado no banco de dados</param>
        public void Update(Requisicao objeto)
        {
            foreach (var item in objeto.ItensRequisicao)
            {
                Contexto.Entry(item.Produto).State = EntityState.Unchanged;
            }

            //removendo os itens que foram retirados da requisicao
            ArmazemEntities originalContext = new ArmazemEntities();
            var originalComposicao = originalContext.Requisicao.Find(objeto.Id);

            originalComposicao.ItensRequisicao.ForEach(x =>
            {
                if (!objeto.ItensRequisicao.Any(y => y.Id == x.Id))
                    Contexto.ItemRequisicao.Remove(Contexto.ItemRequisicao.Find(x.Id));

            });

            Contexto.Entry(objeto).State = EntityState.Modified;
            Contexto.SaveChanges();
        }
    }
}
