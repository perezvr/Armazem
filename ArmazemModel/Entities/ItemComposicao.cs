
namespace ArmazemModel.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ItemComposicao")]
    public partial class ItemComposicao
    {
        [Key]
        public int Id { get; set; }
        public int Qtde { get; set; }
        public int ProdutoCodigo { get; set; }
        public int ComposicaoId { get; set; }

        [ForeignKey("ComposicaoId")]
        public virtual Composicao Composicao { get; set; }
        [ForeignKey("ProdutoCodigo")]
        public virtual Produto Produto { get; set; }

        public string GetSubTotal
        {
            get
            {
                return (Qtde * Produto.PrecoCusto).Value.ToString("n2");
            }
        }
    }
}
