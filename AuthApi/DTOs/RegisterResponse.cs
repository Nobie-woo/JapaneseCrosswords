using System;
namespace AuthApi.DTOs
{
public class RegisterResponse
{
public bool Success { get; set; }
public string Message { get; set; } = string.Empty;
public Guid? UserId { get; set; }
}
}