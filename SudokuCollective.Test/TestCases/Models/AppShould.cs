using System;
using System.Collections.Generic;
using NUnit.Framework;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Models;

namespace SudokuCollective.Test.TestCases.Models
{
    public class AppShould
    {
        private IApp sut;

        [SetUp]
        public void Setup()
        {
            sut = new App();
        }

        [Test]
        [Category("Models")]
        public void ImplementIDBEntry()
        {
            // Arrange and Act

            // Assert
            Assert.That(sut, Is.InstanceOf<IEntityBase>());
        }

        [Test]
        [Category("Models")]
        public void HaveAnID()
        {
            // Arrange and Act

            // Assert
            Assert.That(sut.Id, Is.TypeOf<int>());
            Assert.That(sut.Id, Is.EqualTo(0));
        }

        [Test]
        [Category("Models")]
        public void HaveExpectedProperties()
        {
            // Arrange and Act

            // Assert
            Assert.That(sut.Name, Is.TypeOf<string>());
            Assert.That(sut.License, Is.TypeOf<string>());
            Assert.That(sut.OwnerId, Is.TypeOf<int>());
            Assert.That(sut.DevUrl, Is.TypeOf<string>());
            Assert.That(sut.LiveUrl, Is.TypeOf<string>());
            Assert.That(sut.IsActive, Is.TypeOf<bool>());
            Assert.That(sut.InDevelopment, Is.TypeOf<bool>());
            Assert.That(sut.PermitSuperUserAccess, Is.TypeOf<bool>());
            Assert.That(sut.PermitCollectiveLogins, Is.TypeOf<bool>());
            Assert.That(sut.UseCustomEmailConfirmationAction, Is.TypeOf<bool>());
            Assert.That(sut.UseCustomPasswordResetAction, Is.TypeOf<bool>());
            Assert.That(sut.DisableCustomUrls, Is.TypeOf<bool>());
            Assert.That(sut.CustomEmailConfirmationAction, Is.TypeOf<string>());
            Assert.That(sut.CustomPasswordResetAction, Is.TypeOf<string>());
            Assert.That(sut.GameCount, Is.TypeOf<int>());
            Assert.That(sut.UserCount, Is.TypeOf<int>());
            Assert.That(sut.TimeFrame, Is.TypeOf<TimeFrame>());
            Assert.That(sut.AccessDuration, Is.TypeOf<int>());
            Assert.That(sut.DateCreated, Is.TypeOf<DateTime>());
            Assert.That(sut.DateUpdated, Is.TypeOf<DateTime>());
            Assert.That(sut.Users, Is.InstanceOf<ICollection<UserApp>>());
        }

        [Test]
        [Category("Models")]
        public void DefaultToDeactiveStatus()
        {
            // Arrange and Act

            // Assert
            Assert.That(sut.IsActive, Is.False);
        }

        [Test]
        [Category("Models")]
        public void ProvideLicenseByCallingGetLicense()
        {
            // Arrange and Act

            // Assert
            Assert.That(sut.GetLicense(0, 0), Is.TypeOf<string>());
            Assert.That(sut.GetLicense(0, 0), Is.EqualTo(sut.License));
        }

        [Test]
        [Category("Models")]
        public void HaveATrueActiveStatusIfActivateAppCalled()
        {
            // Arrange

            // Act
            sut.ActivateApp();

            // Assert
            Assert.That(sut.IsActive, Is.True);
        }

        [Test]
        [Category("Models")]
        public void HaveAFalseActiveStatusIfDeactivateAppCalled()
        {
            // Arrange

            // Act
            sut.DeactivateApp();

            // Assert
            Assert.That(sut.IsActive, Is.False);
        }

        [Test]
        [Category("Models")]
        public void HaveAFieldToTrackAppDevelopmentStatusThatDefaultsToTrue()
        {
            // Arrange

            // Act

            // Assert
            Assert.That(sut.InDevelopment, Is.InstanceOf<bool>());
            Assert.That(sut.InDevelopment, Is.True);
        }

