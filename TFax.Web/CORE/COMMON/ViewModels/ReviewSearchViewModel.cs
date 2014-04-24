using PagedList;
using TFax.Web.CORE.DAL;

namespace TFax.Web.CORE.COMMON.ViewModels
{
 

    public class ReviewSearchViewModel
    {

        public IPagedList<Review> Objects { get; set; }

        public long? CountryId { get; set; }

        public long? StateId { get; set; }

        public long? CityId { get; set; }
         
        public int? GradeLevel { get; set; }

        public long? SubjectId { get; set; }

        public int? OverallRatingFrom { get; set; }

        public int? OverallRatingTo { get; set; }
         

    }
}