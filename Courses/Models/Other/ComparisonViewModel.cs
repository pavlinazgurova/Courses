using System;
using System.Collections.Generic;

namespace Courses.Models.Other
{
    public class ComparisonViewModel
    {
        public Course Course1 { get; set; }

        public List<Tuple<string,bool>> Course1SKills { get; set; }

        public List<Tuple<string, bool>> Course1Positions { get; set; }

        public List<Tuple<string, bool>> Course1ProfesionalFields { get; set; }

        public Course Course2 { get; set; }

        public List<Tuple<string,bool>> Course2SKills { get; set; }

        public List<Tuple<string, bool>> Course2Positions { get; set; }

        public List<Tuple<string, bool>> Course2ProfesionalFields { get; set; }
    }
}