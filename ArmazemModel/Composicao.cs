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
    
    public partial class Composicao
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Composicao()
        {
            this.Produto_Insumo = new HashSet<Produto_Insumo>();
        }
    
        public int Id { get; set; }
        public int Produto_Codigo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Produto_Insumo> Produto_Insumo { get; set; }
        public virtual Produto Produto { get; set; }
    }
}
