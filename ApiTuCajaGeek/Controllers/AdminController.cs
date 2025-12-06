using ApiTuCajaGeek.AppData;
using ApiTuCajaGeek.DTOs;
using ApiTuCajaGeek.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiTuCajaGeek.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly AdminService _service;
        private readonly ILogger<AdminController> _logger;

        public AdminController(AdminService service, ILogger<AdminController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost("roles/crear")]
        public async Task<IActionResult> CrearRol([FromBody] CreateRoleDto dto)
        {
            _logger.LogInformation("Solicitud para crear rol: {Nombre}", dto.Nombre);

            try
            {
                var rol = await _service.CreateRoleAsync(dto);

                return Ok(new
                {
                    Message = "Rol creado exitosamente",
                    RolId = rol.Rol_id
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando rol: {Nombre}", dto.Nombre);
                return BadRequest(new ErrorResponse
                {
                    Message = "No se pudo crear el rol",
                    Detail = ex.Message
                });
            }
        }

        [HttpPut("roles/editar/{id:guid}")]
        public async Task<IActionResult> EditarRol(Guid id, [FromBody] CreateRoleDto dto)
        {
            try
            {
                var updated = await _service.UpdateRoleAsync(id, dto);

                if (updated == null)
                    return NotFound(new { Message = "Rol no encontrado" });

                return Ok(new { Message = "Rol actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "Error al actualizar rol",
                    Detail = ex.Message
                });
            }
        }

        [HttpDelete("roles/eliminar/{id:guid}")]
        public async Task<IActionResult> EliminarRol(Guid id)
        {
            var ok = await _service.DeleteRoleAsync(id);

            if (!ok)
                return NotFound(new { Message = "Rol no encontrado" });

            return Ok(new { Message = "Rol eliminado correctamente" });
        }

        [HttpPost("usuarios/crear")]
        public async Task<IActionResult> CrearUsuario([FromBody] CreateUserDto dto)
        {
            _logger.LogInformation("Solicitud para crear usuario: {Email}", dto.Email_user);

            try
            {
                var user = await _service.CreateUserAsync(dto);

                return Ok(new
                {
                    Message = "Usuario creado exitosamente",
                    UserId = user.User_Id
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando usuario: {Email}", dto.Email_user);

                return BadRequest(new ErrorResponse
                {
                    Message = "No se pudo crear el usuario",
                    Detail = ex.Message
                });
            }
        }

        [HttpPut("usuarios/editar/{id:guid}")]
        public async Task<IActionResult> EditarUsuario(Guid id, [FromBody] CreateUserDto dto)
        {
            try
            {
                var updated = await _service.UpdateUserAsync(id, dto);

                if (updated == null)
                    return NotFound(new { Message = "Usuario no encontrado" });

                return Ok(new { Message = "Usuario actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "Error al actualizar usuario",
                    Detail = ex.Message
                });
            }
        }

        [HttpPut("usuarios/inactivar/{id:guid}")]
        public async Task<IActionResult> InactivarUsuario(Guid id)
        {
            var ok = await _service.InactivateUserAsync(id);

            if (!ok)
                return NotFound(new { Message = "Usuario no encontrado" });

            return Ok(new { Message = "Usuario inactivado correctamente" });
        }
    }
}
