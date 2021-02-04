﻿using System.Collections.Generic;
using NUnit.Framework;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Models;

namespace SudokuCollective.Test.TestCases.Models
{
    public class DifficultyShould
    {
        private Difficulty sut;

        [SetUp]
        public void Setup()
        {
            sut = new Difficulty();
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
        public void HasANameValue()
        {
            // Arrange and Act

            // Assert
            Assert.That(sut.Name, Is.InstanceOf<string>());
        }

        [Test]
        [Category("Models")]
        public void HasADifficultyLevel()
        {
            // Arrange and Act

            // Assert
            Assert.That(sut.DifficultyLevel, Is.InstanceOf<DifficultyLevel>());
        }

        [Test]
        [Category("Models")]
        public void HaveADisplayNameDifferentThanTheName()
        {
            // Arrange

            // Act
            sut.Name = "name";
            sut.DisplayName = "displayName";

            // Assert
            Assert.That(sut.DisplayName, Is.InstanceOf<string>());
            Assert.That(sut.DisplayName, Is.Not.EqualTo(sut.Name));
        }

        [Test]
        [Category("Models")]
        public void HasANavigationPropertyToSudokuMatrices()
        {
            // Arrange and Act

            // Assert
            Assert.That(sut.Matrices, Is.InstanceOf<List<SudokuMatrix>>());
        }

        [Test]
        [Category("Models")]
        public void HasAJsonConstructor()
        {
            // Arrange
            var id = 2;
            var name = "name";
            var displayName = "displayName";
            var difficultyLevel = DifficultyLevel.TEST;

            // Act
            sut = new Difficulty(
                id,
                name,
                displayName,
                difficultyLevel);

            // Assert
            Assert.That(sut, Is.InstanceOf<Difficulty>());
        }
    }
}
