﻿using CloudinaryDotNet;
using LMSRepository.Data;
using LMSRepository.Dto;
using LMSRepository.Interfaces;
using LMSRepository.Interfaces.Helpers;
using LMSRepository.Interfaces.Models;
using LMSService.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagementSystem.API.Controllers
{
    [Authorize(Policy = Helpers.Role.RequireLibrarianRole)]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepo;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;
        private readonly IAdminService _adminService;

        public AdminController(
            DataContext context,
            UserManager<User> userManager,
            IUserRepository userRepo,
            IOptions<CloudinarySettings> cloudinaryConfig, IAdminService adminService)
        {
            _context = context;
            _userManager = userManager;
            _userRepo = userRepo;
            _cloudinaryConfig = cloudinaryConfig;

            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
            _adminService = adminService;
        }

        //[Authorize(Policy = Helpers.Role.RequireLibrarianRole)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _adminService.GetAdminUsers();

            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> GetTest(AddAdminDto addAdminDto)
        {
            var user = await _adminService.CreateUser(addAdminDto);

            return CreatedAtRoute("Get", new { id = user.Id }, user);
        }

        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _adminService.GetAdminUser(id);

            return Ok(user);
        }

        // PUT: api/Admin/5
        [HttpPut]
        public async Task<IActionResult> Put(UpdateAdminDto updateAdminDto)
        {
            var user = await _adminService.GetAdminUser(updateAdminDto.Id);

            if (user == null)
            {
                return BadRequest("User for not found");
            }

            await _adminService.UpdateUser(updateAdminDto);

            return NoContent();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpGet("admins")]
        public async Task<IActionResult> GetAdmins()
        {
            //var users = await _userManager.Users
            //    .Include(p => p.ProfilePicture)
            //    .Include(c => c.UserRoles)
            //        .ThenInclude(ur => ur.Role)
            //    //.Where(u => u.UserRoles.Any(r => r.Role.Name. == (nameof(EnumRoles.Admin) || nameof(EnumRoles.Librarian))))
            //    .Where(u => u.UserRoles.Any(r => r.Role.Name != (nameof(EnumRoles.Member))))
            //    .OrderBy(u => u.Lastname).ToListAsync();

            //return Ok(users);

            var userList = await (from user in _context.Users
                                  orderby user.UserName
                                  select new
                                  {
                                      Id = user.Id,
                                      UserName = user.UserName,
                                      Test = (_context.UserRoles.Where(r => r.UserId == user.Id).Select(a => a.Role.Name)),
                                      Roles = (from userRole in user.UserRoles
                                               join role in _context.Roles
                                               on userRole.RoleId
                                               equals role.Id
                                               select role.Name)
                                  }).ToListAsync();

            return Ok(userList);
        }
    }
}