﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SudokuCollective.Core.Interfaces.Repositories;
using SudokuCollective.Core.Models;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Repositories;
using SudokuCollective.Test.TestData;

namespace SudokuCollective.Test.TestCases.Repositories
{
    public class PasswordResetsRepositoryShould
    {
        private DatabaseContext context;
        private IPasswordResetsRepository<PasswordReset> sut;
        private PasswordReset newPasswordReset;

        [SetUp]
        public async Task Setup()
        {
            context = await TestDatabase.GetDatabaseContext();
            sut = new PasswordResetsRepository<PasswordReset>(context);

            newPasswordReset = new PasswordReset(2, 1);
        }

        [Test]
        [Category("Repository")]
        public async Task CreatePasswordReset()
        {
            // Arrange

            // Act
            var result = await sut.Create(newPasswordReset);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That((PasswordReset)result.Object, Is.InstanceOf<PasswordReset>());
        }

        [Test]
        [Category("Repository")]
        public async Task ReturnFalseIfPasswordResetsFails()
        {
            // Arrange
            newPasswordReset.Id = 2;

            // Act
            var result = await sut.Create(newPasswordReset);

            // Assert
            Assert.That(result.Success, Is.False);
        }

        [Test]
        [Category("Repository")]
        public async Task GetPasswordResetsByToken()
        {
            // Arrange
            var token = context
                .PasswordResets
                .Where(pr => pr.Id == 1)
                .Select(pr => pr.Token)
                .FirstOrDefault();

            // Act
            var result = await sut.Get(token);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That((PasswordReset)result.Object, Is.InstanceOf<PasswordReset>());
        }

        [Test]
        [Category("Repository")]
        public async Task ReturnFalseIfGetByTokenFails()
        {
            // Arrange
            var token = Guid.NewGuid().ToString();

            // Act
            var result = await sut.Get(token);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Object, Is.Null);
        }

        [Test]
        [Category("Repository")]
        public async Task GetAllPasswordResets()
        {
            // Arrange

            // Act
            var result = await sut.GetAll();

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Objects.ConvertAll(pr => (PasswordReset)pr), Is.InstanceOf<List<PasswordReset>>());
        }

        [Test]
        [Category("Repository")]
        public async Task DeletePasswordResets()
        {
            // Arrange
            var passwordReset = context.PasswordResets.FirstOrDefault(ec => ec.Id == 1);

            // Act
            var result = await sut.Delete(passwordReset);

            // Assert
            Assert.That(result.Success, Is.True);
        }

        [Test]
        [Category("Repository")]
        public async Task ReturnFalseIfDeletePasswordResetsFails()
        {
            // Arrange

            // Act
            var result = await sut.Delete(newPasswordReset);

            // Assert
            Assert.That(result.Success, Is.False);
        }

        [Test]
        [Category("Repository")]
        public async Task ConfirmItHasAnEmailConfirmation()
        {
            // Arrange

            // Act
            var result = await sut.HasEntity(1);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        [Category("Repository")]
        public async Task ConfirmPasswordResetsByUserAndAppId()
        {
            // Arrange
            var passwordReset = context.PasswordResets.FirstOrDefault();

            // Act
            var result = await sut.HasOutstandingPasswordReset(passwordReset.UserId, passwordReset.AppId);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        [Category("Repository")]
        public async Task GetPasswordResetsByUserAndAppId()
        {
            // Arrange
            var passwordReset = context.PasswordResets.FirstOrDefault();

            // Act
            var result = await sut.RetrievePasswordReset(passwordReset.UserId, passwordReset.AppId);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That((PasswordReset)result.Object, Is.InstanceOf<PasswordReset>());
        }

        [Test]
        [Category("Repository")]
        public async Task ReturnFalseIfGetPasswordResetsByUserAndAppIdFails()
        {
            // Arrange
            var passwordReset = context.PasswordResets.FirstOrDefault();

            // Act
            var result = await sut.RetrievePasswordReset(passwordReset.UserId + 3, passwordReset.AppId);

            // Assert
            Assert.That(result.Success, Is.False);
        }
    }
}
