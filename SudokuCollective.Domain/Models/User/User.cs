using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using SudokuCollective.Domain.Enums;
using SudokuCollective.Domain.Interfaces;

namespace SudokuCollective.Domain.Models {

    public class User : IUser {

        private string _userName;

        public int Id { get; set; }
        [Required]
        public string UserName {

            get {

                return _userName;
            }

            set {

                if (!string.IsNullOrEmpty(value)) {

                    var regex = new Regex("^[a-zA-Z0-9-._]*$");

                    if (regex.IsMatch(value)) {

                        _userName = value;
                    }

                } else {

                    _userName = string.Empty;
                }
            } 
        }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string FullName { 
            get => string.Format("{0} {1}", FirstName, LastName); 
        }
        [Required]
        public string Email { get; set; }
        [JsonIgnore]
        [Required]
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public bool IsSuperUser {

            get {

                var result = false;

                foreach (var role in Roles)
                {

                    if (role.Role.RoleLevel == RoleLevel.SUPERUSER)
                    {

                        result = true;
                    }
                }

                return result;
            }
        }
        public bool IsAdmin {

            get {

                var result = false;

                foreach (var role in Roles)
                {

                    if (role.Role.RoleLevel == RoleLevel.ADMIN)
                    {

                        result = true;
                    }
                }

                return result;
            }
        }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public ICollection<Game> Games { get; set; }
        public ICollection<UserRole> Roles { get; set; }
        public ICollection<UserApp> Apps { get; set; }

        public User(
            string firstName, 
            string lastName, 
            string password) : this() {

            FirstName = firstName;
            LastName = lastName;
            Password = password;
        }

        public User() {

            Id = 0;
            UserName = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            NickName = string.Empty;
            DateCreated = DateTime.UtcNow;
            DateUpdated = DateTime.UtcNow;
            Games = new List<Game>();
            Roles = new List<UserRole>();
            Apps = new List<UserApp>();
            
            if (string.IsNullOrEmpty(FirstName)) {

                FirstName = string.Empty;
            }
            
            if (string.IsNullOrEmpty(LastName)) {

                LastName = string.Empty;
            }
            
            if (string.IsNullOrEmpty(Password)) {

                Password = string.Empty;
            }

            IsActive = true;
        }

        [JsonConstructor]
        public User(
            int id, 
            string userName,
            string firstName, 
            string lastName, 
            string nickName,
            string email, 
            string password,
            bool isActive,
            DateTime dateCreated,
            DateTime dateUpdated) {

            Id = id;
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            NickName = nickName;
            Email = email;
            Password = password;
            IsActive = isActive;
            DateCreated = dateCreated;
            DateUpdated = dateUpdated;
        }

        public void ActivateUser() {

            IsActive = true;
        }

        public void DeactiveUser() {

            IsActive = false;
        }
    }
}
