using Microsoft.AspNetCore.Mvc;
using MvcExamenAWS.Models;
using MvcExamenAWS.Services;

namespace MvcExamenAWS.Controllers
{
    public class ConciertosController : Controller
    {
        private ServiceConciertos service;
        private ServiceStorageS3 serviceS3;

        public ConciertosController(ServiceConciertos service, ServiceStorageS3 serviceS3)
        {
            this.service = service;
            this.serviceS3 = serviceS3;
        }

        public async Task<IActionResult> Eventos()
        {
            List<Evento> eventos = await this.service.GetEventosAsync();
            foreach (var even in eventos)
            { // Sirve para ambos (Public y Private)
                try
                {
                    //using (Stream imageStream = await this.serviceS3.GetFileAsync(even.Imagen))
                    //{
                    //    using (MemoryStream memoryStream = new MemoryStream())
                    //    {
                    //        await imageStream.CopyToAsync(memoryStream);
                    //        byte[] bytes = memoryStream.ToArray();
                    //        string base64Image = Convert.ToBase64String(bytes);
                    //        even.Imagen = "data:image;base64," + base64Image;
                    //    }
                    //}
                    even.Imagen = $"https://bucket-examen.s3.amazonaws.com/" + even.Imagen;
                }
                catch (Exception ex)
                {
                    even.Imagen = null;
                }
            }
            return View(eventos);
        }

        public async Task<IActionResult> Categorias()
        {
            List<Categoria> categorias = await this.service.GetCategoriasAsync();
            return View(categorias);
        }

        public async Task<IActionResult> EventosCategoria(int id)
        {
            List<Evento>? eventos = await this.service.FindEventoCategoriaAsync(id);
            return View("Eventos", eventos);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Evento evento, IFormFile file)
        {
            evento.Imagen = file.FileName;
            using (Stream stream = file.OpenReadStream())
            {
                await this.serviceS3.UploadFileAsync(evento.Imagen, stream);
            }
            await this.service.CreateEventoAsync(evento);
            return RedirectToAction("Eventos");
        }
    }
}
