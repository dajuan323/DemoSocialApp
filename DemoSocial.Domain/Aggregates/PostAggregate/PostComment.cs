namespace DemoSocial.Domain.Aggregates.PostAggregate;

public class PostComment
{
    private PostComment()
    {
        
    }
    public Guid CommentId { get; private set; }
    public Guid PostId { get; private set; }
    public string Text { get; private set; }
    public Guid UserProfileId { get; private set; }
    public DateTime DateCreated { get; private set; }
    public DateTime LastModified { get; private set; }

    //Factories
    /// <summary>
    ///   Creates a post comment
    /// </summary>
    /// <param name="postId">The ID of the post to which the comment belongs</param> 
    /// <param name="Text">Text content of the comment</param>
    /// <param name="userProfileId">The ID of the user who created the comment</param>
    /// <returns><see cref="PostComment"/></returns>
    /// 

    public static PostComment CreatePostComment(Guid postId, string Text, Guid userProfileId)
    {
        PostCommentValidator validator = new();
        PostComment objectToValidate = new()
        {
            PostId = postId,
            Text = Text,
            UserProfileId = userProfileId,
            DateCreated = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        };

        var validationResult = validator.Validate(objectToValidate);

        if (validationResult.IsValid) return objectToValidate;

        PostCommentNotValidException exception = new("Post comment is not valid.");

        validationResult.Errors.ForEach(vr => exception.ValidationErrors.Add(vr.ErrorMessage));

        throw exception;
    }

    //public methods

    public void UpdatePostComment(string updatedText)
    {
        if (string.IsNullOrWhiteSpace(updatedText))
        {
            PostNotValidException exception = new("Cannot update post" + "Post text is not valid.");

            exception.ValidationErrors.Add("the Provided text is either null or white space.");
            throw exception;
        }
        Text = updatedText;
        LastModified = DateTime.UtcNow;
    }
}
