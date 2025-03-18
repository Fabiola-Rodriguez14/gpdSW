using gpdSW.Helpers;
using gpdSW.Models.Entities;
using gpdSW.Models.ViewModels.AccountVM;
using gpdSW.repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Net;

namespace gpdSW.Controllers
{
    public class Account : Controller
    {
        CheeseandmoreContext context;
        Repository<Usuarios> repositoryUsuarios;

        public Account()
        {
            context = new CheeseandmoreContext();
            repositoryUsuarios = new Repository<Usuarios>(context);
        }

        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registro(RegistroViewModel vm)
        {
            if (string.IsNullOrWhiteSpace(vm.Nombre))
                ModelState.AddModelError("", "Ingrese su nombre de usuario.");
            else if (string.IsNullOrWhiteSpace(vm.Correo))
                ModelState.AddModelError("", "Ingrese su correo.");
            else if (repositoryUsuarios.GetAll().Any(x => x.Correo.ToLower() == vm.Correo.ToLower())) // --
                ModelState.AddModelError("", "El correo ingresado ya se encuentra registrado.");
            else if (string.IsNullOrWhiteSpace(vm.Contraseña))
                ModelState.AddModelError("", "Ingrese su contraseña.");
            else if (!Regex.IsMatch(vm.Contraseña, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$"))
                ModelState.AddModelError("", "La contraseña debe tener al menos 8 caracteres, una minúscula, una mayúscula y un número.");
            // --
            else if (string.IsNullOrWhiteSpace(vm.RepetirContraseña))
                ModelState.AddModelError("", "Repita la contraseña.");
            else if (vm.Contraseña != vm.RepetirContraseña)
                ModelState.AddModelError("", "Las contraseñas no coinciden, intente de nuevo.");

            if (ModelState.IsValid)
            {
                Usuarios user = new Usuarios
                {
                    NombreUsuario = vm.Nombre,
                    Correo = vm.Correo,
                    Contrasena = Encriptacion.Encriptar(vm.Contraseña),
                    Rol = 2 // cliente por defecto
                };

                repositoryUsuarios.Insert(user);

                return RedirectToAction("RegistroCompleto");
            }
            else
                return View(vm);
        }

        public IActionResult RegistroCompleto()
        {
            return View();
        }

        // --

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel vm)
        {
            if (string.IsNullOrWhiteSpace(vm.Correo))
                ModelState.AddModelError("", "Ingrese su correo.");
            if (string.IsNullOrWhiteSpace(vm.Contraseña))
                ModelState.AddModelError("", "Ingrese su contraseña.");

            if (ModelState.IsValid)
            {
                var user = repositoryUsuarios.GetAll().FirstOrDefault(x => x.Correo == vm.Correo && x.Contrasena == Encriptacion.Encriptar(vm.Contraseña));

                if (user == null)
                {
                    ModelState.AddModelError("", "El nombre o contraseña son incorrectos");
                    return View(vm);
                }

                // Claims
                List<Claim> claims =
                [
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Nombre", user.NombreUsuario),
                    new Claim(ClaimTypes.Email, user.Correo),
                    new Claim("Rol", user.Rol.ToString()),
                ];

                ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal principal = new(identity);

                this.HttpContext.SignInAsync(principal);

                // Redireccionar según su rol 
                if (user.Rol == 2)
                    return RedirectToAction("Index", "Home", new { area = "User" }); // user
                else if (user.Rol == 1)
                    return RedirectToAction("Index", "Home", new { area = "Admin" }); // admin
                else
                    return RedirectToAction("Index", "Home");
            }
            return View(vm);
        }

        // --

        [HttpGet]
        public IActionResult RestablecerContraseña()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult RestablecerContraseña(RestContViewModel vm)
        //{
        //    if (string.IsNullOrWhiteSpace(vm.Correo))
        //        ModelState.AddModelError("", "Ingrese su correo, por favor.");
        //    else if (repositoryUsuarios.GetAll().Any(x => x.Correo.ToLower() == vm.Correo.ToLower()))
        //    {
        //        // Generar un OTP
        //        string otpGenerado = "12345"; // Simulación del código OTP

        //        TempData["OTP"] = otpGenerado;  // Guardar el OTP temporalmente
        //        TempData["Correo"] = vm.Correo; // Guardar temporalmente el correo

        //        return RedirectToAction("ValidarOTP");
        //    }
        //    else
        //        ModelState.AddModelError("", "El correo ingresado no se encuentra registrado.");

        //    return View();
        //}

        [HttpPost]
        public IActionResult RestablecerContraseña(RestContViewModel vm)
        {
            if (string.IsNullOrWhiteSpace(vm.Correo))
            {
                ModelState.AddModelError("", "Ingrese su correo, por favor.");
                return View();
            }

            var usuario = repositoryUsuarios.GetAll().FirstOrDefault(x => x.Correo.ToLower() == vm.Correo.ToLower());
            if (usuario == null)
            {
                ModelState.AddModelError("", "El correo ingresado no se encuentra registrado.");
                return View();
            }

            Random random = new Random();
            string otpGenerado = random.Next(100000, 999999).ToString();

            TempData["OTP"] = otpGenerado;
            TempData["Correo"] = vm.Correo;

            try
            {
                EnviarCorreo(vm.Correo, "Código de Restablecimiento", $"Su código OTP es: {otpGenerado}");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al enviar el correo: {ex.InnerException?.Message ?? ex.Message}");
                return View();
            }



            return RedirectToAction("ValidarOTP");
        }

        private void EnviarCorreo(string destino, string asunto, string mensaje)
        {
            using (var smtp = new SmtpClient("smtp.gmail.com", 587)) 
            {
                smtp.Credentials = new NetworkCredential("chesseandmore99@gmail.com", "zaes vetn msss twoo");
                smtp.EnableSsl = true; 


                //Mensaje sin formato
                //var correo = new MailMessage
                //{
                //    From = new MailAddress("chesseandmore99@gmail.com"),
                //    Subject = asunto,
                //    Body = mensaje,
                //    IsBodyHtml = false
                //};

                var correo = new MailMessage
{
    From = new MailAddress("chesseandmore99@gmail.com"),
    Subject = asunto,
    Body = $@"
        <html>
        <head>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    background-color: #f4f4f4;
                    margin: 0;
                    padding: 20px;
                }}
                .container {{
                    max-width: 600px;
                    background: white;
                    padding: 20px;
                    border-radius: 10px;
                    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                    text-align: center;
                }}
                h2 {{
                    color: #333;
                }}
                p {{
                    font-size: 16px;
                    color: #555;
                    line-height: 1.5;
                }}
                .footer {{
                    margin-top: 20px;
                    font-size: 12px;
                    color: #777;
                    text-align: center;
                }}
                .logo {{
                    width: 150px;
                    margin-bottom: 15px;
                }}
            </style>
        </head>
        <body>
            <div class='container'>
                <img src='https://i.postimg.cc/s2L6SKQS/LogoSw.png class='logo' alt='Logo de Chesse & More'>
                <h2>{asunto}</h2>
                <p>{mensaje}</p>
                <div class='footer'>
                    <p>&copy; {DateTime.Now.Year} Chesse & More. Todos los derechos reservados.</p>
                </div>
            </div>
        </body>
        </html>",
    IsBodyHtml = true
};
                correo.To.Add(destino);
                smtp.Send(correo);
            }
        }



