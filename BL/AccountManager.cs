using PB.BL.Domain.Account;
using PB.BL.Domain.Dashboards;
using PB.BL.Domain.Items;
using PB.BL.Domain.Platform;
using PB.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Account;
using System.Security.Cryptography;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using PB.DAL.EF;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace PB.BL
{
  //This class talks with the SupportCenterUserStore and tells it which
  //data to store, it also handles some logic and settings
  public class AccountManager : UserManager<PB.BL.Domain.Account.Profile>
  {

    private IntegratieUserStore store;
    private IProfileRepo ProfileRepo;
    private UnitOfWorkManager UowManager;



    //public AccountManager(IntegratieUserStore store, UnitOfWorkManager uowMgr) : base(store)
    //{
    //Console.WriteLine("Gebruik accountmanager constructor met store and uow");
    //  UowManager = uowMgr;
    //  this.store = store;
    //  ProfileRepo profileRepo = new ProfileRepo(UowManager.UnitOfWork);

    //  CreateRolesandUsers();


    //}
    public AccountManager(IntegratieUserStore store) : base(store)
    {

      Console.WriteLine("Gebruik accountmanager constructor met store");
      
      //UowManager = uowMgr;
      this.store = store;
      initNonExistingRepo(true);
       //ProfileRepo profileRepo = new ProfileRepo(UowManager.UnitOfWork);
      CreateRolesandUsers();


    }



    public static AccountManager Create(IdentityFactoryOptions<AccountManager> options, IOwinContext context)
    {
      Console.WriteLine("Create accountmanager wordt gedaan");

      var manager = new AccountManager(new IntegratieUserStore(context.Get<IntegratieDbContext>()));
      manager.UserValidator = new UserValidator<BL.Domain.Account.Profile>(manager)
      {
        AllowOnlyAlphanumericUserNames = false,
        RequireUniqueEmail = true
      };

      manager.PasswordValidator = new PasswordValidator
      {
        RequiredLength = 6,
        RequireNonLetterOrDigit = false,
        RequireDigit = true,
        RequireLowercase = false,
        RequireUppercase = false
      };

      manager.UserLockoutEnabledByDefault = true;
      manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(10);
      manager.MaxFailedAccessAttemptsBeforeLockout = 10;

      //manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<PB.BL.Domain.Account.Profile>
      //{
      //  MessageFormat = "Your security code is {0}"
      //});

      //manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<PB.BL.Domain.Account.Profile>
      //{
      //  Subject = "Security code",
      //  BodyFormat = "Your security Code is {0}"

      //});
      //manager.EmailService = new EmailService();
      //manager.SmsService = new SmsService();

      var dataProtectionProvider = options.DataProtectionProvider;
      if (dataProtectionProvider != null)
      {
        manager.UserTokenProvider = new DataProtectorTokenProvider<BL.Domain.Account.Profile>(dataProtectionProvider.Create("ASP.NET Identity"));

      }

      return manager;

    }

    public List<IdentityRole> GetAllRoles()
    {
      return store.ReadAllRoles();
    }

    private void CreateRolesandUsers()
    {
      IntegratieDbContext context = store.ReadContext();

      var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

      if (!roleManager.RoleExists("SuperAdmin"))
      {
        //Create SuperAdmin role
        var role = new IdentityRole();
        role.Name = "SuperAdmin";
        roleManager.Create(role);

      }

      //Create Admin role
      if (!roleManager.RoleExists("Admin"))
      {
        var role = new IdentityRole();
        role.Name = "Admin";
        roleManager.Create(role);

      }
      //Create User role   
      if (!roleManager.RoleExists("User"))
      {
        var role = new IdentityRole();
        role.Name = "User";
        roleManager.Create(role);

      }
    }


    public void initNonExistingRepo(bool createWithUnitOfWork = false)
    {
      if (ProfileRepo == null)
      {
        if (createWithUnitOfWork)
        {
          if (UowManager == null)
          {
            UowManager = new UnitOfWorkManager();
            Console.WriteLine("UOW MADE IN ACCOUNT MANAGER for profile repo");
          }
          else
          {
            Console.WriteLine("uo bestaat al");
          }

          ProfileRepo = new ProfileRepo(UowManager.UnitOfWork);
        }
        else
        {
          ProfileRepo = new ProfileRepo();
          Console.WriteLine("OLD WAY REPO ACCOUNTMGR");
        }
      }
    }


    #region Profile
  
    public Profile AddProfile(string username, string email)
    {
      initNonExistingRepo();
      Profile profile = new Profile()
      {
        UserName = username,
        Email = email,

      };
      profile.UserData = new UserData() { Profile = profile, Id="766" };

      profile = AddProfile(profile);
      UowManager.Save();
      return profile;
    }

    private Profile AddProfile(Profile profile)
    {
      initNonExistingRepo();
      Profile newProfile = ProfileRepo.CreateProfile(profile);
      UowManager.Save();
      return profile;
    }

    public void ChangeProfile(Profile profile)
    {
      initNonExistingRepo();
      ProfileRepo.UpdateProfile(profile);
      UowManager.Save();
    }

    public Profile GetProfile(string username)
    {
      initNonExistingRepo();
      return ProfileRepo.ReadProfile(username);
    }

    public IEnumerable<Profile> GetProfiles()
    {
      initNonExistingRepo();
      return ProfileRepo.ReadProfiles();
    }

    public void RemoveProfile(string username)
    {
      initNonExistingRepo();
      ProfileRepo.DeleteProfile(username);
      UowManager.Save();
    }
    #endregion

   

    public void LinkAlertsToProfile(List<Alert> alerts)
    {
      alerts.ForEach(a =>
      {
        a.Profile.Alerts.Add(a);
        ProfileRepo.UpdateProfile(a.Profile);
      });

      UowManager.Save();
    }

    #region passwordEncryption
    private byte[] Get_SALT(int maximumSaltLength)
    {
      var salt = new byte[maximumSaltLength];

      //Require NameSpace: using System.Security.Cryptography;
      using (var random = new RNGCryptoServiceProvider())
      {
        random.GetNonZeroBytes(salt);
      }

      return salt;
    }

    public string Get_HASH_SHA512(string password, string username, byte[] salt)
    {
      try
      {
        //required NameSpace: using System.Text;
        //Plain Text in Byte
        byte[] plainTextBytes = Encoding.UTF8.GetBytes(password + username);

        //Plain Text + SALT Key in Byte
        byte[] plainTextWithSaltBytes = new byte[plainTextBytes.Length + salt.Length];

        for (int i = 0; i < plainTextBytes.Length; i++)
        {
          plainTextWithSaltBytes[i] = plainTextBytes[i];
        }

        for (int i = 0; i < salt.Length; i++)
        {
          plainTextWithSaltBytes[plainTextBytes.Length + i] = salt[i];
        }

        HashAlgorithm hash = new SHA512Managed();
        byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);
        byte[] hashWithSaltBytes = new byte[hashBytes.Length + salt.Length];

        for (int i = 0; i < hashBytes.Length; i++)
        {
          hashWithSaltBytes[i] = hashBytes[i];
        }

        for (int i = 0; i < salt.Length; i++)
        {
          hashWithSaltBytes[hashBytes.Length + i] = salt[i];
        }

        return Convert.ToBase64String(hashWithSaltBytes);
      }
      catch
      {
        return string.Empty;
      }
    }

    public bool CompareHashValue(string password, string username, string OldHASHValue, byte[] SALT)
    {
      try
      {
        string expectedHashString = Get_HASH_SHA512(password, username, SALT);

        return (OldHASHValue == expectedHashString);
      }
      catch
      {
        return false;
      }
    }
    #endregion
  }
}
