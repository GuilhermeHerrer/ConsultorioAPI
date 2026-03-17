using ConsultorioAPI.Data;
using ConsultorioAPI.Models;
using ConsultorioAPI.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConsultorioAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultoriosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ViaCepService _viaCepService;

        public ConsultoriosController(AppDbContext context, ViaCepService viaCepService)
        {
            _context = context;
            _viaCepService = viaCepService;
        }

        [HttpGet]
        public async Task<ActionResult> GetConsultorio()
        {
            var consultorios = await _context.Consultorios.ToListAsync();
            return Ok(consultorios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetConsultorioId(int id)
        {
            var consultorio = await _context.Consultorios.FindAsync(id);
            if (consultorio == null)
            {
                return NotFound();
            }
            return Ok(consultorio);
        }

        [HttpPost]
        public async Task<ActionResult> PostConsultorio(Consultorio consultorio)
        {
            if(string.IsNullOrEmpty(consultorio.Cep) || consultorio.Cep.Length != 8)
            {
                return BadRequest("O campo CEP é obrigatório.");
            }
            var endereco = await _viaCepService.AcharEnderecoAsync(consultorio.Cep);
            if (endereco == null)
            {
                return NotFound("CEP não encontrado.");
            }

            consultorio.Logradouro = endereco.Logradouro;
            consultorio.Bairro = endereco.Bairro;
            consultorio.Localidade = endereco.Localidade;
            consultorio.Uf = endereco.Uf;
            _context.Consultorios.Add(consultorio);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetConsultorio), new { id = consultorio.Id }, consultorio);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutConsultorio(int id, Consultorio consultorio)
        {
            if (id != consultorio.Id)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var enderecoExistente = await _context.Consultorios.FindAsync(id);

            if (enderecoExistente == null)
            {
                return NotFound();
            }
            if(string.IsNullOrEmpty(consultorio.Cep) || consultorio.Cep.Length != 8)
            {
                return BadRequest("O campo CEP é obrigatório.");
            }
            else
            {
                var enderecoViaCep = await _viaCepService.AcharEnderecoAsync(consultorio.Cep);
                enderecoExistente.Logradouro = enderecoViaCep.Logradouro;
                enderecoExistente.Bairro = enderecoViaCep.Bairro;
                enderecoExistente.Localidade = enderecoViaCep.Localidade;
                enderecoExistente.Uf = enderecoViaCep.Uf;
            }
            enderecoExistente.Nome = consultorio.Nome;
            enderecoExistente.Numero = consultorio.Numero;

            _context.Update(enderecoExistente);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteConsultorio(int id) 
        {             
            var consultorio = await _context.Consultorios.FindAsync(id);
            if (consultorio == null)
            {
                return NotFound();
            }
            _context.Consultorios.Remove(consultorio);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
