namespace ArmazemModel.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Composicao")]
    public class Composicao
    {
        public Composicao()
        {
            ItensComposcicao = new List<ItemComposicao>();
        }

        [Key]
        public int Id { get; set; }
        public int ProdutoCodigo { get; set; }

        [ForeignKey("ProdutoCodigo")]
        public virtual Produto Produto { get; set; }
        public virtual List<ItemComposicao> ItensComposcicao { get; set; }
    }
}
