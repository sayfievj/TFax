using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using TFax.Web.CORE.BLL.Infrastructure;

namespace TFax.Web.CORE.COMMON.Controllers
{
    public class DbController<T> : AppController where T : class
    {
        protected IBaseRepository<T> DbRepository { get; set; }

        public DbController()
        {
            DbRepository = new BaseRepository<T>(UnitOfWork);
        }

    }
}