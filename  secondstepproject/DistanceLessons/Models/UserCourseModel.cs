using System.Collections.Generic;

namespace DistanceLessons.Models
{
    public class UserCourseModel
    {
        public Cours Course { get; set; }
        public int Subscribe { get; set; }
    }

    public class UserCourseAndCategoriesModel
    {
        public List<Category> Categories { get; set; }
        public List<UserCours> UserCourses { get; set; }
        public List<Lesson> CoursesLessons { get; set; } 
    }
}