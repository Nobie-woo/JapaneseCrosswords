using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using AuthApi.Data;
using AuthApi.Models;
using AuthApi.DTOs;

namespace AuthApi.Controllers
{

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
private readonly AppDbContext _db;
private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();
public AuthController(AppDbContext db){
  _db = db;
}

[HttpPost("register")]

public async Task<IActionResult> Register([FromBody] RegisterRequest req){

  if (string.IsNullOrWhiteSpace(req.Username) || string.IsNullOrWhiteSpace(req.Password)){
    var error = new ApiResponse<RegisterResponse>{
      Succeeded = false,
      Message = "Username и Password обязательны",
      Data = new RegisterResponse { Success = false, Message = "Username и Password обязательны" }
    };
    return BadRequest(error);
  }

  if (await _db.Users.AnyAsync(u => u.Username == req.Username)){
    var error = new ApiResponse<RegisterResponse>{
      Succeeded = false,
      Message = "Пользователь с таким именем уже есть",
      Data = new RegisterResponse { Success = false, Message = "Пользователь с таким именем уже есть" }
    };
    return BadRequest(error);
  }

  var user = new User { Username = req.Username, CreatedAt = DateTime.UtcNow };
  user.PasswordHash = _passwordHasher.HashPassword(user, req.Password);
  _db.Users.Add(user);
  await _db.SaveChangesAsync();
  var successData = new RegisterResponse { UserId = user.Id, Success = true, Message = "Пользователь зарегистрирован" };
  var ok = new ApiResponse<RegisterResponse> { Succeeded = true, Message = "Пользователь зарегистрирован", Data = successData };
  return Ok(ok);
}

[HttpPost("login")]

public async Task<IActionResult> Login([FromBody] LoginRequest req){
  if (string.IsNullOrWhiteSpace(req.Username) || string.IsNullOrWhiteSpace(req.Password)){
    var resp = new ApiResponse<LoginResponse>{
      Succeeded = false,
      Message = "Username и Password обязательны",
      Data = new LoginResponse { Success = false, Message = "Username и Password обязательны" }
    };
    return BadRequest(resp);
  }

  var user = await _db.Users.SingleOrDefaultAsync(u => u.Username == req.Username);

  if (user == null){
    var resp = new ApiResponse<LoginResponse>{
      Succeeded = false,
      Message = "Неверные учетные данные",
      Data = new LoginResponse { Success = false, Message = "Неверные учетные данные" }
    };
    return Unauthorized(resp);
  }

  var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, req.Password);

  if (result == PasswordVerificationResult.Failed){
    var resp = new ApiResponse<LoginResponse>{
      Succeeded = false,
      Message = "Неверные учетные данные",
      Data = new LoginResponse { Success = false, Message = "Неверные учетные данные" }
    };
    return Unauthorized(resp);
  }

  var loginData = new LoginResponse { UserId = user.Id, Success = true, Message = "Успешный вход" };
  var ok = new ApiResponse<LoginResponse> { Succeeded = true, Message = "Успешный вход", Data = loginData };
  return Ok(ok);
}
}
}