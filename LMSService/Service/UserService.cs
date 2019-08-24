namespace LMSService.Service
{
    //public class UserService /*: IUserService*/
    //{
    //    private readonly IMapper _mapper;
    //    private readonly IUserRepository _userRepo;

    //    private readonly UserManager<User> _userManager;
    //    private readonly IEmailSender _emailSender;
    //    private readonly ILibraryCardRepository _libraryCardRepository;
    //    private readonly ILogger<UserService> _logger;
    //    private readonly DataContext _context;

    //    public UserService(IUserRepository userRepo, IMapper mapper, DataContext context,
    //        UserManager<User> userManager, IEmailSender emailSender, ILibraryCardRepository libraryCardRepository, ILogger<UserService> logger)
    //    {
    //        _userRepo = userRepo;
    //        _mapper = mapper;
    //        _userManager = userManager;
    //        _emailSender = emailSender;
    //        _libraryCardRepository = libraryCardRepository;
    //        _logger = logger;
    //        _context = context;
    //    }

    //    public async Task<UserForDetailedDto> GetUser(int userId)
    //    {
    //        var user = await _userRepo.GetUser(userId);

    //        var userToReturn = _mapper.Map<UserForDetailedDto>(user);

    //        return userToReturn;
    //    }

    //    public async Task<UserForDetailedDto> SearchUser(SearchUserDto searchUser)
    //    {
    //        var user = await _userRepo.SearchUsers(searchUser);

    //        var userToReturn = _mapper.Map<UserForDetailedDto>(user);

    //        return userToReturn;
    //    }

    //    public async Task<UserForDetailedDto> GetUserByCardNumber(int cardId)
    //    {
    //        var user = await _userRepo.GetUserByCardId(cardId);

    //        var userToReturn = _mapper.Map<UserForDetailedDto>(user);

    //        return userToReturn;
    //    }

    //    public async Task<UserForDetailedDto> GetUserByEmail(string email)
    //    {
    //        var user = await _userRepo.GetUserByEmail(email);

    //        var userToReturn = _mapper.Map<UserForDetailedDto>(user);

    //        return userToReturn;
    //    }

    //    public async Task<IEnumerable<UserForListDto>> GetUsers()
    //    {
    //        var users = await _userRepo.GetUsers();

    //        var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);

    //        return usersToReturn;
    //    }

    //    public async Task<IEnumerable<UserForListDto>> SearchUsers(string searchString)
    //    {
    //        var users = await _userRepo.SearchUsers(searchString);

    //        var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);

    //        return usersToReturn;
    //    }

    //    public async Task UpdateMember(UserForUpdateDto userForUpdate)
    //    {
    //        var user = await _userRepo.GetUser(userForUpdate.Id);

    //        if (user == null)
    //        {
    //            throw new NoValuesFoundException("User was not found");
    //        }

    //        _mapper.Map(userForUpdate, user);
    //        await _context.SaveChangesAsync();
    //    }

    //    public async Task DeleteUser(int memberId)
    //    {
    //        var member = await _userRepo.GetUser(memberId);

    //        if (member == null)
    //        {
    //            _logger.LogWarning($"memberID: {memberId} was not found");
    //            throw new NoValuesFoundException($"memberID: {memberId} was not found");
    //        }

    //        _context.Remove(member.LibraryCard);
    //        _context.Remove(member);
    //        await _context.SaveChangesAsync();
    //        _logger.LogInformation($"memberID: {member.Id} was deleted");
    //    }

    //    private async Task MemberWelcomeMessage(UserForDetailedDto user)
    //    {
    //        var body = $"Welcome {TitleCase(user.FirstName)}, " +
    //            $"<p>A Sentinel Library account has been created for you.</p> " +
    //            $"<p>Your Library Card Number is {user.LibraryCardNumber}</p> " +
    //            $" " +
    //            $"<p>Thanks.</p> " +
    //            $"<p>Management</p>";

    //        await _emailSender.SendEmail(user.Email, "Welcome Letter", body);
    //    }

    //    public static string TitleCase(string strText)
    //    {
    //        return new CultureInfo("en").TextInfo.ToTitleCase(strText.ToLower());
    //    }

    //    public async Task<IEnumerable<UserForDetailedDto>> SearchUsers(SearchUserDto searchUser)
    //    {
    //        var users = await _userRepo.SearchUsers(searchUser);

    //        var usersToReturn = _mapper.Map<IEnumerable<UserForDetailedDto>>(users);

    //        return usersToReturn;
    //    }

    //    public async Task<PagedList<User>> GetAllMembersAsync(PaginationParams paginationParams)
    //    {
    //        var users = _userRepo.GetAll();

    //        return await PagedList<User>.CreateAsync(users, paginationParams.PageNumber, paginationParams.PageSize);
    //    }
    //}
}