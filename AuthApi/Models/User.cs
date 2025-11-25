using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AuthApi.Models
{
public class User
{
[Key]
[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
public Guid Id { get; set; } = Guid.NewGuid();
public User(){
  CreatedAt = DateTime.UtcNow;
  Score = 0;
}
[Required]
public string Username { get; set; } = string.Empty;
[Required]
public string PasswordHash { get; set; } = string.Empty;
public int Score { get; set; } = 0;
public DateTime CreatedAt { get; set; }
}
}