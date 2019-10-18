﻿using System;
using System.Collections.Generic;
using NUnit.Framework;
using SudokuCollective.Domain;
using SudokuCollective.Domain.Interfaces;
using SudokuCollective.Domain.Models;

namespace SudokuCollective.Tests.TestCases.Models {

    public class UserShould {

        [Test]
        [Category("Models")]
        public void ImplementIDBEntry() {

            // Arrange and Act
            var sut = new User();

            // Assert
            Assert.That(sut, Is.InstanceOf<IEntityBase>());
        }

        [Test]
        [Category("Models")]
        public void HaveAnID() {

            // Arrange and Act
            var sut = new User();

            // Assert
            Assert.That(sut.Id, Is.TypeOf<int>());
            Assert.That(sut.Id, Is.EqualTo(0));
        }

        [Test]
        [Category("Models")]
        public void AcceptFirstAndLastName() {

            // Arrange and Act
            var sut = new User(
                "John", 
                "Doe", 
                "Password");

            // Assert
            Assert.That(sut.FirstName, Is.EqualTo("John"));
            Assert.That(sut.LastName, Is.EqualTo("Doe"));
            Assert.That(sut.FullName, Is.EqualTo("John Doe"));
            Assert.That(sut.Password, Is.EqualTo("Password"));
        }

        [Test]
        [Category("Models")]
        public void HaveWorkingProperties() {

            // Arrange
            var sut = new User();

            // Act
            sut.NickName = "Johnny";
            sut.Email = "JohnDoe@example.com";

            // Assert
            Assert.That(sut.NickName, Is.EqualTo("Johnny"));
            Assert.That(sut.Email, Is.EqualTo("JohnDoe@example.com"));
        }

        [Test]
        [Category("Models")]
        public void HaveAccurateDateCreated() {

            // Arrange and Act
            var sut = new User();
            var currentDateTime = DateTime.UtcNow;

            // Assert
            Assert.That(sut.DateCreated, Is.EqualTo(currentDateTime).Within(1).Seconds);
        }

        [Test]
        [Category("Models")]
        public void HaveAGameList() {

            // Arrange and Act
            var sut = new User();

            // Assert
            Assert.That(sut.Games.Count, Is.EqualTo(0));
            Assert.That(sut.Games, Is.TypeOf<List<Game>>());
        }

        [Test]
        [Category("Models")]
        public void HaveARolesList() {

            // Arrange and Act
            var sut = new User();

            // Assert
            Assert.That(sut.Roles.Count, Is.EqualTo(0));
            Assert.That(sut.Roles, Is.TypeOf<List<UserRole>>());
        }

        [Test]
        [Category("Models")]
        public void HaveAnAppList() {

            // Arrange and Act
            var sut = new User();

            // Assert
            Assert.That(sut.Apps.Count, Is.EqualTo(0));
            Assert.That(sut.Apps, Is.TypeOf<List<UserApp>>());
        }

        [Test]
        [Category("Models")]
        public void HaveAPassword() {

            // Arrange and Act
            var sut = new User();

            // Assert
            Assert.That(sut.Password, Is.TypeOf<string>());
        }

        [Test]
        [Category("Models")]
        public void HaveAnActiveStatus() {

            // Arrange and Act
            var sut = new User();

            // Assert
            Assert.That(sut.IsActive, Is.TypeOf<bool>());
        }

        [Test]
        [Category("Models")]
        public void HaveAMethodToActivate() {

            // Arrange
            var sut = new User();

            // Act
            sut.ActivateUser();

            // Assert
            Assert.That(sut.IsActive, Is.True);
        }

        [Test]
        [Category("Models")]
        public void HaveAMethodToDeactivate() {

            // Arrange
            var sut = new User();

            // Act
            sut.DeactiveUser();

            // Assert
            Assert.That(sut.IsActive, Is.False);
        }

        [Test]
        [Category("Models")]
        public void HaveAUserNameThatAcceptsAlphaNumericCharacters() {

            // Arrange
            var sut = new User();

            // Act
            sut.UserName = "Good.User-Name";

            // Assert
            Assert.That(sut.UserName, Is.EqualTo("Good.User-Name"));
        }

        [Test]
        [Category("Models")]
        public void HaveAUserNameThatRejectsSpecialCharacters() {

            // Arrange
            var sut = new User();

            // Act
            sut.UserName = "B@dUs3rN$m#";

            // Assert
            Assert.That(sut.UserName, Is.Empty);
        }

        [Test]
        [Category("Models")]
        public void ProvideSuperUserStatus() {

            // Arrange
            var sut = new User();

            // Act

            // Assert
            Assert.That(sut.IsSuperUser, Is.False);
        }

        [Test]
        [Category("Models")]
        public void ProvideAdminStatus() {

            // Arrange
            var sut = new User();

            // Act

            // Assert
            Assert.That(sut.IsAdmin, Is.False);
        }
    }
}
