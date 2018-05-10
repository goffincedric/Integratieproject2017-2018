using Domain.Accounts;
using Microsoft.AspNet.Identity.EntityFramework;
using PB.BL.Domain.Accounts;
using PB.BL.Domain.Dashboards;
using PB.BL.Domain.Items;
using PB.BL.Domain.Platform;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Threading.Tasks;

namespace PB.DAL.EF
{
    [DbConfigurationType(typeof(IntegratieDbConfiguration))]
    public class IntegratieDbContext : IdentityDbContext<Profile>
    {
        private readonly bool delaySave;

        public IntegratieDbContext() : base("IntegratieDB_EFCodeFirst", throwIfV1Schema: false)
        {

        }

        public IntegratieDbContext(bool unitOfworkPresent = false) : base("IntegratieDB_EFCodeFirst")
        {
            //Database.SetInitializer(new IntegratieDbInitializer()); // Verplaatst naar DbConfiguration
            delaySave = unitOfworkPresent;
        }


        public static IntegratieDbContext Create()
        {
            return new IntegratieDbContext();
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /*
             * Database Configuration
             */
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
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
                .WithRequiredPrincipal(ud => ud.Profile)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Profile>()
                .HasMany(p => p.Subscriptions)
                .WithMany(i => i.SubscribedProfiles)
                .Map(m => { m.ToTable("tblSubscriptions"); });

            modelBuilder.Entity<Profile>()
                .HasMany(p => p.AdminPlatforms)
                .WithMany(p => p.Admins)
                .Map(m => { m.ToTable("tblSubplatformAdmins"); });

            modelBuilder.Entity<Profile>()
                .HasMany(p => p.Dashboards)
                .WithRequired(d => d.Profile)
                .HasForeignKey(d => d.UserId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Profile>()
                .HasMany(p => p.ProfileAlerts)
                .WithRequired(pa => pa.Profile)
                .HasForeignKey(pa => pa.UserId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Profile>()
                .HasMany(p => p.WeeklyReviews)
                .WithRequired(w => w.Profile)
                .HasForeignKey(w => w.UserId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<WeeklyReview>()
                .HasMany(wr => wr.WeeklyReviewsProfileAlerts)
                .WithRequired(wrpa => wrpa.WeeklyReview)
                .HasForeignKey(wrpa => wrpa.WeeklyReviewId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<WeeklyReviewProfileAlerts>()
                .HasKey(wrpa => new { wrpa.WeeklyReviewId, wrpa.ProfileAlertId });

            modelBuilder.Entity<ProfileAlert>()
                .HasMany(pa => pa.WeeklyReviewsProfileAlerts)
                .WithRequired(wrpa => wrpa.ProfileAlert)
                .HasForeignKey(wrpa => wrpa.ProfileAlertId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Alert>()
                .HasMany(a => a.ProfileAlerts)
                .WithRequired(pa => pa.Alert)
                .HasForeignKey(pa => pa.AlertId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Item>()
                .HasMany(i => i.Alerts)
                .WithRequired(a => a.Item)
                .HasForeignKey(a => a.ItemId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Item>()
                .HasMany(t => t.Comparisons)
                .WithMany(t => t.Items)
                .Map(m => { m.ToTable("tblComparisonItem"); });

            modelBuilder.Entity<Item>()
                .HasMany(t => t.Keywords)
                .WithMany(t => t.Items)
                .Map(m => { m.ToTable("tblKeywordItem"); });

            modelBuilder.Entity<Item>()
                .HasMany(i => i.SubPlatforms)
                .WithMany(s => s.Items)
                .Map(m => { m.ToTable("tblSubplatformItem"); });

            modelBuilder.Entity<Theme>()
                .HasMany(t => t.Persons)
                .WithMany(p => p.Themes)
                .Map(m => { m.ToTable("tblPersonThemes"); });

            modelBuilder.Entity<Theme>()
                .HasMany(t => t.Organisations)
                .WithMany(o => o.Themes)
                .Map(m => { m.ToTable("tblOrganisationThemes"); });

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

            modelBuilder.Entity<Subplatform>()
                .HasMany(s => s.Dashboards)
                .WithRequired(d => d.Subplatform)
                .HasForeignKey(d => d.SubplatformId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Subplatform>()
                .HasMany(s => s.Settings)
                .WithRequired(ss => ss.Subplatform)
                .HasForeignKey(ss => ss.SubplatformId)
                .WillCascadeOnDelete(true);

            //modelBuilder.Entity<SubplatformSetting>()
            //    .HasKey(ss => new { ss.SubplatformId, ss.SettingName });

            modelBuilder.Entity<Subplatform>()
                .HasMany(s => s.Pages)
                .WithRequired(p => p.Subplatform)
                .HasForeignKey(p => p.SubplatformId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Page>()
                .HasMany(p => p.Tags)
                .WithRequired(t => t.Page)
                .HasForeignKey(t => t.PageId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Dashboard>()
                .HasMany(d => d.Zones)
                .WithRequired(z => z.Dashboard)
                .HasForeignKey(z => z.DashboardId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Zone>()
                .HasMany(z => z.Elements)
                .WithRequired(e => e.Zone)
                .HasForeignKey(e => e.ZoneId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Comparison>()
                .HasMany(c => c.Elements)
                .WithOptional(e => e.Comparison)
                .WillCascadeOnDelete(true);        // FOREIGN KEY?

            //identity tables
            modelBuilder.Entity<Profile>().ToTable("tblProfile");
            modelBuilder.Entity<IdentityUserRole>().ToTable("tblUserRole");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("tblUserLogin");
            modelBuilder.Entity<IdentityRole>().ToTable("tblRole");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("tblUserClaim");
        }

        public override int SaveChanges()
        {
            if (delaySave) return -1;
            try
            {
                base.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
            }
            return -1;
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

        async internal Task<int> CommitChangesAsync()
        {
            if (delaySave)
            {
                try
                {
                    return await base.SaveChangesAsync();
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


        public DbSet<UserData> UserData { get; set; }
        public DbSet<UserSetting> UserSettings { get; set; }
        public DbSet<WeeklyReview> WeeklyReviews { get; set; }
        public DbSet<ProfileAlert> ProfileAlerts { get; set; }
        public DbSet<Alert> Alerts { get; set; }

        public DbSet<Comparison> Comparisons { get; set; }
        public DbSet<Dashboard> Dashboards { get; set; }
        public DbSet<Element> Elements { get; set; }
        //public DbSet<Graph> Graphs { get; set; }
        //public DbSet<Map> Maps { get; set; }
        //public DbSet<Ranking> Rankings { get; set; }
        public DbSet<Zone> Zones { get; set; }

        //public DbSet<Function> Functions { get; set; }
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
