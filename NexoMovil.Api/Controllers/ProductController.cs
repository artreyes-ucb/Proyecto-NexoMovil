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
    }
}