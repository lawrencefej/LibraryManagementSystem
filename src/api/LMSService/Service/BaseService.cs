using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LMSEntities.Helpers;
using LMSService.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace LMSService.Service
{
    public class BaseService<TBase, TDetail, TList, TService>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        // private readonly ILogger<TDetail> _logger;
        public IMapper Mapper { get; }

        public BaseService(IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            // Mapper = mapper;
            // _logger = logger;
            _mapper = mapper;
            Mapper = _mapper;
            _httpContextAccessor = httpContextAccessor;

        }

        public BaseService(IMapper mapper)
        {
            _mapper = mapper;
            Mapper = _mapper;

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
                TDetail assetForReturn = _mapper.Map<TDetail>(item);

                return LmsResponseHandler<TDetail>.Successful(assetForReturn);
            }

            // _logger.LogInformation($"Unsuccessful Item retrieval");

            return LmsResponseHandler<TDetail>.Failed("");
        }

        protected async Task<PagedList<TList>> MapPagination(IQueryable<TBase> queryableList, PaginationParams paginationParams)
        {
            PagedList<TBase> returnList = await PagedList<TBase>.CreateAsync(queryableList, paginationParams.PageNumber, paginationParams.PageSize);

            PagedList<TList> assetToReturn = _mapper.Map<PagedList<TList>>(returnList);

            return PagedListMapper<TList, TBase>.MapPagedList(assetToReturn, returnList);
        }

        // protected LmsResponseHandler<TDetail> MapPagination(TBase item, TDetail itemToReturn)
        // {
        //     PagedList<TDetail> itemsToReturn = _mapper.Map<PagedList<TDetail>>(item);

        //     return PagedListMapper<TDetail, TBase>.MapPagedList(itemsToReturn, item);
        // }

        protected int GetLoggedInUserId()
        {
            return _httpContextAccessor.HttpContext.User.GetUserId();
        }

        protected int GetLoggedInUserEmail()
        {
            return _httpContextAccessor.HttpContext.User.GetUserId();
        }

        protected bool IsCurrentUser(int id)
        {
            return _httpContextAccessor.HttpContext.User.IsCurrentUser(id);
        }

    }
}
