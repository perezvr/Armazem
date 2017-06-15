using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmazemModel
{
    public class DAL<T> : IDAL<T> where T : class
    {
        public ArmazemEntities Contexto;

        protected DAL()
        {
            Contexto = new ArmazemEntities();
        }

        /// <summary>
        /// Adiciona um objeto no banco de dados
        /// </summary>
        /// <param name="objeto">Objeto a ser incluído no banco de dados</param>
        public void Add(T objeto)
        {
            Contexto.Set<T>().Add(objeto);
            Contexto.SaveChanges();
        }

        /// <summary>
        /// Adiciona um objeto no banco de dados dentro de um contexto já existente
        /// </summary>
        /// <param name="objeto">Objeto a ser incluído no banco de dados</param>
        /// <param name="contexto">Contexto de entidades</param>
        public void Add(T objeto, ArmazemEntities contexto)
        {
            Contexto = contexto;

            Contexto.Set<T>().Add(objeto);
            Contexto.SaveChanges();
        }

        /// <summary>
        /// Remove do banco de dados objetos que correspondam à expressão
        /// </summary>
        /// <param name="expressao">Expressão utilizada para filtrar itens a serem excluídos</param>
        public void Delete(Func<T, bool> expressao)
        {
            Contexto.Set<T>().RemoveRange(Contexto.Set<T>().Where(expressao));
            Contexto.SaveChanges();
        }

        /// <summary>
        /// Remove do banco de dados objetos que correspondam à expressão dentro de um contexto já existente
        /// </summary>
        /// <param name="expressao">Expressão utilizada para filtrar itens a serem excluídos</param>
        /// <param name="contexto">Contexto de entidades</param>
        public void Delete(Func<T, bool> expressao, ArmazemEntities contexto)
        {
            Contexto = contexto;

            Contexto.Set<T>().RemoveRange(Contexto.Set<T>().Where(expressao));
            Contexto.SaveChanges();
        }

        /// <summary>
        /// Procura um objeto no banco de dados pela chave primária
        /// </summary>
        /// <param name="id">Chave primária</param>
        /// <returns>Objeto encontrado pela chave primária</returns>
        public T FindById(int id)
        {
            return Contexto.Set<T>().Find(id);
        }

        /// <summary>
        /// Atualiza um registro no banco de dados
        /// </summary>
        /// <param name="objeto">Objeto a ser atualizado no banco de dados</param>
        public void Update(T objeto)
        {
            Contexto.Set<T>().Attach(objeto);
            Contexto.Entry(objeto).State = EntityState.Modified;
            Contexto.SaveChanges();
        }

        /// <summary>
        /// Atualiza um registro no banco de dados dentro de um contexto já existente
        /// </summary>
        /// <param name="objeto">Objeto a ser atualizado no banco de dados</param>
        /// <param name="contexto">Contexto de entidades</param>
        public void Update(T objeto, ArmazemEntities contexto)
        {
            Contexto = contexto;

            Contexto.Set<T>().Attach(objeto);
            Contexto.Entry(objeto).State = EntityState.Modified;
            Contexto.SaveChanges();
        }

        public List<T> GetList(Func<T, bool> expressao)
        {
            return Contexto.Set<T>().Where(expressao).ToList();
        }

        public T Get(Func<T, bool> predicate)
        {
            return Contexto.Set<T>().Where(predicate).FirstOrDefault();
        }
    }
}
