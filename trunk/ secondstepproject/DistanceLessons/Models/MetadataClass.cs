using System;
//for attributes
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using DistanceLessons.Attributes;

namespace DistanceLessons.Models
{
    [MetadataType(typeof(InformationMetadata))]
    public partial class Information
    {
        public class InformationMetadata
        {
            [ScaffoldColumn(false)]
            public Guid InformationId { get; set; }

            [Display(Name = "Information_LastName", ResourceType = typeof(Resources.Metadata))]
            [Required(ErrorMessageResourceType = typeof(Resources.Metadata), ErrorMessageResourceName = "Information_LastName_Required")]
            [StringLength(50, ErrorMessageResourceType = typeof(Resources.Metadata), ErrorMessageResourceName = "Information_LastName_StringLength")]
            public string LastName { get; set; }

            [Display(Name = "Information_FirstName", ResourceType = typeof(Resources.Metadata))]
            [Required(ErrorMessageResourceType = typeof(Resources.Metadata), ErrorMessageResourceName = "Information_FirstName_Required")]
            [StringLength(50, ErrorMessageResourceType = typeof(Resources.Metadata), ErrorMessageResourceName = "Information_FirstName_StringLength")]
            public string FirstName { get; set; }

            [Display(Name = "Information_MidName", ResourceType = typeof(Resources.Metadata))]
            [Required(ErrorMessageResourceType = typeof(Resources.Metadata), ErrorMessageResourceName = "Information_MidName_Required")]
            [StringLength(50, ErrorMessageResourceType = typeof(Resources.Metadata), ErrorMessageResourceName = "Information_MidName_StringLength")]
            public string MidName { get; set; }

            [ScaffoldColumn(false)]
            [Display(Name = "Information_DateOfBirth", ResourceType = typeof(Resources.Metadata))]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
            public DateTime DateOfBirth { get; set; }

            [Display(Name = "Information_About", ResourceType = typeof(Resources.Metadata))]
            [StringLength(256, ErrorMessageResourceType = typeof(Resources.Metadata), ErrorMessageResourceName = "Information_About_StringLength")]
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

            [Display(Name = "Contact_Skype", ResourceType = typeof(Resources.Metadata))]
            [StringLength(50, ErrorMessageResourceType = typeof(Resources.Metadata), ErrorMessageResourceName = "Contact_Skype_StringLength")]
            public string Skype { get; set; }

            [Display(Name = "Contact_ICQ", ResourceType = typeof(Resources.Metadata))]
            [StringLength(15, ErrorMessageResourceType = typeof(Resources.Metadata), ErrorMessageResourceName = "Contact_ICQ_StringLength")]
            public string ICQ_QIP { get; set; }

            [Display(Name = "Contact_Tel", ResourceType = typeof(Resources.Metadata))]
            [StringLength(15, ErrorMessageResourceType = typeof(Resources.Metadata), ErrorMessageResourceName = "Contact_Tel_StringLength")]
            public string Telephone { get; set; }

            [Display(Name = "Contact_Other", ResourceType = typeof(Resources.Metadata))]
            [StringLength(120, ErrorMessageResourceType = typeof(Resources.Metadata), ErrorMessageResourceName = "Contact_Other_StringLength")]
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

            [Display(Name = "U_Login", ResourceType = typeof(Resources.Metadata))]
            public string Login { get; set; }

            [Display(Name = "U_Email", ResourceType = typeof(Resources.Metadata))]
            [DataType("Email", ErrorMessage="Текст не є електронною скринькою")]
            public string Email { get; set; }

            [Display(Name = "U_CreatedDate", ResourceType = typeof(Resources.Metadata))]
            public DateTime CreatedDate { get; set; }

            [Display(Name = "U_LastLogin", ResourceType = typeof(Resources.Metadata))]
            public string LastLoginDate { get; set; }

            [Display(Name = "U_Actived", ResourceType = typeof(Resources.Metadata))]
            public bool IsActived { get; set; }

            [Display(Name = "U_Banned", ResourceType = typeof(Resources.Metadata))]
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

            [Display(Name = "R_Role", ResourceType = typeof(Resources.Metadata))]
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

