using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PlanIt.API.Controllers;
using PlanIt.API.Entities;
using PlanIt.API.Profiles;
using PlanIt.API.Repositories;
using PlanIt.Models.DTOs;

namespace PlanIt.Test;

public class UserControllerTest
{
    private UserController _userController;

    public UserControllerTest()
    {
        var loggerMock = new Mock<ILogger<UserController>>();

        var pageSize = 10;
        var pageNumber = 1;

        var users = new List<User>()
        {
            new User("joeblow", "joe@blow.com"),
            new User("mickey.mouse", "mickey@disney.com"),
            new User("anonymous", "user@gmail.com")
        };
        var pageMetaData = new PageMetadata(users.Count, pageSize, pageNumber);

        var userRepositoryMock = new Mock<IUserRepository>();
        userRepositoryMock.Setup(m => m.GetUsersAsync(null, null, pageNumber, pageSize))
            .ReturnsAsync((users, pageMetaData));

        // var mapperMock = new Mock<IMapper>();
        // mapperMock.Setup(m => 
        //         m.Map<User, UserWithoutExperiencesDto>(It.IsAny<User>()))
        //     .Returns(new UserWithoutExperiencesDto());

        var mapperConfiguration = new MapperConfiguration(
            cfg => cfg.AddProfile<UserProfile>());
        var mapper = new Mapper(mapperConfiguration);

        _userController = new UserController(loggerMock.Object, userRepositoryMock.Object, mapper);  
        _userController.ControllerContext.HttpContext = new DefaultHttpContext();
    }
    
    [Fact]
    public async Task GetUsers_GetAction_MustReturnOkObjectResult()
    {
        var result = await _userController.GetUsers(null, null);

        var actionResult = Assert.IsType<ActionResult<IEnumerable<UserWithoutExperiencesDto>>>(result);
        Assert.IsType<OkObjectResult>(actionResult.Result);
    }
}