﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

            [DisplayName("Додаткова інформація:")]
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

            [DisplayName("Скайп")]
            [StringLength(50, ErrorMessage = "Скайп повинен мати менше 50 символів")]
            public string Skype { get; set; }

            [DisplayName("ICQ")]
            [StringLength(20, ErrorMessage = "Номер аськи повинен мати менше 20 символів")]
            public string ICQ_QIP { get; set; }

            [DisplayName("Контактний телефон")]
            [StringLength(50, ErrorMessage = "Телефон повинен мати менше 50 символів")]
            public string Telephone { get; set; }

            [DisplayName("Інші контактні дані")]
            [StringLength(128, ErrorMessage = "Інші контактні дані обмежені 128 символами")]
            public string Other { get; set; }

            [ScaffoldColumn(false)]
            public Guid UserId { get; set; }

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

    [MetadataType(typeof(CategoriesMetadata))]
    public partial class Categories
    {
        public class CategoriesMetadata
        {
            [ScaffoldColumn(false)]
            public Guid CategoryId { get; set; }

            [Required(ErrorMessage = "Заповніть поле категорії.")]
            [DisplayName("Категорія")]
            [StringLength(64, ErrorMessage = "Назва категорії обмежена 64 символами")]
            public string Category { get; set; }
        }
    }

    [MetadataType(typeof(CourseMetadata))]
    public partial class Course
    {
        public class CourseMetadata
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

            [DisplayName("Чи потрібна підписка")]
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
            [DisplayName("Дата публікації")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
            public DateTime Publication { get; set; }

            [ScaffoldColumn(false)]
            public Guid CourseId { get; set; }

            [DisplayName("Опис лекції")]
            [StringLength(64, ErrorMessage = "Опис лекції обмежений 64 символами")]
            public string Description { get; set; }

            [ScaffoldColumn(false)]
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

            [Required(ErrorMessage = "Опис модуля відсутній")]
            [DisplayName("Опис модуля")]
            [StringLength(256, ErrorMessage = "Опис модуля обмежений 256 символами")]
            public string Title { get; set; }

            [Required(ErrorMessage = "Час здачі тестів модуля відсутній")]
            [DisplayName("Час здачі тестів модуля")]
            public int TimeForPassTest { get; set; }

            [ScaffoldColumn(false)]
            [DisplayName("Час початку тестування")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
            public DateTime DateBeginTesting { get; set; }

            [ScaffoldColumn(false)]
            [DisplayName("Час закінчення тестування")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
            public DateTime DatEndTesting { get; set; }

            [ScaffoldColumn(false)]
            public Guid CourseId { get; set; }

            [Required(ErrorMessage = "Час здачі тестів модуля відсутній")]
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