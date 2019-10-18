using System.Collections.Generic;
using Newtonsoft.Json;
using SudokuCollective.Domain.Enums;
using SudokuCollective.Domain.Interfaces;

namespace SudokuCollective.Domain.Models {

    public class Role : IRole {

        public int Id { get; set; }
        public string Name { get; set; }
        public RoleLevel RoleLevel { get; set; }
        public ICollection<UserRole> Users { get; set; }

        public Role() {
            
            Id = 0;
            Name = string.Empty;
            RoleLevel = RoleLevel.NULL;
        }

        [JsonConstructor]
        public Role(int id, string name, RoleLevel roleLevel) {
            
            Id = id;
            Name = name;
            RoleLevel = roleLevel;
        }
    }
}
