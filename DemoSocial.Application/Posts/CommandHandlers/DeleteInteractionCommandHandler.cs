using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSocial.Application.Posts.CommandHandlers;

internal class DeleteInteractionCommandHandler(
    IDataContext _context,
    PostErrorMessages _errorMessages,
    IUnitOfWork _unitOfWork) : IRequestHandler<DeleteInteractionCommand, OperationResult<PostInteraction>>
{
    public async Task<OperationResult<PostInteraction>> Handle(DeleteInteractionCommand request, CancellationToken cancellationToken)
    {
        OperationResult<PostInteraction> result = new();

        try
        {
            Post? post = await _context.Posts
                .Include(p => p.Interactions)
                .FirstOrDefaultAsync(p => p.PostId == request.PostId, cancellationToken);

            if (post is null)
            {
                result.AddError(ErrorCode.NotFound, 
                    string.Format(_errorMessages.PostNotFound, request.PostId));
                return result;
            }


            PostInteraction? interaction = post.Interactions.FirstOrDefault(i
                    => i.InteractionId == request.InteractionId);

            if (interaction is null)
            {
                result.AddError(ErrorCode.NotFound, 
                    string.Format(_errorMessages.InteractionsNotFound, request.InteractionId));
                return result;
            }

            if (interaction.UserProfileId != request.UserProfileId)
            {
                result.AddError(ErrorCode.InteractionRemovalNotAuthorized,
                    _errorMessages.RemovalNotAuthorized);
                return result;
            }

            post.RemoveInteraction(interaction);
            _context.Posts.Update(post);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            result.Payload = interaction;
        }
        catch (Exception e)
        {

            result.AddUnknownError($"{e.Message}");
        }
        

        return result;
    }
}
