namespace Courses.Models.Other
{
    using System.ComponentModel.DataAnnotations;

    public class CourseViewModel
    {
        [Display(Name = "Бакалавър")]
        public bool Bachelor { get; set; }
        [Display(Name = "Магистър")]
        public bool Master { get; set; }
        [Display(Name = "Доктор")]
        public bool Doctor { get; set; }

        [Display(Name = "Редовно")]
        public bool Redovno { get; set; }
        [Display(Name = "Задочно")]
        public bool Zadochno { get; set; }

        public Duration Duration { get; set; }
    }
}