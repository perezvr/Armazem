namespace ArmazemModel.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Produto")]
    public class Produto
    {
        [Key]
        public int Codigo { get; set; }
        public string Descricao { get; set; }
        public int Tipo { get; set; }
        public Nullable<decimal> PrecoCusto { get; set; }
        public Nullable<decimal> PrecoVenda { get; set; }
        public Nullable<int> EstoqueAtual { get; set; }
    }
}
