﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PB.BL.Domain.Account
{
    [Table("tblAlert")]
    public class Alert
    {
        [Key]
        public int AlertId { get; set; }
        [Required]
        public string Text { get; set; }
        public string Description { get; set; }
        public bool IsFlaggedImportant { get; set; }
        public bool IsRead { get; set; }
        [Required]
        public DateTime TimeStamp { get; set; }
        public string Username { get; set; }
        [Required]
        [ForeignKey("Username")]
        public Profile Profile { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is Alert)) return false;
            Alert alert = (Alert)obj;

            if (!alert.Text.ToLower().Equals(Text.ToLower())) return false;
            if (!alert.Description.ToLower().Equals(Description.ToLower())) return false;
            if (!alert.Username.ToLower().Equals(Username.ToLower())) return false;

            if (!alert.TimeStamp.Date.Equals(TimeStamp.Date)) return false;
            return true;
        }

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
            else if (timedifH > 1)
            {
                returnString = "Ongeveer " + timedifH.ToString("F0") + " uur geleden";
            }
            else if (timedifM > 1)
            {
                returnString = timedifM.ToString("F0") + " minuten geleden";
            } else
            {
                returnString = "Zonet";
            }
            return returnString;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return Description + "; " + Text + ";";
        }
    }
}
