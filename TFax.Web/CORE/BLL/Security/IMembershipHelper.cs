using System;
using System.Threading.Tasks;

using TFax.Web.CORE.COMMON.Enums;
using TFax.Web.CORE.COMMON.ViewModels;
using TFax.Web.CORE.DAL;

namespace TFax.Web.CORE.BLL.Security
{
    public interface IMembershipHelper : IDisposable
    {

        MembershipStore Store { get; set; }

        string UserName { get; }

        long Id { get; }

        long? ProfileId { get; }

        string FullName { get; }

        bool IsAuthenticated { get; }

        MemberMaster GetMember();

        Profile_Master Profile { get; }

        Role Role { get; }

        bool CheckRole(Role role);

        bool IsValid(string userName, string password);

        bool CheckActive(string userName);

        bool IsMemberId(long? id);

        bool IsActivated { get; }

        bool SignIn(LoginViewModel model);

        MemberMaster SignUp(RegisterViewModel model);

        void SignOut();

    }
}