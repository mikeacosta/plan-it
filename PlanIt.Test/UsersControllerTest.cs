using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PlanIt.API.Controllers;
using PlanIt.API.Entities;
using PlanIt.API.Models;
using PlanIt.API.Profiles;
using PlanIt.API.Repositories;

namespace PlanIt.Test;

public class UsersControllerTest
{
    private UsersController _usersController;

    public UsersControllerTest()
    {
        var loggerMock = new Mock<ILogger<UsersController>>();
        
        var userRepositoryMock = new Mock<IUserRepository>();
        userRepositoryMock.Setup(m => m.GetUsersAsync())
            .ReturnsAsync(new List<User>()
            {
                new User("joeblow", "joe@blow.com"),
                new User("mickey.mouse", "mickey@disney.com"),
                new User("anonymous", "user@gmail.com")
            });

        // var mapperMock = new Mock<IMapper>();
        // mapperMock.Setup(m => 
        //         m.Map<User, UserWithoutExperiencesDto>(It.IsAny<User>()))
        //     .Returns(new UserWithoutExperiencesDto());

        var mapperConfiguration = new MapperConfiguration(
            cfg => cfg.AddProfile<UserProfile>());
        var mapper = new Mapper(mapperConfiguration);

        _usersController = new UsersController(loggerMock.Object, userRepositoryMock.Object, mapper);        
    }
    
    [Fact]
    public async Task GetUsers_GetAction_MustReturnOkObjectResult()
    {
        var result = await _usersController.GetUsers();

        var actionResult = Assert.IsType<ActionResult<IEnumerable<UserWithoutExperiencesDto>>>(result);
        Assert.IsType<OkObjectResult>(actionResult.Result);
    }
}