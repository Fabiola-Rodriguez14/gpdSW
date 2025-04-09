using gpdSW.Models.Entities;
using gpdSW.Models.ViewModels;
using gpdSW.repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;



using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using gpdSW.Models.PayPal_Order;
using System.Globalization;
using System.Text.RegularExpressions;

namespace gpdSW.Controllers
{
    public class HomeController : Controller
    {
        CheeseandmoreContext context;
        Repository<Productos> productosRepository;
        Repository<Recomendaciones> recomendacionesRepository;
        Repository<Carrito> carritoRepository;
        Repository<Pedido> pedidoRepository;
        Repository<Detallepedido> detallePedidoRepository;


        public HomeController()
        {
            context = new CheeseandmoreContext();
            productosRepository = new(context);
            recomendacionesRepository = new(context);
            carritoRepository = new(context);
            pedidoRepository = new(context);
            detallePedidoRepository = new(context);
        }


        public IActionResult Index(string xd)
        {
            CheeseandmoreContext context = new CheeseandmoreContext();
            ReseñasViewModel vm = new();

            vm.listitaresenas = context.Recomendaciones.Select(x => new resenenonas
            {
                Id = x.Id,
                Descripcion = x.Descripcion,
                Estrellas = x.Estrellas,
                NombreUsuario = x.IdUsuariosNavigation.NombreUsuario

            });

            return View(vm);
        }


        //////////////////////////////////////////////  CATEGORIAS
        public IActionResult Catalogo(int? id, string? terminoBusqueda)
        {
            Random rnd = new Random();
            CheeseandmoreContext context = new CheeseandmoreContext();


            if (terminoBusqueda != null)
            {
                CatalogoViewModel vm = new CatalogoViewModel()
                {
                    listaproducttos = context.Productos.Where(x => x.Nombre.Contains(terminoBusqueda)).Select(x => new poductoslist
                    {
                        Id = x.Id,
                        IdCategoria = x.IdCategoria,
                        Nombre = x.Nombre,
                        Precio = x.Precio,
                        Stock = x.Stock,
                        Preciopromocion = x.Preciopromocion
                    }).AsEnumerable() // Traer los datos a memoria
                .OrderBy(x => rnd.Next()), // Orden aleatorio en memoria


                    listacategorias = context.Catalogo.Select(x => new categCatalogo
                    {
                        Id = x.Id,
                        Categorias = x.Categorias,
                    })
                };
                return View(vm);
            }
            if (id == null)
            {
                CatalogoViewModel vm = new CatalogoViewModel()
                {
                    listaproducttos = context.Productos.Select(x => new poductoslist
                    {
                        Id = x.Id,
                        IdCategoria = x.IdCategoria,
                        Nombre = x.Nombre,
                        Precio = x.Precio,
                        Stock = x.Stock,
                        Preciopromocion = x.Preciopromocion
                    }).AsEnumerable() // Traer los datos a memoria
                .OrderBy(x => rnd.Next()), // Orden aleatorio en memoria


                    listacategorias = context.Catalogo.Select(x => new categCatalogo
                    {
                        Id = x.Id,
                        Categorias = x.Categorias,
                    })
                };
                return View(vm);
            }
            else
            {
                CatalogoViewModel vm = new CatalogoViewModel()
                {
                    listaproducttos = context.Productos.Where(x => x.IdCategoria == id).Select(x => new poductoslist
                    {
                        Id = x.Id,
                        IdCategoria = x.IdCategoria,
                        Nombre = x.Nombre,
                        Precio = x.Precio,
                        Stock = x.Stock,
                        Preciopromocion = x.Preciopromocion
                    }).AsEnumerable()
                .OrderBy(x => rnd.Next()),


                    listacategorias = context.Catalogo.Select(x => new categCatalogo
                    {
                        Id = x.Id,
                        Categorias = x.Categorias,
                    })
                };
                return View(vm);
            }

        }



