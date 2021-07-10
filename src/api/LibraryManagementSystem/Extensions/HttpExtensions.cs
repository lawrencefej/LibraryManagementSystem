using LMSEntities.Helpers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LibraryManagementSystem.Extensions
{
    public static class HttpExtensions
    {
        public static void AddPagination(this HttpResponse response,
            int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            PaginationHeader paginationHeader = new(currentPage, itemsPerPage, totalItems, totalPages);
            JsonSerializerSettings camelCaseFormatter = new()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            response.Headers.Add("Pagination",
                 JsonConvert.SerializeObject(paginationHeader, camelCaseFormatter));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}
