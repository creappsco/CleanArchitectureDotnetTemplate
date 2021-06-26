using Clean.Core.Application.Models.Authentication;
using Clean.Infraestructure.Identity.Models;
using Clean.Infraestructure.Identity.Services;
using IFC.Core.Application.Contracts.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Clean.Tests.UnitTests.Identity
{
    public class AuthenticationServiceTests
    {
        [Fact]
        public async Task Register_Should_Register_User_When_User_Not_Exists()
        {
            //Arrange
            var registeredUsers = new List<RegistrationRequest>
            {
                new RegistrationRequest
                {
                    Email = "email1@clean.co",
                    FirstName = "Unit",
                    LastName = "Test 1",
                    Password = "123pass"
                }
            };
            var user = new RegistrationRequest
            {
                Email = "email2@clean.co",
                FirstName = "Unit",
                LastName = "Test",
                Password = "123pass"
            };

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManager.Object.UserValidators.Add(new UserValidator<ApplicationUser>());
            userManager.Object.PasswordValidators.Add(new PasswordValidator<ApplicationUser>());
            userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                        .Callback((string email) => registeredUsers.Find(x => x.Email.Equals(email)));

            userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                        .Callback((string email) => registeredUsers.Find(x => x.Email.Equals(email)));

            userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                        .ReturnsAsync(IdentityResult.Success);

            userManager.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                        .Returns(Task.FromResult(IdentityResult.Success));

            var contextAccessor = new Mock<IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
            var signInManager = new Mock<SignInManager<ApplicationUser>>(userManager.Object, contextAccessor.Object, claimsFactory.Object, null, null, null, null);

            var jwtSettings = new Mock<IOptions<JwtSettings>>();
            var authServiceMock = new AuthenticationService(userManager.Object, signInManager.Object, jwtSettings.Object);
            //Act
            var response = await authServiceMock.RegisterAsync(user, "User");
            //Assert
            Assert.NotNull(response);
        }

        [Fact]
        public async Task Register_Should_ThrowError_When_User_Exists()
        {
            //Arrange
            var registeredUsers = new List<RegistrationRequest>
            {
                new RegistrationRequest
                {
                    Email = "email1@clean.co",
                    FirstName = "Unit",
                    LastName = "Test 1",
                    Password = "123pass"
                }
            };
            var user = new RegistrationRequest
            {
                Email = "email1@clean.co",
                FirstName = "Unit",
                LastName = "Test",
                Password = "123pass"
            };

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManager.Object.UserValidators.Add(new UserValidator<ApplicationUser>());
            userManager.Object.PasswordValidators.Add(new PasswordValidator<ApplicationUser>());
            userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                        .ReturnsAsync(new ApplicationUser { Email = registeredUsers.FirstOrDefault().Email });

            userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                        .Callback((string email) => registeredUsers.Find(x => x.Email.Equals(email)));

            userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                        .ReturnsAsync(IdentityResult.Success);

            userManager.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                        .Returns(Task.FromResult(IdentityResult.Success));

            var contextAccessor = new Mock<IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
            var signInManager = new Mock<SignInManager<ApplicationUser>>(userManager.Object, contextAccessor.Object, claimsFactory.Object, null, null, null, null);

            var jwtSettings = new Mock<IOptions<JwtSettings>>();
            var authServiceMock = new AuthenticationService(userManager.Object, signInManager.Object, jwtSettings.Object);
            //Act 
            //Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => authServiceMock.RegisterAsync(user, "User"));
            Assert.Equal("El usuario 'email1@clean.co' ya se encuentra registrado.", ex.Message);
        }


        [Fact]
        public async Task Authenticate_Should_ThrowError_When_Invalid_login_UserNotExist()
        {
            //Arrange
            var registeredUsers = new List<RegistrationRequest>
            {
                new RegistrationRequest
                {
                    Email = "email1@clean.co",
                    FirstName = "Unit",
                    LastName = "Test 1",
                    Password = "123pass"
                }
            };
            var user = new RegistrationRequest
            {
                Email = "email2@clean.co",
                FirstName = "Unit",
                LastName = "Test",
                Password = "123pass"
            };

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManager.Object.UserValidators.Add(new UserValidator<ApplicationUser>());
            userManager.Object.PasswordValidators.Add(new PasswordValidator<ApplicationUser>());
 
            userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                        .ReturnsAsync(IdentityResult.Success);

            userManager.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                        .Returns(Task.FromResult(IdentityResult.Success));

            var contextAccessor = new Mock<IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
            var signInManager = new Mock<SignInManager<ApplicationUser>>(userManager.Object, contextAccessor.Object, claimsFactory.Object, null, null, null, null);

            var jwtSettings = new Mock<IOptions<JwtSettings>>();
            var authServiceMock = new AuthenticationService(userManager.Object, signInManager.Object, jwtSettings.Object);
            var authRequest = new AuthenticationRequest
            {
                Email = user.Email,
                Password = user.Password
            };
            //Act 
            //Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => authServiceMock.AuthenticateAsync(authRequest));
            Assert.Equal("El Usuario con Email email2@clean.co no fué encontrado.", ex.Message);
        }
        [Fact]
        public async Task Authenticate_Should_ThrowError_When_Password_Invalid()
        {
            //Arrange
            var registeredUsers = new List<RegistrationRequest>
            {
                new RegistrationRequest
                {
                    Email = "email1@clean.co",
                    FirstName = "Unit",
                    LastName = "Test 1",
                    Password = "123pass"
                }
            };
            var user = new RegistrationRequest
            {
                Email = "email1@clean.co",
                FirstName = "Unit",
                LastName = "Test",
                Password = "123pass"
            };

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManager.Object.UserValidators.Add(new UserValidator<ApplicationUser>());
            userManager.Object.PasswordValidators.Add(new PasswordValidator<ApplicationUser>());

            userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                      .ReturnsAsync(new ApplicationUser { Email = registeredUsers.FirstOrDefault().Email });

            userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                        .ReturnsAsync(IdentityResult.Success);

            userManager.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                        .Returns(Task.FromResult(IdentityResult.Success));

            var contextAccessor = new Mock<IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
            var signInManager = new Mock<SignInManager<ApplicationUser>>(userManager.Object, contextAccessor.Object, claimsFactory.Object, null, null, null, null);
            signInManager.Setup(x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                        .ReturnsAsync(SignInResult.Failed);

            var jwtSettings = new Mock<IOptions<JwtSettings>>();
            var authServiceMock = new AuthenticationService(userManager.Object, signInManager.Object, jwtSettings.Object);
            var authRequest = new AuthenticationRequest
            {
                Email = user.Email,
                Password = user.Password
            };
            //Act 
            //Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => authServiceMock.AuthenticateAsync(authRequest));
            Assert.Equal("La información para 'email1@clean.co' no es válida.", ex.Message);
        }
    }
}
