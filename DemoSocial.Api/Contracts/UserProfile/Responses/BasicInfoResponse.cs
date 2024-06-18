﻿namespace DemoSocial.Api.Contracts.UserProfile.Responses;

public record BasicInfoResponse
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateTime DateOfBirth { get;  set; }
    public string CurrentCity { get;  set; }
}
