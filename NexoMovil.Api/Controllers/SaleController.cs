using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NexoMovil.Core.DTOs;
using NexoMovil.Core.Entities;
using NexoMovil.Core.Interfaces;

namespace NexoMovil.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaleController : ControllerBase
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public SaleController(
            ISaleRepository saleRepository,
            IProductRepository productRepository,
            IMapper mapper)
        {
            _saleRepository = saleRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        // GET: api/Sale
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sales = await _saleRepository.GetAll();
            var saleDtos = _mapper.Map<IEnumerable<SaleDto>>(sales);
            return Ok(saleDtos);
        }

        // GET: api/Sale/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var sale = await _saleRepository.GetById(id);
            if (sale == null) return NotFound();

            var saleDto = _mapper.Map<SaleDto>(sale);
            return Ok(saleDto);
        }

        // POST: api/Sale
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSaleDto createSaleDto)
        {
            // Validación básica de entrada
            if (createSaleDto.Quantity <= 0)
                return BadRequest("La cantidad debe ser mayor a cero.");

            // Buscar el producto
            var product = await _productRepository.GetById(createSaleDto.ProductId);
            if (product == null)
                return NotFound("Producto no encontrado.");

            // RN-01: Verificar stock disponible
            if (product.Stock < createSaleDto.Quantity)
                return BadRequest($"Stock insuficiente. Disponible: {product.Stock}.");

            // RN-02: Calcular el monto total automáticamente
            var totalAmount = product.Price * createSaleDto.Quantity;

            // Crear la venta
            var sale = new Sale
            {
                ProductId = createSaleDto.ProductId,
                UserId = createSaleDto.UserId,
                Date = DateTime.Now,
                IsActive = true,
                Quantity = createSaleDto.Quantity,
                TotalAmount = totalAmount
            };

            // Descontar stock del producto
            product.Stock -= createSaleDto.Quantity;
            _productRepository.Update(product);

            await _saleRepository.Add(sale);
            await _saleRepository.SaveChangesAsync();

            var saleDto = _mapper.Map<SaleDto>(sale);
            return CreatedAtAction(nameof(GetById), new { id = sale.Id }, saleDto);
        }

        // PATCH: api/Sale/{id}/anular  →  baja lógica (RN-04)
        [HttpPatch("{id}/anular")]
        public async Task<IActionResult> Anular(int id)
        {
            var sale = await _saleRepository.GetById(id);
            if (sale == null) return NotFound();

            if (!sale.IsActive)
                return BadRequest("La venta ya está anulada.");

            // Baja lógica: no se borra físicamente
            sale.IsActive = false;

            // Devolver el stock al producto (porque la venta se anula)
            var product = await _productRepository.GetById(sale.ProductId);
            if (product != null)
            {
                product.Stock += sale.Quantity;
                _productRepository.Update(product);
            }

            _saleRepository.Update(sale);
            await _saleRepository.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Sale/{id}  →  baja lógica (RN-04)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var sale = await _saleRepository.GetById(id);
            if (sale == null) return NotFound();

            // Cambiamos la eliminación física por baja lógica (RN-04)
            sale.IsActive = false;

            // Devolvemos el stock al producto
            var product = await _productRepository.GetById(sale.ProductId);
            if (product != null)
            {
                product.Stock += sale.Quantity;
                _productRepository.Update(product);
            }

            _saleRepository.Update(sale);
            await _saleRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}