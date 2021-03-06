﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.APIModels.PageModels;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Core.Models;
using SudokuCollective.Core.Interfaces.Repositories;
using SudokuCollective.Data.Helpers;
using SudokuCollective.Data.Messages;
using SudokuCollective.Data.Models.ResultModels;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels.UserResults;
using SudokuCollective.Core.Interfaces.DataModels;

namespace SudokuCollective.Data.Services
{
    public class UsersService : IUsersService
    {
        #region Fields
        private readonly IUsersRepository<User> usersRepository;
        private readonly IAppsRepository<App> appsRepository;
        private readonly IRolesRepository<Role> rolesRepository;
        private readonly IAppAdminsRepository<AppAdmin> appAdminsRepository;
        private readonly IEmailConfirmationsRepository<EmailConfirmation> emailConfirmationsRepository;
        private readonly IPasswordResetsRepository<PasswordReset> passwordResetsRepository;
        private readonly IEmailService emailService;
        #endregion

        #region Constructor
        public UsersService(
            IUsersRepository<User> usersRepo,
            IAppsRepository<App> appsRepo,
            IRolesRepository<Role> rolesRepo,
            IAppAdminsRepository<AppAdmin> appAdminsRepo,
            IEmailConfirmationsRepository<EmailConfirmation> emailConfirmationsRepo,
            IPasswordResetsRepository<PasswordReset> passwordResetsRepo,
            IEmailService emailServ)
        {
            usersRepository = usersRepo;
            appsRepository = appsRepo;
            rolesRepository = rolesRepo;
            appAdminsRepository = appAdminsRepo;
            emailConfirmationsRepository = emailConfirmationsRepo;
            passwordResetsRepository = passwordResetsRepo;
            emailService = emailServ;
        }
        #endregion

        #region Methods
        public async Task<IUserResult> GetUser(
            int id,
            string license,
            bool fullRecord = true)
        {
            var result = new UserResult();

            try
            {
                if (await usersRepository.HasEntity(id))
                {
                    var response = await usersRepository.GetById(id, fullRecord);

                    if (response.Success)
                    {
                        var user = (User)response.Object;

                        if (fullRecord)
                        {
                            foreach (var userApp in user.Apps)
                            {
                                userApp.App = null;
                            }

                            foreach (var userRole in user.Roles)
                            {
                                userRole.Role.Users = null;
                            }

                            foreach (var game in user.Games)
                            {
                                game.User = null;
                                game.SudokuMatrix.Difficulty.Matrices = null;
                            }
                        }

                        result.Success = response.Success;
                        result.Message = UsersMessages.UserFoundMessage;
                        result.User = user;

                        var app = (App)(await appsRepository.GetByLicense(license)).Object;
                        var appAdmins = (await appAdminsRepository.GetAll())
                            .Objects
                            .ConvertAll(aa => (AppAdmin)aa)
                            .ToList();

                        if (result
                            .User
                            .Roles
                            .Any(ur => ur.Role.RoleLevel == RoleLevel.ADMIN))
                        {
                            if (!user.IsSuperUser)
                            {
                                if (!appAdmins.Any(aa =>
                                    aa.AppId == app.Id &&
                                    aa.UserId == result.User.Id &&
                                    aa.IsActive))
                                {
                                    var adminRole = result
                                        .User
                                        .Roles
                                        .FirstOrDefault(ur =>
                                            ur.Role.RoleLevel == RoleLevel.ADMIN);

                                    result.User.Roles.Remove(adminRole);
                                }
                            }
                            else
                            {
                                if (!app.PermitSuperUserAccess)
                                {
                                    if (user.Roles.Any(ur => ur.Role.RoleLevel == RoleLevel.SUPERUSER))
                                    {
                                        var superUserRole = user
                                            .Roles
                                            .FirstOrDefault(ur => ur.Role.RoleLevel == RoleLevel.SUPERUSER);

                                        user.Roles.Remove(superUserRole);
                                    }

                                    if (user.Roles.Any(ur => ur.Role.RoleLevel == RoleLevel.ADMIN))
                                    {
                                        var adminRole = user
                                            .Roles
                                            .FirstOrDefault(ur => ur.Role.RoleLevel == RoleLevel.ADMIN);

                                        user.Roles.Remove(adminRole);
                                    }
                                }
                            }
                        }

                        return result;
                    }
                    else if (!response.Success && response.Exception != null)
                    {
                        result.Success = response.Success;
                        result.Message = response.Exception.Message;

                        return result;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = UsersMessages.UserNotFoundMessage;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = UsersMessages.UserNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<IUsersResult> GetUsers(
            int requestorId,
            string license,
            IPaginator paginator,
            bool fullRecord = true)
        {
            var result = new UsersResult();

            try
            {
                var response = await usersRepository.GetAll(fullRecord);

                if (response.Success)
                {
                    if (paginator != null)
                    {
                        if (StaticDataHelpers.IsPageValid(paginator, response.Objects))
                        {
                            if (paginator.SortBy == SortValue.NULL)
                            {
                                result.Users = response.Objects.ConvertAll(u => (IUser)u);
                            }
                            else if (paginator.SortBy == SortValue.ID)
                            {
                                if (!paginator.OrderByDescending)
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Users.Add((IUser)obj);
                                    }

                                    result.Users = result.Users
                                        .OrderBy(u => u.Id)
                                        .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                        .Take(paginator.ItemsPerPage)
                                        .ToList();
                                }
                                else
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Users.Add((IUser)obj);
                                    }

                                    result.Users = result.Users
                                        .OrderByDescending(u => u.Id)
                                        .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                        .Take(paginator.ItemsPerPage)
                                        .ToList();
                                }
                            }
                            else if (paginator.SortBy == SortValue.USERNAME)
                            {
                                if (!paginator.OrderByDescending)
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Users.Add((IUser)obj);
                                    }

                                    result.Users = result.Users
                                        .OrderBy(u => u.UserName)
                                        .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                        .Take(paginator.ItemsPerPage)
                                        .ToList();
                                }
                                else
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Users.Add((IUser)obj);
                                    }

                                    result.Users = result.Users
                                        .OrderByDescending(u => u.UserName)
                                        .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                        .Take(paginator.ItemsPerPage)
                                        .ToList();
                                }
                            }
                            else if (paginator.SortBy == SortValue.FIRSTNAME)
                            {
                                if (!paginator.OrderByDescending)
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Users.Add((IUser)obj);
                                    }

                                    result.Users = result.Users
                                        .OrderBy(u => u.FirstName)
                                        .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                        .Take(paginator.ItemsPerPage)
                                        .ToList();
                                }
                                else
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Users.Add((IUser)obj);
                                    }

