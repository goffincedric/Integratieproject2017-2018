using PB.BL.Domain.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.DAL.Interfaces
{
    public interface IAlertRepo
    {
        IEnumerable<Alert> ReadAlerts();
        Alert CreatAlert(Alert alert);
        IEnumerable<Alert> CreatAlerts(List<Alert> alerts);
        Alert ReadAlert(int alertId);
        void UpdateAlert(Alert alert);
        void DeleteAlert(int alertId);
    }
}
