using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NexoMovil.Core.DTOs;
using NexoMovil.Core.Entities;
using NexoMovil.Core.Interfaces;

namespace NexoMovil.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly ISaleRepository _saleRepository;   
        private readonly IMapper _mapper;

        public SaleController(ISaleRepository saleRepository, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
        }

        // GET: api/sale/dto/mapper
        [HttpGet("dto/mapper")]
        public async Task<IActionResult> GetSalesDtoMapper()
        {
            var sales = await _saleRepository.GetAllWithDetailsAsync();  
            var saleDtos = _mapper.Map<IEnumerable<SaleResponseDto>>(sales);
            return Ok(saleDtos);
        }

        // GET: api/sale/dto/mapper/1
        [HttpGet("dto/mapper/{id}")]
        public async Task<IActionResult> GetSaleByIdDtoMapper(int id)
        {
            var sale = await _saleRepository.GetByIdWithDetailsAsync(id); 
            if (sale == null)
                return NotFound("Venta no encontrada.");

            return Ok(_mapper.Map<SaleResponseDto>(sale));
        }


        // POST: api/sale/dto/mapper
        [HttpPost("dto/mapper")]
        public async Task<IActionResult> InsertSaleDtoMapper(SaleRequestDto newSaleDto)
        {
            var sale = _mapper.Map<Sale>(newSaleDto);

            await _saleRepository.Add(sale);
            await _saleRepository.SaveChangesAsync();

            var saleResponse = _mapper.Map<SaleResponseDto>(sale);

            return Created($"api/sale/{sale.Id}", saleResponse);
        }

        // PUT: api/sale/dto/mapper/1
        [HttpPut("dto/mapper/{id}")]
        public async Task<IActionResult> UpdateSaleDtoMapper(int id, [FromBody] SaleRequestDto saleDto)
        {
            var sale = await _saleRepository.GetById(id);
            if (sale == null)
                return NotFound("Venta no encontrada.");

            _mapper.Map(saleDto, sale);
            _saleRepository.Update(sale);
            await _saleRepository.SaveChangesAsync();

            var saleResponse = _mapper.Map<SaleResponseDto>(sale);

            return Ok(saleResponse);
        }

        // DELETE: api/sale/dto/mapper/1
        [HttpDelete("dto/mapper/{id}")]
        public async Task<IActionResult> DeleteSaleDtoMapper(int id)
        {
            var sale = await _saleRepository.GetById(id);
            if (sale == null)
                return NotFound("Venta no encontrada.");

            await _saleRepository.Delete(id);
            await _saleRepository.SaveChangesAsync();

            return NoContent();
        }

    }
}