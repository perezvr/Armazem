using ArmazemModel.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmazemModel.DAL
{
    public class ComposicaoDAL : DAL<Composicao>
    {
        /// <summary>
        /// Adiciona um objeto no banco de dados
        /// </summary>
        /// <param name="objeto">Objeto a ser incluído no banco de dados</param>
        public void Add(Composicao objeto)
        {
            Contexto.Entry(objeto.Produto).State = EntityState.Unchanged;

            foreach (var item in objeto.ItensComposcicao)
            {
                Contexto.Entry(item.Produto).State = EntityState.Unchanged;
            }

            Contexto.Composicao.Add(objeto);
            Contexto.SaveChanges();
        }

        /// <summary>
        /// Atualiza um registro no banco de dados
        /// </summary>
        /// <param name="objeto">Objeto a ser atualizado no banco de dados</param>
        public void Update(Composicao objeto)
        {

            Contexto.Entry(objeto.Produto).State = EntityState.Unchanged;

            foreach (var item in objeto.ItensComposcicao)
            {
                Contexto.Entry(item.Produto).State = EntityState.Unchanged;
            }

            //removendo os itens que foram retirados da composicao
            ArmazemEntities originalContext = new ArmazemEntities();
            var originalComposicao = originalContext.Composicao.Find(objeto.Id);

            originalComposicao.ItensComposcicao.ForEach( x => {
                if (!objeto.ItensComposcicao.Any(y => y.Id == x.Id))
                    Contexto.ItemComposicao.Remove(Contexto.ItemComposicao.Find(x.Id));

            });

            Contexto.Entry(objeto).State = EntityState.Modified;
            Contexto.SaveChanges();
        }
    }
}
