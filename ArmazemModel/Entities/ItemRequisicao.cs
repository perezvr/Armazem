namespace ArmazemModel.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ItemRequisicao")]
    public partial class ItemRequisicao
    {
        [Key]
        public int Id { get; set; }
        public int Qtde { get; set; }
        public decimal PrecoCusto { get; set; }
        public decimal PrecoVenda { get; set; }
        public int RequisicaoId { get; set; }
        public int ProdutoCodigo { get; set; }

        [ForeignKey("ProdutoCodigo")]
        public virtual Produto Produto { get; set; }
        [ForeignKey("RequisicaoId")]
        public virtual Requisicao Requisicao { get; set; }

        public string GetSubTotal
        {
            get
            {
                return (Qtde * PrecoCusto).ToString("n2");
            }
        }

        public string GetSubTotalVenda
        {
            get
            {
                return (Qtde * PrecoVenda).ToString("n2");
            }
        }
    }
}
