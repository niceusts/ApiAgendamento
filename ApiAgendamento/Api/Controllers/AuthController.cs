using ApiAgendamento.Domain.Entities;
using ApiAgendamento.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;

namespace ApiAgendamento.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
            return BadRequest("E-mail já cadastrado");

        int? medicoId = null;
        int? pacienteId = null;

        if (dto.Tipo == "medico")
        {
            var medico = new Medico(dto.Email, ""); // Nome e especialidade podem ser ajustados depois
            _context.Medicos.Add(medico);
            await _context.SaveChangesAsync();
            medicoId = medico.Id;
        }
        else if (dto.Tipo == "paciente")
        {
            var paciente = new Paciente(dto.Email, ""); // Nome e email podem ser ajustados depois
            _context.Pacientes.Add(paciente);
            await _context.SaveChangesAsync();
            pacienteId = paciente.Id;
        }

        var usuario = new Usuario
        {
            Email = dto.Email,
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
            Tipo = dto.Tipo,
            MedicoId = medicoId,
            PacienteId = pacienteId
        };
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        return Ok("Usuário cadastrado com sucesso");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash))
            return Unauthorized("Usuário ou senha inválidos");

        // Buscar configurações JWT diretamente das variáveis de ambiente
        var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
        var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
        var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");

        if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
            throw new InvalidOperationException("JWT_KEY, JWT_ISSUER ou JWT_AUDIENCE não estão definidos nas variáveis de ambiente ou no .env.");

        var key = Encoding.ASCII.GetBytes(jwtKey);
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new Claim("email", usuario.Email),
            new Claim("tipo", usuario.Tipo),
            new Claim("medicoId", usuario.MedicoId?.ToString() ?? string.Empty),
            new Claim("pacienteId", usuario.PacienteId?.ToString() ?? string.Empty)
        };
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(8),
            Issuer = jwtIssuer,
            Audience = jwtAudience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return Ok(new
        {
            usuario.Id,
            usuario.Email,
            usuario.Tipo,
            usuario.MedicoId,
            usuario.PacienteId,
            token = tokenString
        });
    }
}
