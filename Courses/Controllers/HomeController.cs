namespace Courses.Controllers
{
    using Courses.Models;
    using Courses.Models.Other;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult HomePage()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Search()
        {
            return View();
        }

        public ActionResult AdvancedSearch()
        {
            var db = new CoursesEntities();
            var courses = db.Course.AsEnumerable();
            ViewBag.Departments = db.Departments.Select(x => x.NameDepartments).ToList();

            var skills = db.Skills.Select(s => new {
                SkillId = s.SkillID,
                SkillName = s.Name
            }).ToList();
            ViewBag.NewSkills = new MultiSelectList(skills, "SkillId", "SkillName");

            var positions = db.Positions.Select(s => new {
                PositionId = s.PositionID,
                PositionName = s.Name
            }).ToList();
            ViewBag.NewPositions = new MultiSelectList(positions, "PositionId", "PositionName");

            var profesionalFields = db.ProfessionalField.Select(s => new {
                ProfesionalFieldId = s.ProfessionalFieldID,
                ProfesionalFieldName = s.Name
            }).ToList();
            ViewBag.NewProfesionalFields = new MultiSelectList(profesionalFields, "ProfesionalFieldId", "ProfesionalFieldName");

            return View(new CourseViewModel()
            {
                Bachelor = false,
                Master = false,
                Doctor = false,
                Redovno = false,
                Zadochno = false
            });
        }

        [HttpGet]
        public ActionResult Result(string searchString, FilterModel filterModel, int page = 1)
        {
            var db = new CoursesEntities();
            var courses = db.Course.ToList();
            var pageSize = 5;

            ViewBag.SearchString = searchString;

            if (!string.IsNullOrEmpty(searchString))
            {
                courses = courses.Where(s => s.Name.ToLower().Contains(searchString.ToLower())).ToList();
            }
            else
            {
                if (filterModel.NewSkills != null && filterModel.NewSkills.Count >= 1)
                {
                    var courseCollection = db.CourseSkill
                          .Where(x => filterModel.NewSkills.Contains(x.SkillID ?? -1))
                          .Select(x => x.CourseID)
                          .Distinct()
                          .ToList();

                    courses = courses
                        .Where(c => courseCollection.Any(x => c.CourseID.Equals(x)))
                        .ToList();
                }

                if (filterModel.NewPositions != null && filterModel.NewPositions.Count >= 1)
                {
                    var courseCollection = db.CoursePoosition
                          .Where(x => filterModel.NewPositions.Contains(x.PositionID ?? -1))
                          .Select(x => x.CourseID)
                          .Distinct()
                          .ToList();

                    courses = courses
                        .Where(c => courseCollection.Any(x => c.CourseID.Equals(x)))
                        .ToList();
                }

                if (filterModel.NewProfesionalFields != null && filterModel.NewProfesionalFields.Count >= 1)
                {
                    var courseCollection = db.CourseProfessionalField
                          .Where(x => filterModel.NewProfesionalFields.Contains(x.ProfessinalFieldID ?? -1))
                          .Select(x => x.CourseID)
                          .Distinct()
                          .ToList();

                    courses = courses
                        .Where(c => courseCollection.Any(x => c.CourseID.Equals(x)))
                        .ToList();
                }

                if (!string.IsNullOrEmpty(filterModel.Departments))
                {
                    var de = db.Departments.ToList();
                    var depID = de.Find(d => d.NameDepartments == filterModel.Departments).DepartmentID;

                    courses = courses.Where(c => c.DepartmentID == depID).ToList();
                }

                if (filterModel.Duration != 0)
                {
                    courses = courses.Where(c => c.Duration == (int)filterModel.Duration).ToList();
                }

                if (filterModel.bachelor && filterModel.master && filterModel.doctor)
                {
                    courses = courses.Where(c => c.LevelOfEducationID == 1 || c.LevelOfEducationID == 2 || c.LevelOfEducationID == 3).ToList();
                }
                else if (filterModel.bachelor && filterModel.master && !filterModel.doctor)
                {
                    courses = courses.Where(c => c.LevelOfEducationID == 1 || c.LevelOfEducationID == 2).ToList();
                }
                else if (!filterModel.bachelor && filterModel.master && filterModel.doctor)
                {
                    courses = courses.Where(c => c.LevelOfEducationID == 3 || c.LevelOfEducationID == 2).ToList();
                }
                else if (filterModel.bachelor && !filterModel.master && filterModel.doctor)
                {
                    courses = courses.Where(c => c.LevelOfEducationID == 3 || c.LevelOfEducationID == 1).ToList();
                }
                else if (filterModel.bachelor && !filterModel.master && !filterModel.doctor)
                {
                    courses = courses.Where(c => c.LevelOfEducationID == 1).ToList();
                }
                else if (!filterModel.bachelor && filterModel.master && !filterModel.doctor)
                {
                    courses = courses.Where(c => c.LevelOfEducationID == 2).ToList();
                }
                else if (!filterModel.bachelor && !filterModel.master && filterModel.doctor)
                {
                    courses = courses.Where(c => c.LevelOfEducationID == 3).ToList();
                }

                if (filterModel.redovno && filterModel.zadochno)
                {
                    courses = courses.Where(c => c.FormOfEducationID == 1 || c.FormOfEducationID == 2).ToList();
                }
                else if (filterModel.redovno && !filterModel.zadochno)
                {
                    courses = courses.Where(c => c.FormOfEducationID == 1).ToList();
                }
                else if (!filterModel.redovno && filterModel.zadochno)
                {
                    courses = courses.Where(c => c.FormOfEducationID == 2).ToList();
                }
            }

            ViewBag.CurrentPage = page;

            return View(courses.Skip((page - 1) * pageSize).Take(pageSize).OrderBy(x => x.Name).ToList());
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            var db = new CoursesEntities();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var course = db.Course.Find(id);

            if (course == null)
            {
                return HttpNotFound();
            }

            var skillIds = db.CourseSkill.
                Where(y => y.CourseID == id).
                Select(x => x.SkillID).
                ToList();
            var skillNames = db.Skills.
                Where(x => skillIds.Contains(x.SkillID)).
                Select(y => y.Name).
                ToList();

            var positionIds = db.CoursePoosition.
                Where(y => y.CourseID == id).
                Select(x => x.PositionID).
                ToList();
            var positionNames = db.Positions.
                Where(x => positionIds.Contains(x.PositionID)).
                Select(y => y.Name).
                ToList();

            var profFieldIds = db.CourseProfessionalField.
                Where(y => y.CourseID == id).
                Select(x => x.ProfessinalFieldID).
                ToList();
            var profFieldNames = db.ProfessionalField.
                Where(x => profFieldIds.Contains(x.ProfessionalFieldID)).
                Select(y => y.Name).
                ToList();

            var allOtherCourses = db.Course.Where(x => x.CourseID != id).ToList();
            var equal = new Dictionary<int, List<int?>>();
            var difference = new Dictionary<int, List<int?>>();

            foreach (var item in allOtherCourses)
            {
                var current = item.CourseSkill.Select(x => x.SkillID).Intersect(skillIds).ToList();

                equal[item.CourseID] = current;
            }

            foreach (var item in allOtherCourses)
            {
                var current = item.CourseSkill.Select(x => x.SkillID).Except(skillIds).
                    Concat(skillIds.Except(item.CourseSkill.Select(x => x.SkillID))).ToList();

                difference[item.CourseID] = current;
            }

            var result = new Dictionary<int, double>();

            foreach (var item in equal)
            {
                if (difference[item.Key].Count > 0 && item.Value.Count > 0)
                {
                    result[item.Key] = ((double)item.Value.Count / difference[item.Key].Count) * 100;
                }
            }

            var courseDetails = new CourseDetails();

            foreach (var item in result)
            {
                if (item.Value >= 50.00)
                {
                    var current = allOtherCourses.SingleOrDefault(x => x.CourseID == item.Key);
                    courseDetails.SimilarCourses[item.Key] = current.Name;
                }
            }

            courseDetails.Name = course.Name;
            courseDetails.Duration = course.Duration;
            courseDetails.Characterization = course.Characterization;
            courseDetails.Department = course.Departments.NameDepartments;
            courseDetails.FormOfEducation = course.FormOfEducation.NameFormOfEducation;
            courseDetails.LevelOfEducation = course.LevelOfEducation.NameLevelOfEducation;
            courseDetails.OtherPosition = course.OtherPosition;
            courseDetails.OtherProfessionalField = course.OtherProfessionalField;
            courseDetails.OtherSkills = course.OtherSkills;
            courseDetails.Skills = skillNames;
            courseDetails.ProfessionalFields = profFieldNames;
            courseDetails.Positions = positionNames;

            return View(courseDetails);
        }

        [HttpGet]
        public ActionResult Comparison()
        {
            var db = new CoursesEntities();
            var courses = db.Course.AsEnumerable();
            ViewBag.Courses = db.Course.Select(x => x.Name).ToList();

            return View();
        }

        [HttpPost]
        public ActionResult Comparison(string course1, string course2)
        {
            var vm = new ComparisonViewModel();
            var db = new CoursesEntities();

            var firstCourse = db.Course.FirstOrDefault(x => x.Name == course1);
            var secondCourse = db.Course.FirstOrDefault(x => x.Name == course2);

            if (string.IsNullOrWhiteSpace(course1) || string.IsNullOrWhiteSpace(course2) ||
                firstCourse.CourseID == secondCourse.CourseID)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var course1SKillIds = db.CourseSkill
                .Where(x => x.CourseID == firstCourse.CourseID)
                .Select(x => x.SkillID)
                .ToList();

            var course2SKillIds = db.CourseSkill
                .Where(x => x.CourseID == secondCourse.CourseID)
                .Select(x => x.SkillID)
                .ToList();

            var skills1 = db.Skills
                .Where(x => course1SKillIds.Contains(x.SkillID))
                .Select(x => x.Name)
                .OrderBy(x => x)
                .ToList();

            var skills2 = db.Skills
                .Where(x => course2SKillIds.Contains(x.SkillID))
                .Select(x => x.Name)
                .OrderBy(x => x)
                .ToList();

            var course1PositionIds = db.CoursePoosition
                .Where(x => x.CourseID == firstCourse.CourseID)
                .Select(x => x.PositionID)
                .ToList();

            var course2PositionIds = db.CoursePoosition
                .Where(x => x.CourseID == secondCourse.CourseID)
                .Select(x => x.PositionID)
                .ToList();

            var positions1 = db.Positions
                .Where(x => course1PositionIds.Contains(x.PositionID))
                .Select(x => x.Name)
                .OrderBy(x => x)
                .ToList();

            var positions2 = db.Positions
                .Where(x => course2PositionIds.Contains(x.PositionID))
                .Select(x => x.Name)
                .OrderBy(x => x)
                .ToList();

            var course1ProfFieldIds = db.CourseProfessionalField
                .Where(x => x.CourseID == firstCourse.CourseID)
                .Select(x => x.ProfessinalFieldID)
                .ToList();

            var course2ProfFieldIds = db.CourseProfessionalField
                .Where(x => x.CourseID == secondCourse.CourseID)
                .Select(x => x.ProfessinalFieldID)
                .ToList();

            var profFields1 = db.ProfessionalField
                .Where(x => course1ProfFieldIds.Contains(x.ProfessionalFieldID))
                .Select(x => x.Name)
                .OrderBy(x => x)
                .ToList();

            var profFields2 = db.ProfessionalField
                .Where(x => course2ProfFieldIds.Contains(x.ProfessionalFieldID))
                .Select(x => x.Name)
                .OrderBy(x => x)
                .ToList();

            vm.Course1 = firstCourse;
            vm.Course2 = secondCourse;

            vm.Course1SKills = SetUniqueItems(skills1, skills2);
            vm.Course2SKills = SetUniqueItems(skills2, skills1);
            vm.Course1Positions = SetUniqueItems(positions1, positions2);
            vm.Course2Positions = SetUniqueItems(positions2, positions1);
            vm.Course1ProfesionalFields = SetUniqueItems(profFields1, profFields2);
            vm.Course2ProfesionalFields = SetUniqueItems(profFields2, profFields1);

            return View("ComparisonResult", vm);
        }

        private List<Tuple<string, bool>> SetUniqueItems(List<string> coreSet, List<string> secondSet)
        {
            var result = new List<Tuple<string, bool>>();
            foreach (var item in coreSet)
            {
                if (secondSet.Contains(item))
                {
                    result.Add(new Tuple<string, bool>(item, true));
                }
                else
                {
                    result.Add(new Tuple<string, bool>(item, false));
                }
            }
            return result;
        }

        [HttpGet]
        public ActionResult AllCourses(int page = 1)
        {
            var db = new CoursesEntities();
            var courses = db.Course.ToList();
            var pageSize = 5;

            ViewBag.CurrentPage = page;

            return View(courses.Skip((page - 1) * pageSize).Take(pageSize).OrderBy(x => x.Name).ToList());
        }
    }
}