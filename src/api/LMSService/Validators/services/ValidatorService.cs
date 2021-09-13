using System;
using System.Collections.ObjectModel;
using System.Linq;
using LibraryManagementSystem.API.Helpers;
using LMSEntities.Models;
using LMSRepository.Data;

namespace LMSService.Validators.services
{
    public class ValidatorService : IValidatorService
    {
        private readonly DataContext _context;
        public ValidatorService(DataContext context)
        {
            _context = context;

        }

        public bool DoesIsbnExist(string isbn)
        {
            return !_context.LibraryAssets.Any(p => p.ISBN == isbn);
        }

        public bool DoesCategoryExist(int id)
        {
            return _context.Categories.Any(a => a.Id == id);
        }

        public bool DoesAuthorExist(int id)
        {
            return _context.Authors.Any(a => a.Id == id);
        }
        public bool DoesAssetExist(int id)
        {
            return _context.LibraryAssets.Any(a => a.Id == id);
        }

        public bool IsValidYear(int year)
        {
            return year >= 1000 && year <= DateTime.Today.Year + 2;
        }

        public bool IsValidAge(DateTime dob)
        {
            int age = dob.CalculateAge();

            return age is >= 14 and <= 120;
        }

        public bool IsValidState(int stateId)
        {
            ReadOnlyCollection<State> states = State.GetStates();

            return states.Any(s => s.Id == stateId);
        }
    }
}
