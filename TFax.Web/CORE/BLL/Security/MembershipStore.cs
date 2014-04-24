namespace TFax.Web.CORE.BLL.Security
{
    public class MembershipStore
    {
        public long Id { get; set; }

        public string Login { get; set; }

        public string Passsword { get; set; }

        public long? Role { get; set; }

    }
}