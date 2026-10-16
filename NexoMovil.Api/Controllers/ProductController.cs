using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NexoMovil.Core.DTOs;
using NexoMovil.Core.Entities;
using NexoMovil.Core.Interfaces;

namespace NexoMovil.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductController(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        // GET: api/product/dto/mapper
        [HttpGet("dto/mapper")]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productRepository.GetAllAvailableAsync();
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
            return Ok(productDtos);
        }

        // GET: api/product/dto/mapper/1
        [HttpGet("dto/mapper/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productRepository.GetById(id);
            if (product == null)
                return NotFound(new { message = "Producto no encontrado." });

            return Ok(_mapper.Map<ProductDto>(product));
        }

        // POST: api/product/dto/mapper
        [HttpPost("dto/mapper")]
        public async Task<IActionResult> Insert([FromBody] ProductDto newProductDto)
        {
            var product = _mapper.Map<Product>(newProductDto);

            await _productRepository.Add(product);
            await _productRepository.SaveChangesAsync();

            var createdDto = _mapper.Map<ProductDto>(product);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, createdDto);
        }

        // PUT: api/product/dto/mapper/1
        [HttpPut("dto/mapper/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductDto productDto)
        {
            if (id != productDto.Id)
                return BadRequest(new { message = "El id del producto no coincide." });

            var product = await _productRepository.GetById(id);
            if (product == null)
                return NotFound(new { message = "Producto no encontrado." });

            _mapper.Map(productDto, product);
            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync();

            return Ok(_mapper.Map<ProductDto>(product));
        }

        // DELETE: api/product/dto/mapper/1
        [HttpDelete("dto/mapper/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetById(id);
            if (product == null)
                return NotFound(new { message = "Producto no encontrado." });

            await _productRepository.Delete(id);
            await _productRepository.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Product/ingreso-inventario
        [HttpPost("ingreso-inventario")]
        public async Task<IActionResult> IngresoInventario([FromBody] InventoryIngressDto dto)
        {
            // Validación manual (RN-03: la cantidad debe ser mayor a cero)
            if (dto.Quantity <= 0)
                return BadRequest("La cantidad debe ser mayor a cero.");

            // Buscar el producto en la base de datos
            var product = await _productRepository.GetById(dto.ProductId);

            // Si no existe, retornar 404 (Flujo alternativo 3a del CU-01)
            if (product == null)
                return NotFound("Producto no encontrado.");

            // Incrementar el stock
            product.Stock += dto.Quantity;

            // Persistir el cambio (Update es void, luego SaveChangesAsync)
            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync();

            // Confirmar éxito (Flujo principal paso 6)
            return Ok(new
            {
                message = "Stock actualizado correctamente.",
                productId = product.Id,
                nuevoStock = product.Stock
            });
        }
    }
}