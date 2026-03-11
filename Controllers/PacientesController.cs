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
        public async Task<ActionResult<IEnumerable<Paciente>>> GetPaciente()
        {
            var pacientes = await _context.Pacientes.ToListAsync();
            return pacientes;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Paciente>> GetPaciente(int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null)
            {
                return NotFound();
            }
            return paciente;
        }

        [HttpPost]

        public async Task<ActionResult<Paciente>> PostPaciente(Paciente paciente)
        { 
            _context.Pacientes.Add(paciente);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPaciente), new { id = paciente.Id }, paciente);
        }

        [HttpPut("{id}")]

        public async Task<ActionResult> PutPaciente(int id, Paciente paciente)
        {
            if (id != paciente.Id)
            {
                return BadRequest();
            }
            _context.Entry(paciente).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(paciente);
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
