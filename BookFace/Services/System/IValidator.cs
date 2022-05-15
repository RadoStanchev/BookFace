using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookFace.Services.System
{
    public interface IValidator
    {
        (bool, ICollection<ValidationResult>) IsValid(object obj);
    }
}
