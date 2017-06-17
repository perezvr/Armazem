using ArmazemModel.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmazemModel.DAL
{
    interface IDAL<T> where T : class
    {
        void Add(T objeto);
        void Add(T objeto, ArmazemEntities contexto);
        void Update(T objeito);
        void Update(T objeito, ArmazemEntities contexto);
        void Delete(Func<T, bool> expressao);
        void Delete(Func<T, bool> expressao, ArmazemEntities contexto);
        T FindById(int id);
    }
}
