using System.Collections.Generic;
using PB.BL.Domain.Accounts;

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

        ProfileAlert ReadProfileAlert(int profileAlertId);
        IEnumerable<ProfileAlert> ReadProfileAlerts();
        IEnumerable<ProfileAlert> ReadProfileAlerts(string userId);
        void UpdateProfileAlert(ProfileAlert profileAlert);
    }
}