            [Display(Name = "T_Question", ResourceType = typeof(Resources.Metadata))]
            [Required(ErrorMessageResourceType = typeof(Resources.Metadata), ErrorMessageResourceName = "T_Question_Required")]
            [StringLength(256, ErrorMessageResourceType = typeof(Resources.Metadata), ErrorMessageResourceName = "T_Question_StringLength")]
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

            [Display(Name = "A_Answer", ResourceType = typeof(Resources.Metadata))]
            [Required(ErrorMessageResourceType = typeof(Resources.Metadata), ErrorMessageResourceName = "A_Answer_Required")]
            [StringLength(128, ErrorMessageResourceType = typeof(Resources.Metadata), ErrorMessageResourceName = "A_Answer_StringLength")]
            public string Answer1 { get; set; }

            [Display(Name = "A_Valid", ResourceType = typeof(Resources.Metadata))]
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

            [Display(Name = "N_Title", ResourceType = typeof(Resources.Metadata))]
            [Required(ErrorMessageResourceType = typeof(Resources.Metadata), ErrorMessageResourceName = "N_Title_Required")]
            public string Title { get; set; }

            [AllowHtml]
            [Display(Name = "N_Text", ResourceType = typeof(Resources.Metadata))]
            [Required(ErrorMessageResourceType = typeof(Resources.Metadata), ErrorMessageResourceName = "N_Text_Required")]
            public string Text { get; set; }

            [ScaffoldColumn(false)]
            [Display(Name = "N_Publication", ResourceType = typeof(Resources.Metadata))]
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

            [Display(Name = "C_Title", ResourceType = typeof(Resources.Metadata))]
            [Required(ErrorMessageResourceType = typeof(Resources.Metadata), ErrorMessageResourceName = "C_Title_Required")]
            [StringLength(64, ErrorMessageResourceType = typeof(Resources.Metadata), ErrorMessageResourceName = "C_Title_StringLength")]
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

            [Display(Name = "Co_Title", ResourceType = typeof(Resources.Metadata))]
            [Required(ErrorMessageResourceType = typeof(Resources.Metadata), ErrorMessageResourceName = "C_Title_Required")]
            [StringLength(64, ErrorMessageResourceType = typeof(Resources.Metadata), ErrorMessageResourceName = "C_Title_StringLength")]
            public string Title { get; set; }

            [Display(Name = "Co_Desc", ResourceType = typeof(Resources.Metadata))]
            [StringLength(512, ErrorMessageResourceType = typeof(Resources.Metadata), ErrorMessageResourceName = "Co_Desc_StringLength")]
            public string Description { get; set; }

            [ScaffoldColumn(false)]
            public Guid CategoryId { get; set; }

            [ScaffoldColumn(false)]
            public Guid UserId { get; set; }

            [Display(Name = "Сo_Subscrb", ResourceType = typeof(Resources.Metadata))]
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

            [Display(Name = "L_Title", ResourceType = typeof(Resources.Metadata))]
            [Required(ErrorMessageResourceType = typeof(Resources.Metadata), ErrorMessageResourceName = "C_Title_Required")]
            [StringLength(64, ErrorMessageResourceType = typeof(Resources.Metadata), ErrorMessageResourceName = "C_Title_StringLength")]
            public string Title { get; set; }

            [ScaffoldColumn(false)]
            [Display(Name = "N_Publication", ResourceType = typeof(Resources.Metadata))]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy HH:mm}")]
            public DateTime Publication { get; set; }

            [ScaffoldColumn(false)]
            public Guid CourseId { get; set; }

            [Display(Name = "L_Desc", ResourceType = typeof(Resources.Metadata))]
            [StringLength(50, ErrorMessageResourceType = typeof(Resources.Metadata), ErrorMessageResourceName = "L_Desc_StringLength")]
            public string Description { get; set; }

            [ScaffoldColumn(false)]
            [Display(Name = "L_Author", ResourceType = typeof(Resources.Metadata))]
            public Guid UserId { get; set; }

            [Display(Name = "L_Accepted", ResourceType = typeof(Resources.Metadata))]
            public bool IsAcceptMainTeacher { get; set; }

