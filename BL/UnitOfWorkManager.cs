using PB.DAL;

namespace PB.BL
{
    public class UnitOfWorkManager
    {
        private UnitOfWork uof;

        public UnitOfWork UnitOfWork
        {
            get { return uof ?? (uof = new UnitOfWork()); }
        }

        public void Save()
        {
            UnitOfWork.CommitChanges();
        }
    }
}
