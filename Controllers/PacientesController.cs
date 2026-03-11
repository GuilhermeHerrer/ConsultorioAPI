using ConsultorioAPI.Data;
using ConsultorioAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConsultorioAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacientesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public PacientesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetPaciente()
        {
            var pacientes = await _context.Pacientes.ToListAsync();
            return Ok(pacientes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetPacienteId(int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null)
            {
                return NotFound();
            }
            return Ok(paciente);
        }

        [HttpPost]

        public async Task<ActionResult> PostPaciente(Paciente paciente)
        { 
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await _context.Pacientes.AnyAsync(p => p.Email == paciente.Email || p.CPF == paciente.CPF))
            {
                return BadRequest("Email ou CPF já cadastrado.");
            }
            _context.Pacientes.Add(paciente);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPaciente), new { id = paciente.Id }, paciente);
        }

        [HttpPut("{id}")]

        public async Task<ActionResult> PutPaciente(int id, Paciente paciente)
        {
            if (id != paciente.Id)
            {
                return BadRequest("ID do paciente não correponde ao ID da URL");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pacienteExistente = await _context.Pacientes.FindAsync(id);

            if (pacienteExistente == null)
            {
                return NotFound();
            }
            if (await _context.Pacientes.AnyAsync(p => p.Email == paciente.Email || p.CPF == paciente.CPF))
            {
                return BadRequest("Email ou CPF já cadastrado.");
            }

            pacienteExistente.Nome = paciente.Nome;
            pacienteExistente.Email = paciente.Email;
            pacienteExistente.CPF = paciente.CPF;

            _context.Update(pacienteExistente);
            await _context.SaveChangesAsync();
            return Ok(pacienteExistente);
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult> DeletePaciente(int id) 
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null)
            {
                return NotFound();
            }
            _context.Pacientes.Remove(paciente);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
