using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookFace.Services.System
{
    public class ClassValidator : IValidator
    {
        public (bool, ICollection<ValidationResult>) IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return (Validator.TryValidateObject(dto, validationContext, validationResult, true), validationResult);
        }
    }
}
