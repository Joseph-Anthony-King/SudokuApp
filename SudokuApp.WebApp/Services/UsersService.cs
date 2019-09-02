using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SudokuApp.Models;
using SudokuApp.WebApp.Models.DataModel;
using SudokuApp.WebApp.Models.DTO;
using SudokuApp.WebApp.Services.Interfaces;

namespace SudokuApp.WebApp.Services
{

    public class UsersService : IUserService {

        private readonly ApplicationDbContext _context;

        public UsersService(ApplicationDbContext context) {

            _context = context;
        }

        public async Task<ActionResult<User>> GetUser(int id) {

            var user = await _context.Users.Include(u => u.Games).Include(u => u.Permissions).SingleOrDefaultAsync(u => u.Id == id);

            if (user == null) {

                return new User {

                    UserName = String.Empty,
                    FirstName = String.Empty,
                    LastName = String.Empty,
                    NickName = string.Empty,
                    Email = string.Empty,
                    Password = string.Empty
                };
            }

            return user;
        }

        public async Task<ActionResult<IEnumerable<User>>> GetUsers() {

            return await _context.Users.Include(u => u.Games).Include(u => u.Permissions).ToListAsync();
        }

        public async Task<User> CreateUser(UserDTO userDTO) {

            User user = new User {

                UserName = userDTO.UserName,
                FirstName = userDTO.FirstName, 
                LastName = userDTO.LastName,
                NickName = userDTO.NickName,
                DateCreated = DateTime.Now,
                Email = userDTO.Email,
                Password = userDTO.Password
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            return user;
        }

        public async Task UpdateUser(int id, User user) {

            if (id == user.Id) {

                _context.Entry(user).State = EntityState.Modified;
                
                await _context.SaveChangesAsync();
            }
        }

        public async Task<User> DeleteUser(int id) {

            var user = await _context.Users.FindAsync(id);

            if (user == null) {

                return new User {

                    UserName = String.Empty,
                    FirstName = String.Empty,
                    LastName = String.Empty,
                    NickName = string.Empty,
                    Email = string.Empty,
                    Password = string.Empty
                };
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }
    }
}
