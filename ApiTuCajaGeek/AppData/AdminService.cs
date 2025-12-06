using ApiTuCajaGeek.Data;
using ApiTuCajaGeek.DTOs;
using ApiTuCajaGeek.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ApiTuCajaGeek.AppData
{
    public class AdminService
    {
        private readonly ApiTuCajaGeekContext _context;
        private readonly PasswordHasher<Users> _passwordHasher;

        public AdminService(ApiTuCajaGeekContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<Users>();
        }

        public async Task<Rol> CreateRoleAsync(CreateRoleDto dto)
        {
            if (await _context.Rol.AnyAsync(r => r.Name_Rol == dto.Nombre))
                throw new Exception("El rol ya existe.");

            var rol = new Rol
            {
                Name_Rol = dto.Nombre
            };

            _context.Rol.Add(rol);
            await _context.SaveChangesAsync();
            return rol;
        }

        public async Task<Rol?> UpdateRoleAsync(Guid id, CreateRoleDto dto)
        {
            var rol = await _context.Rol.FindAsync(id);
            if (rol == null) return null;

            rol.Name_Rol = dto.Nombre;
            await _context.SaveChangesAsync();

            return rol;
        }

        public async Task<bool> DeleteRoleAsync(Guid id)
        {
            var rol = await _context.Rol.FindAsync(id);
            if (rol == null) return false;

            _context.Rol.Remove(rol);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Users> CreateUserAsync(CreateUserDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email_user == dto.Email_user))
                throw new Exception("El correo ya está registrado.");

            if (!await _context.Rol.AnyAsync(r => r.Rol_id == dto.Rol_Id))
                throw new Exception("El rol asignado no existe.");

            var newUser = new Users
            {
                Name_user = dto.Name_user,
                LastName_user = dto.LastName_user,
                Email_user = dto.Email_user,
                Cell_number_user = dto.Cell_number_user,
                User_state = true,
                Rol_id = dto.Rol_Id
            };

            newUser.Password_hash = _passwordHasher.HashPassword(newUser, dto.Password);

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return newUser;
        }

        public async Task<Users?> UpdateUserAsync(Guid id, CreateUserDto dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;

            if (!await _context.Rol.AnyAsync(r => r.Rol_id == dto.Rol_Id))
                throw new Exception("El rol asignado no existe.");

            user.Name_user = dto.Name_user;
            user.LastName_user = dto.LastName_user;
            user.Cell_number_user = dto.Cell_number_user;
            user.Rol_id = dto.Rol_Id;

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                user.Password_hash = _passwordHasher.HashPassword(user, dto.Password);
            }

            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> InactivateUserAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            user.User_state = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
