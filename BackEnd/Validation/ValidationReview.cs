using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Validation
{
    public class ValidationReview : ValidationAttribute
    {

        public ValidationReview(int valueMax)
           : base("{0} Supera Maximo(5) o Minimo(1) de Valoración")
        {
            _valuemax = valueMax;
        }

        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                int nummaxvaloracion;
                if (Int32.TryParse(value.ToString(), out nummaxvaloracion))
                {
                    if (nummaxvaloracion > _valuemax || nummaxvaloracion<1)
                    {
                        var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                        return new ValidationResult(errorMessage);
                    }
                }
            }
            return ValidationResult.Success;
        }

        private readonly int _valuemax;

    }
}
