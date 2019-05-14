namespace Courses.Controllers
{
    using Courses.Models;
    using Courses.Models.Other;
    using System;
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
            ViewBag.ProfessionalFields = db.ProfessionalField.Select(x => x.Name).ToList();
            ViewBag.Skills = db.Skills.Select(x => x.Name).ToList();
            ViewBag.Positions = db.Positions.Select(x => x.Name).ToList();

            return View(new CourseViewModel()
            {
                Bachelor = false,
                Master = false,
                Doctor = false
            });
        }

        [HttpGet]
        public ActionResult Result(string searchString, FilterModel filterModel, int page = 1)
        {
            var db = new CoursesEntities();
            var courses = db.Course.ToList();
            var pageSize = 3;

            ViewBag.SearchString = searchString;

            if (!string.IsNullOrEmpty(searchString))
            {
                courses = courses.Where(s => s.Name.ToLower().Contains(searchString.ToLower())).ToList();
            }
            else
            {
                if (!string.IsNullOrEmpty(filterModel.Departments))
                {
                    var de = db.Departments.ToList();
                    var depID = de.Find(d => d.NameDepartments == filterModel.Departments).DepartmentID;

                    courses = courses.Where(c => c.DepartmentID == depID).ToList();
                }

                if (!string.IsNullOrEmpty(filterModel.ProfessionalFields))
                {
                    var pFields = db.ProfessionalField.ToList();
                    var cPFields = db.CourseProfessionalField.ToList();

                    var pfID = pFields.Find(pf => pf.Name == filterModel.ProfessionalFields).ProfessionalFieldID;
                    var cpfID = cPFields.FindAll(cpf => cpf.ProfessinalFieldID == pfID).Select(x => x.CourseID).ToList();

                    courses = courses.Where(c => cpfID.Any(x => c.CourseID.Equals(x))).ToList();
                }

                if (!string.IsNullOrEmpty(filterModel.Skills))
                {
                    var s = db.Skills.ToList();
                    var cSkills = db.CourseSkill.ToList();

                    var sID = s.Find(pf => pf.Name == filterModel.Skills).SkillID;
                    var csID = cSkills.FindAll(cs => cs.SkillID == sID).Select(x => x.CourseID).ToList();

                    courses = courses.Where(c => csID.Any(x => c.CourseID.Equals(x))).ToList();
                }

                if (!string.IsNullOrEmpty(filterModel.Positions))
                {
                    var p = db.Positions.ToList();
                    var cPositions = db.CoursePoosition.ToList();

                    var pID = p.Find(pf => pf.Name == filterModel.Positions).PositionID;
                    var cpID = cPositions.FindAll(cp => cp.PositionID == pID).Select(x => x.CourseID).ToList();

                    courses = courses.Where(c => cpID.Any(x => c.CourseID.Equals(x))).ToList();
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

            return View(courses.Skip((page - 1) * pageSize).Take(pageSize).ToList());
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

            return View(course);
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

            vm.Course1 = firstCourse;
            vm.Course2 = secondCourse;
            return View("ComparisonResult", vm);
        }

        [HttpGet]
        public ActionResult AllCourses(int page = 1)
        {
            var db = new CoursesEntities();
            var courses = db.Course.ToList();
            var pageSize = 3;

            ViewBag.CurrentPage = page;

            return View(courses.Skip((page - 1) * pageSize).Take(pageSize).ToList());
        }
    }
}