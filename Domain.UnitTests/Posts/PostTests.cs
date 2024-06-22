using DemoSocial.Domain.Aggregates.PostAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Domain.UnitTests.Posts;

public class PostTests
{
    // [ThingUnderTest]_Should_[ExpectedResult]_[Conditions]
    [Fact]
    public void CreatePost_Should_ReturnNull_WhenValueIsNull()
    {
        // Arrange
        string? value = null;
        Guid id = Guid.NewGuid();

        // Act
        var post = Post.CreatePost(id, value);

        // Assert
        
    }
}
