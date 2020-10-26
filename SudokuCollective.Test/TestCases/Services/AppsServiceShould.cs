﻿using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.PageModels;
using SudokuCollective.Data.Models.RequestModels;
using SudokuCollective.Data.Services;
using SudokuCollective.Core.Models;
using SudokuCollective.Test.TestData;

namespace SudokuCollective.Test.TestCases.Services
{
    public class AppsServiceShould
    {
        private DatabaseContext _context;
        private IAppsService sut;
        private DateTime dateCreated;
        private string license;
        private BaseRequest baseRequest;
        private int userId;
        private int appId;

        [SetUp]
        public async Task Setup()
        {
            _context = await TestDatabase.GetDatabaseContext();
            sut = new AppsService(_context);
            dateCreated = DateTime.UtcNow;
            license = TestObjects.GetLicense();
            baseRequest = TestObjects.GetBaseRequest();
            userId = 1;
            appId = 1;
        }

        [Test]
        [Category("Services")]
        public async Task GetAppByID()
        {
            // Arrange

            // Act
            var result = await sut.GetApp(1);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.App, Is.TypeOf<App>());
        }

        [Test]
        [Category("Services")]
        public async Task GetAppByIDReturnsFalseIfNotFound()
        {
            // Arrange

            // Act
            var result = await sut.GetApp(3);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("App not found"));
            Assert.That(result.App.IsActive, Is.False);
        }

        [Test]
        [Category("Services")]
        public async Task GetMultipleApps()
        {
            // Arrange

            // Act
            var result = await sut.GetApps(new PageListModel());

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Apps.Count, Is.EqualTo(2));
        }

        [Test]
        [Category("Services")]
        public async Task CreateApps()
        {
            // Arrange

            // Act
            var result = await sut.CreateApp(new LicenseRequest()
            {

                Name = "Test App 3",
                OwnerId = 1,
                DevUrl = "https://localhost:5001",
                LiveUrl = "https://testapp3.com"
            });

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.App.Id, Is.TypeOf<int>());
            Assert.That(result.App.IsActive, Is.True);
        }

        [Test]
        [Category("Services")]
        public async Task NotCreateAppsIfOwnerDoesNotExist()
        {
            // Arrange

            // Act
            var result = await sut.CreateApp(new LicenseRequest()
            {

                Name = "Test App 3",
                OwnerId = 4,
                DevUrl = "https://localhost:5001",
                LiveUrl = "https://testapp3.com"
            });

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Intended owner id does not exist"));
        }

        [Test]
        [Category("Services")]
        public async Task GetAppByLicense()
        {
            // Arrange

            // Act
            var result = await sut.GetAppByLicense(license);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.App.Id, Is.EqualTo(1));
            Assert.That(result.App, Is.TypeOf<App>());
        }

        [Test]
        [Category("Services")]
        public async Task NotGetAppByLicenseIfInvalid()
        {
            // Arrange
            var invalidLicense = "5CDBFC8F-F304-4703-831B-750A7B7F8531";

            // Act
            var result = await sut.GetAppByLicense(invalidLicense);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("App not found"));
        }

        [Test]
        [Category("Services")]
        public async Task RetrieveLicense()
        {
            // Arrange

            // Act
            var result = await sut.GetLicense(1);

            // Assert
            Assert.That(result.License, Is.EqualTo(license));
        }

        [Test]
        [Category("Services")]
        public async Task NotRetrieveLicenseIfAppDoesNotExist()
        {
            // Arrange

            // Act
            var result = await sut.GetLicense(5);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("App not found"));
            Assert.That(result.License, Is.Not.EqualTo(license));
        }

        [Test]
        [Category("Services")]
        public async Task GetUsersByApp()
        {
            // Arrange

            // Act
            var result = await sut.GetAppUsers(baseRequest);

            // Assert
            Assert.That(result.Users.Count, Is.EqualTo(2));
        }

        [Test]
        [Category("Services")]
        public async Task UpdateApps()
        {
            // Arrange

            // Act
            var result = await sut.UpdateApp(
                new AppRequest()
                {

                    License = license,
                    RequestorId = 1,
                    PageListModel = new PageListModel(),
                    Name = "Test App 1... UPDATED!",
                    DevUrl = "https://localhost:4200",
                    LiveUrl = "https://testapp.com"
                }
            );

            var checkAppName = _context.Apps.FirstOrDefault(predicate: a => a.Id == 1);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(checkAppName.Name, Is.EqualTo("Test App 1... UPDATED!"));
        }

        [Test]
        [Category("Services")]
        public async Task AddUsersToApp()
        {
            // Arrange
            var user = new User()
            {
                Id = 4,
                UserName = "TestUser3",
                FirstName = "John",
                LastName = "Doe",
                NickName = "Johnny Boy",
                Email = "testuser3@example.com",
                Password = "password1",
                IsActive = true,
                DateCreated = dateCreated,
                DateUpdated = dateCreated
            };

            var userRole = new UserRole()
            {
                UserId = 4,
                RoleId = 4
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            await _context.UsersRoles.AddAsync(userRole);
            await _context.SaveChangesAsync();

            // Act
            var result = await sut.AddAppUser(3, baseRequest);

            var appUsers = _context.Users
                .Where(u => u.Apps.Any(app => app.AppId == 1)).ToList();

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(appUsers.Count, Is.EqualTo(3));
        }

        [Test]
        [Category("Services")]
        public async Task RemoveUsersFromApps()
        {
            // Arrange

            // Act
            var result = await sut.RemoveAppUser(2, baseRequest);

            var appUsers = _context.Users
                .Where(u => u.Apps.Any(app => app.AppId == 1)).ToList();

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(appUsers.Count, Is.EqualTo(1));
        }

        [Test]
        [Category("Services")]
        public async Task DeleteApps()
        {
            // Arrange

            // Act
            var confirmAppCountBeforeDeletion = _context.Apps.ToList();
            var confirmGameCountBeforeDeletion = _context.Games.ToList();
            var confirmUserCountBeforeDeletion = _context.Users.ToList();

            var result = await sut.DeleteOrResetApp(2);

            var confirmAppCountAfterDeletion = _context.Apps.ToList();
            var confirmGameCountAfterDeletion = _context.Games.ToList();
            var confirmUserCountAfterDeletion = _context.Users.ToList();

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(confirmAppCountBeforeDeletion.Count, Is.EqualTo(2));
            Assert.That(confirmAppCountAfterDeletion.Count, Is.EqualTo(1));
            Assert.That(confirmGameCountBeforeDeletion.Count, Is.EqualTo(2));
            Assert.That(confirmGameCountAfterDeletion.Count, Is.EqualTo(1));
            Assert.That(confirmUserCountBeforeDeletion.Count, Is.EqualTo(3));
            Assert.That(confirmUserCountAfterDeletion.Count, Is.EqualTo(2));
        }

        [Test]
        [Category("Services")]
        public async Task ActivateApps()
        {
            // Arrange

            // Act
            var result = await sut.ActivateApp(1);

            var confrirmAppStatus = _context.Apps
                .FirstOrDefault(predicate: a => a.Id == 1);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(confrirmAppStatus.IsActive, Is.True);
        }

        [Test]
        [Category("Services")]
        public async Task DeactivateApps()
        {
            // Arrange

            // Act
            var result = await sut.DeactivateApp(1);

            var confrirmAppStatus = _context.Apps
                .FirstOrDefault(predicate: a => a.Id == 1);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(confrirmAppStatus.IsActive, Is.False);
        }

        [Test]
        [Category("Services")]
        public async Task PermitValidRequests()
        {
            // Arrange

            // Act
            var result = await sut.IsRequestValidOnThisLicense(license, userId, appId);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        [Category("Services")]
        public async Task DenyInvalidLicenseRequests()
        {
            // Arrange
            var invalidLicense = "5CDBFC8F-F304-4703-831B-750A7B7F8531";

            // Act
            var result = await sut.IsRequestValidOnThisLicense(invalidLicense, userId, appId);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        [Category("Services")]
        public async Task DenyRequestWhereUserIsNotRegisteredToApp()
        {
            // Arrange
            var user = new User()
            {
                Id = 4,
                UserName = "TestUser3",
                FirstName = "John",
                LastName = "Doe",
                NickName = "Johnny Boy",
                Email = "testuser3@example.com",
                Password = "password1",
                IsActive = true,
                DateCreated = dateCreated,
                DateUpdated = dateCreated
            };

            var userRole = new UserRole()
            {
                UserId = 4,
                RoleId = 4
            };

            var userApp = new UserApp()
            {
                UserId = 4,
                AppId = 2
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            await _context.UsersRoles.AddAsync(userRole);
            await _context.SaveChangesAsync();
            await _context.UsersApps.AddAsync(userApp);
            await _context.SaveChangesAsync();

            // Act
            var userIsInApp = _context.Apps
                .Where(a => a.Id == 1)
                .Any(a => a.Users.Any(ua => ua.UserId == user.Id));

            var result = await sut.IsRequestValidOnThisLicense(license, user.Id, appId);

            // Assert
            Assert.That(userIsInApp, Is.False);
            Assert.That(result, Is.False);
        }

        [Test]
        [Category("Services")]
        public async Task PermitSuperUserSystemWideAccess()
        {
            // Arrange
            var newAppResult = await sut.CreateApp(new LicenseRequest()
            {
                Name = "Test App 3",
                OwnerId = 2,
                DevUrl = "https://localhost:5001",
                LiveUrl = "https://testapp3.com"
            });

            var license = _context.Apps
                .Where(a => a.Id == 3)
                .Select(a => a.License)
                .FirstOrDefault();

            var superUser = (User)_context.Users.Where(user => user.Id == 1).FirstOrDefault();

            // Act
            var superUserIsInApp = newAppResult.App.Users
                .Any(ua => ua.UserId == superUser.Id);

            var result = await sut.IsRequestValidOnThisLicense(license, superUser.Id, appId);

            // Assert
            Assert.That(superUserIsInApp, Is.False);
            Assert.That(superUser.IsSuperUser, Is.True);
            Assert.That(result, Is.True);
        }

        [Test]
        [Category("Services")]
        public async Task PermitOwnerRequests()
        {
            // Arrange

            // Act
            var result = await sut.IsOwnerOfThisLicense(license, userId, appId);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        [Category("Services")]
        public async Task DenyInvalidOwnerRequests()
        {
            // Arrange
            var invalidLicense = "5CDBFC8F-F304-4703-831B-750A7B7F8531";

            // Act
            var result = await sut.IsOwnerOfThisLicense(invalidLicense, userId, appId);

            // Assert
            Assert.That(result, Is.False);
        }
    }
}