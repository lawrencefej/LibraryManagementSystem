using LMSEntities.Helpers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LibraryManagementSystem.API.Helpers
{
    public static class Extensions
    {
        // public static void AddApplicationError(this HttpResponse response, string message)
        // {
        //     response.Headers.Add("Application-Error", message);
        //     response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
        //     response.Headers.Add("Access-Control-Allow-Origin", "*");
        // }

        // public static void AddPagination(this HttpResponse response,
        //     int currentPage, int itemsPerPage, int totalItems, int totalPages)
        // {
        //     PaginationHeader paginationHeader = new(currentPage, itemsPerPage, totalItems, totalPages);
        //     JsonSerializerSettings camelCaseFormatter = new()
        //     {
        //         ContractResolver = new CamelCasePropertyNamesContractResolver()
        //     };
        //     response.Headers.Add("Pagination",
        //          JsonConvert.SerializeObject(paginationHeader, camelCaseFormatter));
        //     response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        // }

        // public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        // {
        //     app.UseMiddleware<CustomExceptionMiddleware>();
        // }
    }
}
