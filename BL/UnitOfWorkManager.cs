using System.Threading.Tasks;
using PB.DAL;

namespace PB.BL
{
    public class UnitOfWorkManager
    {
        private UnitOfWork uof;
        

        public UnitOfWork UnitOfWork => uof ?? (uof = new UnitOfWork());

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