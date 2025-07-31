using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using LoginAppMiguel.Models;
using LoginAppMiguel.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace LoginAppMiguel.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UsersController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index(string? message = null, string? messageType = null)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || currentUser.IsBlocked)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Login", "Account");
            }
            var users = await _userManager.Users
                .OrderByDescending(u => u.LastLoginTime ?? u.RegistrationTime)
                .ToListAsync();
            var userDisplayModels = users.Select(u => new UserDisplayModel
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                LastLoginTime = u.LastLoginTime,
                IsBlocked = u.IsBlocked,
                RegistrationTime = u.RegistrationTime
            }).ToList();

            var viewModel = new UserManagementViewModel
            {
                Users = userDisplayModels,
                Message = message,
                MessageType = messageType
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> PerformAction([FromBody] UserActionRequest request)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || currentUser.IsBlocked)
            {
                return Json(new { success = false, message = "Your session has expired or your account has been blocked.", redirectToLogin = true });
            }

            if (request.UserIds == null || !request.UserIds.Any())
            {
                return Json(new { success = false, message = "No users selected." });
            }

            var message = "";var messageType = "success";
            try
            {
                switch (request.Action.ToLower())
                {
                    case "block":
                        await BlockUsers(request.UserIds);
                        message = $"Se bloquearon {request.UserIds.Count} user(s) successfully.";
                        break;
                    case "unblock":
                        await UnblockUsers(request.UserIds);
                        message = $"Se desbloquearon {request.UserIds.Count} user(s) successfully.";
                        break;
                    case "delete":
                        await DeleteUsers(request.UserIds);
                        message = $"Se eliminaron {request.UserIds.Count} user(s) successfully.";
                        break;
                    default:
                        return Json(new { success = false, message = "Invalid action or something went wrong." });
                }
                if (request.UserIds.Contains(currentUser.Id))
                {
                    await _signInManager.SignOutAsync();
                    return Json(new { success = true, message = message, redirectToLogin = true });
                }
                return Json(new { success = true, message = message, messageType = messageType });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"error to do this action: {ex.Message}" });
            }
        }

        private async Task BlockUsers(List<string> userIds)
        {
            var users = await _userManager.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
            foreach (var user in users)
            {
                user.IsBlocked = true;
                await _userManager.UpdateAsync(user);
            }
        }

        private async Task UnblockUsers(List<string> userIds)
        {
            var users = await _userManager.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
            foreach (var user in users)
            {
                user.IsBlocked = false;
                await _userManager.UpdateAsync(user);
            }
        }

        private async Task DeleteUsers(List<string> userIds)
        {
            var users = await _userManager.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
            foreach (var user in users)
            {
                await _userManager.DeleteAsync(user);
            }
        }
    }
}
