namespace ArmazemModel.DAL
{
    using ArmazemModel.Entities;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Linq;

    public class ArmazemEntities : DbContext
    {
        public ArmazemEntities()
            : base("name=ArmazemEntities") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public virtual DbSet<Produto> Produto { get; set; }
        public virtual DbSet<Composicao> Composicao { get; set; }
        public virtual DbSet<ItemComposicao> ItemComposicao { get; set; }
        public virtual DbSet<Requisicao> Requisicao { get; set; }
        public virtual DbSet<ItemRequisicao> ItemRequisicao { get; set; }
    }
}