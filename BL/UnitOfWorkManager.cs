using PB.DAL;
using System.Threading.Tasks;

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

        public async Task<int> SaveAsync()
        {
            return await UnitOfWork.CommitChangesAsync();
        }
    }
}
