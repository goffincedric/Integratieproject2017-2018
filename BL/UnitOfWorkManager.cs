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

        public int Save()
        {
            return UnitOfWork.CommitChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await UnitOfWork.CommitChangesAsync();
        }
    }
}
