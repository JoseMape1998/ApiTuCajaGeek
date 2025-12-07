using ApiTuCajaGeek.AppData;
using ApiTuCajaGeek.DTOs;
using ApiTuCajaGeek.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiTuCajaGeek.Controllers
{
    [Authorize]
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

        // ==================== ROLES ====================

        [HttpGet("roles")]
        public async Task<IActionResult> ObtenerRoles()
        {
            _logger.LogInformation("Solicitud para obtener todos los roles");

            try
            {
                var roles = await _service.GetAllRolesAsync();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo roles");
                return StatusCode(500, new ErrorResponse
                {
                    Message = "Error interno al obtener roles",
                    Detail = ex.Message
                });
            }
        }

        [AllowAnonymous]
        [HttpGet("roles/{id:guid}")]
        public async Task<IActionResult> ObtenerRolPorId(Guid id)
        {
            _logger.LogInformation("Solicitud para obtener rol {Id}", id);

            try
            {
                var rol = await _service.GetRoleByIdAsync(id);

                if (rol == null)
                {
                    _logger.LogWarning("Rol {Id} no encontrado", id);
                    return NotFound(new { Message = "Rol no encontrado" });
                }

                return Ok(rol);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo rol {Id}", id);
                return StatusCode(500, new ErrorResponse
                {
                    Message = "Error interno al obtener rol",
                    Detail = ex.Message
                });
            }
        }

        [HttpPost("roles/crear")]
        public async Task<IActionResult> CrearRol([FromBody] CreateRoleDto dto)
        {
            _logger.LogInformation("Solicitud para crear rol: {Nombre}", dto.Nombre);

            if (string.IsNullOrWhiteSpace(dto.Nombre))
                return BadRequest(new { Message = "El nombre del rol es obligatorio" });

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
            _logger.LogInformation("Solicitud para editar rol {Id}", id);

            try
            {
                var updated = await _service.UpdateRoleAsync(id, dto);

                if (updated == null)
                {
                    _logger.LogWarning("Rol {Id} no encontrado para edición", id);
                    return NotFound(new { Message = "Rol no encontrado" });
                }

                return Ok(new { Message = "Rol actualizado correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error editando rol {Id}", id);
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
            _logger.LogInformation("Solicitud para eliminar rol {Id}", id);

            try
            {
                var ok = await _service.DeleteRoleAsync(id);

                if (!ok)
                {
                    _logger.LogWarning("Rol {Id} no encontrado para eliminación", id);
                    return NotFound(new { Message = "Rol no encontrado" });
                }

                return Ok(new { Message = "Rol eliminado correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando rol {Id}", id);
                return StatusCode(500, new ErrorResponse
                {
                    Message = "Error al eliminar rol",
                    Detail = ex.Message
                });
            }
        }

        // ==================== USUARIOS ====================

        [HttpGet("usuarios")]
        public async Task<IActionResult> ObtenerUsuarios()
        {
            _logger.LogInformation("Solicitud para obtener todos los usuarios");

            try
            {
                var usuarios = await _service.GetAllUsersAsync();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo usuarios");
                return StatusCode(500, new ErrorResponse
                {
                    Message = "Error interno al obtener usuarios",
                    Detail = ex.Message
                });
            }
        }

        [AllowAnonymous]
        [HttpGet("usuarios/{id:guid}")]
        public async Task<IActionResult> ObtenerUsuarioPorId(Guid id)
        {
            _logger.LogInformation("Solicitud para obtener usuario {Id}", id);

            try
            {
                var user = await _service.GetUserByIdAsync(id);

                if (user == null)
                {
                    _logger.LogWarning("Usuario {Id} no encontrado", id);
                    return NotFound(new { Message = "Usuario no encontrado" });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo usuario {Id}", id);
                return StatusCode(500, new ErrorResponse
                {
                    Message = "Error interno al obtener usuario",
                    Detail = ex.Message
                });
            }
        }

        [AllowAnonymous]
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
            _logger.LogInformation("Solicitud para editar usuario {Id}", id);

            try
            {
                var updated = await _service.UpdateUserAsync(id, dto);

                if (updated == null)
                {
                    _logger.LogWarning("Usuario {Id} no encontrado para edición", id);
                    return NotFound(new { Message = "Usuario no encontrado" });
                }

                return Ok(new { Message = "Usuario actualizado correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error editando usuario {Id}", id);
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
            _logger.LogInformation("Solicitud para inactivar usuario {Id}", id);

            try
            {
                var ok = await _service.InactivateUserAsync(id);

                if (!ok)
                {
                    _logger.LogWarning("Usuario {Id} no encontrado para inactivar", id);
                    return NotFound(new { Message = "Usuario no encontrado" });
                }

                return Ok(new { Message = "Usuario inactivado correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inactivando usuario {Id}", id);
                return StatusCode(500, new ErrorResponse
                {
                    Message = "Error al inactivar usuario",
                    Detail = ex.Message
                });
            }
        }
    }
}
