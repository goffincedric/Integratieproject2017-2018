using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Infrastructure.Annotations;
using PB.BL.Domain.Account;
using PB.BL.Domain.Dashboards;
using PB.BL.Domain.Items;
using PB.BL.Domain.Platform;

namespace PB.DAL.EF
{
  [DbConfigurationType(typeof(IntegratieDbConfiguration))]
  public class IntegratieDbContext : System.Data.Entity.DbContext
  {
    internal IntegratieDbContext() : base("IntegratieDB_EFCodeFirst")
    {
      System.Data.Entity.Database.SetInitializer<IntegratieDbContext>(new IntegratieDbInitializer());
    }

    public DbSet<Profile> Profiles { get; set; }
    public DbSet<UserData> UserData { get; set; }
    public DbSet<UserSetting> UserSettings { get; set; }

    //public DbSet<Comparison> Comparisons { get; set; }
    //public DbSet<Dashboard> Dashboards { get; set; }
    //public DbSet<Element> Elements { get; set; }
    //public DbSet<Graph> Graphs { get; set; }
    //public DbSet<Map> Maps { get; set; }
    //public DbSet<Ranking> Rankings { get; set; }
    //public DbSet<Zone> Zones { get; set; }

    public DbSet<Function> Functions { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Keyword> Keywords { get; set; }
    public DbSet<Organisation> Organisations { get; set; }
    public DbSet<Person> Persons { get; set; }
    public DbSet<Record> Records { get; set; }
    public DbSet<Theme> Themes { get; set; }

    //public DbSet<Page> Pages { get; set; }
    //public DbSet<Style> Styles { get; set; }
    //public DbSet<SubPlatform> SubPlatforms { get; set; }
    //public DbSet<SubplatformSetting> SubplatformSettings { get; set; }
    //public DbSet<Tag> Tags { get; set; }


    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      modelBuilder.Properties<DateTime>()
        .Configure(c => c.HasColumnType("datetime2"));
      //modelBuilder.Entity<Record>().Property(r => r.Date).HasColumnType("datetime2");
    }
  }
}
    