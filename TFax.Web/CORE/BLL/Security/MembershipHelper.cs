using System;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using TFax.Web.CORE.BLL.Infrastructure;
using TFax.Web.CORE.COMMON.Enums;
using TFax.Web.CORE.COMMON.Helpers;
using TFax.Web.CORE.COMMON.ViewModels;
using TFax.Web.CORE.DAL;

namespace TFax.Web.CORE.BLL.Security
{
    public class MembershipHelper : IMembershipHelper
    {

        private readonly IBaseRepository<MemberMaster> _repository;

        public MembershipHelper()
        {
            using (var uniOfWork = DependencyResolver.Current.GetService<IUnitOfWork>())
                _repository = new BaseRepository<MemberMaster>(uniOfWork);
        }

        public static IMembershipHelper Current
        {
            get { return DependencyResolver.Current.GetService<IMembershipHelper>(); }
        }

        public MembershipStore Store
        {
            get { return SessionHelper.GetStore<MembershipStore>(AppSettings.STORE.LOGIN_STORE_KEY); }
            set
            {
                SessionHelper.SetStore(value, AppSettings.STORE.LOGIN_STORE_KEY);
            }
        }

        public MemberMaster GetMember()
        {
            var store = Store;

            return store == null
                ? null
                : _repository.Find(f => f.Email_Login == store.Login && f.Password == store.Passsword);
        }

        public Profile_Master Profile
        {
            get
            {
                var member = GetMember();

                return member != null
                    ? member.Profile_Master.FirstOrDefault(f => f.Id == ProfileId)
                    : null;
            }
        }

        public string UserName
        {
            get
            {
                var member = GetMember();

                return member == null
                    ? "Guest"
                    : member.Email_Login;
            }
        }

        public long Id
        {
            get
            {
                var member = GetMember();

                return member == null
                    ? 0
                    : member.Id;
            }
        }

        public long? ProfileId
        {
            get
            {
                var member = GetMember();
                if (member == null)
                    return null;

                var profile = member.Profile_Master.FirstOrDefault(f => f.MemberId == member.Id);

                return profile != null
                    ? profile.Id
                    : (long?)null;
            }
        }

        public string FullName
        {
            get
            {
                var member = GetMember();

                return member == null
                    ? String.Empty
                    : member.GetFullName();
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                var store = Store;

                return store != null;
            }
        }

        public Role Role
        {
            get
            {

                var store = Store;

                return store == null
                    ? (Role)0
                    : (Role)store.Role;
            }
        }

        public bool CheckRole(Role role)
        {
            return Role == role;
        }

        public bool IsValid(string userName, string password)
        {
            return _repository.Any(f => f.Email_Login == userName && f.Password == password);
        }

        public bool CheckActive(string userName)
        {
            return _repository.Any(f => f.Email_Login == userName && f.IsActivated == true || f.ActivationCodeExpired >= DateTime.Now);
        }

        public bool IsActivated
        {
            get
            {
                var store = Store;

                return store != null && CheckActive(store.Login);
            }
        }

        public bool SignIn(LoginViewModel model)
        {
            var member = _repository.Find(f => f.Email_Login == model.UserName && f.Password == model.Password);

            if (member == null)
                return false;

            var store = new MembershipStore
            {
                Login = model.UserName,
                Passsword = model.Password,
                Role = member.MemberRoleId
            };

            Store = store;

            return true;
        }

        public MemberMaster SignUp(RegisterViewModel model)
        {
            if (IsValid(model.UserName, model.Password))
                return null;

            var member = new MemberMaster
            {
                Email_Login = model.UserName,
                Password = model.Password,
                MemberRoleId = model.RoleId
            };
            {
                member.RegisteredDate = DateTime.Now;
                member.RegisteredIP = UtilHelper.GetVisitorIPAddress();
                member.ActivationCode = Crypto.HashPassword(model.Password);
                member.ActivationCodeExpired = DateTime.Now.AddDays(AppSettingsHelper.GetValue<int>(AppSettings.KEYS.ACCOUNT_ACTIVATION_EXPIRED_DAYS));

                _repository.Insert(member);

                return member;
            }
        }

        public bool IsMemberId(long? id)
        {
            return IsAuthenticated && IsActivated && Id == id;
        }

        public void SignOut()
        {
            SessionHelper.Remove(AppSettings.STORE.LOGIN_STORE_KEY);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }
}