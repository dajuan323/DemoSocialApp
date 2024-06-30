using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSocial.Application.Posts.CommandHandlers;

internal class AddInteractionCommandHandler(
    IDataContext _context,
	PostErrorMessages _errorMessages,
	IUnitOfWork _unitOfWork) : IRequestHandler<AddInteraction, OperationResult<PostInteraction>>
{
    public async Task<OperationResult<PostInteraction>> Handle(AddInteraction request, CancellationToken _)
    {
        OperationResult<PostInteraction> result = new();

		try
		{
			var post = await _context.Posts
				.Include(p => p.Interactions)
				.ThenInclude(p => p.UserProfile)
				.FirstOrDefaultAsync(p => p.PostId == request.PostId);

			if (post == null) 
			{
				result.AddError(
					ErrorCode.NotFound, _errorMessages.PostNotFound);
			}

			var interaction = PostInteraction.CreatePostInteraction(
				request.PostId,
				request.UserProfileId,
				request.InteractionType);

			post?.AddInteraction(interaction);

			_context.Posts.Update(post);
			await _unitOfWork.SaveChangesAsync(_);
		}
		catch (Exception e)
		{

			result.AddUnknownError($"{e.Message}");
		}

		return result;
    }
}
