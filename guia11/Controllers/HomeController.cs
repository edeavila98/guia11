using Firebase.Auth;
using guia11.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using Firebase.Storage;



namespace guia11.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SubirArchivo(IFormFile archivo)
        {



            //Leemos el archivo subido. Stream archivoASubir archivo. OpenReadStream();
            Stream archivoAsubir = archivo.OpenReadStream();

            //Configuramos la conexion hacia FireBase
            string email = "edenilson.avila@catolica.edu.sv"; // Correo para autenticar en FireBase
            string clave = "12345678"; // Contraseña establaecida en la autenticar en Firebase
            string ruta = "guia11-3d3cf.appspot.com"; // URL donde se guardaran los archivo. string api_key = "AIzaSyAhSFON@lsHDGE "
            string api_key = "AIzaSyDTYQtn4HlwJ1oIwlxHrquDZGgcyxBpHxY";

            var auth = new FirebaseAuthProvider(new FirebaseConfig(api_key));
            var autenticarFireBase = await auth.SignInWithEmailAndPasswordAsync(email, clave);

            var cancellation = new CancellationTokenSource();
            var tokenUser = autenticarFireBase.FirebaseToken;


            var tareaCargarArchivo = new FirebaseStorage(ruta,
                                                        new FirebaseStorageOptions
                                                        {
                                                            AuthTokenAsyncFactory = () => Task.FromResult(tokenUser), ThrowOnCancel = true
                                                        }

                                                    ).Child("Arhivos")
                                                    .Child(archivo.FileName)
                                                    .PutAsync(archivoAsubir, cancellation.Token);

            var urlArchivoCargado = await tareaCargarArchivo;


            return RedirectToAction("Index");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