        public IActionResult Productos(int id)
        {

            CheeseandmoreContext context = new CheeseandmoreContext();

            var datos = context.Productos.Where(x => x.Id == id).Select(x => x.Id);
            if (datos == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                var vm = context.Productos.Where(x => x.Id == id).Select(x => new infoProductos
                {
                    Id = x.Id,
                    Nombre = x.Nombre,
                    IdCategoria = x.IdCategoriaNavigation.Categorias,
                    catid = x.IdCategoria,
                    Precio = x.Precio,
                    Stock = x.Stock,
                    Descripcion = x.Descripcion,
                    Preciopromocion = x.Preciopromocion

                }).FirstOrDefault();

                if (vm == null)
                {
                    return NotFound();
                }
                return View(vm);
            }



        }



        public IActionResult VerProductoView(int? id)
        {
            CheeseandmoreContext context = new CheeseandmoreContext();
            var vm = context.Productos.Where(x => x.Id == id).Select(x => new infoProductos
            {
                Id = x.Id,
                Precio = x.Precio,
                Stock = x.Stock,
                Descripcion = x.Descripcion,
                Preciopromocion = x.Preciopromocion

            }).FirstOrDefault();


            return View(vm);
        }



        /////////////////////////////////////////////////////  EVENTOS
        public IActionResult Eventos()
        {
            CheeseandmoreContext context = new CheeseandmoreContext();
            EventosViewModel vm = new EventosViewModel();

            vm.listaeventos = context.Eventos.Select(x => new eventitos
            {
                Id = x.Id,
                Nombre = x.Nombre
            });
            return View(vm);
        }



        public IActionResult Detalleseventosv2(int id)
        {
            CheeseandmoreContext context = new CheeseandmoreContext();


            var vm = context.Detalleseventosv2.Where(x => x.IdEvento == id).Select(x => new VerEventosViewModel
            {
                Id = x.Id,
                Descripcion = x.Descripcion,

                Nombre = x.Nombre,

                IdEvento = x.IdEvento
            })
                  .FirstOrDefault();

            if (vm == null)
            {
                return NotFound();
            }

            return View(vm);
        }



        /////////////////////////////////////////////////////  EVENTOS
        [HttpGet]
        public IActionResult Contacto()
        {

            return View();
        }



        [HttpPost]
        public IActionResult Contacto(ContactoViewModel vm)
        {

            return View();
        }

        /////////////////////////////////////////////////////  Reseñas
        [HttpGet]
        public IActionResult Reseñas()
        {

            return View();
        }



