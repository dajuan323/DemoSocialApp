using DemoSocial.Domain.Aggregates.UserProfileAggregate;
using DemoSocial.Domain.Exceptions;
using DemoSocial.Domain.Validators.PostValidators;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSocial.Domain.Aggregates.PostAggregate;

public class Post
{
    private readonly List<PostComment> _comments = [];
    private readonly List<PostInteraction> _interactions = [];
    private Post()
    {
      
    }
    public Guid PostId { get; private set; }
    public Guid UserProfileId { get; private set; }
    public UserProfile UserProfile { get; private set; }
    public string TextContent { get; private set; }
    public DateTime DateCreated { get; private set; }
    public DateTime LastModified { get; private set; }
    public IEnumerable<PostComment> Comments { get { return _comments; } }
    public IEnumerable<PostInteraction> Interactions { get { return _interactions; } }


    /// <summary>
    /// Creates new post instance
    /// </summary>
    /// <param name="userProfileId">User profile Id</param>
    /// <param name="textContent">Post content</param>
    /// <returns><see cref="PostNotValidException"/></returns>

    public static Post CreatePost(Guid userProfileId, string textContent)
    {
        PostValidator validator = new();
        Post objectToValidate = new()
        {
            UserProfileId = userProfileId,
            TextContent = textContent,
            DateCreated = DateTime.UtcNow,
            LastModified = DateTime.UtcNow,

        };

        var validationResult = validator.Validate(objectToValidate);
        if (validationResult.IsValid) return objectToValidate;

        PostNotValidException exception = new("Post not valid");

        validationResult.Errors.ForEach(vr => exception.ValidationErrors.Add(vr.ErrorMessage));

        throw exception;
    }

    // Public methods

    /// <summary>
    /// Updates the post text
    /// </summary>
    /// <param name="newText">The updated post text</param>
    /// <exception cref="PostNotValidException"></exception>

    public void UpdatePostText(string newText)
    {
        if (string.IsNullOrWhiteSpace(newText))
        {
            PostNotValidException exception = new("Cannot update post" + "Post text is not valid.");

            exception.ValidationErrors.Add("the Provided text is either null or white space.");
            throw exception;
        }
        TextContent = newText;
        LastModified = DateTime.UtcNow;
    }

    public void AddPostComment(PostComment newComment)
    {
        _comments.Add(newComment);
    }

    public void RemoveComment(PostComment toRemove)
    {
        _comments.Remove(toRemove);
    }

    public void AddInteraction(PostInteraction newInteraction)
    {
        _interactions.Add(newInteraction);
    }

    public void RemoveInteraction(PostInteraction toRemove)
    {
        _interactions.Remove(toRemove);
    }
}
