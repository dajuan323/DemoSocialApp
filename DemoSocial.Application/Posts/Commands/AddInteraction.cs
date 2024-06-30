using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSocial.Application.Posts.Commands;

public sealed record AddInteraction(
    Guid PostId,
    Guid UserProfileId,
    InteractionType InteractionType) : IRequest<OperationResult<PostInteraction>>;
