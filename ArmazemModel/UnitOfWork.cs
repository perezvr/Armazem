namespace ArmazemModel
{
    public class UnitOfWork
    {
        public ArmazemEntities Context { get; set; }

        public UnitOfWork(bool configureContextForPerformance = false)
        {
            Context = new ArmazemEntities();

            if(configureContextForPerformance)
            {
                Context.Configuration.AutoDetectChangesEnabled = false;
                Context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                //Context.Configuration.LazyLoadingEnabled = false;
                Context.Configuration.ProxyCreationEnabled = false;
                Context.Configuration.UseDatabaseNullSemantics = false;
                Context.Configuration.ValidateOnSaveEnabled = false;
            }
        }

        public void BeginTransaction(System.Data.IsolationLevel? isoLevel = null)
        {
            if (isoLevel == null)
                Context.Database.BeginTransaction();
            else
                Context.Database.BeginTransaction((System.Data.IsolationLevel)isoLevel);
        }

        public void RollBack()
        {
            Context.Database.CurrentTransaction.Rollback();
        }

        public void Commit()
        {
            Context.Database.CurrentTransaction.Commit();
            Context.SaveChanges();
        }
    }
}
