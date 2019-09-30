using System;
using System.Collections.Generic;
using SudokuCollective.Models.Interfaces;
using Newtonsoft.Json;

namespace SudokuCollective.Models {

    public class User : IUser {

        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string FullName { 
            get => string.Format("{0} {1}", FirstName, LastName); 
        }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public ICollection<Game> Games { get; set; }
        public ICollection<UserRole> Roles { get; set; }
        public ICollection<UserApp> Apps { get; set; }
        public bool IsActive { get; set; }

        public User(
            string firstName, 
            string lastName, 
            string password) : this() {

            FirstName = firstName;
            LastName = lastName;
            Password = password;
        }

        public User() {

            DateCreated = DateTime.UtcNow;
            DateUpdated = DateTime.UtcNow;
            Games = new List<Game>();
            
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
            string firstName, 
            string lastName, 
            string nickName,
            DateTime dateCreated, 
            DateTime dateUpdated, 
            string email, 
            string password,
            bool isActive) : this() {

            Id = id;
            FirstName = firstName;
            LastName = lastName;
            NickName = nickName;
            DateCreated = dateCreated;
            DateUpdated = dateUpdated;
            Email = email;
            Password = password;
            IsActive = isActive;
        }

        public void ActivateUser() {

            IsActive = true;
        }

        public void DeactiveUser() {

            IsActive = false;
        }
    }
}
