using System;
namespace AuthApi.DTOs
{
public class LoginResponse
{
public bool Success { get; set; }
public string Message { get; set; } = string.Empty;
public Guid? UserId { get; set; }
}
}