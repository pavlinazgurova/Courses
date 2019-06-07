namespace Courses.Models.Other
{
    using System.Collections.Generic;

    public class FilterModel
    {
        public string Departments { get; set; }

        public bool bachelor { get; set; }

        public bool master { get; set; }

        public bool doctor { get; set; }

        public bool redovno { get; set; }

        public bool zadochno { get; set; }

        public Duration Duration { get; set; }

        public List<int> NewSkills { get; set; }

        public List<int> NewPositions { get; set; }

        public List<int> NewProfesionalFields { get; set; }
    }
}