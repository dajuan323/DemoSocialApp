using DemoSocial.Application.Posts.CommandHandlers;
using DemoSocial.Application.Posts.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.Posts.Commands;

public class CreatePostCommandHandlerTests
{
    [Fact]
    public void Handle_Should_ReturnFailureResult_WhenEmailIsNotUnique()
    {
        // Arrange
        var testUserProfileId = Guid.NewGuid();
        var command = new CreatePostCommand(testUserProfileId, "Test Text");


        // Act

        // Assert
    }
}
