using api.core.Data.Enums;
using api.core.Data.requests;
using api.core.Data.Responses;
using System.Collections.Generic;
using System;

namespace api.core.services.abstractions;

public interface IDraftEventService
{
    public EventResponseDTO AddDraftEvent(Guid userId, DraftEventRequestDTO request);

    public bool UpdateDraftEvent(Guid userId, Guid eventId, DraftEventRequestDTO request);
}
