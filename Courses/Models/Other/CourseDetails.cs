using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Courses.Models.Other
{
    public class CourseDetails
    {
        public CourseDetails()
        {
            SimilarCourses = new Dictionary<int, string>();
        }

        public int CourseID { get; set; }

        public string Name { get; set; }

        public int? Duration { get; set; }

        public string Characterization { get; set; }

        public string OtherSkills { get; set; }

        public virtual List<string> Skills { get; set; }

        public string OtherPosition { get; set; }

        public virtual List<string> Positions { get; set; }

        public string OtherProfessionalField { get; set; }

        public virtual List<string> ProfessionalFields { get; set; }

        public virtual string Department { get; set; }

        public virtual string FormOfEducation { get; set; }

        public virtual string LevelOfEducation { get; set; }

        public Dictionary<int, string> SimilarCourses { get; set; }
    }
}