        [Test]
        [Category("Models")]
        public void HaveTheAbilityToDisableCustomUrlsThatDefaultsToTrue()
        {
            // Arrange

            // Act

            // Assert
            Assert.That(sut.DisableCustomUrls, Is.InstanceOf<bool>());
            Assert.That(sut.DisableCustomUrls, Is.True);
        }

        [Test]
        [Category("Models")]
        public void UseCustomEmailConfirmationAction()
        {
            // Arrange
            var devUrl = "http://localhost:5001";
            var customAction = "customurl";

            // Act
            sut.DevUrl = devUrl;
            sut.CustomEmailConfirmationAction = customAction;
            sut.DisableCustomUrls = false;

            // Assert
            Assert.That(sut.UseCustomEmailConfirmationAction, Is.True);
            Assert.That(sut.CustomEmailConfirmationAction, Is.EqualTo(customAction));
        }

        [Test]
        [Category("Models")]
        public void UseCustomPasswordResetAction()
        {
            // Arrange
            var devUrl = "http://localhost:5001";
            var customAction = "customurl";

            // Act
            sut.DevUrl = devUrl;
            sut.CustomPasswordResetAction = customAction;
            sut.DisableCustomUrls = false;

            // Assert
            Assert.That(sut.UseCustomPasswordResetAction, Is.True);
            Assert.That(sut.CustomPasswordResetAction, Is.EqualTo(customAction));
        }

        [Test]
        [Category("Models")]
        public void TrackGameCountThatDefaultsToZero()
        {
            // Arrange

            // Act

            // Assert
            Assert.That(sut.GameCount, Is.InstanceOf<int>());
            Assert.That(sut.GameCount, Is.EqualTo(0));

        }

        [Test]
        [Category("Models")]
        public void TrackGameCount()
        {
            // Arrange

            // Act
            var initialGameCount = sut.GameCount;

            var user = new User();
            sut.Users.Add(new UserApp { App = (App)sut, User = user });
            _ = new Game(user, new SudokuMatrix(), new Difficulty(), 0);

            var finalGameCount = sut.GameCount;

            // Assert
            Assert.That(initialGameCount, Is.EqualTo(0));
            Assert.That(finalGameCount, Is.EqualTo(1));

        }

        [Test]
        [Category("Models")]
        public void TrackUserCountThatDefaultsToZero()
        {
            // Arrange

            // Act

            // Assert
            Assert.That(sut.UserCount, Is.InstanceOf<int>());
            Assert.That(sut.UserCount, Is.EqualTo(0));

        }

        [Test]
        [Category("Models")]
        public void TrackUserCount()
        {
            // Arrange

            // Act
            var initialUserCount = sut.UserCount;

            var user = new User();
            sut.Users.Add(new UserApp { App = (App)sut, User = user });

            var finalUserCount = sut.UserCount;

            // Assert
            Assert.That(initialUserCount, Is.EqualTo(0));
            Assert.That(finalUserCount, Is.EqualTo(1));
        }

        [Test]
        [Category("Models")]
        public void HaveAConstructorWhichAcceptsProperties()
        {
            // Arrange
            string name = "name";
            string license = "license";
            int ownerId = 0;
            string devUrl = "devUrl";
            string liveUrl = "liveUrl";

            // Act
            var app = new App(name, license, ownerId, devUrl, liveUrl);

            // Assert
            Assert.That(app, Is.TypeOf<App>());
        }

        [Test]
        [Category("Models")]
        public void HasAJsonConstructor()
        {
            // Arrange

            // Act
            sut = new App(
                0,
                "name",
                "license",
                0,
                "devUrl",
                "liveUrl",
                true,
                false,
                true,
                true,
                true,
                null,
                null,
                TimeFrame.DAYS,
                1,
                DateTime.Now,
                DateTime.MinValue);

            // Assert
            Assert.That(sut, Is.InstanceOf<App>());
        }

        [Test]
        [Category("Models")]
        public void DefaultAllowSuperUserAccessToFalse()
        {
            // Arrange

            // Act

            // Assert
            Assert.That(sut.PermitSuperUserAccess, Is.InstanceOf<bool>());
            Assert.That(sut.PermitSuperUserAccess, Is.False);
        }
    }
}
