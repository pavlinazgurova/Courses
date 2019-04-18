namespace Courses.Models.Other
{
    public class FilterModel
    {
        public string Departments { get; set; }

        public string ProfessionalFields { get; set; }

        public string Skills { get; set; }

        public string Positions { get; set; }

        public bool bachelor { get; set; }

        public bool master { get; set; }

        public bool doctor { get; set; }

        public bool redovno { get; set; }

        public bool zadochno { get; set; }

        public Duration Duration { get; set; }
    }
}