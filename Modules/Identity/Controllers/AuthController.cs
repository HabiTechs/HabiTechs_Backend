// --- USINGS CORREGIDOS ---
using HabiTechs.Modules.Identity.DTOs;
using HabiTechs.Modules.Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

// --- NAMESPACE CORREGIDO ---
namespace HabiTechs.Modules.Identity.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly TokenService _tokenService;

    public AuthController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        TokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _tokenService = tokenService;
    }

    [HttpPost("init-roles")]
    [AllowAnonymous] 
    public async Task<IActionResult> InitRoles()
    {
        string[] roleNames = { "Admin", "Guardia", "Residente" };
        foreach (var roleName in roleNames)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
        return Ok("Roles 'Admin', 'Guardia', 'Residente' creados o ya existentes.");
    }

    [HttpPost("register")]
    [AllowAnonymous] 
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        if (!await _roleManager.RoleExistsAsync("Residente"))
        {
            return BadRequest("Error: El rol 'Residente' no existe. Ejecute /init-roles primero.");
        }
        
        var user = new IdentityUser { UserName = registerDto.Email, Email = registerDto.Email };
        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        await _userManager.AddToRoleAsync(user, "Residente");
        return Ok(new { Message = $"Usuario {user.Email} registrado como Residente." });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null)
        {
            return Unauthorized("Email o contraseña inválidos");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
        if (!result.Succeeded)
        {
            return Unauthorized("Email o contraseña inválidos");
        }

        var roles = await _userManager.GetRolesAsync(user);
        var token = _tokenService.CreateToken(user, roles);

        return Ok(new
        {
            Email = user.Email,
            Roles = roles,
            Token = token
        });
    }

    [HttpPost("assign-guard-role")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignGuardRole([FromBody] string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return NotFound("Usuario no encontrado");
        }
        
        await _userManager.RemoveFromRoleAsync(user, "Residente");
        await _userManager.AddToRoleAsync(user, "Guardia");

        return Ok($"Usuario {email} es ahora un Guardia.");
    }
}