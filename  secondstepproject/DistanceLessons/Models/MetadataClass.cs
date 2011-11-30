using System;
//for attributes
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DistanceLessons.Models
{
    [MetadataType(typeof(InformationMetadata))]
    public partial class Information
    {
        public class InformationMetadata
        {
            [ScaffoldColumn(false)]
            public Guid InformationId { get; set; }

            [DisplayName("Прізвище:")]
            [Required(ErrorMessage = "Введіть прізвище")]
            [StringLength(50, ErrorMessage = "Прізвище повинно мати менше 50 символів")]
            public string LastName { get; set; }

            [DisplayName("Ім'я:")]
            [Required(ErrorMessage = "Введіть ім'я")]
            [StringLength(50, ErrorMessage = "Ім'я повинно мати менше 50 символів")]
            public string FirstName { get; set; }

            [DisplayName("По-батькові:")]
            [Required(ErrorMessage = "Введіть по-батькові")]
            [StringLength(50, ErrorMessage = "Поле по-батькові повинно мати менше 50 символів")]
            public string MidName { get; set; }

            [ScaffoldColumn(false)]
            [DisplayName("Дата народження:")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
            public DateTime DateOfBirth { get; set; }

            [DisplayName("Про себе:")]
            [StringLength(256, ErrorMessage = "Поле додаткової інформації обмежено 256 символами")]
            public string About { get; set; }

            [ScaffoldColumn(false)]
            public Guid UserId { get; set; }

        }
    }


    [MetadataType(typeof(ContactMetadata))]
    public partial class Contact
    {

        public class ContactMetadata
        {
            [ScaffoldColumn(false)]
            public Guid ContactId { get; set; }

            [DisplayName("Скайп:")]
            [StringLength(50, ErrorMessage = "Скайп повинен мати менше 50 символів")]
            public string Skype { get; set; }

            [DisplayName("ICQ:")]
            [StringLength(20, ErrorMessage = "Номер аськи повинен мати менше 20 символів")]
            public string ICQ_QIP { get; set; }

            [DisplayName("Контактний телефон:")]
            [StringLength(50, ErrorMessage = "Телефон повинен мати менше 50 символів")]
            public string Telephone { get; set; }

            [DisplayName("Інші контактні дані:")]
            [StringLength(128, ErrorMessage = "Інші контактні дані обмежені 128 символами")]
            public string Other { get; set; }

            [ScaffoldColumn(false)]
            public Guid UserId { get; set; }

        }
    }

    [MetadataType(typeof(UserMetadata))]
    public partial class User
    {

        public class UserMetadata
        {
            [ScaffoldColumn(false)]
            public Guid UserId { get; set; }

  
            [Display(Name = "Логін")]
            public string Login { get; set; }

            [DisplayName("Електронна скринька")]
            [DataType("Email",ErrorMessage="Текст не є електронною скринькою")]
            public string Email { get; set; }

            [DisplayName("Дата реєстрації")]
            public DateTime CreatedDate { get; set; }

            [DisplayName("Час останнього входження на сторінку")]
            public string LastLoginDate { get; set; }

            [DisplayName("Чи активувався")]
             public bool IsActived { get; set; }

            [DisplayName("Чи заблокований")]
            public bool IsLockedOut { get; set; }

        }
    }

    [MetadataType(typeof(RoleMetadata))]
    public partial class Role
    {
        public class RoleMetadata
        {
            [ScaffoldColumn(false)]
            public Guid RoleId { get; set; }

            [DisplayName("Роль")]
            public string Name { get; set; }
        }
    }
    [MetadataType(typeof(TestMetadata))]
    public partial class Test
    {

        public class TestMetadata
        {
            [ScaffoldColumn(false)]
            public Guid TestId { get; set; }

            [DisplayName("Запитання")]
            [Required(ErrorMessage = "Заповніть поле запитання.")]
            [StringLength(256, ErrorMessage = "Запитання обмежене 256 символами")]
            public string Question { get; set; }

            [ScaffoldColumn(false)]
            public Guid LessonId { get; set; }
        }
    }

    [MetadataType(typeof(AnswerMetadata))]
    public partial class Answer
    {

        public class AnswerMetadata
        {
            [ScaffoldColumn(false)]
            public Guid AnswerId { get; set; }

            [Required(ErrorMessage = "Заповніть поле відповіді.")]
            [DisplayName("Текст відповіді")]
            [StringLength(128, ErrorMessage = "Відповідь обмежена 128 символами")]
            public string Answer1 { get; set; }

            [DisplayName("Чи правильна відповідь?")]
            public bool Valid { get; set; }

            [ScaffoldColumn(false)]
            public Guid TestId { get; set; }
        }
    }


    [MetadataType(typeof(NewsMetadata))]
    public partial class News
    {
        public class NewsMetadata
        {
            [ScaffoldColumn(false)]
            public Guid NewId { get; set; }

            [DisplayName("Заголовок")]
            public string Title { get; set; }

            [AllowHtml]
            [DisplayName("Введіть текст:")]
            public string Text { get; set; }

            [ScaffoldColumn(false)]
            [DisplayName("Дата:")]
            public DateTime Publication { get; set; }

            [ScaffoldColumn(false)]
            public Guid UserId { get; set; }
        }
    }

    [MetadataType(typeof(CategoryMetadata))]
    public partial class Category
    {
        public class CategoryMetadata
        {
            [ScaffoldColumn(false)]
            public Guid CategoryId { get; set; }

            [Required(ErrorMessage = "Заповніть поле категорії.")]
            [DisplayName("Категорія")]
            [StringLength(64, ErrorMessage = "Назва категорії обмежена 64 символами")]
            public string Category1 { get; set; }
        }
    }

    [MetadataType(typeof(CoursMetadata))]
    public partial class Cours
    {
        public class CoursMetadata
        {
            [ScaffoldColumn(false)]
            public Guid CourseId { get; set; }

            [Required(ErrorMessage = "Заповніть поле назви курсу.")]
            [DisplayName("Назва курсу")]
            [StringLength(64, ErrorMessage = "Назва курсу обмежена 64 символами")]
            public string Title { get; set; }

            [DisplayName("Опис курсу")]
            [StringLength(512, ErrorMessage = "Опис курсу обмежений 512 символами")]
            public string Description { get; set; }

            [ScaffoldColumn(false)]
            public Guid CategoryId { get; set; }

            [ScaffoldColumn(false)]
            public Guid UserId { get; set; }

            [DisplayName("Потрібна підписка")]
            public bool IsSubscribing { get; set; }
        }
    }

    [MetadataType(typeof(LessonMetadata))]
    public partial class Lesson
    {
        public class LessonMetadata
        {
            [ScaffoldColumn(false)]
            public Guid LessonId { get; set; }

            [Required(ErrorMessage = "Заповніть поле назва лекції.")]
            [DisplayName("Назва лекції")]
            [StringLength(64, ErrorMessage = "Назва лекції обмежена 64 символами")]
            public string Title { get; set; }

            [ScaffoldColumn(false)]
            [DisplayName("Час публікації")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy HH:mm}")]
            public DateTime Publication { get; set; }

            [ScaffoldColumn(false)]
            public Guid CourseId { get; set; }

            [DisplayName("Опис лекції")]
            [StringLength(64, ErrorMessage = "Опис лекції обмежений 64 символами")]
            public string Description { get; set; }

            [ScaffoldColumn(false)]
            [DisplayName("Автор лекції")]
            public Guid UserId { get; set; }

            [DisplayName("Затверджена викладачем курсу")]
            public bool IsAcceptMainTeacher { get; set; }

            [ScaffoldColumn(false)]
            public Guid? ModuleId { get; set; }
        }
    }


    [MetadataType(typeof(MessegeMetadata))]
    public partial class Messege
    {
        public class MessegeMetadata
        {
            [ScaffoldColumn(false)]
            public Guid MessageId { get; set; }

            [Required(ErrorMessage = "Введіть текст листа")]
            [DisplayName("Текст листа")]
            [StringLength(512, ErrorMessage = "Текст листа обмежений 512 символами")]
            public string Message { get; set; }

            [ScaffoldColumn(false)]
            [DisplayName("Час відправки")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
            public DateTime DateOfSender { get; set; }

            [ScaffoldColumn(false)]
            public Guid UserId_From { get; set; }

            [ScaffoldColumn(false)]
            public Guid UserId_To { get; set; }

            [ScaffoldColumn(false)]
            public byte Status { get; set; }
        }
    }

    [MetadataType(typeof(ModuleMetadata))]
    public partial class Module
    {
        public class ModuleMetadata
        {
            [ScaffoldColumn(false)]
            public Guid ModuleId { get; set; }

            [Required(ErrorMessage = "Назва модуля відсутня")]
            [DisplayName("Назва модуля")]
            [StringLength(256, ErrorMessage = "Опис модуля обмежений 256 символами")]
            public string Title { get; set; }

            [Required(ErrorMessage = "Час здачі тестів модуля відсутній")]
            [DisplayName("Час здачі тестів модуля, хв")]
            [Range(1, 300, ErrorMessage = "Час на проходження модульного контролю повинен бути в межах від 1 до 300 хвилин")]

            public int TimeForPassTest { get; set; }

            [ScaffoldColumn(false)]
            [DisplayName("Дата початку тестування")]
            [Required(ErrorMessage = "Дата початку тестування відсутня")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
                 public DateTime DateBeginTesting { get; set; }

            [ScaffoldColumn(false)]
            [Required(ErrorMessage = "Дата закінчення тестування відсутня")]
            [DisplayName("Дата закінчення тестування")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
            
            public DateTime DateEndTesting { get; set; }

            [ScaffoldColumn(false)]
            public Guid CourseId { get; set; }

            [Required(ErrorMessage = "Максимальна кількість балів за модуль відсутній")]
            [DisplayName("Кількість балів за модуль")]
            [Range(0, 100, ErrorMessage = "Кількість балів за модуль не може бути меншою 0 і більшою 100")]

            public int MaxPoints { get; set; }

            [DisplayName("Кількість запитань для здачі модуля")]
            public int? CountQuestions { get; set; }
        }
    }

    [MetadataType(typeof(UserModuleMetadata))]
    public partial class UserModule
    {
        public class UserModuleMetadata
        {
            [ScaffoldColumn(false)]
            public Guid UserModuleId { get; set; }
            
            [DisplayName("Результат здачі модуля")]
            public float? Passed { get; set; }

            [ScaffoldColumn(false)]
            public Guid? ModuleId { get; set; }

            [ScaffoldColumn(false)]
            public Guid UserId { get; set; }

            [ScaffoldColumn(false)]
            [DisplayName("Час початку тестування")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
            public DateTime SpendTime { get; set; }

            [ScaffoldColumn(false)]
            [DisplayName("Час закінчення тестування")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
            public DateTime PassedTime { get; set; }
        }
    }



}