namespace PB.DAL.Migrations
{
    using PB.BL.Domain.Accounts;
    using PB.BL.Domain.Dashboards;
    using PB.BL.Domain.Items;
    using PB.BL.Domain.Platform;
    using PB.BL.Domain.Settings;
    using PB.DAL.EF;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<IntegratieDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(IntegratieDbContext ctx)
        {
            Subplatform pbSubplatform = ctx.Subplatforms.FirstOrDefaultAsync(s => s.Name.ToLower().Equals("Politieke Barometer".ToLower())).Result;

            //Makes PB subplatform

            if (pbSubplatform == null)
            {
                pbSubplatform = new Subplatform()
                {
                    Name = "Politieke Barometer",
                    URL = "DUMMYURL",
                    DateOnline = DateTime.Now,
                    Settings = new List<SubplatformSetting>()
                    {
                        new SubplatformSetting()
                        {
                            SettingName = Setting.Platform.DAYS_TO_KEEP_RECORDS,
                            Value = "31"
                        }
                    },
                    Admins = new List<Profile>(),
                    Items = new List<Item>(),
                    Pages = new List<Page>()
                };
            }

            List<Item> OrganisationsToAdd = new List<Item>()
            {
                new Organisation()
                {
                    Name = "PVDA",
                    FullName = "Partij van de Arbeid",
                    IconURL=@"~/Content/Images/Partijen/pvda.jpg",
                    Keywords = new List<Keyword>(),
                    People = new List<Person>(),
                    Records = new List<Record>(),
                    SubPlatforms = new List<Subplatform>()
                    {
                        pbSubplatform
                    },
                    SubscribedProfiles = new List<Profile>(),
                    Alerts = new List<Alert>(),
                    Comparisons = new List<Comparison>()
                },
                new Organisation()
                {
                    Name = "CD&V",
                    FullName = "Christen-Democratisch en Vlaams",
                    IconURL=@"~/Content/Images/Partijen/cdv.png",
                    Keywords = new List<Keyword>(),
                    People = new List<Person>(),
                    Records = new List<Record>(),
                    SubPlatforms = new List<Subplatform>()
                    {
                        pbSubplatform
                    },
                    SubscribedProfiles = new List<Profile>(),
                    Alerts = new List<Alert>(),
                    Comparisons = new List<Comparison>()
                },
                new Organisation()
                {
                    Name =  "SP.A",
                    FullName ="Socialistische Partij Anders",
                    IconURL=@"~/Content/Images/Partijen/spa.jpg",
                    Keywords = new List<Keyword>(),
                    People = new List<Person>(),
                    Records = new List<Record>(),
                    SubPlatforms = new List<Subplatform>()
                    {
                        pbSubplatform
                    },
                    SubscribedProfiles = new List<Profile>(),
                    Alerts = new List<Alert>(),
                    Comparisons = new List<Comparison>()
                },
                new Organisation()
                {
                    Name = "Open Vld",
                    FullName = "Open Vlaamse Liberalen en Democraten",
                    IconURL=@"~/Content/Images/Partijen/openvld.png",
                    Keywords = new List<Keyword>(),
                    People = new List<Person>(),
                    Records = new List<Record>(),
                    SubPlatforms = new List<Subplatform>()
                    {
                        pbSubplatform
                    },
                    SubscribedProfiles = new List<Profile>(),
                    Alerts = new List<Alert>(),
                    Comparisons = new List<Comparison>()
                },
                new Organisation()
                {
                    Name = "Groen",
                    FullName = "Groen",
                    IconURL=@"~/Content/Images/Partijen/groen.jpg",
                    Keywords = new List<Keyword>(),
                    People = new List<Person>(),
                    Records = new List<Record>(),
                    SubPlatforms = new List<Subplatform>()
                    {
                        pbSubplatform
                    },
                    SubscribedProfiles = new List<Profile>(),
                    Alerts = new List<Alert>(),
                    Comparisons = new List<Comparison>()
                },
                new Organisation()
                {
                    Name = "N-VA",
                    FullName = "Nieuw-Vlaamse Alliantie",
                    IconURL=@"~/Content/Images/Partijen/nva.jpg",
                    Keywords = new List<Keyword>(),
                    People = new List<Person>(),
                    Records = new List<Record>(),
                    SubPlatforms = new List<Subplatform>()
                    {
                        pbSubplatform
                    },
                    SubscribedProfiles = new List<Profile>(),
                    Alerts = new List<Alert>(),
                    Comparisons = new List<Comparison>()
                },
                new Organisation()
                {
                    Name = "VB",
                    FullName ="Vlaams Belang" ,
                    IconURL=@"~/Content/Images/Partijen/vb.png",
                    Keywords = new List<Keyword>(),
                    People = new List<Person>(),
                    Records = new List<Record>(),
                    SubPlatforms = new List<Subplatform>()
                    {
                        pbSubplatform
                    },
                    SubscribedProfiles = new List<Profile>(),
                    Alerts = new List<Alert>(),
                    Comparisons = new List<Comparison>()
                }
            };
            ctx.Organisations.ForEachAsync(o =>
            {
                Organisation organisation = (Organisation)OrganisationsToAdd.FirstOrDefault(org => org.Equals(o));
                if (organisation != null) OrganisationsToAdd.Remove(o);
            }).Wait();

            List<Item> ThemesToAdd = new List<Item>()
            {
                new Theme()
                {
                    Name = "Migratie",
                    IconURL=@"~/Content/Images/Themes/migratie.png",
                    Alerts = new List<Alert>(),
                    Comparisons = new List<Comparison>(),
                    Keywords = new List<Keyword>(),
                    Records = new List<Record>(),
                    SubPlatforms = new List<Subplatform>()
                    {
                        pbSubplatform
                    },
                    SubscribedProfiles = new List<Profile>()
                },
                new Theme()
                {
                    Name = "Onderwijs",
                    IconURL=@"~/Content/Images/Themes/onderwijs.png",
                    Alerts = new List<Alert>(),
                    Comparisons = new List<Comparison>(),
                    Keywords = new List<Keyword>(),
                    Records = new List<Record>(),
                    SubPlatforms = new List<Subplatform>()
                    {
                        pbSubplatform
                    },
                    SubscribedProfiles = new List<Profile>()
                },
                new Theme()
                {
                    Name = "Veiligheid",
                    IconURL=@"~/Content/Images/Themes/veiligheid.png",
                    Alerts = new List<Alert>(),
                    Comparisons = new List<Comparison>(),
                    Keywords = new List<Keyword>(),
                    Records = new List<Record>(),
                    SubPlatforms = new List<Subplatform>()
                    {
                        pbSubplatform
                    },
                    SubscribedProfiles = new List<Profile>()
                },
                new Theme()
                {
                    Name = "Zorg",
                    IconURL=@"~/Content/Images/Themes/zorg.png",
                    Alerts = new List<Alert>(),
                    Comparisons = new List<Comparison>(),
                    Keywords = new List<Keyword>(),
                    Records = new List<Record>(),
                    SubPlatforms = new List<Subplatform>()
                    {
                        pbSubplatform
                    },
                    SubscribedProfiles = new List<Profile>()
                },
                new Theme()
                {
                    Name = "Europa",
                    IconURL=@"~/Content/Images/Themes/eu.png",
                    Alerts = new List<Alert>(),
                    Comparisons = new List<Comparison>(),
                    Keywords = new List<Keyword>(),
                    Records = new List<Record>(),
                    SubPlatforms = new List<Subplatform>()
                    {
                        pbSubplatform
                    },
                    SubscribedProfiles = new List<Profile>()
                },
                new Theme()
                {
                    Name = "Milieu",
                    IconURL=@"~/Content/Images/Themes/eu.png",
                    Alerts = new List<Alert>(),
                    Comparisons = new List<Comparison>(),
                    Keywords = new List<Keyword>(),
                    Records = new List<Record>(),
                    SubPlatforms = new List<Subplatform>()
                    {
                        pbSubplatform
                    },
                    SubscribedProfiles = new List<Profile>()
                },
                new Theme()
                {
                    Name="Mobiliteit",
                    IconURL=@"~/Content/Images/Themes/mobiliteit.png",
                    Alerts = new List<Alert>(),
                    Comparisons = new List<Comparison>(),
                    Keywords = new List<Keyword>(),
                    Records = new List<Record>(),
                    SubPlatforms = new List<Subplatform>()
                    {
                        pbSubplatform
                    },
                    SubscribedProfiles = new List<Profile>()
                }
            };
            ctx.Themes.ForEachAsync(t =>
            {
                Theme theme = (Theme)ThemesToAdd.FirstOrDefault(them => them.Equals(t));
                if (theme != null) ThemesToAdd.Remove(t);
            }).Wait();

            if (OrganisationsToAdd.Count != 0) ctx.Items.AddRange(OrganisationsToAdd);
            if (ThemesToAdd.Count != 0) ctx.Items.AddRange(ThemesToAdd);

            ctx.SaveChanges();
        }
    }
}
