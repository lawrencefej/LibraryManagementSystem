using System;
using LMSService.ErrorHandling;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace LibraryManagementSystem.API.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        //public static void AddPagination(this HttpResponse response,
        //    int currentPage, int itemsPerPage, int totalItems, int totalPages)
        //{
        //    var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);
        //    var camelCaseFormatter = new JsonSerializerSettings();
        //    camelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();
        //    response.Headers.Add("Pagination",
        //         JsonConvert.SerializeObject(paginationHeader, camelCaseFormatter));
        //    response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        //}

        public static int CalculateAge(this DateTime dob)
        {
            int age = DateTime.Today.Year - dob.Year;
            if (dob.AddYears(age) > DateTime.Today)
            {
                age--;
            }

            return age;
        }

        public static bool IsValidYear(this int year)
        {
            return year >= 1000 && year <= DateTime.Today.Year + 2;
        }

        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomExceptionMiddleware>();
        }
    }
}