                                    result.Users = result.Users
                                        .OrderByDescending(u => u.FirstName)
                                        .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                        .Take(paginator.ItemsPerPage)
                                        .ToList();
                                }
                            }
                            else if (paginator.SortBy == SortValue.LASTNAME)
                            {
                                if (!paginator.OrderByDescending)
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Users.Add((IUser)obj);
                                    }

                                    result.Users = result.Users
                                        .OrderBy(u => u.LastName)
                                        .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                        .Take(paginator.ItemsPerPage)
                                        .ToList();
                                }
                                else
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Users.Add((IUser)obj);
                                    }

                                    result.Users = result.Users
                                        .OrderByDescending(u => u.LastName)
                                        .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                        .Take(paginator.ItemsPerPage)
                                        .ToList();
                                }
                            }
                            else if (paginator.SortBy == SortValue.FULLNAME)
                            {
                                if (!paginator.OrderByDescending)
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Users.Add((IUser)obj);
                                    }

                                    result.Users = result.Users
                                        .OrderBy(u => u.FullName)
                                        .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                        .Take(paginator.ItemsPerPage)
                                        .ToList();
                                }
                                else
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Users.Add((IUser)obj);
                                    }

                                    result.Users = result.Users
                                        .OrderByDescending(u => u.FullName)
                                        .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                        .Take(paginator.ItemsPerPage)
                                        .ToList();
                                }
                            }
                            else if (paginator.SortBy == SortValue.NICKNAME)
                            {
                                if (!paginator.OrderByDescending)
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Users.Add((IUser)obj);
                                    }

                                    result.Users = result.Users
                                        .OrderBy(u => u.NickName)
                                        .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                        .Take(paginator.ItemsPerPage)
                                        .ToList();
                                }
                                else
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Users.Add((IUser)obj);
                                    }

                                    result.Users = result.Users
                                        .OrderByDescending(u => u.NickName)
                                        .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                        .Take(paginator.ItemsPerPage)
                                        .ToList();
                                }
                            }
                            else if (paginator.SortBy == SortValue.DATECREATED)
                            {
                                if (!paginator.OrderByDescending)
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Users.Add((IUser)obj);
                                    }

                                    result.Users = result.Users
                                        .OrderBy(u => u.DateCreated)
                                        .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                        .Take(paginator.ItemsPerPage)
                                        .ToList();
                                }
                                else
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Users.Add((IUser)obj);
                                    }

                                    result.Users = result.Users
                                        .OrderByDescending(u => u.DateCreated)
                                        .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                        .Take(paginator.ItemsPerPage)
                                        .ToList();
                                }
                            }
                            else if (paginator.SortBy == SortValue.DATEUPDATED)
                            {
                                if (!paginator.OrderByDescending)
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Users.Add((IUser)obj);
                                    }

                                    result.Users = result.Users
                                        .OrderBy(u => u.DateUpdated)
                                        .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                        .Take(paginator.ItemsPerPage)
                                        .ToList();
                                }
                                else
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Users.Add((IUser)obj);
                                    }

                                    result.Users = result.Users
                                        .OrderByDescending(u => u.DateUpdated)
                                        .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                        .Take(paginator.ItemsPerPage)
                                        .ToList();
                                }
                            }
                            else
                            {
                                result.Success = false;
                                result.Message = ServicesMesages.SortValueNotImplementedMessage;

                                return result;
                            }
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = ServicesMesages.PageNotFoundMessage;

                            return result;
                        }
                    }
                    else
                    {
                        result.Users = response.Objects.ConvertAll(u => (IUser)u);
                    }

                    var app = (App)(await appsRepository.GetByLicense(license)).Object;
                    var appAdmins = (await appAdminsRepository.GetAll())
                        .Objects
                        .ConvertAll(aa => (AppAdmin)aa)
                        .ToList();

                    foreach (var user in result.Users)
                    {
                        if (user
                            .Roles
                            .Any(ur => ur.Role.RoleLevel == RoleLevel.ADMIN))
                        {
                            if (!user.IsSuperUser)
                            {
                                if (!appAdmins.Any(aa =>
                                    aa.AppId == app.Id &&
                                    aa.UserId == user.Id &&
                                    aa.IsActive))
                                {
                                    var adminRole = user
                                        .Roles
                                        .FirstOrDefault(ur =>
                                            ur.Role.RoleLevel == RoleLevel.ADMIN);

                                    user.Roles.Remove(adminRole);
                                }
                            }
                            else
                            {
                                if (!app.PermitSuperUserAccess)
                                {
                                    if (user.Roles.Any(ur => ur.Role.RoleLevel == RoleLevel.SUPERUSER))
                                    {
                                        var superUserRole = user
                                            .Roles
                                            .FirstOrDefault(ur => ur.Role.RoleLevel == RoleLevel.SUPERUSER);

                                        user.Roles.Remove(superUserRole);
                                    }

                                    if (user.Roles.Any(ur => ur.Role.RoleLevel == RoleLevel.ADMIN))
                                    {
                                        var adminRole = user
                                            .Roles
                                            .FirstOrDefault(ur => ur.Role.RoleLevel == RoleLevel.ADMIN);

                                        user.Roles.Remove(adminRole);
                                    }
                                }
                            }
                        }
                    }

                    if (fullRecord)
                    {
                        foreach (var user in result.Users)
                        {
                            foreach (var userApp in user.Apps)
                            {
                                userApp.App = null;
                            }

                            foreach (var userRole in user.Roles)
                            {
                                userRole.Role.Users = null;
                            }

                            foreach (var game in user.Games)
                            {
                                game.User = null;
                                game.SudokuMatrix.Difficulty.Matrices = null;
                            }
                        }
                    }

                    var requestor = (User)(await usersRepository.GetById(requestorId)).Object;

                    if (!requestor.IsSuperUser)
                    {
                        // Filter out user emails from the frontend...
                        foreach (var user in result.Users)
                        {
                            var emailConfirmed = user.EmailConfirmed;
                            user.Email = null;
                            user.EmailConfirmed = emailConfirmed;
                        }
                    }

                    result.Success = response.Success;
                    result.Message = UsersMessages.UsersFoundMessage;

                    return result;
                }
                else if (!response.Success && response.Exception != null)
                {
                    result.Success = response.Success;
                    result.Message = response.Exception.Message;

                    return result;
                }
                else
                {
                    result.Success = false;
                    result.Message = UsersMessages.UsersNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<IUserResult> CreateUser(
            IRegisterRequest request,
            string baseUrl,
            string emailTemplatePath)
        {
            var result = new UserResult();

            var isUserNameUnique = false;
            var isEmailUnique = false;

            // User name accepsts alphanumeric and special characters except double and single quotes
            var regex = new Regex("^[^-]{1}?[^\"\']*$");

            if (!string.IsNullOrEmpty(request.UserName))
            {
                isUserNameUnique = await usersRepository.IsUserNameUnique(request.UserName);
            }

            if (!string.IsNullOrEmpty(request.Email))
            {
                isEmailUnique = await usersRepository.IsEmailUnique(request.Email);
            }

            if (string.IsNullOrEmpty(request.UserName)
                || string.IsNullOrEmpty(request.Email)
                || !isUserNameUnique
                || !isEmailUnique
                || !regex.IsMatch(request.UserName))
            {
                if (string.IsNullOrEmpty(request.UserName))
                {
                    result.Success = false;
                    result.Message = UsersMessages.UserNameRequiredMessage;

                    return result;
                }
                else if (string.IsNullOrEmpty(request.Email))
                {
                    result.Success = false;
                    result.Message = UsersMessages.EmailRequiredMessage;

                    return result;
                }
                else if (!regex.IsMatch(request.UserName))
                {
                    result.Success = false;
                    result.Message = UsersMessages.UserNameInvalidMessage;

                    return result;
                }
                else if (!isUserNameUnique)
                {
                    result.Success = isUserNameUnique;
                    result.Message = UsersMessages.UserNameUniqueMessage;

                    return result;
                }
                else
                {
                    result.Success = isEmailUnique;
                    result.Message = UsersMessages.EmailUniqueMessage;

                    return result;
                }
            }
            else
            {
                try
                {
                    if (await appsRepository.IsAppLicenseValid(request.License))
                    {
                        var appResponse = await appsRepository.GetByLicense(request.License);

                        var app = (App)appResponse.Object;

                        if (!app.IsActive)
                        {
                            result.Success = false;
                            result.Message = AppsMessages.AppDeactivatedMessage;

                            return result;
                        }

                        if (appResponse.Success)
                        {
                            var salt = BCrypt.Net.BCrypt.GenerateSalt();

                            var user = new User(
                                0,
                                request.UserName,
                                request.FirstName,
                                request.LastName,
                                request.NickName,
                                request.Email,
                                false,
                                false,
                                BCrypt.Net.BCrypt.HashPassword(request.Password, salt),
                                false,
                                true,
                                DateTime.UtcNow,
                                DateTime.MinValue);

                            user.Apps.Add(
                                new UserApp()
                                {
                                    User = user,
                                    App = app,
                                    AppId = app.Id
                                });

                            var userResponse = await usersRepository.Add(user);

                            if (userResponse.Success)
                            {
                                user = (User)userResponse.Object;

                                if (user.Roles.Any(ur => ur.Role.RoleLevel == RoleLevel.ADMIN))
                                {
                                    var appAdmin = new AppAdmin(app.Id, user.Id);

                                    _ = await appAdminsRepository.Add(appAdmin);
                                }

                                var emailConfirmation = new EmailConfirmation(
                                    user.Id,
                                    app.Id);

                                emailConfirmation = await EnsureEmailConfirmationTokenIsUnique(emailConfirmation);

                                emailConfirmation = (EmailConfirmation)(await emailConfirmationsRepository.Create(emailConfirmation))
                                    .Object;

                                string EmailConfirmationAction;

                                if (app.UseCustomEmailConfirmationAction)
                                {
                                    if (app.InDevelopment)
                                    {
                                        EmailConfirmationAction = string.Format("{0}/{1}/{2}",
                                            app.DevUrl,
                                            app.CustomEmailConfirmationAction,
                                            emailConfirmation.Token);
                                    }
                                    else
                                    {
                                        EmailConfirmationAction = string.Format("{0}/{1}/{2}",
                                            app.LiveUrl,
                                            app.CustomEmailConfirmationAction,
                                            emailConfirmation.Token);
                                    }
                                }
                                else
                                {
                                    EmailConfirmationAction = string.Format("https://{0}/confirmEmail/{1}",
                                        baseUrl,
                                        emailConfirmation.Token);
                                }

                                var html = File.ReadAllText(emailTemplatePath);
                                var appTitle = app.Name;
                                var url = string.Empty;

                                if (app.InDevelopment)
                                {
                                    url = app.DevUrl;
                                }
                                else
                                {
                                    url = app.LiveUrl;
                                }

                                html = html.Replace("{{USER_NAME}}", user.UserName);
                                html = html.Replace("{{CONFIRM_EMAIL_URL}}", EmailConfirmationAction);
                                html = html.Replace("{{APP_TITLE}}", appTitle);
                                html = html.Replace("{{URL}}", url);

                                var emailSubject = string.Format("Greetings from {0}: Please Confirm Email", appTitle);

                                result.ConfirmationEmailSuccessfullySent = emailService
                                    .Send(user.Email, emailSubject, html);

                                foreach (var userRole in user.Roles)
                                {
                                    userRole.Role.Users = new List<UserRole>();
                                }

                                foreach (var userApp in user.Apps)
                                {
                                    userApp.App.Users = new List<UserApp>();
                                }

                                result.Success = userResponse.Success;
                                result.Message = UsersMessages.UserCreatedMessage;
                                result.User = user;

                                return result;
                            }
                            else if (!userResponse.Success && userResponse.Exception != null)
                            {
                                result.Success = userResponse.Success;
                                result.Message = userResponse.Exception.Message;

                                return result;
                            }
                            else
                            {
                                result.Success = userResponse.Success;
                                result.Message = UsersMessages.UserNotCreatedMessage;

                                return result;
                            }
                        }
                        else if (!appResponse.Success && appResponse.Exception != null)
                        {
                            result.Success = appResponse.Success;
                            result.Message = appResponse.Exception.Message;

                            return result;
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = AppsMessages.AppNotFoundMessage;

                            return result;
                        }
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = AppsMessages.AppNotFoundMessage;

                        return result;
                    }
                }
                catch (Exception exp)
                {
                    result.Success = false;
                    result.Message = exp.Message;

                    return result;
                }
            }
        }

        public async Task<IUserResult> UpdateUser(
            int id,
            IUpdateUserRequest request,
            string baseUrl,
            string emailTemplatePath)
        {
            var result = new UserResult();

            // User name accepsts alphanumeric and special characters except double and single quotes
            var regex = new Regex("^[^-]{1}?[^\"\']*$");

            var isUserNameUnique = await usersRepository.IsUpdatedUserNameUnique(id, request.UserName);
            var isEmailUnique = await usersRepository.IsUpdatedEmailUnique(id, request.Email);

            if (string.IsNullOrEmpty(request.UserName)
                || string.IsNullOrEmpty(request.Email)
                || !isUserNameUnique
                || !isEmailUnique
                || !regex.IsMatch(request.UserName))
            {
                if (string.IsNullOrEmpty(request.UserName))
                {
                    result.Success = false;
                    result.Message = UsersMessages.UserNameRequiredMessage;

                    return result;
                }
                else if (string.IsNullOrEmpty(request.Email))
                {
                    result.Success = false;
                    result.Message = UsersMessages.EmailRequiredMessage;

                    return result;
                }
                else if (!regex.IsMatch(request.UserName))
                {
                    result.Success = false;
                    result.Message = UsersMessages.UserNameInvalidMessage;

                    return result;
                }
                else if (!isUserNameUnique)
                {
                    result.Success = isUserNameUnique;
                    result.Message = UsersMessages.UserNameUniqueMessage;

                    return result;
                }
                else
                {
                    result.Success = isEmailUnique;
                    result.Message = UsersMessages.EmailUniqueMessage;

                    return result;
                }
            }
            else
            {
                try
                {
                    if (await usersRepository.HasEntity(id))
                    {
                        var userResponse = await usersRepository.GetById(id, true);

                        if (userResponse.Success)
                        {
                            var user = (User)userResponse.Object;
                            var app = (App)(await appsRepository.GetById(request.AppId)).Object;

                            user.UserName = request.UserName;
                            user.FirstName = request.FirstName;
                            user.LastName = request.LastName;
                            user.NickName = request.NickName;
                            user.DateUpdated = DateTime.UtcNow;

                            if (!user.Email.ToLower().Equals(request.Email.ToLower()))
                            {
                                if (!user.ReceivedRequestToUpdateEmail)
                                {
                                    user.ReceivedRequestToUpdateEmail = true;
                                }

                                EmailConfirmation emailConfirmation;

                                if (await emailConfirmationsRepository.HasOutstandingEmailConfirmation(user.Id, app.Id))
                                {
                                    emailConfirmation = (EmailConfirmation)(await emailConfirmationsRepository
                                        .RetrieveEmailConfirmation(user.Id, app.Id)).Object;

                                    if (!user.EmailConfirmed)
                                    {
                                        user.Email = emailConfirmation.OldEmailAddress;
                                    }

                                    emailConfirmation.OldEmailAddress = user.Email;
                                    emailConfirmation.NewEmailAddress = request.Email;
                                }
                                else
                                {
                                    emailConfirmation = new EmailConfirmation(
                                        user.Id,
                                        request.AppId,
                                        user.Email,
                                        request.Email);
                                }

                                emailConfirmation = await EnsureEmailConfirmationTokenIsUnique(emailConfirmation);

                                IRepositoryResponse emailConfirmationResponse;

                                if (emailConfirmation.Id == 0)
                                {
                                    emailConfirmationResponse = await emailConfirmationsRepository
                                        .Create(emailConfirmation);
                                }
                                else
                                {
                                    emailConfirmationResponse = await emailConfirmationsRepository
                                        .Update(emailConfirmation);
                                }


                                string EmailConfirmationAction;

                                if (app.UseCustomEmailConfirmationAction)
                                {
                                    if (app.InDevelopment)
                                    {
                                        EmailConfirmationAction = string.Format("{0}/{1}/{2}",
                                            app.DevUrl,
                                            app.CustomEmailConfirmationAction,
                                            emailConfirmation.Token);
                                    }
                                    else
                                    {
                                        EmailConfirmationAction = string.Format("{0}/{1}/{2}",
                                            app.LiveUrl,
                                            app.CustomEmailConfirmationAction,
                                            emailConfirmation.Token);
                                    }
                                }
                                else
                                {
                                    EmailConfirmationAction = string.Format("https://{0}/confirmEmail/{1}",
                                        baseUrl,
                                        ((EmailConfirmation)emailConfirmationResponse.Object).Token);
                                }

                                var html = File.ReadAllText(emailTemplatePath);
                                var appTitle = app.Name;
                                var url = string.Empty;

                                if (app.InDevelopment)
                                {
                                    url = app.DevUrl;
                                }
                                else
                                {
                                    url = app.LiveUrl;
                                }

                                html = html.Replace("{{USER_NAME}}", user.UserName);
                                html = html.Replace("{{CONFIRM_EMAIL_URL}}", EmailConfirmationAction);
                                html = html.Replace("{{APP_TITLE}}", appTitle);
                                html = html.Replace("{{URL}}", url);

                                var emailSubject = string.Format("Greetings from {0}: Please Confirm Old Email", appTitle);

                                result.ConfirmationEmailSuccessfullySent = emailService
                                    .Send(user.Email, emailSubject, html);
                            }

                            var updateUserResponse = await usersRepository.Update(user);

                            if (updateUserResponse.Success)
                            {
                                user = (User)updateUserResponse.Object;

                                result.Success = userResponse.Success;
                                result.Message = UsersMessages.UserUpdatedMessage;
                                result.User = user;

                                return result;
                            }
                            else if (!updateUserResponse.Success && updateUserResponse.Exception != null)
                            {
                                result.Success = userResponse.Success;
                                result.Message = userResponse.Exception.Message;

                                return result;
                            }
                            else
                            {
                                result.Success = false;
                                result.Message = UsersMessages.UserNotUpdatedMessage;

                                return result;
                            }
                        }
                        else if (!userResponse.Success && userResponse.Exception != null)
                        {
                            result.Success = userResponse.Success;
                            result.Message = userResponse.Exception.Message;

                            return result;
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = UsersMessages.UserNotFoundMessage;

                            return result;
                        }
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = UsersMessages.UserNotFoundMessage;

                        return result;
                    }
                }
                catch (Exception exp)
                {
                    result.Success = false;
                    result.Message = exp.Message;

                    return result;
                }
            }
        }

        public async Task<IBaseResult> RequestPasswordReset(
            IRequestPasswordResetRequest request,
            string baseUrl,
            string emailTemplatePath)
        {
            var result = new BaseResult();

            try
            {
                var appResult = await appsRepository.GetByLicense(request.License);

                if (appResult.Success)
                {
                    var userResult = await usersRepository.GetByEmail(request.Email);

                    if (userResult.Success)
                    {
                        var app = (App)appResult.Object;
                        var user = (User)userResult.Object;
                        PasswordReset passwordReset;

                        if (user.Apps.Any(ua => ua.AppId == app.Id))
                        {
                            if (!user.EmailConfirmed)
                            {
                                result.Success = false;
                                result.Message = UsersMessages.UserEmailNotConfirmedMessage;

                                return result;
                            }

                            if (await passwordResetsRepository.HasOutstandingPasswordReset(user.Id, app.Id))
                            {
                                passwordReset = (PasswordReset)(await passwordResetsRepository.RetrievePasswordReset(
                                    user.Id,
                                    app.Id)).Object;

                                passwordReset = await EnsurePasswordResetTokenIsUnique(passwordReset);

                                passwordReset = (PasswordReset)(await passwordResetsRepository.Update(passwordReset)).Object;

                                if (!user.ReceivedRequestToUpdatePassword)
                                {
                                    user.ReceivedRequestToUpdatePassword = true;

                                    user = (User)(await usersRepository.Update(user)).Object;
                                }

                                return SendPasswordResetEmail(
                                    user,
                                    app,
                                    passwordReset,
                                    emailTemplatePath,
                                    baseUrl,
                                    result,
                                    false);
                            }
                            else
                            {
                                passwordReset = new PasswordReset(user.Id, app.Id);

                                passwordReset = await EnsurePasswordResetTokenIsUnique(passwordReset);

                                var passwordResetResult = await passwordResetsRepository.Create(passwordReset);

                                if (passwordResetResult.Success)
                                {
                                    user.ReceivedRequestToUpdatePassword = true;

                                    user = (User)(await usersRepository.Update(user)).Object;

                                    return SendPasswordResetEmail(
                                        user,
                                        app,
                                        passwordReset,
                                        emailTemplatePath,
                                        baseUrl,
                                        result,
                                        true);
                                }
                                else if (!passwordResetResult.Success && passwordResetResult.Exception != null)
                                {
                                    result.Success = passwordResetResult.Success;
                                    result.Message = passwordResetResult.Exception.Message;

                                    return result;
                                }
                                else
                                {
                                    result.Success = userResult.Success;
                                    result.Message = UsersMessages.UnableToProcessPasswordResetRequesMessage;

                                    return result;
                                }
                            }
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = AppsMessages.UserNotSignedUpToAppMessage;

                            return result;
                        }
                    }
                    else if (!userResult.Success && userResult.Exception != null)
                    {
                        result.Success = userResult.Success;
                        result.Message = userResult.Exception.Message;

                        return result;
                    }
                    else
                    {
                        result.Success = userResult.Success;
                        result.Message = UsersMessages.NoUserIsUsingThisEmailMessage;

                        return result;
                    }
                }
                else if (!appResult.Success && appResult.Exception != null)
                {
                    result.Success = appResult.Success;
                    result.Message = appResult.Exception.Message;

                    return result;
                }
                else
                {
                    result.Success = appResult.Success;
                    result.Message = AppsMessages.AppNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<IInitiatePasswordResetResult> InitiatePasswordReset(string token)
        {
            var result = new InitiatePasswordResetResult();

            try
            {
                var passwordResetResult = await passwordResetsRepository.Get(token);

                if (passwordResetResult.Success)
                {
                    var passwordReset = (PasswordReset)passwordResetResult.Object;

                    var userResult = await usersRepository.GetById(passwordReset.UserId);

                    if (userResult.Success)
                    {
                        var user = (User)userResult.Object;

                        var appResult = await appsRepository.GetById(passwordReset.AppId);

                        if (appResult.Success)
                        {
                            var app = (App)appResult.Object;

                            if (user.Apps.Any(ua => ua.AppId == app.Id))
                            {
                                if (user.ReceivedRequestToUpdatePassword)
                                {
                                    result.Success = true;
                                    result.Message = UsersMessages.UserFoundMessage;
                                    result.User = user;
                                    result.App = app;

                                    return result;
                                }
                                else
                                {
                                    result.Success = false;
                                    result.Message = UsersMessages.NoOutstandingRequestToResetPassworMessage;

                                    return result;
                                }
                            }
                            else
                            {
                                result.Success = false;
                                result.Message = AppsMessages.UserNotSignedUpToAppMessage;

                                return result;
                            }
                        }
                        else if (!appResult.Success && appResult.Exception != null)
                        {
                            result.Success = passwordResetResult.Success;
                            result.Message = passwordResetResult.Exception.Message;

                            return result;
                        }
                        else
                        {
                            result.Success = appResult.Success;
                            result.Message = AppsMessages.AppNotFoundMessage;

                            return result;
                        }
                    }
                    else if (!userResult.Success && userResult.Exception != null)
                    {
                        result.Success = passwordResetResult.Success;
                        result.Message = passwordResetResult.Exception.Message;

                        return result;
                    }
                    else
                    {
                        result.Success = userResult.Success;
                        result.Message = UsersMessages.UserNotFoundMessage;

                        return result;
                    }
                }
                else if (!passwordResetResult.Success && passwordResetResult.Exception != null)
                {
                    result.Success = passwordResetResult.Success;
                    result.Message = passwordResetResult.Exception.Message;

                    return result;
                }
                else
                {
                    result.Success = passwordResetResult.Success;
                    result.Message = UsersMessages.PasswordResetRequestNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<IBaseResult> UpdatePassword(IPasswordResetRequest request)
        {
            var result = new BaseResult();
            var salt = BCrypt.Net.BCrypt.GenerateSalt();

            try
            {
                if (await usersRepository.HasEntity(request.UserId))
                {
                    var userResponse = await usersRepository.GetById(request.UserId, true);

                    if (userResponse.Success)
                    {
                        var user = (User)userResponse.Object;

                        if (user.ReceivedRequestToUpdatePassword)
                        {
                            user.Password = BCrypt.Net.BCrypt
                                    .HashPassword(request.NewPassword, salt);

                            user.DateUpdated = DateTime.UtcNow;

                            user.ReceivedRequestToUpdatePassword = false;

                            var updateUserResponse = await usersRepository.Update(user);

                            if (updateUserResponse.Success)
                            {
                                result.Success = userResponse.Success;
                                result.Message = UsersMessages.PasswordResetMessage;

                                return result;
                            }
                            else if (!updateUserResponse.Success && updateUserResponse.Exception != null)
                            {
                                result.Success = userResponse.Success;
                                result.Message = userResponse.Exception.Message;

                                return result;
                            }
                            else
                            {
                                result.Success = false;
                                result.Message = UsersMessages.PasswordNotResetMessage;

                                return result;
                            }
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = UsersMessages.NoOutstandingRequestToResetPassworMessage;

                            return result;
                        }
                    }
                    else if (!userResponse.Success && userResponse.Exception != null)
                    {
                        result.Success = false;
                        result.Message = UsersMessages.UserNotFoundMessage;

                        return result;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = UsersMessages.UsersFoundMessage;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = UsersMessages.UserNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<IBaseResult> DeleteUser(int id)
        {
            var result = new BaseResult();

            try
            {
                if (await usersRepository.HasEntity(id))
                {
                    var userResponse = await usersRepository.GetById(id);

                    if (((User)userResponse.Object).Id == 1 && ((User)userResponse.Object).IsSuperUser)
                    {
                        result.Success = false;
                        result.Message = UsersMessages.SuperUserCannotBeDeletedMessage;

                        return result;
                    }

                    if (userResponse.Success)
                    {
                        var deletionResponse = await usersRepository.Delete((User)userResponse.Object);

                        if (deletionResponse.Success)
                        {
                            var admins = (await appAdminsRepository.GetAll())
                                .Objects
                                .ConvertAll(aa => (AppAdmin)aa)
                                .Where(aa => aa.UserId == id)
                                .ToList();

                            _ = await appAdminsRepository.DeleteRange(admins);

                            result.Success = true;
                            result.Message = UsersMessages.UserDeletedMessage;

                            return result;
                        }
                        else if (!deletionResponse.Success && deletionResponse.Exception != null)
                        {
                            result.Success = deletionResponse.Success;
                            result.Message = deletionResponse.Exception.Message;

                            return result;
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = UsersMessages.UserNotDeletedMessage;

                            return result;
                        }
                    }
                    else if (!userResponse.Success && userResponse.Exception != null)
                    {
                        result.Success = userResponse.Success;
                        result.Message = userResponse.Exception.Message;

                        return result;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = UsersMessages.UserNotFoundMessage;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = UsersMessages.UserNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<IRolesResult> AddUserRoles(
            int userid,
            List<int> roleIds,
            string license)
        {
            var result = new RolesResult();

            try
            {
                if (await usersRepository.HasEntity(userid))
                {
                    if (await rolesRepository.IsListValid(roleIds))
                    {
                        var response = await usersRepository.AddRoles(userid, roleIds);

                        if (response.Success)
                        {
                            var roles = response
                                .Objects
                                .ConvertAll(ur => (UserRole)ur)
                                .ToList();

                            var app = (App)(await appsRepository.GetByLicense(license)).Object;

                            foreach (var role in roles)
                            {
                                if (role.Role.RoleLevel == RoleLevel.ADMIN)
                                {
                                    var appAdmin = (AppAdmin)(await appAdminsRepository.Add(new AppAdmin(app.Id, userid))).Object;
                                }

                                result.Roles.Add(role.Role);
                            }

                            result.Success = response.Success;
                            result.Message = UsersMessages.RolesAddedMessage;

                            return result;
                        }
                        else if (!response.Success && response.Exception != null)
                        {
                            result.Success = response.Success;
                            result.Message = response.Exception.Message;

                            return result;
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = UsersMessages.RolesNotAddedMessage;

                            return result;
                        }
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = UsersMessages.RolesInvalidMessage;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = UsersMessages.UserNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<IBaseResult> RemoveUserRoles(
            int userid,
            List<int> roleIds,
            string license)
        {
            var result = new BaseResult();

            try
            {
                if (await usersRepository.HasEntity(userid))
                {
                    if (await rolesRepository.IsListValid(roleIds))
                    {
                        var response = await usersRepository.RemoveRoles(userid, roleIds);

                        if (response.Success)
                        {
                            var roles = response
                                .Objects
                                .ConvertAll(ur => (UserRole)ur)
                                .ToList();

                            var app = (App)(await appsRepository.GetByLicense(license)).Object;

                            foreach (var role in roles)
                            {
                                if (role.Role.RoleLevel == RoleLevel.ADMIN)
                                {
                                    var appAdmin = (AppAdmin)
                                        (await appAdminsRepository.GetAdminRecord(app.Id, userid))
                                        .Object;

                                    _ = await appAdminsRepository.Delete(appAdmin);
                                }
                            }

                            result.Success = response.Success;
                            result.Message = UsersMessages.RolesRemovedMessage;

                            return result;
                        }
                        else if (!response.Success && response.Exception != null)
                        {
                            result.Success = response.Success;
                            result.Message = response.Exception.Message;

                            return result;
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = UsersMessages.RolesNotRemovedMessage;

                            return result;
                        }
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = UsersMessages.RolesInvalidMessage;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = UsersMessages.UserNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<IBaseResult> ActivateUser(int id)
        {
            var result = new BaseResult();

            try
            {
                if (await usersRepository.HasEntity(id))
                {
                    if (await usersRepository.ActivateUser(id))
                    {
                        result.Success = true;
                        result.Message = UsersMessages.UserActivatedMessage;

                        return result;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = UsersMessages.UserNotActivatedMessage;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = UsersMessages.UserNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<IBaseResult> DeactivateUser(int id)
        {
            var result = new BaseResult();

            try
            {
                if (await usersRepository.HasEntity(id))
                {
                    if (await usersRepository.DeactivateUser(id))
                    {
                        result.Success = true;
                        result.Message = UsersMessages.UserDeactivatedMessage;

                        return result;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = UsersMessages.UserNotDeactivatedMessage;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = UsersMessages.UserNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<IConfirmEmailResult> ConfirmEmail(
            string token,
            string baseUrl,
            string emailTemplatePath)
        {
            var result = new ConfirmEmailResult();

            try
            {
                var emailConfirmationResponse = await emailConfirmationsRepository.Get(token);

                if (emailConfirmationResponse.Success)
                {
                    var emailConfirmation = (EmailConfirmation)emailConfirmationResponse.Object;

                    if (!emailConfirmation.IsUpdate)
                    {
                        var response = await usersRepository.ConfirmEmail(emailConfirmation);

                        if (response.Success)
                        {
                            var removeEmailConfirmationResponse = await emailConfirmationsRepository.Delete(emailConfirmation);

                            var user = (User)response.Object;

                            result.Success = response.Success;
                            result.UserName = user.UserName;

                            result.AppTitle = user
                                .Apps
                                .Where(ua => ua.AppId == emailConfirmation.AppId)
                                .Select(ua => ua.App.Name)
                                .FirstOrDefault();

                            if (user
                                .Apps
                                .Where(ua => ua.AppId == emailConfirmation.AppId)
                                .Select(ua => ua.App.InDevelopment)
                                .FirstOrDefault())
                            {
                                result.Url = user
                                    .Apps
                                    .Where(ua => ua.AppId == emailConfirmation.AppId)
                                    .Select(ua => ua.App.DevUrl)
                                    .FirstOrDefault();
                            }
                            else
                            {
                                result.Url = user
                                    .Apps
                                    .Where(ua => ua.AppId == emailConfirmation.AppId)
                                    .Select(ua => ua.App.LiveUrl)
                                    .FirstOrDefault();
                            }

                            result.Message = UsersMessages.EmailConfirmedMessage;

                            return result;
                        }
                        else if (!response.Success && response.Exception != null)
                        {
                            result.Success = response.Success;
                            result.Message = response.Exception.Message;

                            return result;
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = UsersMessages.EmailNotConfirmedMessage;

                            return result;
                        }
                    }
                    else if (emailConfirmation.IsUpdate && !(bool)emailConfirmation.OldEmailAddressConfirmed)
                    {
                        var response = await usersRepository.UpdateUserEmail(emailConfirmation);
                        var user = (User)response.Object;
                        var app = (App)(await appsRepository.GetById(emailConfirmation.AppId)).Object;

                        if (response.Success)
                        {
                            var html = File.ReadAllText(emailTemplatePath);
                            var EmailConfirmationAction = string.Format("https://{0}/confirmEmail/{1}",
                                baseUrl,
                                emailConfirmation.Token);
                            var appTitle = app.Name;
                            var url = string.Empty;

                            if (app.InDevelopment)
                            {
                                url = app.DevUrl;
                            }
                            else
                            {
                                url = app.LiveUrl;
                            }

                            html = html.Replace("{{USER_NAME}}", user.UserName);
                            html = html.Replace("{{CONFIRM_EMAIL_URL}}", EmailConfirmationAction);
                            html = html.Replace("{{APP_TITLE}}", appTitle);
                            html = html.Replace("{{URL}}", url);

                            var emailSubject = string.Format("Greetings from {0}: Please Confirm New Email", appTitle);

                            result.ConfirmationEmailSuccessfullySent = emailService
                                .Send(user.Email, emailSubject, html);

                            emailConfirmation.OldEmailAddressConfirmed = true;

                            emailConfirmation = (EmailConfirmation)(await emailConfirmationsRepository.Update(emailConfirmation)).Object;

                            result.Success = response.Success;
                            result.UserName = user.UserName;
                            result.IsUpdate = emailConfirmation.IsUpdate;
                            result.AppTitle = appTitle;
                            result.Url = url;
                            result.Message = UsersMessages.OldEmailConfirmedMessage;

                            return result;
                        }
                        else if (!response.Success && response.Exception != null)
                        {
                            result.Success = response.Success;
                            result.Message = response.Exception.Message;

                            return result;
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = UsersMessages.OldEmailNotConfirmedMessage;

                            return result;
                        }
                    }
                    else
                    {
                        var response = await usersRepository.ConfirmEmail(emailConfirmation);

                        if (response.Success)
                        {
                            var removeEmailConfirmationResponse = await emailConfirmationsRepository.Delete(emailConfirmation);

                            var user = (User)response.Object;

                            result.Success = response.Success;
                            result.UserName = user.UserName;
                            result.IsUpdate = emailConfirmation.IsUpdate;
                            result.NewEmailAddressConfirmed = true;
                            result.AppTitle = user
                                .Apps
                                .Where(ua => ua.AppId == emailConfirmation.AppId)
                                .Select(ua => ua.App.Name)
                                .FirstOrDefault();


                            if (user
                                .Apps
                                .Where(ua => ua.AppId == emailConfirmation.AppId)
                                .Select(ua => ua.App.InDevelopment)
                                .FirstOrDefault())
                            {
                                result.Url = user
                                    .Apps
                                    .Where(ua => ua.AppId == emailConfirmation.AppId)
                                    .Select(ua => ua.App.DevUrl)
                                    .FirstOrDefault();
                            }
                            else
                            {
                                result.Url = user
                                    .Apps
                                    .Where(ua => ua.AppId == emailConfirmation.AppId)
                                    .Select(ua => ua.App.LiveUrl)
                                    .FirstOrDefault();
                            }

                            result.Message = UsersMessages.EmailConfirmedMessage;

                            return result;
                        }
                        else if (!response.Success && response.Exception != null)
                        {
                            result.Success = response.Success;
                            result.Message = response.Exception.Message;

                            return result;
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = UsersMessages.EmailNotConfirmedMessage;

                            return result;
                        }
                    }
                }
                else if (!emailConfirmationResponse.Success && emailConfirmationResponse.Exception != null)
                {
                    result.Success = emailConfirmationResponse.Success;
                    result.Message = emailConfirmationResponse.Exception.Message;

                    return result;
                }
                else
                {
                    result.Success = false;
                    result.Message = UsersMessages.EmailNotConfirmedMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<IUserResult> ResendEmailConfirmation(
            int userId,
            int appId,
            string baseUrl,
            string emailTemplatePath)
        {
            var result = new UserResult();

            try
            {
                if (await usersRepository.HasEntity(userId))
                {
                    var user = (User)((await usersRepository.GetById(userId)).Object);

                    if (!user.EmailConfirmed)
                    {
                        if (await appsRepository.HasEntity(appId))
                        {
                            var app = (App)((await appsRepository.GetById(appId)).Object);

                            if (await emailConfirmationsRepository.HasOutstandingEmailConfirmation(userId, appId))
                            {
                                var emailConfirmationResponse = await emailConfirmationsRepository.RetrieveEmailConfirmation(userId, appId);

                                if (emailConfirmationResponse.Success)
                                {
                                    var emailConfirmation = (EmailConfirmation)emailConfirmationResponse.Object;

                                    string EmailConfirmationAction;

                                    if (app.UseCustomEmailConfirmationAction)
                                    {
                                        if (app.InDevelopment)
                                        {
                                            EmailConfirmationAction = string.Format("{0}/{1}/{2}",
                                                app.DevUrl,
                                                app.CustomEmailConfirmationAction,
                                                emailConfirmation.Token);
                                        }
                                        else
                                        {
                                            EmailConfirmationAction = string.Format("{0}/{1}/{2}",
                                                app.LiveUrl,
                                                app.CustomEmailConfirmationAction,
                                                emailConfirmation.Token);
                                        }
                                    }
                                    else
                                    {
                                        EmailConfirmationAction = string.Format("https://{0}/confirmEmail/{1}",
                                            baseUrl,
                                            ((EmailConfirmation)emailConfirmationResponse.Object).Token);
                                    }

                                    var html = File.ReadAllText(emailTemplatePath);
                                    var appTitle = app.Name;
                                    var url = string.Empty;

                                    if (app.InDevelopment)
                                    {
                                        url = app.DevUrl;
                                    }
                                    else
                                    {
                                        url = app.LiveUrl;
                                    }

                                    html = html.Replace("{{USER_NAME}}", user.UserName);
                                    html = html.Replace("{{CONFIRM_EMAIL_URL}}", EmailConfirmationAction);
                                    html = html.Replace("{{APP_TITLE}}", appTitle);
                                    html = html.Replace("{{URL}}", url);

                                    var emailSubject = string.Format("Greetings from {0}: Please Confirm Email", appTitle);

                                    result.ConfirmationEmailSuccessfullySent = emailService
                                        .Send(user.Email, emailSubject, html);

                                    if ((bool)result.ConfirmationEmailSuccessfullySent)
                                    {
                                        result.User = user;
                                        result.Success = true;
                                        result.Message = UsersMessages.EmailConfirmationEmailResentMessage;

                                        return result;
                                    }
                                    else
                                    {
                                        result.Success = false;
                                        result.Message = UsersMessages.EmailConfirmationEmailNotResentMessage;

                                        return result;
                                    }

                                }
                                else if (!emailConfirmationResponse.Success && emailConfirmationResponse.Exception != null)
                                {
                                    result.Success = emailConfirmationResponse.Success;
                                    result.Message = emailConfirmationResponse.Exception.Message;

                                    return result;
                                }
                                else
                                {
                                    result.Success = false;
                                    result.Message = UsersMessages.EmailConfirmationRequestNotFoundMessage;

                                    return result;
                                }

                            }
                            else
                            {
                                result.Success = false;
                                result.Message = UsersMessages.EmailConfirmationRequestNotFoundMessage;

                                return result;
                            }
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = AppsMessages.AppNotFoundMessage;

                            return result;
                        }
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = UsersMessages.EmailConfirmedMessage;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = UsersMessages.UserNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<IBaseResult> ResendPasswordReset(
            int userId,
            int appId,
            string baseUrl,
            string emailTamplatePath)
        {
            var result = new BaseResult();

            try
            {
                if (await usersRepository.HasEntity(userId))
                {
                    var user = (User)((await usersRepository.GetById(userId)).Object);

                    if (user.ReceivedRequestToUpdatePassword)
                    {
                        if (await appsRepository.HasEntity(appId))
                        {
                            var app = (App)((await appsRepository.GetById(appId)).Object);

                            if (await passwordResetsRepository.HasOutstandingPasswordReset(userId, appId))
                            {
                                var passwordReset = (PasswordReset)
                                    ((await passwordResetsRepository.RetrievePasswordReset(userId, appId)).Object);

                                string EmailConfirmationAction;

                                if (app.UseCustomPasswordResetAction)
                                {
                                    if (app.InDevelopment)
                                    {
                                        EmailConfirmationAction = string.Format("{0}/{1}/{2}",
                                            app.DevUrl,
                                            app.CustomPasswordResetAction,
                                            passwordReset.Token);
                                    }
                                    else
                                    {
                                        EmailConfirmationAction = string.Format("{0}/{1}/{2}",
                                            app.LiveUrl,
                                            app.CustomPasswordResetAction,
                                            passwordReset.Token);
                                    }
                                }
                                else
                                {
                                    EmailConfirmationAction = string.Format("https://{0}/passwordReset/{1}",
                                        baseUrl,
                                        passwordReset.Token);
                                }

                                var html = File.ReadAllText(emailTamplatePath);
                                var appTitle = app.Name;
                                var url = string.Empty;

                                if (app.InDevelopment)
                                {
                                    url = app.DevUrl;
                                }
                                else
                                {
                                    url = app.LiveUrl;
                                }

                                html = html.Replace("{{USER_NAME}}", user.UserName);
                                html = html.Replace("{{CONFIRM_EMAIL_URL}}", EmailConfirmationAction);
                                html = html.Replace("{{APP_TITLE}}", appTitle);
                                html = html.Replace("{{URL}}", url);

                                var emailSubject = string.Format("Greetings from {0}: Password Update Request Received", appTitle);

                                result.Success = emailService
                                    .Send(user.Email, emailSubject, html);

                                if (result.Success)
                                {
                                    result.Message = UsersMessages.PasswordResetEmailResentMessage;

                                    return result;
                                }
                                else
                                {
                                    result.Success = false;
                                    result.Message = UsersMessages.PasswordResetEmailNotResentMessage;

                                    return result;
                                }
                            }
                            else
                            {
                                result.Success = false;
                                result.Message = UsersMessages.NoOutstandingRequestToResetPassworMessage;

                                return result;
                            }
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = AppsMessages.AppNotFoundMessage;

                            return result;
                        }
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = UsersMessages.NoOutstandingRequestToResetPassworMessage;

                        return result;
                    }

                }
                else
                {
                    result.Success = false;
                    result.Message = UsersMessages.UserNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<IUserResult> CancelEmailConfirmationRequest(int id, int appId)
        {
            var result = new UserResult();

            try
            {
                if (await usersRepository.HasEntity(id))
                {
                    if (await appsRepository.HasEntity(appId))
                    {
                        if (await emailConfirmationsRepository.HasOutstandingEmailConfirmation(id, appId))
                        {
                            var user = (User)(await usersRepository.GetById(id)).Object;
                            var emailConfirmation = (EmailConfirmation)(await emailConfirmationsRepository.RetrieveEmailConfirmation(id, appId)).Object;

                            var response = await emailConfirmationsRepository.Delete(emailConfirmation);

                            if (response.Success)
                            {
                                // Role back email request
                                user.Email = emailConfirmation.OldEmailAddress;
                                user.ReceivedRequestToUpdateEmail = false;
                                user.EmailConfirmed = true;

                                result.User = (User)(await usersRepository.Update(user)).Object;
                                result.Success = response.Success;
                                result.Message = UsersMessages.EmailConfirmationRequestCancelledMessage;

                                return result;
                            }
                            else if (response.Success == false && response.Exception != null)
                            {
                                result.User = (User)(await usersRepository.Update(user)).Object;
                                result.Success = response.Success;
                                result.Message = response.Exception.Message;

                                return result;
                            }
                            else
                            {
                                result.User = (User)(await usersRepository.Update(user)).Object;
                                result.Success = false;
                                result.Message = UsersMessages.EmailConfirmationRequestNotCancelledMessage;

                                return result;
                            }
                        }
                        else
                        {
                            result.User = (User)(await usersRepository.GetById(id)).Object;
                            result.Success = false;
                            result.Message = UsersMessages.EmailConfirmationRequestNotFoundMessage;

                            return result;
                        }
                    }
                    else
                    {
                        result.User = (User)(await usersRepository.GetById(id)).Object;
                        result.Success = false;
                        result.Message = AppsMessages.AppNotFoundMessage;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = UsersMessages.UserNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<IUserResult> CancelPasswordResetRequest(int id, int appId)
        {
            var result = new UserResult();

            try
            {
                if (await usersRepository.HasEntity(id))
                {
                    if (await appsRepository.HasEntity(appId))
                    {
                        if (await passwordResetsRepository.HasOutstandingPasswordReset(id, appId))
                        {
                            var user = (User)(await usersRepository.GetById(id)).Object;
                            var passwordReset = (PasswordReset)(await passwordResetsRepository.RetrievePasswordReset(id, appId)).Object;

                            var response = await passwordResetsRepository.Delete(passwordReset);

                            if (response.Success)
                            {
                                // Role back password reset
                                user.ReceivedRequestToUpdatePassword = false;

                                result.User = (User)(await usersRepository.Update(user)).Object;
                                result.Success = response.Success;
                                result.Message = UsersMessages.PasswordResetRequestCancelledMessage;

                                return result;
                            }
                            else if (response.Success == false && response.Exception != null)
                            {
                                result.User = (User)(await usersRepository.Update(user)).Object;
                                result.Success = response.Success;
                                result.Message = response.Exception.Message;

                                return result;
                            }
                            else
                            {
                                result.User = (User)(await usersRepository.Update(user)).Object;
                                result.Success = false;
                                result.Message = UsersMessages.PasswordResetRequestNotCancelledMessage;

                                return result;
                            }
                        }
                        else
                        {
                            result.User = (User)(await usersRepository.GetById(id)).Object;
                            result.Success = false;
                            result.Message = UsersMessages.PasswordResetRequestNotFoundMessage;

                            return result;
                        }
                    }
                    else
                    {
                        result.User = (User)(await usersRepository.GetById(id)).Object;
                        result.Success = false;
                        result.Message = AppsMessages.AppNotFoundMessage;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = UsersMessages.UserNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<IUserResult> CancelAllEmailRequests(int id, int appId)
        {
            var result = new UserResult();

            try
            {
                if (await usersRepository.HasEntity(id))
                {
                    if (await appsRepository.HasEntity(appId))
                    {
                        var emailConfirmationExists = await emailConfirmationsRepository.HasOutstandingEmailConfirmation(id, appId);
                        var passwordResetExists = await passwordResetsRepository.HasOutstandingPasswordReset(id, appId);
                        var user = (User)(await usersRepository.GetById(id)).Object;

                        if (emailConfirmationExists || passwordResetExists)
                        {
                            if (emailConfirmationExists)
                            {
                                if (await emailConfirmationsRepository.HasOutstandingEmailConfirmation(id, appId))
                                {
                                    var emailConfirmation = (EmailConfirmation)(await emailConfirmationsRepository.RetrieveEmailConfirmation(id, appId)).Object;

                                    var response = await emailConfirmationsRepository.Delete(emailConfirmation);

                                    if (response.Success)
                                    {
                                        // Role back email request
                                        user.Email = emailConfirmation.OldEmailAddress;
                                        user.ReceivedRequestToUpdateEmail = false;
                                        user.EmailConfirmed = true;

                                        user = (User)(await usersRepository.Update(user)).Object;
                                        result.Success = response.Success;
                                        result.Message = UsersMessages.EmailConfirmationRequestCancelledMessage;
                                    }
                                    else if (response.Success == false && response.Exception != null)
                                    {
                                        result.Success = response.Success;
                                        result.Message = response.Exception.Message;
                                    }
                                    else
                                    {
                                        result.Success = false;
                                        result.Message = UsersMessages.EmailConfirmationRequestNotCancelledMessage;
                                    }
                                }
                                else
                                {
                                    result.Success = false;
                                    result.Message = UsersMessages.EmailConfirmationRequestNotFoundMessage;
                                }
                            }

                            if (passwordResetExists)
                            {
                                var passwordReset = (PasswordReset)(await passwordResetsRepository.RetrievePasswordReset(id, appId)).Object;

                                var response = await passwordResetsRepository.Delete(passwordReset);

                                if (response.Success)
                                {
                                    // Role back password reset
                                    user.ReceivedRequestToUpdatePassword = false;

                                    user = (User)(await usersRepository.Update(user)).Object;
                                    result.Success = response.Success;
                                    result.Message = string.IsNullOrEmpty(result.Message) ?
                                        UsersMessages.PasswordResetRequestCancelledMessage :
                                        string.Format("{0} and {1}", result.Message, UsersMessages.PasswordResetRequestCancelledMessage);
                                }
                                else if (response.Success == false && response.Exception != null)
                                {
                                    result.Success = result.Success ? result.Success : response.Success;
                                    result.Message = string.IsNullOrEmpty(result.Message) ?
                                        response.Exception.Message :
                                        string.Format("{0} and {1}", result.Message, response.Exception.Message);
                                }
                                else
                                {
                                    result.Success = false;
                                    result.Message = string.IsNullOrEmpty(result.Message) ?
                                        UsersMessages.PasswordResetRequestNotCancelledMessage :
                                        string.Format("{0} and {1}", result.Message, UsersMessages.PasswordResetRequestNotCancelledMessage);
                                }
                            }

                            result.User = user;
                            return result;
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = UsersMessages.EmailRequestsNotFoundMessage;

                            return result;
                        }
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = AppsMessages.AppNotFoundMessage;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = UsersMessages.UserNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Message = exp.Message;

                return result;
            }
        }

        private async Task<EmailConfirmation> EnsureEmailConfirmationTokenIsUnique(EmailConfirmation emailConfirmation)
        {
            var emailConfirmationResposnse = await emailConfirmationsRepository.GetAll();

            if (emailConfirmationResposnse.Success)
            {
                bool tokenNotUnique;

                var emailConfirmations = emailConfirmationResposnse
                    .Objects
                    .ConvertAll(ec => (EmailConfirmation)ec);

                do
                {
                    if (emailConfirmations
                        .Any(ec => ec.Token.ToLower()
                        .Equals(emailConfirmation.Token.ToLower())))
                    {
                        tokenNotUnique = true;

                        emailConfirmation.Token = Guid.NewGuid().ToString();
                    }
                    else
                    {
                        tokenNotUnique = false;
                    }

                } while (tokenNotUnique);
            }

            return emailConfirmation;
        }

        private async Task<PasswordReset> EnsurePasswordResetTokenIsUnique(PasswordReset passwordReset)
        {
            var passwordResetResponse = await passwordResetsRepository.GetAll();

            if (passwordResetResponse.Success)
            {
                bool tokenUnique;

                var passwordResets = passwordResetResponse
                    .Objects
                    .ConvertAll(pu => (PasswordReset)pu);

                do
                {
                    if (passwordResets
                        .Where(pw => pw.Id != passwordReset.Id)
                        .ToList()
                        .Count > 0)
                    {
                        if (passwordResets
                            .Where(pw => pw.Id != passwordReset.Id)
                            .Any(pw => pw.Token.ToLower().Equals(passwordReset.Token.ToLower())))
                        {
                            tokenUnique = false;

                            passwordReset.Token = Guid.NewGuid().ToString();
                        }
                        else
                        {
                            tokenUnique = true;
                        }
                    }
                    else
                    {
                        passwordReset.Token = Guid.NewGuid().ToString();

                        tokenUnique = true;
                    }

                } while (!tokenUnique);
            }
            else
            {
                if (passwordReset.Id != 0)
                {
                    passwordReset.Token = Guid.NewGuid().ToString();
                }
            }

            return passwordReset;
        }

        private BaseResult SendPasswordResetEmail(
            User user,
            App app,
            PasswordReset passwordReset,
            string emailTemplatePath,
            string baseUrl,
            BaseResult result,
            bool newRequest)
        {
            string EmailConfirmationAction;

            if (app.UseCustomPasswordResetAction)
            {
                if (app.InDevelopment)
                {
                    EmailConfirmationAction = string.Format("{0}/{1}/{2}",
                        app.DevUrl,
                        app.CustomPasswordResetAction,
                        passwordReset.Token);
                }
                else
                {
                    EmailConfirmationAction = string.Format("{0}/{1}/{2}",
                        app.LiveUrl,
                        app.CustomPasswordResetAction,
                        passwordReset.Token);
                }
            }
            else
            {
                EmailConfirmationAction = string.Format("https://{0}/passwordReset/{1}",
                    baseUrl,
                    passwordReset.Token);
            }

            var html = File.ReadAllText(emailTemplatePath);
            var appTitle = app.Name;
            string url;

            if (app.InDevelopment)
            {
                url = app.DevUrl;
            }
            else
            {
                url = app.LiveUrl;
            }

            html = html.Replace("{{USER_NAME}}", user.UserName);
            html = html.Replace("{{CONFIRM_EMAIL_URL}}", EmailConfirmationAction);
            html = html.Replace("{{APP_TITLE}}", appTitle);
            html = html.Replace("{{URL}}", url);

            var emailSubject = string.Format("Greetings from {0}: Password Update Request Received", appTitle);

            result.Success = emailService
                .Send(user.Email, emailSubject, html);

            if (result.Success)
            {
                if (newRequest)
                {
                    result.Message = UsersMessages.ProcessedPasswordResetRequestMessage;
                }
                else
                {
                    result.Message = UsersMessages.ResentPasswordResetRequestMessage;
                }

                return result;
            }
            else
            {
                result.Message = UsersMessages.UnableToProcessPasswordResetRequesMessage;

                return result;
            }
        }
        #endregion
    }
}
