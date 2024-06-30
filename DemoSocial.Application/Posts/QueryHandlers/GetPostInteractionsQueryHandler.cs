using DemoSocial.Application.Posts.Queries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.DataContracts;
using System.Text;
using System.Threading.Tasks;

namespace DemoSocial.Application.Posts.QueryHandlers;

internal class GetPostInteractionsQueryHandler(
    IDataContext _context,
	PostErrorMessages _errorMessages) : IRequestHandler<GetPostInteractionsQuery, OperationResult<List<PostInteraction>>>
{
	
    public async Task<OperationResult<List<PostInteraction>>> Handle(GetPostInteractionsQuery request, CancellationToken cancellationToken)
    {
         OperationResult<List<PostInteraction>> result = new();

		try
		{
			var post = await _context.Posts
				.Include(p=>p.Interactions)
				.ThenInclude(i => i.UserProfile)
				.FirstOrDefaultAsync(p => p.PostId == request.PostId, cancellationToken);

            if (post == null) result.AddError(ErrorCode.NotFound, _errorMessages.PostNotFound);

            result.Payload = post.Interactions.ToList();

        }
        catch (Exception e)
		{
			result.AddUnknownError($"{e.Message}"); ;
		}

		return result;
    }
}
