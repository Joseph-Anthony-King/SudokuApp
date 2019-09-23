using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SudokuCollective.Models;
using SudokuCollective.Models.Enums;
using SudokuCollective.WebApi.Helpers;
using SudokuCollective.WebApi.Models.DataModel;
using SudokuCollective.WebApi.Models.TaskModels.RoleRequests;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.WebApi.Services
{

    public class RolesService : IRolesService {

        private readonly ApplicationDbContext _context;

        public RolesService(ApplicationDbContext context) {

            _context = context;
        }

        public async Task<RoleTaskResult> GetRole(
            int id, bool fullRecord = true) {

            var roleTaskResult = new RoleTaskResult() {

                Result = false,
                Role = new Role() {

                    Id = 0,
                    Name = string.Empty,
                    RoleLevel = RoleLevel.NULL,
                    Users = new List<UserRole>()
                }
            };

            try {

                var role = new Role();

                if (fullRecord) {

                    role = await _context.Roles
                        .Include(r => r.Users)
                        .SingleOrDefaultAsync(d => d.Id == id);

                    if (role == null) {

                        role = new Role {

                            Id = 0,
                            Name = string.Empty,
                            RoleLevel = RoleLevel.NULL,
                            Users = new List<UserRole>()
                        };
                    }

                    foreach (var r in role.Users) {

                        r.User = await _context.Users
                            .Where(u => u.Id == r.UserId)
                            .Include(u => u.Roles)
                            .FirstOrDefaultAsync();

                        r.User.Games = await _context.Games
                            .Where(g => g.User.Id == r.UserId)
                            .Include(g => g.SudokuMatrix)
                            .ToListAsync();                    
                        
                        foreach (var game in r.User.Games) {

                            game.SudokuMatrix = 
                                await StaticApiHelpers.AttachSudokuMatrix(game, _context);
                        }
                    }

                    roleTaskResult.Result = true;
                    roleTaskResult.Role = role;

                } else {

                    role = await _context.Roles
                        .SingleOrDefaultAsync(d => d.Id == id);

                    roleTaskResult.Result = true;
                    roleTaskResult.Role = role;
                }

                return roleTaskResult;
            }
            catch (Exception) 
            {
                return roleTaskResult;
            }
        }

        public async Task<RoleListTaskResult> GetRoles(
            bool fullRecord = true) {

            var roleListTaskResult = new RoleListTaskResult() {

                Result = false,
                Roles = new List<Role>()
            };

            try {

                var roles = new List<Role>();

                if (fullRecord) {

                    roles = await _context.Roles
                        .Include(r => r.Users)
                        .ToListAsync();

                    foreach (var role in roles) {

                        foreach (var r in role.Users) {

                            r.User = await _context.Users
                                .Where(u => u.Id == r.UserId)
                                .Include(u => u.Roles)
                                .FirstOrDefaultAsync();

                            r.User.Games = await _context.Games
                                .Where(g => g.User.Id == r.UserId)
                                .Include(g => g.SudokuMatrix)
                                .ToListAsync();                    
                            
                            foreach (var game in r.User.Games) {

                                game.SudokuMatrix = 
                                    await StaticApiHelpers.AttachSudokuMatrix(game, _context);
                            }
                        }
                    }

                    roleListTaskResult.Result = true;
                    roleListTaskResult.Roles = roles;

                } else {

                    roles = await _context.Roles.ToListAsync();

                    roleListTaskResult.Result = true;
                    roleListTaskResult.Roles = roles;
                }

                return roleListTaskResult;

            } catch (Exception) {

                return roleListTaskResult;
            }
        }

        public async Task<RoleTaskResult> CreateRole(string name, 
            RoleLevel roleLevel) {

            var roleTaskResult = new RoleTaskResult() {

                Result = false,
                Role = new Role() {

                    Id = 0,
                    Name = string.Empty,
                    RoleLevel = RoleLevel.NULL,
                    Users = new List<UserRole>()
                }
            };

            try {

                Role role = new Role() { Name = name, RoleLevel = roleLevel };

                _context.Roles.Add(role);
                await _context.SaveChangesAsync();
                
                roleTaskResult.Result = true;
                roleTaskResult.Role = role;

                return roleTaskResult;

            } catch (Exception) {

                return roleTaskResult;
            }
        }

        public async Task<bool> UpdateRole(int id, Role role) {

            var result = false;
            
            try {

                if (id == role.Id) {

                    _context.Entry(role).State = EntityState.Modified;
                    
                    await _context.SaveChangesAsync();

                    result = true;
                }

                return result;

            } catch (Exception) {

                return result;
            }
        }

        public async Task<bool> DeleteRole(int id) {

            var result = false;

            try {

                var role = await _context.Roles.FindAsync(id);

                if (role != null) {

                    _context.Roles.Remove(role);
                    await _context.SaveChangesAsync();

                    result = true;
                }

                return result;

            } catch (Exception) {

                return result;
            }
        }
    }
}
