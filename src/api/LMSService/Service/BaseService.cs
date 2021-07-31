using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LMSEntities.Helpers;
using LMSRepository.Data;
using LMSService.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace LMSService.Service
{
    public class BaseService<TBase, TDetail, TList, TService>
    {
        protected IMapper Mapper { get; }

        protected ILogger<TService> Logger { get; }

        protected IHttpContextAccessor HttpContextAccessor { get; }

        protected DataContext Context { get; set; }

        public BaseService(DataContext context, IMapper mapper, ILogger<TService> logger, IHttpContextAccessor httpContextAccessor)
        {
            Context = context;
            Mapper = mapper;
            Logger = logger;
            HttpContextAccessor = httpContextAccessor;
        }

        public BaseService(DataContext context, IMapper mapper, ILogger<TService> logger)
        {
            Context = context;
            Mapper = mapper;
            Logger = logger;

        }

        protected static LmsResponseHandler<TDetail> ReturnErrors(IEnumerable<IdentityError> identityErrors)
        {
            List<string> errors = new();

            foreach (IdentityError error in identityErrors)
            {
                errors.Add(error.Description);
            }

            return LmsResponseHandler<TDetail>.Failed(errors);
        }

        protected LmsResponseHandler<TDetail> MapDetailReturn(TBase item)
        {
            if (item != null)
            {
                TDetail assetForReturn = Mapper.Map<TDetail>(item);

                return LmsResponseHandler<TDetail>.Successful(assetForReturn);
            }

            Logger.LogInformation($"Unsuccessful Item retrieval");

            return LmsResponseHandler<TDetail>.Failed("No data was found");
        }

        protected async Task<PagedList<TList>> MapPagination(IQueryable<TBase> queryableList, PaginationParams paginationParams)
        {
            PagedList<TBase> returnList = await PagedList<TBase>.CreateAsync(queryableList, paginationParams.PageNumber, paginationParams.PageSize);

            PagedList<TList> assetToReturn = Mapper.Map<PagedList<TList>>(returnList);

            return PagedListMapper<TList, TBase>.MapPagedList(assetToReturn, returnList);
        }

        protected int GetLoggedInUserId()
        {
            return HttpContextAccessor.HttpContext.User.GetUserId();
        }

        protected int GetLoggedInUserEmail()
        {
            return HttpContextAccessor.HttpContext.User.GetUserId();
        }

        protected bool IsCurrentUser(int id)
        {
            return HttpContextAccessor.HttpContext.User.IsCurrentUser(id);
        }

    }
}
