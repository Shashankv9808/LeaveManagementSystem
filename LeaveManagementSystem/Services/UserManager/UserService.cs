namespace LeaveManagementSystem.Services.UserManager;
public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ApplicationUser?> GetCurrentUserAsync()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user == null)
            return null;
        return await _userManager.GetUserAsync(user);
    }

    public async Task<List<ApplicationUser>> GetUsersInRoleAsync(string role)
    {
        if (string.IsNullOrEmpty(role))
            throw new ArgumentException("Role cannot be null or empty.", nameof(role));

        var users = await _userManager.GetUsersInRoleAsync(role);
        List<ApplicationUser> usersList = users.ToList();
        return usersList;
    }
    public async Task<ApplicationUser> GetUserByID(string userId) => await _userManager.FindByIdAsync(userId);
}