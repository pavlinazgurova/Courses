//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Courses.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Course
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Course()
        {
            this.CoursePoosition = new HashSet<CoursePoosition>();
            this.CourseProfessionalField = new HashSet<CourseProfessionalField>();
            this.CourseSkill = new HashSet<CourseSkill>();
            this.CoursePoosition1 = new HashSet<CoursePoosition>();
            this.CourseProfessionalField1 = new HashSet<CourseProfessionalField>();
            this.CourseSkill1 = new HashSet<CourseSkill>();
        }
    
        public int CourseID { get; set; }
        public string Name { get; set; }
        public Nullable<int> LevelOfEducationID { get; set; }
        public Nullable<int> FormOfEducationID { get; set; }
        public Nullable<int> Duration { get; set; }
        public Nullable<int> DepartmentID { get; set; }
        public string Characterization { get; set; }
        public string OtherSkills { get; set; }
        public string OtherPosition { get; set; }
        public string OtherProfessionalField { get; set; }
    
        public virtual Departments Departments { get; set; }
        public virtual FormOfEducation FormOfEducation { get; set; }
        public virtual LevelOfEducation LevelOfEducation { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CoursePoosition> CoursePoosition { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CourseProfessionalField> CourseProfessionalField { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CourseSkill> CourseSkill { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CoursePoosition> CoursePoosition1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CourseProfessionalField> CourseProfessionalField1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CourseSkill> CourseSkill1 { get; set; }
    }
}
