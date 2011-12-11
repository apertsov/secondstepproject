using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using DistanceLessons.Models;

namespace DistanceLessons.Attributes
{
    public class ExistUserInDBAttribute : ValidationAttribute, IClientValidatable
    {
        private DataEntitiesManager _db = new DataEntitiesManager();

        public override bool IsValid(object value)
        {
            String text = value as String;
            return (_db.ExistUserWithOutRegister(text));
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(metadata.DisplayName),
                ValidationType = "existuserindb",
            };
            yield return rule;
        }
    }
}