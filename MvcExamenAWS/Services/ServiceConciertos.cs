using MvcExamenAWS.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace MvcExamenAWS.Services
{
    public class ServiceConciertos
    {
        private MediaTypeWithQualityHeaderValue header;
        private string UrlApi;

        public ServiceConciertos(IConfiguration configuration /*, KeysModel keysModel*/)
        {
            this.header = new MediaTypeWithQualityHeaderValue("application/json");
            this.UrlApi = configuration.GetConnectionString("ApiConciertos");
        }

        #region GENERAL
        private async Task<T> CallApiAsync<T>(string request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                string url = this.UrlApi + request;

                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }
        #endregion 

        public async Task<List<Categoria>> GetCategoriasAsync()
        {
            string request = "/api/Eventos/GetCategorias";
            List<Categoria> categorias = await this.CallApiAsync<List<Categoria>>(request);
            return categorias;
        }

        public async Task<List<Evento>> GetEventosAsync()
        {
            string request = "/api/Eventos/GetEventos";
            List<Evento> eventos = await this.CallApiAsync<List<Evento>>(request);
            return eventos;
        }

        public async Task<List<Evento>> FindEventoCategoriaAsync(int idcategoria)
        {
            string request = "/api/Eventos/FindCubosMarca/" + idcategoria;
            List<Evento> eventos = await this.CallApiAsync<List<Evento>>(request);
            return eventos;
        }

        public async Task CreateEventoAsync(Evento evento)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/Eventos/CreateEvento";

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);

                string json = JsonConvert.SerializeObject(evento);

                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(this.UrlApi + request, content);
            }
        }
    }
}
