using System;
using PB.BL.Domain.Items;

namespace Domain.JSONConversion
{
    public class JPerson
    {
        public JPerson(int? id, string first_name, string last_name, string full_name, string district, string level,
            string gender, string twitter, string site, DateTime dateOfBirth, string facebook, string postalcode,
            string position, string organisation, string town)
        {
            Id = id ?? 0;
            First_name = first_name;
            Last_name = last_name;
            District = district;
            Level = level;
            switch (gender.ToLower())
            {
                case "m":
                    Gender = Gender.MALE;
                    break;
                case "v":
                    Gender = Gender.FEMALE;
                    break;
                default:
                    Gender = Gender.OTHERS;
                    break;
            }

            Twitter = twitter;
            Site = site;
            DateOfBirth = dateOfBirth;
            Facebook = facebook;
            Postalcode = postalcode;
            Full_name = full_name;
            Position = position;
            Organistion = organisation;
            Town = town;
        }

        public int Id { get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public string Full_name { get; set; }
        public string District { get; set; }
        public string Level { get; set; }
        public Gender Gender { get; set; }
        public string Twitter { get; set; }
        public string Site { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Facebook { get; set; }
        public string Postalcode { get; set; }
        public string Position { get; set; }
        public string Organistion { get; set; }
        public string Town { get; set; }
    }
}