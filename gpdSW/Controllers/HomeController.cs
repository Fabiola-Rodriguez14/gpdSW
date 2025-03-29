using gpdSW.Models.Entities;
using gpdSW.Models.ViewModels;
using gpdSW.repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace gpdSW.Controllers
{
    public class HomeController : Controller
    {
        CheeseandmoreContext context;
        Repository<Productos> productosRepository;
        Repository<Recomendaciones> recomendacionesRepository;

        public HomeController()
        {
            context = new CheeseandmoreContext();
            productosRepository = new(context);
            recomendacionesRepository = new(context);
        }

        public IActionResult Index()
        {
            CheeseandmoreContext context = new CheeseandmoreContext();
            ReseñasViewModel vm = new();

                            var IdUsuario = User.FindFirst("Id")?.Value;

                            if (IdUsuario == null)
                            {
                                return RedirectToAction("Login", "Account");
                            }

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
        public IActionResult Catalogo(int? id)
        {
            Random rnd = new Random();
            CheeseandmoreContext context = new CheeseandmoreContext();


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
                    IdUsuarios = Convert.ToInt32(idusuario)
                };

                recomendacionesRepository.Insert(datos);

            }
            return View();
        }

[HttpPost]
        public async Task<JsonResult> Paypal(/*string precio, string producto*/)
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


           string precio =Convert.ToString(vm.PrecioTotal);

            var precioFormateado = decimal.Parse(precio).ToString("F2", CultureInfo.InvariantCulture);
            using (var client = new HttpClient())
            {
                // INGRESA TUS CREDENCIALES AQUI -> CLIENT ID - SECRET
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
                }
                else
                {
                    var statusCode = response.StatusCode; 
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    respuesta = $"Error: {statusCode} - {errorMessage}";
                }
            }

            return Json(new { status = status, respuesta = respuesta });
        }



    }
}
