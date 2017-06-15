using ArmazemModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ArmazemModel.Util;

namespace ArmazemController
{
    public static class Produto_Controller
    {
        static ProdutoDAL produtoDAL = new ProdutoDAL();

        private static void ValidSaveProduto(Produto produto)
        {
            if (string.IsNullOrWhiteSpace(produto.Descricao))
                throw new ValidationException("A descrição do produto é obrigatória!");
            if (produto.Tipo.Equals(0))
                throw new ValidationException("O tipo do produto é obrigatório!");
        }

        public static void Salvar(Produto produto)
        {
            try
            {
                ValidSaveProduto(produto);

                if (produto.Codigo.Equals(0))
                    produtoDAL.Add(produto);
                else
                    produtoDAL.Update(produto);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static Produto PesquisaPorCodigo(int codigo)
        {
            try
            {
                return produtoDAL.FindById(codigo) ?? new Produto();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static Produto PesquisaProdutoSimplesPorCodigo(int codigo)
        {
            try
            {
                return produtoDAL.Get(x => x.Tipo.Equals((int)TIPO_PRODUTO.SIMPLES) && x.Codigo.Equals(codigo)) ?? new Produto();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Deletar(int codigo)
        {
            try
            {
                produtoDAL.Delete(x => x.Codigo.Equals(codigo));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<Produto> Listar(Func<Produto, bool> expressao)
        {
            try
            {
                return produtoDAL.GetList(expressao) ?? new List<Produto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<Produto> ListarPorDescricao(string descricao)
        {
            return produtoDAL.GetList(x => x.Descricao.ToUpper().Contains(descricao.ToUpper()));
        }

    }
}

//TODO colocar máscara de moeda nas grids