            [ScaffoldColumn(false)]
            public Guid? ModuleId { get; set; }
        }
    }


    [MetadataType(typeof(MessageMetadata))]
    public partial class Message
    {

        public class MessageMetadata
        {
            [ScaffoldColumn(false)]
            public Guid MessageId { get; set; }

            [ScaffoldColumn(false)]
            public Guid UserId_From { get; set; }

            [ScaffoldColumn(false)]
            public Guid UserId_To { get; set; }

            [Display(Name = "M_Title", ResourceType = typeof(Resources.Metadata))]
            [Required(ErrorMessageResourceType = typeof(Resources.Metadata), ErrorMessageResourceName = "M_Title_Required")]
            public string Title { get; set; }

            [Display(Name = "M_Mes", ResourceType = typeof(Resources.Metadata))]
            [Required(ErrorMessageResourceType = typeof(Resources.Metadata), ErrorMessageResourceName = "M_Mes_Required")]
            public string Message1 { get; set; }
        }
    }

    [MetadataType(typeof(ModuleMetadata))]
    public partial class Module
    {
        public class ModuleMetadata
        {
            [ScaffoldColumn(false)]
            public Guid ModuleId { get; set; }

            [Display(Name = "Mod_Title", ResourceType = typeof(Resources.Metadata))]
            [Required(ErrorMessageResourceType = typeof(Resources.Metadata), ErrorMessageResourceName = "C_Title_Required")]
            [StringLength(256, ErrorMessageResourceType = typeof(Resources.Metadata), ErrorMessageResourceName = "Mod_Title_StringLength")]
            public string Title { get; set; }

            [Display(Name = "Mod_Time", ResourceType = typeof(Resources.Metadata))]
            [Required(ErrorMessageResourceType = typeof(Resources.Metadata), ErrorMessageResourceName = "Mod_Time_Required")]
            [Range(1, 300, ErrorMessage = "Час на проходження модульного контролю може коливатись в межах від 1 до 300 хвилин")]
            public int TimeForPassTest { get; set; }

            [ScaffoldColumn(false)]
            [Display(Name = "Mod_Start", ResourceType = typeof(Resources.Metadata))]
            [Required(ErrorMessageResourceType = typeof(Resources.Metadata), ErrorMessageResourceName = "Mod_Start_Required")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
            public DateTime DateBeginTesting { get; set; }

            [ScaffoldColumn(false)]
            [Display(Name = "Mod_End", ResourceType = typeof(Resources.Metadata))]
            [Required(ErrorMessageResourceType = typeof(Resources.Metadata), ErrorMessageResourceName = "Mod_End_Required")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
            public DateTime DateEndTesting { get; set; }

            [ScaffoldColumn(false)]
            public Guid CourseId { get; set; }

            [Display(Name = "Mod_MaxPoints", ResourceType = typeof(Resources.Metadata))]
            [Required(ErrorMessageResourceType = typeof(Resources.Metadata), ErrorMessageResourceName = "Mod_MaxPoints_Required")]
            [Range(0, 100, ErrorMessage = "Кількість балів за модуль не може бути меншою 0 і більшою 100")]
            public int MaxPoints { get; set; }

            [Display(Name = "Mod_QCount", ResourceType = typeof(Resources.Metadata))]
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

            [Display(Name = "UMod_Passed", ResourceType = typeof(Resources.Metadata))]
            public float? Passed { get; set; }

            [ScaffoldColumn(false)]
            public Guid? ModuleId { get; set; }

            [ScaffoldColumn(false)]
            public Guid UserId { get; set; }

            [ScaffoldColumn(false)]
            [Display(Name = "UMod_Start", ResourceType = typeof(Resources.Metadata))]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
            public DateTime SpendTime { get; set; }

            [ScaffoldColumn(false)]
            [Display(Name = "UMod_End", ResourceType = typeof(Resources.Metadata))]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
            public DateTime PassedTime { get; set; }
        }
    }
    [MetadataType(typeof(SendMessageMetadata))]
    public partial class SendMessageModel
    {
        public class SendMessageMetadata
        {
            [Display(Name = "M_ToUser", ResourceType = typeof(Resources.Metadata))]
            [ExistUserInDB(ErrorMessage = "Такого користувача не існує")]
            public String Name { get; set; }
        }
    }
}