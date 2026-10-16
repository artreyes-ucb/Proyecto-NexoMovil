using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NexoMovil.Core.DTOs;
using NexoMovil.Core.Entities;
using NexoMovil.Core.Interfaces;

namespace NexoMovil.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        // GET: api/User
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userRepository.GetAll();
            var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
            return Ok(userDtos);
        }

        // GET: api/User/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userRepository.GetById(id);
            if (user == null) return NotFound();

            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }

        // POST: api/User
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto createUserDto)
        {
            var existing = await _userRepository.GetByEmailAsync(createUserDto.Email);
            if (existing != null) return Conflict("El correo ya está registrado.");

            var user = _mapper.Map<User>(createUserDto);
            user.IsActive = true;

            await _userRepository.Add(user);
            await _userRepository.SaveChangesAsync();

            var userDto = _mapper.Map<UserDto>(user);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, userDto);
        }

        // PUT: api/User/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateUserDto updateUserDto)
        {
            var user = await _userRepository.GetById(id);
            if (user == null) return NotFound();

            user.FirstName = updateUserDto.FirstName;
            user.LastName = updateUserDto.LastName;
            user.Email = updateUserDto.Email;
            user.Telephone = updateUserDto.Telephone;
            user.Role = updateUserDto.Role;

            if (!string.IsNullOrEmpty(updateUserDto.Password))
            {
                user.Password = updateUserDto.Password;
            }

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            return NoContent();
        }

        // PATCH: api/User/{id}/activar
        [HttpPatch("{id}/activar")]
        public async Task<IActionResult> Activate(int id)
        {
            var user = await _userRepository.GetById(id);
            if (user == null) return NotFound();

            user.IsActive = true;
            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/User/{id}  (baja lógica)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userRepository.GetById(id);
            if (user == null) return NotFound();

            // Baja lógica: no se elimina físicamente, solo se desactiva (RN-04)
            user.IsActive = false;
            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            return NoContent();
        }

    }
}