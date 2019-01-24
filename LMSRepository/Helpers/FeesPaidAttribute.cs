using LMSLibrary.Dto;
using System.ComponentModel.DataAnnotations;

namespace LMSLibrary.Helpers
{
    public class FeesPaidAttribute : ValidationAttribute
    {
        public string Fee { get; set; }
        //private decimal _fee;

        //public FeesPaidAttribute(decimal fee)
        //{
        //    _fee = fee;
        //}
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            decimal d = decimal.Parse(Fee);
            ReserveForCreationDto reserve = (ReserveForCreationDto)validationContext.ObjectInstance;

            if (reserve.Fees != d)
            {
                return new ValidationResult(GetErrorMessage(reserve.Fees));
            }

            return ValidationResult.Success;
        }

        private string GetErrorMessage(decimal fee)
        {
            return $"This member has an outstanding bill of {fee}.";
        }
    }
}
