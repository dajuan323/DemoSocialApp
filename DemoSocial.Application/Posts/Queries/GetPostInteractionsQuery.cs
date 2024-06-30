using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSocial.Application.Posts.Queries;

public sealed record GetPostInteractionsQuery(
    Guid PostId) : IRequest<OperationResult<List<PostInteraction>>>;