        [HttpGet]
        public IActionResult ValidarOTP()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ValidarOTP(RestContViewModel vm)
        {
            if (string.IsNullOrWhiteSpace(vm.OTP.ToString()))
                ModelState.AddModelError("", "Ingrese el código enviado a su correo.");

            if (ModelState.IsValid)
            {
                string otpIngresado = string.Join("", vm.OTP);

                if (otpIngresado == TempData["OTP"].ToString())
                    return RedirectToAction("CambiarContraseña");
                else
                    return RedirectToAction("ValidacionFallida");
            }
            return View();
        }

        [HttpGet]
        public IActionResult CambiarContraseña()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CambiarContraseña(RegistroViewModel vm)
        {
            if (string.IsNullOrWhiteSpace(vm.Contraseña))
                ModelState.AddModelError("", "Ingrese su nueva contraseña.");
            else if (!Regex.IsMatch(vm.Contraseña, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$"))
                ModelState.AddModelError("", "La contraseña debe tener al menos 8 caracteres, una minúscula, una mayúscula y un número.");
            // --
            else if (string.IsNullOrWhiteSpace(vm.RepetirContraseña))
                ModelState.AddModelError("", "Repita la nueva contraseña.");
            else if (vm.Contraseña != vm.RepetirContraseña)
                ModelState.AddModelError("", "Las contraseñas no coinciden, intente de nuevo.");

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            string correo = TempData["Correo"]?.ToString();

            if (string.IsNullOrEmpty(correo))
            {
                ModelState.AddModelError("", "Correo no encontrado.");
            }

            var user = repositoryUsuarios.GetAll().FirstOrDefault(x => x.Correo.ToLower() == correo.ToLower());

            if (user != null)
            {
                // Actualizar la contraseña del usuario
                user.Contrasena = Encriptacion.Encriptar(vm.Contraseña);
                repositoryUsuarios.Update(user);

                return RedirectToAction("ValidacionCorrecta");
            }
            else
            {
                ModelState.AddModelError("", "Usuario no encontrado.");
                return View(vm);
            }
        }

        public IActionResult ValidacionCorrecta()
        {
            return View();
        }
        public IActionResult ValidacionFallida()
        {
            return View();
        }

        // --

        public IActionResult CerrarSesion()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}

