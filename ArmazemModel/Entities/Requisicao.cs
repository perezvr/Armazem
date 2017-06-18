namespace ArmazemModel.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Requisicao")]
    public partial class Requisicao
    {
        public Requisicao()
        {
            ItensRequisicao = new List<ItemRequisicao>();
        }
    
        [Key]
        public int Id { get; set; }
        public DateTime DataAbertura { get; set; }
        public string Responsavel { get; set; }
        public bool Efetivado { get; set; }
        public DateTime DataEfetivacao { get; set; }
    
        public virtual List<ItemRequisicao> ItensRequisicao { get; set; }

        public string GetStringDataAbertura
        {
            get
            {
                return DataAbertura.ToString("dd/MM/yyyy");
            }
        }
    }
}
