using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Infrastructure.Annotations;
using Domain.Items;
using PB.BL.Domain.Account;
using PB.BL.Domain.Dashboards;
using PB.BL.Domain.Items;
using PB.BL.Domain.Platform;
using System.Data.Entity.Validation;

namespace PB.DAL.EF
{
    [DbConfigurationType(typeof(IntegratieDbConfiguration))]
    internal class IntegratieDbContext : System.Data.Entity.DbContext
    {
        private readonly bool delaySave;

        public IntegratieDbContext() : base("IntegratieDB_EFCodeFirst")
        {
            System.Data.Entity.Database.SetInitializer<IntegratieDbContext>(new IntegratieDbInitializer());

        }

        public IntegratieDbContext(bool unitOfworkPresent = false) : base("IntegratieDB_EFCodeFirst")
        {
            System.Data.Entity.Database.SetInitializer<IntegratieDbContext>(new IntegratieDbInitializer());
            delaySave = unitOfworkPresent;
        }




        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /*
             * Database Configuration
             */
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Properties<DateTime>()
                .Configure(c => c.HasColumnType("datetime2"));


            /*
             * Primary keys, configurations, ...
             */
            modelBuilder.Entity<Record>().Property(r => r.Tweet_Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);


            /*
             * Relations, Foreign Keys, ...
             */
            modelBuilder.Entity<Profile>()
                .HasRequired(p => p.UserData)
                .WithRequiredDependent(ud => ud.Profile);

            modelBuilder.Entity<Profile>()
                .HasMany(p => p.Subscriptions)
                .WithMany(i => i.SubscribedProfiles)
                .Map(m => { m.ToTable("tblSubscriptions"); });

            modelBuilder.Entity<Profile>()
                .HasMany(p => p.AdminPlatforms)
                .WithMany(p => p.Admins)
                .Map(m => { m.ToTable("tblSubplatformAdmins"); });

            modelBuilder.Entity<Item>()
                .HasMany(i => i.Comparisons)
                .WithMany(c => c.Items)
                .Map(m => { m.ToTable("tblComparisonItem"); });

            modelBuilder.Entity<Item>()
                .HasMany(t => t.Keywords)
                .WithMany(k => k.Items)
                .Map(m => { m.ToTable("tblKeywordItem"); });

            modelBuilder.Entity<Item>()
                .HasMany(i => i.SubPlatforms)
                .WithMany(s => s.Items)
                .Map(m => { m.ToTable("tblSubplatformItem"); });

            modelBuilder.Entity<Record>()
                .HasMany(r => r.Mentions)
                .WithMany(m => m.Records)
                .Map(m => { m.ToTable("tblRecordMention"); });

            modelBuilder.Entity<Record>()
                .HasMany(r => r.Words)
                .WithMany(r => r.Records)
                .Map(m => { m.ToTable("tblRecordWord"); });

            modelBuilder.Entity<Record>()
                .HasMany(r => r.Hashtags)
                .WithMany(r => r.Records)
                .Map(m => { m.ToTable("tblRecordHashtag"); });

            modelBuilder.Entity<Record>()
                .HasMany(r => r.URLs)
                .WithMany(r => r.Records)
                .Map(m => { m.ToTable("tblRecordUrl"); });

            modelBuilder.Entity<Person>()
                .HasMany(i => i.Records)
                .WithMany(r => r.Persons)
                .Map(m => { m.ToTable("tblPersonRecords"); });

            modelBuilder.Entity<Theme>()
                .HasMany(i => i.Records)
                .WithMany(r => r.Themes)
                .Map(m => { m.ToTable("tblThemeRecords"); });
        }

        public override int SaveChanges()
        {
            if (delaySave) return -1;
            return base.SaveChanges();
        }

        internal int CommitChanges()
        {
            if (delaySave)
            {
                try
                {
                    return base.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Console.WriteLine("Property: {0} Error: {1}",
                                                    validationError.PropertyName,
                                                    validationError.ErrorMessage);
                        }
                    }
                }
            }
            throw new InvalidOperationException("Geen UnitOfWork presented, gebruik SaveChanges in de plaats");
        }

        public DbSet<Profile> Profiles { get; set; }
        public DbSet<UserData> UserData { get; set; }
        public DbSet<UserSetting> UserSettings { get; set; }

        public DbSet<Comparison> Comparisons { get; set; }
        public DbSet<Dashboard> Dashboards { get; set; }
        public DbSet<Element> Elements { get; set; }
        public DbSet<Graph> Graphs { get; set; }
        public DbSet<Map> Maps { get; set; }
        public DbSet<Ranking> Rankings { get; set; }
        public DbSet<Zone> Zones { get; set; }

        public DbSet<Function> Functions { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Keyword> Keywords { get; set; }
        public DbSet<Organisation> Organisations { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<Theme> Themes { get; set; }
        public DbSet<Mention> Mentions { get; set; }
        public DbSet<Url> Urls { get; set; }
        public DbSet<Word> Words { get; set; }
        public DbSet<Hashtag> Hashtags { get; set; }

        public DbSet<Page> Pages { get; set; }
        public DbSet<Style> Styles { get; set; }
        public DbSet<Subplatform> Subplatforms { get; set; }
        public DbSet<SubplatformSetting> SubplatformSettings { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}
