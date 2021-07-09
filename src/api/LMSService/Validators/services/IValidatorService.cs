using System;

namespace LMSService.Validators.services
{
    public interface IValidatorService
    {
        bool DoesIsbnExist(string isbn);

        bool DoesCategoryExist(int id);

        bool DoesAuthorExist(int id);

        bool IsValidYear(int year);

        bool IsValidAge(DateTime dob);
    }
}
