using ArmazemModel;
using ArmazemModel.DAL;
using ArmazemModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using static ArmazemModel.Util;

namespace ArmazemController
{
    public class ProdutoController
    {
        ItemComposicaoController itemComposicaoController;
        ProdutoDAL produtoDAL = null;

        public ProdutoController()
        {
            produtoDAL = new ProdutoDAL();
            itemComposicaoController = new ItemComposicaoController();
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
            try
            {
                ValidSaveProduto(produto);


                if (produtoDAL.FindById(produto.Codigo) == null)
                    produtoDAL.Add(produto);
                else
                    produtoDAL.Update(produto);

            }
            catch (Exception)
            {
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

        public Produto PesquisaProduto(int codigo, TIPO_PRODUTO tipo)
        {
            try
            {
                return produtoDAL.Get(x => x.Tipo.Equals((int)tipo) && x.Codigo.Equals(codigo)) ?? new Produto();
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

        public List<Produto> ListarPorDescricaoETipo(string descricao, TIPO_PRODUTO tipo)
        {
            return produtoDAL.GetList(x => x.Descricao.ToUpper().Contains(descricao.ToUpper()) && x.Tipo.Equals((int)tipo));
        }

        public void SetContext(ArmazemEntities contexto)
        {
            produtoDAL.Contexto = contexto;
        }

    }
}

//TODO colocar máscara de moeda nas grids
