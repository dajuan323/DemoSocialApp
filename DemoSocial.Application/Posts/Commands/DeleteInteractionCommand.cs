using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSocial.Application.Posts.Commands;

public sealed record DeleteInteractionCommand(
    Guid PostId,
    Guid InteractionId,
    Guid UserProfileId) : IRequest<OperationResult<PostInteraction>>;
