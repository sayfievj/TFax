using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TFax.Web.CORE.DAL;

namespace TFax.Web.CORE.COMMON.ViewModels
{

    public enum SearchTutorType : long
    {
        None = 0,
        Tutor = 1,
        Student = 2,
        Class = 3,
        Group = 4
    }

    public class AccountViewModel
    {

        public IEnumerable<Review> Reviews { get; set; }

        public IEnumerable<MyFavorite> Favorites { get; set; }

        public IEnumerable<MyMessage> Messages { get; set; }

        public long? SearchTutorId { get; set; }

        public AccountViewModel()
        {
            SearchTutorId = (long?)SearchTutorType.None;
            Reviews = new Review[] { };
            Favorites = new MyFavorite[] { };
            Messages = new MyMessage[] { };
        }

    }
}