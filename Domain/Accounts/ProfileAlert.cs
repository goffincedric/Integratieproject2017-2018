using Domain.Accounts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PB.BL.Domain.Accounts
{
    [DataContract]
    [Table("tblProfileAlerts")]
    public class ProfileAlert
    {
        [DataMember]
        [Key]
        public int ProfileAlertId { get; set; }
        [DataMember]
        public string UserId { get; set; }
        [DataMember]
        public int AlertId { get; set; }
        [DataMember]
        [Required]
        public bool IsRead { get; set; }
        [DataMember]
        [Required]
        public DateTime TimeStamp { get; set; }

        [DataMember]
        [Required]
        public Profile Profile { get; set; }

        [DataMember]
        [Required]
        public virtual Alert Alert { get; set; }

        public virtual List<WeeklyReviewProfileAlerts> WeeklyReviewsProfileAlerts { get; set; }

        public string GetTime()
        {
            string returnString = "";
            double timedifH = DateTime.Now.Subtract(TimeStamp).TotalHours;
            double timedifM = DateTime.Now.Subtract(TimeStamp).TotalMinutes;
            if (timedifH > 48)
            {
                returnString = TimeStamp.ToLongDateString() + " om " + TimeStamp.ToShortTimeString();
            }
            else if (timedifH > 24)
            {
                returnString = "Gisteren om " + TimeStamp.ToShortTimeString();
            }
            else if (timedifH > .9)
            {
                returnString = "Ongeveer " + timedifH.ToString("F0") + " uur geleden";
            }
            else if (timedifM > 1)
            {
                returnString = timedifM.ToString("F0") + " minuten geleden";
            }
            else
            {
                returnString = "Zonet";
            }
            return returnString;
        }

        public override bool Equals(object obj)
        {
            return obj is ProfileAlert profileAlert &&
                Profile.Id.Equals(profileAlert.Profile.Id) &&
                Alert.AlertId == profileAlert.Alert.AlertId &&
                TimeStamp.Date.Equals(profileAlert.TimeStamp.Date) &&
                Alert.Equals(profileAlert.Alert);
        }

        public override int GetHashCode()
        {
            var hashCode = -92159382;
            hashCode = hashCode * -1521134295 + TimeStamp.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Profile>.Default.GetHashCode(Profile);
            hashCode = hashCode * -1521134295 + EqualityComparer<Alert>.Default.GetHashCode(Alert);
            return hashCode;
        }
    }
}
