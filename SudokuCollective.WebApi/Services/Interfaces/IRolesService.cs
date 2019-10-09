using System.Threading.Tasks;
using SudokuCollective.Domain.Enums;
using SudokuCollective.WebApi.Models.RequestModels.RoleRequests;
using SudokuCollective.WebApi.Models.TaskModels;
using SudokuCollective.WebApi.Models.TaskModels.RoleRequests;

namespace SudokuCollective.WebApi.Services.Interfaces {

    public interface IRolesService {

        Task<RoleTaskResult> GetRole(int id, bool fullRecord = false);
        Task<RoleListTaskResult> GetRoles(bool fullRecord = false);
        Task<RoleTaskResult> CreateRole(string name, RoleLevel roleLevel);
        Task<BaseTaskResult> UpdateRole(int id, UpdateRoleRO updateRoleRO);
        Task<BaseTaskResult> DeleteRole(int id);
    }
}
