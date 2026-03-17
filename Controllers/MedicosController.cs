using ConsultorioAPI.Data;
using ConsultorioAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;


namespace ConsultorioAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MedicosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetMedico()
        {
            var medicos = await _context.Medicos
                .Select(m => new { m.Id, m.Nome, m.Crm, m.ConsultorioId, NomeConsultorio = m.Consultorio.Nome, CepConsultorio = m.Consultorio.Cep })
                .ToListAsync();
            return Ok(medicos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMedicoById(int id)
        {
            var medico = await _context.Medicos
                .Select(m => new {m.Id, m.Nome, m.Crm, m.ConsultorioId ,NomeConsultorio = m.Consultorio.Nome, CepConsultorio = m.Consultorio.Cep })
                .FirstAsync(m => m.Id == id);
            if (medico == null) { return BadRequest("Médico não encontrado."); }
            return Ok(medico);
        }

        [HttpPost]
        public  async Task<IActionResult> PostMedico(Medico medico)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await _context.Medicos.AnyAsync(m => m.Crm == medico.Crm))
            {
                return BadRequest("Este CRM já foi cadastrado.");
            }
            if (!await _context.Consultorios.AnyAsync(c => c.Id == medico.ConsultorioId))
            {
                return BadRequest("Consultório não encontrado.");
            }
            _context.Medicos.Add(medico);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMedico), new { id = medico.Id }, medico);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedico(Medico medico, int id)
        {
            if (medico.Id != id)
            {
                return BadRequest("O Id não pertence a nenhum médico.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var medicoExistente = await _context.Medicos.FindAsync(id);

            if (medicoExistente == null) 
            {
                return NotFound();
            }
            if (await _context.Medicos.AnyAsync(m => m.Crm == medico.Crm && m.Id != id))
            {
                return BadRequest("Este CRM já foi cadastrado.");
            }
            if (!await _context.Consultorios.AnyAsync(c => c.Id == medico.ConsultorioId))
            {
                return BadRequest("Consultório não encontrado.");
            }
            medicoExistente.Nome = medico.Nome;
            medicoExistente.Crm = medico.Crm;
            medicoExistente.ConsultorioId = medico.ConsultorioId;

            _context.Update(medicoExistente);
            await _context.SaveChangesAsync();
            return Ok(medicoExistente);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedico(int id)
        {
            var medico = await _context.Medicos.FindAsync(id);
            if (medico == null)
            {
                return NotFound();
            }
            _context.Medicos.Remove(medico);
            await _context.SaveChangesAsync();
            return Ok(new {aviso = "Médico removido com exito.", medicoRemovido = medico.Nome});
        }
    }
}
