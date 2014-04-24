using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TFax.Web.CORE.DAL;

namespace TFax.Web.CORE.COMMON.ViewModels
{
    public class ReviewViewModel
    {

        [HiddenInput(DisplayValue = false)]
        public long Id { get; set; }

        [Display(Name = "Title")]
        public string ReviewTitle { get; set; }

        [Display(Name = "Subject")]
        [Required]
        public string Subject { get; set; }

        [Display(Name = "Detail")]
        public string Subject_Detail { get; set; }

        [Display(Name = "Location: What was your overall experience with the tutor?")]
        [Required]
        public string ReviewSummary { get; set; }

        [Required]
        [Display(Name = "Subject Knowledge:")]
        public string Description_SubjectKnowledge { get; set; }

        [Required]
        [Display(Name = "Helpfulness:")]
        public string Description_Helpfullness { get; set; }

        [Required]
        [Display(Name = "Passion: Describe Tutors overall attitude in teaching this subject.")]
        public string Description_Passion { get; set; }

        [Required]
        [Display(Name = "Subject knowledge")]
        public Nullable<int> Score_SubjectKnowledge { get; set; }

        [Required]
        [Display(Name = "Passion")]
        public Nullable<int> Score_Helpfullness { get; set; }

        [Required]
        [Display(Name = "Passion")]
        public Nullable<int> Score_Passion { get; set; }

        [Required]
        [Display(Name = "Overall")]
        public Nullable<int> Score_Overall { get; set; }

        [Required]
        [Display(Name = "Communication")]
        public Nullable<int> Communication { get; set; }

    }
}