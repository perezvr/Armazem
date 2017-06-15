using ArmazemModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ArmazemModel.Util;

namespace ArmazemController
{
    public class Produto_Controller
    {
        Produto_Insumo_Controller insumosController;
        ProdutoDAL produtoDAL = null;

        public Produto_Controller()
        {
            produtoDAL = new ProdutoDAL();
            insumosController = new Produto_Insumo_Controller();
        }

        private static void ValidSaveProduto(Produto produto)
        {
            if (string.IsNullOrWhiteSpace(produto.Descricao))
                throw new ValidationException("A descrição do produto é obrigatória!");
            if (produto.Tipo.Equals(0))
                throw new ValidationException("O tipo do produto é obrigatório!");
        }

        public void Salvar(Produto produto)
        {
            UnitOfWork unitOfWork = null;

            try
            {
                ValidSaveProduto(produto);
                unitOfWork = new UnitOfWork(true);
                unitOfWork.BeginTransaction();

                SetContext(unitOfWork.Context);
                insumosController.SetContext(unitOfWork.Context);

                List<Produto_Insumo> insumos = produto.Produto_Insumo.ToList();
                produto.Produto_Insumo = null;

                if (produto.Codigo.Equals(0))
                    produtoDAL.Add(produto);
                else
                    produtoDAL.Update(produto);

                insumos.ForEach(x =>
                {
                    x.Produto_Codigo = produto.Codigo;
                    insumosController.Salvar(x);
                });

                unitOfWork.Commit();

            }
            catch (Exception)
            {
                if (unitOfWork != null)
                    unitOfWork.RollBack();
                throw;
            }
        }

        public Produto PesquisaPorCodigo(int codigo)
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

        public Produto PesquisaProdutoSimplesPorCodigo(int codigo)
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

        public void Deletar(int codigo)
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

        public List<Produto> Listar(Func<Produto, bool> expressao)
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

        public List<Produto> ListarTodos()
        {
            try
            {
                return produtoDAL.GetAll().ToList() ?? new List<Produto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Produto> ListarPorDescricao(string descricao)
        {
            return produtoDAL.GetList(x => x.Descricao.ToUpper().Contains(descricao.ToUpper()));
        }

        public void SetContext(ArmazemEntities contexto)
        {
            produtoDAL.Contexto = contexto;
        }

    }
}

//TODO colocar máscara de moeda nas grids
