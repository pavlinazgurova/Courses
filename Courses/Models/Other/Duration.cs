namespace Courses.Models.Other
{
    using System.ComponentModel.DataAnnotations;

    public enum Duration
    {
        [Display(Name = "2")]
        Two = 2,
        [Display(Name = "3")]
        Three = 3,
        [Display(Name = "4")]
        Four = 4,
        [Display(Name = "8")]
        Eight = 8,
        [Display(Name = "9")]
        Nine = 9,
        [Display(Name = "10")]
        Ten = 10
    }
}