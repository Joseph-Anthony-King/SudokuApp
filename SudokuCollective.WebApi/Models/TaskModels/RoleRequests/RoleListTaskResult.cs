using System.Collections.Generic;
using SudokuCollective.Domain;

namespace SudokuCollective.WebApi.Models.TaskModels.RoleRequests {

    public class RoleListTaskResult : BaseTaskResult {
        
        public IEnumerable<Role> Roles { get; set; }

        public RoleListTaskResult() : base() {

            Roles = new List<Role>();
        }
    }
}
