using FluentValidation;
using LMSEntities.DataTransferObjects;
using LMSEntities.Enumerations;

namespace LMSService.Validators
{
    public class UpdateAdminRoleValidator : AbstractValidator<UpdateAdminRoleDto>
    {
        public UpdateAdminRoleValidator()
        {
            RuleFor(r => r.Id).NotEmpty()
                .WithMessage("Please select a valid user")
                .GreaterThanOrEqualTo(1)
                .WithMessage("Please select a valid user");

            RuleFor(r => r.Role).NotEmpty()
                .IsInEnum()
                .WithMessage("Please select valid user role");

        }
    }
}
