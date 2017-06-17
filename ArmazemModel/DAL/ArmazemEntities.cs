namespace ArmazemModel.DAL
{
    using ArmazemModel.Entities;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Linq;

    public class ArmazemEntities : DbContext
    {
        // Your context has been configured to use a 'ArmazemEntities' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'ArmazemModel.DAL.ArmazemEntities' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'ArmazemEntities' 
        // connection string in the application configuration file.
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