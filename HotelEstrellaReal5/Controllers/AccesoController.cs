using HotelEstrellaReal5.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

namespace HotelEstrellaReal5.Controllers
{
    public class AccesoController : Controller
    {
        static string cadena = "Data Source=DESKTOP-7CUCEV9\\SQLEXPRESS;Initial Catalog=HotelEstrellaReal5;Integrated Security=True;Trust Server Certificate=True";

        //Get: Login
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Registrarse()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registrarse(Usuario oUsuario, Cliente oCliente)
        {
            bool Registrado;
            string mensaje;

            // Verificar que las contraseñas coinciden
            if (oUsuario.Clave == oUsuario.ConfirmarClave)
            {
                // Encriptar la clave antes de almacenarla
                oUsuario.Clave = ConvertirSha256(oUsuario.Clave);
                oUsuario.ConfirmarClave = ConvertirSha256(oUsuario.ConfirmarClave);

            }
            else
            {
                ViewData["Mensaje"] = "Las Contraseñas no coinciden";
                return View();
            }

            // Establecer un rol predeterminado del usuario (por ejemplo, rol de cliente)
            oUsuario.IdRol = 2;
            oCliente.IdEstado = 1;

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmdCliente = new SqlCommand("sp_RegistrarCliente", cn);
                cmdCliente.Parameters.Add("@IdCliente", SqlDbType.Int).Value = oCliente.IdCliente; // El cliente ingresa su IdCliente
                cmdCliente.Parameters.Add("@Nombre", SqlDbType.VarChar).Value = oCliente.NombreCompleto;
                cmdCliente.Parameters.Add("@Telefono", SqlDbType.Char).Value = oCliente.Telefono;
                cmdCliente.Parameters.Add("@Correo", SqlDbType.VarChar).Value = oCliente.Email;
                cmdCliente.Parameters.Add("@FechaNacimiento", SqlDbType.Date).Value = oCliente.FechaNacimiento;
                cmdCliente.Parameters.Add("@IdEstado", SqlDbType.VarChar).Value = oCliente.IdEstado;
                cmdCliente.CommandType = CommandType.StoredProcedure;

                cn.Open();

                // Registrar al cliente
                cmdCliente.ExecuteNonQuery();

                // Ahora pasamos el IdCliente al registro del usuario
                SqlCommand cmd = new SqlCommand("sp_RegistrarUsuario", cn);
                cmd.Parameters.Add("@Nombre", SqlDbType.VarChar).Value = oCliente.NombreCompleto;
                cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = oCliente.Email;
                cmd.Parameters.Add("@Clave", SqlDbType.VarChar).Value = oUsuario.Clave;
                cmd.Parameters.Add("@ConfirmarClave", SqlDbType.VarChar).Value = oUsuario.ConfirmarClave;
                cmd.Parameters.Add("@IdCliente", SqlDbType.Int).Value = oCliente.IdCliente;  // Usamos el mismo IdCliente
                cmd.Parameters.Add("@Rol", SqlDbType.Int).Value = oUsuario.IdRol;
                cmd.Parameters.Add("@Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                // Registrar al usuario
                cmd.ExecuteNonQuery();

                Registrado = Convert.ToBoolean(cmd.Parameters["@Registrado"].Value);
                mensaje = cmd.Parameters["@Mensaje"].Value.ToString();

                if (Registrado)
                {
                    ViewData["Mensaje"] = "Usuario y Cliente Registrados Exitosamente";
                    return RedirectToAction("Login", "Acceso");
                }
                else
                {
                    ViewData["Mensaje"] = mensaje;
                    return View();
                }
            }
        }

        [HttpPost]
        public IActionResult Login(Usuario oUsuario, Cliente oCliente)
        {
            oUsuario.Clave = ConvertirSha256(oUsuario.Clave);

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();

                SqlCommand cmd = new SqlCommand("sp_ValidarUsuario", cn);
                cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = oUsuario.Email;
                cmd.Parameters.Add("@Clave", SqlDbType.VarChar).Value = oUsuario.Clave;
                cmd.CommandType = CommandType.StoredProcedure;

                object result = cmd.ExecuteScalar();
                oUsuario.IdUsuario = result != null ? Convert.ToInt32(result) : 0;
            }

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();

                SqlCommand cmdRol = new SqlCommand("sp_ObtenerRolUsuario", cn);
                cmdRol.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = oUsuario.IdUsuario;
                cmdRol.CommandType = CommandType.StoredProcedure;

                if (oUsuario.IdUsuario != 0)
                {
                    // 1. Obtener el nombre completo usando un procedimiento almacenado
                    SqlCommand cmdNombre = new SqlCommand("sp_OptenerNombreUsuario", cn);
                    cmdNombre.CommandType = CommandType.StoredProcedure;
                    cmdNombre.Parameters.Add("@Email", SqlDbType.VarChar).Value = oUsuario.Email;

                    object nombreCompletoResult = cmdNombre.ExecuteScalar();
                    if (nombreCompletoResult != null)
                    {
                        oUsuario.NombreCompleto = nombreCompletoResult.ToString();  // Asignamos el nombre completo
                    }

                    // 2. Obtener la URL de la foto de perfil desde la base de datos (suponiendo que el campo se llama FotoPerfilUrl)
                    SqlCommand cmdFotoPerfil = new SqlCommand("sp_ObtenerFotoPerfil", cn);
                    cmdFotoPerfil.CommandType = CommandType.StoredProcedure;
                    cmdFotoPerfil.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = oUsuario.IdUsuario;

                    //si el usuario no tiene una foto de perfil guardada se le asignara la imagen predeterminada
                    object fotoPerfilResult = cmdFotoPerfil.ExecuteScalar();
                    string profilePicture = fotoPerfilResult != null ? fotoPerfilResult.ToString() : "/Imagenes/img_pre_per_2.png"; // Imagen predeterminada

                    // 3. Crear los claims con el nombre completo y otros datos
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, oUsuario.Email),
                new Claim(ClaimTypes.Name, oUsuario.NombreCompleto),
            };

                    object rolResult = cmdRol.ExecuteScalar();
                    if (rolResult != null)
                    {
                        // Almacenar el rol en la sesión
                        HttpContext.Session.SetString("Rol", rolResult.ToString());
                    }

                    // Almacenar el usuario en la sesión
                    HttpContext.Session.SetString("Usuario", JsonConvert.SerializeObject(oUsuario));

                    // Guardar la URL de la imagen de perfil en la sesión
                    HttpContext.Session.SetString("UserProfilePicture", profilePicture);

                    // Iniciar sesión
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal).Wait();

                    // Redireccionar según el rol
                    var rol = HttpContext.Session.GetString("Rol");
                    if (rol == "1")
                    {
                        return RedirectToAction("Paginappal", "Inicio");
                    }
                    else if (rol == "2")
                    {
                        return RedirectToAction("PaginappalClientes", "Inicio");
                    }
                    else if (rol == "3")
                    {
                        return RedirectToAction("Dashboard", "Inicio");
                    }
                }
                else
                {
                    ViewData["Mensaje"] = "Usuario o contraseña incorrecta";
                    return View();
                }

                return View();
            }
        }

        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Remove("Usuario");
            return RedirectToAction("Login", "Acceso");
        }

        private string ConvertirSha256(string clave)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(clave));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

    }
}
