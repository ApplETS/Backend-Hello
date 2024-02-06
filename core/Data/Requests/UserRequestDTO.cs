﻿namespace api.core.Data.requests;

public class UserRequestDTO
{
    public required string Name { get; set; }

    public required string Email { get; set; }

    public string? Organisation { get; set; } = null!;

    public string? ActivityArea { get; set; } = null!;
}
