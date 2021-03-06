﻿using NUnit.Framework;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Data.Models.DataModels;
using SudokuCollective.Data.Services;
using SudokuCollective.Test.TestData;
using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuCollective.Test.TestCases.Services
{
    public class EmailServiceShould
    {
        private EmailMetaData emailMetaData;
        private EmailMetaData incorrectEmailMetaData;
        private IEmailService sut;
        private IEmailService sutFailure;
        private string toEmail;
        private string subject;
        private string html;

        [SetUp]
        public void Setup()
        {
            emailMetaData = TestObjects.GetEmailMetaData();
            incorrectEmailMetaData = TestObjects.GetIncorrectEmailMetaData();

            sut = new EmailService(emailMetaData);
            sutFailure = new EmailService(incorrectEmailMetaData);

            toEmail = "SudokuCollective@gmail.com";
            subject = "testing123...";
            html = "<h1>Hello World!</h1>";
        }

        [Test]
        [Category("Services")]
        public void SendEmails()
        {
            // Arrange

            // Act
            var result = sut.Send(toEmail, subject, html);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        [Category("Services")]
        public void ReturnFalseIfSendEmailsFails()
        {
            // Arrange

            // Act
            var result = sutFailure.Send(toEmail, subject, html);

            // Assert
            Assert.That(result, Is.False);
        }
    }
}
