//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ArmazemModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Produto
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Produto()
        {
            this.Item_Requisicao = new HashSet<Item_Requisicao>();
            this.Produto_Insumo = new HashSet<Produto_Insumo>();
            this.Produto_Insumo1 = new HashSet<Produto_Insumo>();
        }
    
        public int Codigo { get; set; }
        public string Descricao { get; set; }
        public int Tipo { get; set; }
        public Nullable<decimal> Preco_Custo { get; set; }
        public Nullable<decimal> Preco_Venda { get; set; }
        public Nullable<int> Estoque_Atual { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Item_Requisicao> Item_Requisicao { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Produto_Insumo> Produto_Insumo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Produto_Insumo> Produto_Insumo1 { get; set; }
    }
}