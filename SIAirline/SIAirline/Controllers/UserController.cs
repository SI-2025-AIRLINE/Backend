using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
using SIAirline.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UserController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/User
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        return await _context.Users.ToListAsync();
    }

    // GET: api/User/5
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return user;
    }

    // POST: api/User
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<User>> CreateUser(User user)
    {
        // Hash password before storing
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

        // Generate verification token
        user.VerificationToken = Guid.NewGuid().ToString();

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    // PUT: api/User/5
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateUser(int id, User user)
    {
        if (id != user.Id)
        {
            return BadRequest();
        }

        var existingUser = await _context.Users.FindAsync(id);
        if (existingUser == null)
        {
            return NotFound();
        }

        // Only update password if it's changed
        if (!string.IsNullOrEmpty(user.PasswordHash) &&
            user.PasswordHash != existingUser.PasswordHash)
        {
            existingUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
        }

        existingUser.FullName = user.FullName;
        existingUser.Email = user.Email;
        existingUser.Role = user.Role;
        existingUser.IsVerified = user.IsVerified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/User/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool UserExists(int id)
    {
        return _context.Users.Any(e => e.Id == id);
    }

    // Additional endpoints for user management

    // GET: api/User/admins
    [HttpGet("admins")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<User>>> GetAdmins()
    {
        return await _context.Users
            .Where(u => u.Role == UserRole.Admin)
            .ToListAsync();
    }

    // GET: api/User/customers
    [HttpGet("customers")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<User>>> GetCustomers()
    {
        return await _context.Users
            .Where(u => u.Role == UserRole.Customer)
            .ToListAsync();
    }

    // POST: api/User/verify/{token}
    [HttpPost("verify/{token}")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyUser(string token)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.VerificationToken == token);

        if (user == null)
        {
            return NotFound("Invalid verification token");
        }

        user.IsVerified = true;
        user.VerificationToken = string.Empty;

        await _context.SaveChangesAsync();

        return Ok("User verified successfully");
    }

    // POST: api/User/reset-password
    [HttpPost("reset-password")]
    [AllowAnonymous]
    public async Task<IActionResult> RequestPasswordReset(string email)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
        {
            // Don't reveal that the user doesn't exist
            return Ok("If your email is registered, you will receive a password reset link");
        }

        // Generate reset token
        user.ResetToken = Guid.NewGuid().ToString();

        await _context.SaveChangesAsync();

        // In a real application, you would send an email with the reset token

        return Ok("Password reset link has been sent to your email");
    }
}