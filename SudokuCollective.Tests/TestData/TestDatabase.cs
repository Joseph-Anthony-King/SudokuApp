﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using SudokuCollective.Domain;
using SudokuCollective.Domain.Enums;
using SudokuCollective.Domain.Models;
using SudokuCollective.WebApi.Models.DataModel;

namespace SudokuCollective.Tests.TestData {

    public static class TestDatabase {

        public static async Task<DatabaseContext> GetDatabaseContext() {

            Mock<IConfiguration> config = new Mock<IConfiguration>();

            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var databaseContext = new DatabaseContext(options, config.Object);

            databaseContext.Database.EnsureCreated();

            var dateCreated = DateTime.UtcNow;

            if (await databaseContext.Roles.CountAsync() <= 0) {

                databaseContext.Roles.AddRange(

                    new Role(1, "Null", RoleLevel.NULL),
                    new Role(2, "Super User", RoleLevel.SUPERUSER),
                    new Role(3, "Admin", RoleLevel.ADMIN),
                    new Role(4, "User", RoleLevel.USER)
                );

                await databaseContext.SaveChangesAsync();
            }

            if (await databaseContext.Difficulties.CountAsync() <= 0)
            {

                databaseContext.Difficulties.AddRange(

                    new Difficulty(1, "Null", "Null", DifficultyLevel.NULL),
                    new Difficulty(2, "Test", "Test", DifficultyLevel.TEST),
                    new Difficulty(3, "Easy", "Steady Sloth", DifficultyLevel.EASY),
                    new Difficulty(4, "Medium", "Leaping Lemur", DifficultyLevel.MEDIUM),
                    new Difficulty(5, "Hard", "Mighty Mountain Lion", DifficultyLevel.HARD),
                    new Difficulty(6, "Evil", "Sneaky Shark", DifficultyLevel.EVIL)
                );

                await databaseContext.SaveChangesAsync();
            }

            if (await databaseContext.Users.CountAsync() <= 0) {

                var salt = BCrypt.Net.BCrypt.GenerateSalt();

                databaseContext.Users.AddRange(

                    new User(
                        1, 
                        "TestSuperUser", 
                        "Test Super", 
                        "User", 
                        "Test Super User", 
                        "TestSuperUser@example.com", 
                        BCrypt.Net.BCrypt.HashPassword("password1", salt), 
                        true, 
                        dateCreated, 
                        DateTime.MinValue),

                    new User(
                        2, 
                        "TestUser", 
                        "Test", 
                        "User", 
                        "Test User", 
                        "TestUser@example.com", 
                        BCrypt.Net.BCrypt.HashPassword("password1", salt),
                        true, 
                        dateCreated, 
                        DateTime.MinValue)
                );

                await databaseContext.SaveChangesAsync();
            }

            if (await databaseContext.UsersRoles.CountAsync() <= 0) {

                databaseContext.UsersRoles.AddRange(

                    new UserRole(1,1,2),
                    new UserRole(2,1,3),
                    new UserRole(3,1,4),
                    new UserRole(4,2,4)
                );

                await databaseContext.SaveChangesAsync();
            }

            if (await databaseContext.Apps.CountAsync() <= 0) {

                databaseContext.Apps.AddRange(

                    new App(
                        1, 
                        "Test App 1", 
                        TestObjects.GetLicense(), 
                        1, 
                        "https://localhost:4200", 
                        "https://testapp.com", 
                        true,
                        dateCreated,
                        DateTime.MinValue),
                    new App(
                        2, 
                        "Test App 2", 
                        "03C0D43F-3AD8-490A-A131-F73C81FE02C0", 
                        1, 
                        "https://localhost:8080", 
                        "https://testapp2.com", 
                        true,
                        dateCreated, 
                        DateTime.MinValue)
                );

                await databaseContext.SaveChangesAsync();
            }

            if (await databaseContext.UsersApps.CountAsync() <= 0) {

                databaseContext.UsersApps.AddRange(

                    new UserApp(1,1,1),
                    new UserApp(2,1,2),
                    new UserApp(3,2,1),
                    new UserApp(4,2,2)
                );

                await databaseContext.SaveChangesAsync();
            }

            if (await databaseContext.SudokuMatrices.CountAsync() <= 0) {

                databaseContext.SudokuMatrices.AddRange(

                    new SudokuMatrix(1,4),
                    new SudokuMatrix(2,4)
                );

                await databaseContext.SaveChangesAsync();
            }

            if (await databaseContext.SudokuCells.CountAsync() <= 0)
            {

                databaseContext.SudokuCells.AddRange(

                    new SudokuCell(81, 1, 1, 1, 1, 4, 4, false, 1),
                    new SudokuCell(58, 2, 2, 1, 1, 2, 0, true, 1),
                    new SudokuCell(57, 3, 3, 1, 1, 8, 8, false, 1),
                    new SudokuCell(56, 4, 4, 2, 1, 6, 0, true, 1),
                    new SudokuCell(55, 5, 5, 2, 1, 1, 0, true, 1),
                    new SudokuCell(54, 6, 6, 2, 1, 5, 0, true, 1),
                    new SudokuCell(53, 7, 7, 3, 1, 9, 0, true, 1),
                    new SudokuCell(52, 8, 8, 3, 1, 3, 0, true, 1),
                    new SudokuCell(51, 9, 9, 3, 1, 7, 0, true, 1),
                    new SudokuCell(50, 10, 1, 1, 2, 3, 3, false, 1),
                    new SudokuCell(49, 11, 2, 1, 2, 9, 0, true, 1),
                    new SudokuCell(48, 12, 3, 1, 2, 6, 6, false, 1),
                    new SudokuCell(47, 13, 4, 2, 2, 2, 0, true, 1),
                    new SudokuCell(46, 14, 5, 2, 2, 8, 0, true, 1),
                    new SudokuCell(45, 15, 6, 2, 2, 7, 0, true, 1),
                    new SudokuCell(44, 16, 7, 3, 2, 1, 1, false, 1),
                    new SudokuCell(43, 17, 8, 3, 2, 4, 4, false, 1),
                    new SudokuCell(42, 18, 9, 3, 2, 5, 0, false, 1),
                    new SudokuCell(59, 19, 1, 1, 3, 1, 0, true, 1),
                    new SudokuCell(61, 20, 2, 1, 3, 5, 0, true, 1),
                    new SudokuCell(80, 21, 3, 1, 3, 7, 7, false, 1),
                    new SudokuCell(62, 22, 4, 2, 3, 3, 0, true, 1),
                    new SudokuCell(79, 23, 5, 2, 3, 4, 0, true, 1),
                    new SudokuCell(78, 24, 6, 2, 3, 9, 9, false, 1),
                    new SudokuCell(77, 25, 7, 3, 3, 6, 0, true, 1),
                    new SudokuCell(76, 26, 8, 3, 3, 8, 0, true, 1),
                    new SudokuCell(75, 27, 9, 3, 3, 2, 2, false, 1),
                    new SudokuCell(74, 28, 1, 4, 4, 5, 0, true, 1),
                    new SudokuCell(73, 29, 2, 4, 4, 8, 8, false, 1),
                    new SudokuCell(72, 30, 3, 4, 4, 4, 4, false, 1),
                    new SudokuCell(71, 31, 4, 5, 4, 9, 0, true, 1),
                    new SudokuCell(70, 32, 5, 5, 4, 3, 0, true, 1),
                    new SudokuCell(69, 33, 6, 5, 4, 6, 0, true, 1),
                    new SudokuCell(68, 34, 7, 6, 4, 2, 0, true, 1),
                    new SudokuCell(67, 35, 8, 6, 4, 7, 0, true, 1),
                    new SudokuCell(66, 36, 9, 6, 4, 1, 0, true, 1),
                    new SudokuCell(65, 37, 1, 4, 5, 7, 7, false, 1),
                    new SudokuCell(64, 38, 2, 4, 5, 6, 0, true, 1),
                    new SudokuCell(63, 39, 3, 4, 5, 1, 0, true, 1),
                    new SudokuCell(41, 40, 4, 5, 5, 8, 8, false, 1),
                    new SudokuCell(40, 41, 5, 5, 5, 2, 2, false, 1),
                    new SudokuCell(39, 42, 6, 5, 5, 4, 4, false, 1),
                    new SudokuCell(38, 43, 7, 6, 5, 5, 5, false, 1),
                    new SudokuCell(16, 44, 8, 6, 5, 9, 0, true, 1),
                    new SudokuCell(15, 45, 9, 6, 5, 3, 0, true, 1),
                    new SudokuCell(14, 46, 1, 4, 6, 2, 2, false, 1),
                    new SudokuCell(13, 47, 2, 4, 6, 3, 0, true, 1),
                    new SudokuCell(12, 48, 3, 4, 6, 9, 9, false, 1),
                    new SudokuCell(11, 49, 4, 5, 6, 7, 7, false, 1),
                    new SudokuCell(10, 50, 5, 5, 6, 5, 5, false, 1),
                    new SudokuCell(17, 51, 6, 5, 6, 1, 0, true, 1),
                    new SudokuCell(9, 52, 7, 6, 6, 4, 4, false, 1),
                    new SudokuCell(7, 53, 8, 6, 6, 6, 6, false, 1),
                    new SudokuCell(6, 54, 9, 6, 6, 8, 0, true, 1),
                    new SudokuCell(5, 55, 1, 7, 7, 9, 0, true, 1),
                    new SudokuCell(4, 56, 2, 7, 7, 7, 0, true, 1),
                    new SudokuCell(3, 57, 3, 7, 7, 5, 0, true, 1),
                    new SudokuCell(2, 58, 4, 8, 7, 1, 0, true, 1),
                    new SudokuCell(1, 59, 5, 8, 7, 6, 0, true, 1),
                    new SudokuCell(8, 60, 6, 8, 7, 8, 8, false, 1),
                    new SudokuCell(18, 61, 7, 9, 7, 3, 3, false, 1),
                    new SudokuCell(19, 62, 8, 9, 7, 2, 2, false, 1),
                    new SudokuCell(20, 63, 9, 9, 7, 4, 0, true, 1),
                    new SudokuCell(37, 64, 1, 7, 8, 6, 0, true, 1),
                    new SudokuCell(36, 65, 2, 7, 8, 1, 0, true, 1),
                    new SudokuCell(35, 66, 3, 7, 8, 2, 2, false, 1),
                    new SudokuCell(34, 67, 4, 8, 8, 4, 0, true, 1),
                    new SudokuCell(33, 68, 5, 8, 8, 7, 7, false, 1),
                    new SudokuCell(32, 69, 6, 8, 8, 3, 0, true, 1),
                    new SudokuCell(31, 70, 7, 9, 8, 8, 0, true, 1),
                    new SudokuCell(30, 71, 8, 9, 8, 5, 0, true, 1),
                    new SudokuCell(29, 72, 9, 9, 8, 9, 0, true, 1),
                    new SudokuCell(28, 73, 1, 7, 9, 8, 0, true, 1),
                    new SudokuCell(27, 74, 2, 7, 9, 4, 0, true, 1),
                    new SudokuCell(26, 75, 3, 7, 9, 3, 0, true, 1),
                    new SudokuCell(25, 76, 4, 8, 9, 5, 0, true, 1),
                    new SudokuCell(24, 77, 5, 8, 9, 9, 0, true, 1),
                    new SudokuCell(23, 78, 6, 8, 9, 2, 0, true, 1),
                    new SudokuCell(22, 79, 7, 9, 9, 7, 7, false, 1),
                    new SudokuCell(21, 80, 8, 9, 9, 1, 0, true, 1),
                    new SudokuCell(60, 81, 9, 9, 9, 6, 6, false, 1),
                    new SudokuCell(162, 1, 1, 1, 1, 3, 0, true, 2),
                    new SudokuCell(139, 2, 2, 1, 1, 8, 8, false, 2),
                    new SudokuCell(138, 3, 3, 1, 1, 1, 1, false, 2),
                    new SudokuCell(137, 4, 4, 2, 1, 9, 9, false, 2),
                    new SudokuCell(136, 5, 5, 2, 1, 2, 0, true, 2),
                    new SudokuCell(135, 6, 6, 2, 1, 6, 6, false, 2),
                    new SudokuCell(134, 7, 7, 3, 1, 4, 0, true, 2),
                    new SudokuCell(133, 8, 8, 3, 1, 5, 0, true, 2),
                    new SudokuCell(132, 9, 9, 3, 1, 7, 7, false, 2),
                    new SudokuCell(131, 10, 1, 1, 2, 9, 0, true, 2),
                    new SudokuCell(130, 11, 2, 1, 2, 7, 7, false, 2),
                    new SudokuCell(129, 12, 3, 1, 2, 5, 0, true, 2),
                    new SudokuCell(128, 13, 4, 2, 2, 1, 0, true, 2),
                    new SudokuCell(127, 14, 5, 2, 2, 3, 3, false, 2),
                    new SudokuCell(126, 15, 6, 2, 2, 4, 4, false, 2),
                    new SudokuCell(125, 16, 7, 3, 2, 2, 2, false, 2),
                    new SudokuCell(124, 17, 8, 3, 2, 8, 8, false, 2),
                    new SudokuCell(123, 18, 9, 3, 2, 6, 6, false, 2),
                    new SudokuCell(140, 19, 1, 1, 3, 4, 4, false, 2),
                    new SudokuCell(142, 20, 2, 1, 3, 2, 2, false, 2),
                    new SudokuCell(161, 21, 3, 1, 3, 6, 0, true, 2),
                    new SudokuCell(143, 22, 4, 2, 3, 8, 0, true, 2),
                    new SudokuCell(160, 23, 5, 2, 3, 5, 0, true, 2),
                    new SudokuCell(159, 24, 6, 2, 3, 7, 0, true, 2),
                    new SudokuCell(158, 25, 7, 3, 3, 1, 1, false, 2),
                    new SudokuCell(157, 26, 8, 3, 3, 3, 0, true, 2),
                    new SudokuCell(156, 27, 9, 3, 3, 9, 0, true, 2),
                    new SudokuCell(155, 28, 1, 4, 4, 1, 1, false, 2),
                    new SudokuCell(154, 29, 2, 4, 4, 3, 0, true, 2),
                    new SudokuCell(153, 30, 3, 4, 4, 7, 0, true, 2),
                    new SudokuCell(152, 31, 4, 5, 4, 4, 4, false, 2),
                    new SudokuCell(151, 32, 5, 5, 4, 9, 0, true, 2),
                    new SudokuCell(150, 33, 6, 5, 4, 2, 0, true, 2),
                    new SudokuCell(149, 34, 7, 6, 4, 8, 0, true, 2),
                    new SudokuCell(148, 35, 8, 6, 4, 6, 0, true, 2),
                    new SudokuCell(147, 36, 9, 6, 4, 5, 4, false, 2),
                    new SudokuCell(146, 37, 1, 4, 5, 5, 0, true, 2),
                    new SudokuCell(145, 38, 2, 4, 5, 4, 0, true, 2),
                    new SudokuCell(144, 39, 3, 4, 5, 2, 2, false, 2),
                    new SudokuCell(122, 40, 4, 5, 5, 3, 0, true, 2),
                    new SudokuCell(121, 41, 5, 5, 5, 6, 6, false, 2),
                    new SudokuCell(120, 42, 6, 5, 5, 8, 8, false, 2),
                    new SudokuCell(119, 43, 7, 6, 5, 9, 0, true, 2),
                    new SudokuCell(97, 44, 8, 6, 5, 7, 0, true, 2),
                    new SudokuCell(96, 45, 9, 6, 5, 1, 0, true, 2),
                    new SudokuCell(95, 46, 1, 4, 6, 6, 0, true, 2),
                    new SudokuCell(94, 47, 2, 4, 6, 9, 9, false, 2),
                    new SudokuCell(93, 48, 3, 4, 6, 8, 0, true, 2),
                    new SudokuCell(92, 49, 4, 5, 6, 5, 5, false, 2),
                    new SudokuCell(91, 50, 5, 5, 6, 7, 0, true, 2),
                    new SudokuCell(98, 51, 6, 5, 6, 1, 0, true, 2),
                    new SudokuCell(90, 52, 7, 6, 6, 3, 0, true, 2),
                    new SudokuCell(88, 53, 8, 6, 6, 4, 0, true, 2),
                    new SudokuCell(87, 54, 9, 6, 6, 2, 2, false, 2),
                    new SudokuCell(86, 55, 1, 7, 7, 2, 0, true, 2),
                    new SudokuCell(85, 56, 2, 7, 7, 6, 0, true, 2),
                    new SudokuCell(84, 57, 3, 7, 7, 3, 0, true, 2),
                    new SudokuCell(83, 58, 4, 8, 7, 7, 7, false, 2),
                    new SudokuCell(82, 59, 5, 8, 7, 4, 0, true, 2),
                    new SudokuCell(89, 60, 6, 8, 7, 9, 0, true, 2),
                    new SudokuCell(99, 61, 7, 9, 7, 5, 5, false, 2),
                    new SudokuCell(100, 62, 8, 9, 7, 1, 0, true, 2),
                    new SudokuCell(101, 63, 9, 9, 7, 8, 0, true, 2),
                    new SudokuCell(118, 64, 1, 7, 8, 7, 0, true, 2),
                    new SudokuCell(117, 65, 2, 7, 8, 1, 0, true, 2),
                    new SudokuCell(116, 66, 3, 7, 8, 4, 0, true, 2),
                    new SudokuCell(115, 67, 4, 8, 8, 2, 0, true, 2),
                    new SudokuCell(114, 68, 5, 8, 8, 8, 0, true, 2),
                    new SudokuCell(113, 69, 6, 8, 8, 5, 0, true, 2),
                    new SudokuCell(112, 70, 7, 9, 8, 6, 0, true, 2),
                    new SudokuCell(111, 71, 8, 9, 8, 9, 9, false, 2),
                    new SudokuCell(110, 72, 9, 9, 8, 3, 0, true, 2),
                    new SudokuCell(109, 73, 1, 7, 9, 8, 8, false, 2),
                    new SudokuCell(108, 74, 2, 7, 9, 5, 0, true, 2),
                    new SudokuCell(107, 75, 3, 7, 9, 9, 9, false, 2),
                    new SudokuCell(106, 76, 4, 8, 9, 6, 0, true, 2),
                    new SudokuCell(105, 77, 5, 8, 9, 1, 0, true, 2),
                    new SudokuCell(104, 78, 6, 8, 9, 3, 3, false, 2),
                    new SudokuCell(103, 79, 7, 9, 9, 7, 0, true, 2),
                    new SudokuCell(102, 80, 8, 9, 9, 2, 0, true, 2),
                    new SudokuCell(141, 81, 9, 9, 9, 4, 0, true, 2)
                );

                await databaseContext.SaveChangesAsync();
            }

            if (await databaseContext.SudokuSolutions.CountAsync() <= 0) {

                databaseContext.SudokuSolutions.AddRange(

                    new SudokuSolution(1, new List<int>(), dateCreated, DateTime.MinValue),
                    new SudokuSolution(2, new List<int>(), dateCreated, DateTime.MinValue)
                );

                await databaseContext.SaveChangesAsync();
            }

            if (await databaseContext.Games.CountAsync() <= 0) {

                databaseContext.Games.AddRange(

                    new Game(1, 1, 1, 1, true, dateCreated, DateTime.MinValue),
                    new Game(2, 2, 2, 2, true, dateCreated, DateTime.MinValue)
                );

                await databaseContext.SaveChangesAsync();
            }

            return databaseContext;
        }
    }
}