        [HttpPost]
        public IActionResult Reseñas(ReseñasViewModel vm)
        {
            var idusuario = User.FindFirst("Id")?.Value;
            var resenaexi = recomendacionesRepository.GetAll().Where(x => x.IdUsuarios == Convert.ToInt32(idusuario)).Count();
            ModelState.Clear();
            if (string.IsNullOrWhiteSpace(vm.Descripcion))
            {
                ModelState.AddModelError("", "LA *Descripcion* ESTA VACIA");
            }
            if (idusuario == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (resenaexi != 0)
            {
                ModelState.AddModelError("", "YA AÑADISTE UNA *Reseña*");
            }
            if (ModelState.IsValid)
            {
                Recomendaciones datos = new()
                {
                    Estrellas = vm.Estrellas,
                    Descripcion = vm.Descripcion,
                    IdUsuarios = Convert.ToInt32(idusuario),
                    Fecha = DateTime.Now,
                };

                recomendacionesRepository.Insert(datos);

            }
            return View();
        }



        public IActionResult Carrito(int? id)
        {

            var idusuario = User.FindFirst("Id")?.Value;
            if (idusuario == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (id == null)
            {
                return View();
            }

            //var cant = carritoRepository.GetAll().Where(x => x.IdUsuario == Convert.ToInt32(idusuario) && x.IdProducto == id);

            var existe = carritoRepository.GetAll()
                .FirstOrDefault(x => x.IdUsuario == Convert.ToInt32(idusuario) && x.IdProducto == id); //-----Deja de ser una lista y

            var xd = productosRepository.GetAll().FirstOrDefault(x => x.Id == id);
            if (xd.Stock >= 1)
            {
                if (existe != null)
                {

                    existe.Cantidad += 1;
                    xd.Stock = xd.Stock - 1;

                    carritoRepository.Update(existe);
                    productosRepository.Update(xd);
                }
                else
                {
                    Carrito datos = new()
                    {
                        IdUsuario = Convert.ToInt32(idusuario),
                        IdProducto = id.Value,
                        Cantidad = 1
                    };
                    carritoRepository.Insert(datos);
                    xd.Stock = xd.Stock - 1;
                    productosRepository.Update(xd);
                }

            }
            else
                return RedirectToAction("Login", "Account");


            return RedirectPreserveMethod("/Home/Productos/" + id + "");
        }



        public IActionResult VerCarrito()
        {
            var idusuario = User.FindFirst("Id")?.Value;
            if (idusuario == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                var vm = context.Usuarios.Where(x => x.Id == Convert.ToInt32(idusuario)).Select(x => new VerCarrito
                {
                    NombreUsuario = x.NombreUsuario,
                    PrecioTotal = x.Carrito.Sum(c =>
                 c.Cantidad * (c.IdProductoNavigation.Preciopromocion.HasValue && c.IdProductoNavigation.Preciopromocion > 0
                     ? c.IdProductoNavigation.Preciopromocion.Value
                     : c.IdProductoNavigation.Precio)),

                    listitacarrito = x.Carrito.Select(x => new CarritoViewModel
                    {
                        Id = x.IdProductoNavigation.Id,
                        Nombre = x.IdProductoNavigation.Nombre,
                        Categorias = x.IdProductoNavigation.IdCategoriaNavigation.Categorias,
                        Precio = x.IdProductoNavigation.Precio,
                        Preciopromocion = x.IdProductoNavigation.Preciopromocion,
                        IdUsuario = Convert.ToInt32(idusuario),
                        Cantidad = x.Cantidad,
                        IdProducto = x.IdProducto
                    }).ToList()
                }).FirstOrDefault();

                // Formateamos el PrecioTotal a dos decimales
                if (vm?.PrecioTotal.HasValue == true)
                {
                    vm.PrecioTotal = Math.Round(vm.PrecioTotal.Value, 2);
                }

                return View(vm);
            }
        }



        public IActionResult carritostock(int? id)
        {

            var idusuario = User.FindFirst("Id")?.Value;
            if (idusuario == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (id == null)
            {
                return View();
            }

            //var cant = carritoRepository.GetAll().Where(x => x.IdUsuario == Convert.ToInt32(idusuario) && x.IdProducto == id);

            var existe = carritoRepository.GetAll()
                .FirstOrDefault(x => x.IdUsuario == Convert.ToInt32(idusuario) && x.IdProducto == id); //-----Deja de ser una lista y

            var xd = productosRepository.GetAll().FirstOrDefault(x => x.Id == id);
            if (xd.Stock >= 1)
            {
                if (existe != null)
                {

                    existe.Cantidad += 1;
                    xd.Stock = xd.Stock - 1;

                    carritoRepository.Update(existe);
                    productosRepository.Update(xd);
                }
                else
                {
                    Carrito datos = new()
                    {
                        IdUsuario = Convert.ToInt32(idusuario),
                        IdProducto = id.Value,
                        Cantidad = 1
                    };
                    carritoRepository.Insert(datos);
                    xd.Stock = xd.Stock - 1;
                    productosRepository.Update(xd);
                }

            }
            else
                return RedirectToAction("Login", "Account");


            return RedirectPreserveMethod("/Home/VerCarrito");
        }



        public IActionResult eliminarcarritostock(int? id)
        {

            var idusuario = User.FindFirst("Id")?.Value;
            if (idusuario == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (id == null)
            {
                return View();
            }

            //var cant = carritoRepository.GetAll().Where(x => x.IdUsuario == Convert.ToInt32(idusuario) && x.IdProducto == id);

            var existe = carritoRepository.GetAll()
                .FirstOrDefault(x => x.IdUsuario == Convert.ToInt32(idusuario) && x.IdProducto == id); //-----Deja de ser una lista y

            var xd = productosRepository.GetAll().FirstOrDefault(x => x.Id == id);
            if (existe.Cantidad >= 1)
            {
                existe.Cantidad -= 1;
                if (existe.Cantidad == 0)
                {
                    xd.Stock = xd.Stock + 1;
                    carritoRepository.Update(existe);

                    carritoRepository.Delete(existe);

                    return RedirectPreserveMethod("/Home/VerCarrito");
                }


                xd.Stock = xd.Stock + 1;




                carritoRepository.Update(existe);
                productosRepository.Update(xd);


            }



            return RedirectPreserveMethod("/Home/VerCarrito");
        }



        [HttpPost]
        public async Task<JsonResult> Paypal(string precio, string producto)
        {
            var idusuario = User.FindFirst("Id")?.Value;

            var vm = context.Usuarios.Where(x => x.Id == Convert.ToInt32(idusuario)).Select(x => new VerCarrito
            {
                PrecioTotal = x.Carrito.Sum(c =>
             c.Cantidad * (c.IdProductoNavigation.Preciopromocion.HasValue && c.IdProductoNavigation.Preciopromocion > 0
                 ? c.IdProductoNavigation.Preciopromocion.Value
                 : c.IdProductoNavigation.Precio))
            }).FirstOrDefault();

            bool status = false;
            string respuesta = string.Empty;


            //string precio = Convert.ToString(vm.PrecioTotal);
            precio = Convert.ToString(vm.PrecioTotal);

            var precioFormateado = decimal.Parse(precio).ToString("F2", CultureInfo.InvariantCulture);
            using (var client = new HttpClient())
            {
                // INGRESAR CREDENCIALES AQUI -> CLIENT ID - SECRET
                var userName = "AdlMD9kp3u2PA2pQglpUqYwjWRC6CSh6cQqDjw5fg-rPHMZ4-Sp-_Fh3XIEfDLpZU6PnqK-Hm4UoRqZY";
                var passwd = "EFOFOQrrl12jKJoetMs9tITtM2zWyrSvX_Ok1k2SIYEKmK8gNo1nAxxhzz2iQIX5QClwUZeSBhXXhOoi";

                client.BaseAddress = new Uri("https://api-m.sandbox.paypal.com");

                var authToken = Encoding.ASCII.GetBytes($"{userName}:{passwd}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));

                var orden = new PayPalOrder()
                {
                    intent = "CAPTURE",
                    purchase_units = new List<Models.PayPal_Order.PurchaseUnit>() {
                 new Models.PayPal_Order.PurchaseUnit() {
                     amount = new Models.PayPal_Order.Amount() {
                         currency_code = "MXN",
                         value = precioFormateado
                     },
                     //description = producto
                 }
             },
                    application_context = new ApplicationContext()
                    {
                        brand_name = "Mi Tienda",
                        landing_page = "NO_PREFERENCE",
                        user_action = "PAY_NOW",
                        return_url = "https://localhost:44377/Account/CompraE",
                        cancel_url = "https://localhost:44377/Home/Index"
                    }
                };

                var json = JsonConvert.SerializeObject(orden);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("/v2/checkout/orders", data);
                status = response.IsSuccessStatusCode;

                if (status)
                {
                    respuesta = await response.Content.ReadAsStringAsync();
                    TempData["pagoExitoso"] = true;
                }
                else
                {
                    var statusCode = response.StatusCode;
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    respuesta = $"Error: {statusCode} - {errorMessage}";
                    TempData["pagoExitoso"] = false;
                }
            }

            return Json(new { status = status, respuesta = respuesta });
        }

        /// PEDIDO
        [HttpGet]
        public IActionResult Pedido()
        {
            var idusuario = User.FindFirst("Id")?.Value;
            bool pago = TempData["pagoExitoso"] != null && (bool)TempData["pagoExitoso"];

            if (idusuario == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (pago)
            {
                var productosEnCarrito = context.Carrito.Include(x => x.IdProductoNavigation).Where(x => x.IdUsuario == Convert.ToInt32(idusuario)).Select(x => new ProductosCarrito
            {
                Id = x.IdProductoNavigation.Id,
                Nombre = x.IdProductoNavigation.Nombre,
                Cantidad = x.Cantidad,
                Precio = (x.IdProductoNavigation.Preciopromocion ?? x.IdProductoNavigation.Precio) * x.Cantidad,
            }).ToList();

            foreach (var p in productosEnCarrito)
            {
                p.Precio = Math.Round(p.Precio.Value, 2);
            }

            PedidoViewModel vm = new PedidoViewModel()
            {
                ProductosCarrito = productosEnCarrito
            };

            // Fechas con pedido agendado
            var fechasOcupadas = context.Pedido.Select(p => p.Fecha.ToString("yyyy-MM-dd")).ToList();
            ViewBag.FechasOcupadas = fechasOcupadas;

            return View(vm);
            }
        else
             return RedirectPreserveMethod("Index");
        }


        [HttpPost]
        public IActionResult Pedido(PedidoViewModel vm)
        {
            var idusuario = User.FindFirst("Id")?.Value;

            if (idusuario == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (string.IsNullOrWhiteSpace(vm.Instrucciones))
                ModelState.AddModelError("", "Ingrese las instrucciones de su pedido, por favor.");
            else if (string.IsNullOrWhiteSpace(vm.Telefono))
                ModelState.AddModelError("", "Ingrese un teléfono para contactarle, por favor.");
            else if (!Regex.IsMatch(vm.Telefono, @"^\d{10}$"))
                ModelState.AddModelError("", "El teléfono debe contener solo 10 dígitos numéricos.");
            else if (vm.Fecha == DateOnly.MinValue)
                ModelState.AddModelError("", "Ingrese la fecha del pedido.");
            else if (vm.Fecha < DateOnly.FromDateTime(DateTime.Now))
                ModelState.AddModelError("", "La fecha ingresada no es válida.");
            else if (pedidoRepository.GetAll().Any(x => (x.Fecha == vm.Fecha)))
                ModelState.AddModelError("", "La fecha seleccionada no está disponible.");

            if (ModelState.IsValid)
            {
                var productosEnCarrito = context.Carrito.Include(x => x.IdProductoNavigation).Where(x => x.IdUsuario == Convert.ToInt32(idusuario)).Select(x => new ProductosCarrito
                {
                    Id = x.IdProductoNavigation.Id,
                    Nombre = x.IdProductoNavigation.Nombre,
                    Cantidad = x.Cantidad,
                    Precio = (x.IdProductoNavigation.Preciopromocion ?? x.IdProductoNavigation.Precio) * x.Cantidad,
                }).ToList();

                var total = Math.Round((productosEnCarrito.Sum(x => x.Precio)).Value, 2);

                // Crear el pedido
                var nuevoPedido = new Pedido
                {
                    IdUsuario = Convert.ToInt32(idusuario),
                    Fecha = vm.Fecha,
                    Instrucciones = vm.Instrucciones,
                    Telefono = vm.Telefono,
                    Total = total,
                    Estado = "pendiente" // 
                };

                pedidoRepository.Insert(nuevoPedido);

                // Crear detalles
                foreach (var producto in productosEnCarrito)
                {
                    var detallePedido = new Detallepedido
                    {
                        IdPedido = nuevoPedido.Id,
                        IdProducto = producto.Id.Value,
                        Cantidad = producto.Cantidad.Value,
                        Precio = (producto.Cantidad * producto.Precio).Value,
                    };

                    detallePedidoRepository.Insert(detallePedido);

                    /* Actualizar el stock del producto
                    var productoEnStock = productosRepository.GetAll().FirstOrDefault(p => p.Id == producto.Id);
                    if (productoEnStock != null)
                    {
                        productoEnStock.Stock -= producto.Cantidad.Value; // Restamos la cantidad
                        productosRepository.Update(productoEnStock);
                    }*/
                }

                // Eliminar los productos del carrito
                var productosCarrito = carritoRepository.GetAll().Where(x => x.IdUsuario == Convert.ToInt32(idusuario)).ToList();

                foreach (var producto in productosCarrito)
                {
                    carritoRepository.Delete(producto);
                }

                return RedirectPreserveMethod("Index");
            }
            else
                return View(vm);
        }
    }
}
