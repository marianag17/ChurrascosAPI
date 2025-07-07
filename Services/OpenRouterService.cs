using System.Text;
using System.Text.Json;
using ChurrascosAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace ChurrascosAPI.Services
{
    public class OpenRouterService : IOpenRouterService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ChurrascosContext _context;
        private readonly string _apiKey;

        public OpenRouterService(HttpClient httpClient, IConfiguration configuration, ChurrascosContext context, ILogger<OpenRouterService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _context = context;
            _apiKey = _configuration["OpenRouter:ApiKey"] ?? throw new InvalidOperationException("OpenRouter API Key not configured");           
        }

        public async Task<string> AnalizeDataAsync(string prompt)
        {
            try
            {          
                
                if (string.IsNullOrEmpty(_apiKey))
                {
                    throw new Exception("API Key de OpenRouter no configurada");
                }

                var businessData = await GetBusinessDataForAnalysis();
                
                var enhancedPrompt = $@"
DATOS ACTUALES DEL RESTAURANTE DE CHURRASCOS Y DULCES TÍPICOS:
{JsonSerializer.Serialize(businessData, new JsonSerializerOptions { WriteIndented = true })}

ANÁLISIS SOLICITADO: {prompt}

Por favor analiza la información y proporciona insights específicos y recomendaciones accionables para el negocio.
Incluye datos numéricos específicos cuando sea posible y sugiere acciones concretas.
";

                var request = new
                {
                    model = "microsoft/wizardlm-2-8x22b",
                    messages = new[]
                    {
                        new { role = "system", content = "Eres un experto analista de datos para restaurantes especializados en churrascos y dulces típicos guatemaltecos. Proporciona análisis detallados y recomendaciones prácticas basadas en datos reales. Responde en español con insights accionables y específicos." },
                        new { role = "user", content = enhancedPrompt }
                    },
                    max_tokens = 1500,
                    temperature = 0.7
                };

                return await SendRequestAsync(request);
            }
            catch (Exception )
            {
                throw;
            }
        }

        public async Task<string> GetRecommendationsAsync(RecommendationRequest request)
        {
            try
            {
                var businessData = await GetBusinessDataForAnalysis();
                
                var prompt = $@"
CONTEXTO DEL NEGOCIO:
{JsonSerializer.Serialize(businessData, new JsonSerializerOptions { WriteIndented = true })}

TIPO DE ANÁLISIS: {request.TipoAnalisis}
DATOS ADICIONALES: {JsonSerializer.Serialize(request.Datos)}

Proporciona recomendaciones específicas para mejorar el negocio de churrascos y dulces típicos guatemaltecos.
";

                var llmRequest = new
                {
                    model = "microsoft/wizardlm-2-8x22b",
                    messages = new[]
                    {
                        new { role = "system", content = "Eres un consultor experto en restaurantes guatemaltecos y análisis de datos gastronómicos. Proporciona recomendaciones prácticas y específicas para el mercado guatemalteco." },
                        new { role = "user", content = prompt }
                    },
                    max_tokens = 1500,
                    temperature = 0.8
                };

                return await SendRequestAsync(llmRequest);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> ChatBotResponseAsync(string userMessage, string conversationHistory = "")
        {
            try
            {              
                var businessData = await GetBasicBusinessInfo();
                
                var systemPrompt = $@"
Eres el asistente virtual del restaurante 'Churrascos & Dulces Típicos' en Guatemala.

INFORMACIÓN ACTUAL DEL NEGOCIO:
{JsonSerializer.Serialize(businessData, new JsonSerializerOptions { WriteIndented = true })}

Responde de manera amigable y útil en español guatemalteco.
";

                var request = new
                {
                    model = "meta-llama/llama-3.1-70b-instruct",
                    messages = new[]
                    {
                        new { role = "system", content = systemPrompt },
                        new { role = "user", content = userMessage }
                    },
                    max_tokens = 800,
                    temperature = 0.8
                };

                return await SendRequestAsync(request);
            }
            catch (Exception )
            {
                throw;
            }
        }

        private async Task<object> GetBusinessDataForAnalysis()
        {
            try
            {
                var today = DateTime.Today;
                var thisMonth = new DateTime(today.Year, today.Month, 1);

                var ventasHoy = await _context.Ventas.Where(v => v.Fecha.Date == today).SumAsync(v => v.Total);
                var ventasMes = await _context.Ventas.Where(v => v.Fecha >= thisMonth).SumAsync(v => v.Total);
                var cantidadVentasHoy = await _context.Ventas.CountAsync(v => v.Fecha.Date == today);
                var cantidadVentasMes = await _context.Ventas.CountAsync(v => v.Fecha >= thisMonth);

                var inventarioBajo = await _context.Inventario
                    .Where(i => i.Cantidad <= i.StockMinimo)
                    .ToListAsync();

                var productosPopulares = await _context.VentasItems
                    .Where(vi => vi.Venta.Fecha >= thisMonth)
                    .GroupBy(vi => vi.Producto.Nombre)
                    .Select(g => new { 
                        Producto = g.Key, 
                        CantidadVendida = g.Sum(vi => vi.Cantidad), 
                        IngresosGenerados = g.Sum(vi => vi.Subtotal) 
                    })
                    .OrderByDescending(x => x.CantidadVendida)
                    .Take(5)
                    .ToListAsync();

                return new
                {
                    ResumenVentas = new
                    {
                        VentasHoy = ventasHoy,
                        VentasMes = ventasMes,
                        CantidadVentasHoy = cantidadVentasHoy,
                        CantidadVentasMes = cantidadVentasMes
                    },
                    ProductosPopulares = productosPopulares,
                    InventarioAlerta = new
                    {
                        ItemsBajoStock = inventarioBajo.Count,
                        DetalleItems = inventarioBajo.Select(i => new { i.Nombre, i.Cantidad, i.StockMinimo }).ToList()
                    },
                    FechaAnalisis = DateTime.Now
                };
            }
            catch (Exception )
            {
                throw;
            }
        }

        private async Task<object> GetBasicBusinessInfo()
        {
            try
            {
                var churrascos = await _context.Churrascos
                    .Where(c => c.Disponible)
                    .Select(c => new { c.Nombre, c.Precio })
                    .ToListAsync();

                var dulces = await _context.DulcesTipicos
                    .Where(d => d.Disponible)
                    .Select(d => new { d.Nombre, d.Precio })
                    .ToListAsync();

                return new
                {
                    MenuChurrascos = churrascos,
                    MenuDulces = dulces,
                    TotalProductos = churrascos.Count + dulces.Count
                };
            }
            catch (Exception )
            {
                throw;
            }
        }

        private async Task<string> SendRequestAsync(object request)
        {
            try
            {
                
                var json = JsonSerializer.Serialize(request, new JsonSerializerOptions 
                { 
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                
                
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
                _httpClient.DefaultRequestHeaders.Add("HTTP-Referer", "https://churrascos-guatemala.com");
                _httpClient.DefaultRequestHeaders.Add("X-Title", "Sistema Churrascos Guatemala");

                
                var response = await _httpClient.PostAsync("https://openrouter.ai/api/v1/chat/completions", content);
                
                
                var responseContent = await response.Content.ReadAsStringAsync();
                
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error en OpenRouter: {response.StatusCode} - {responseContent}");
                }


                var result = JsonSerializer.Deserialize<OpenRouterResponse>(responseContent);
                
                var finalResponse = result?.choices?.FirstOrDefault()?.message?.content ?? "No se pudo obtener respuesta del LLM";
                
                return finalResponse;
            }
            catch (HttpRequestException ex)
            {
            
                throw new Exception($"Error de conexión con OpenRouter: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                
                throw new Exception($"Timeout en la solicitud a OpenRouter: {ex.Message}");
            }
            catch (JsonException ex)
            {
                
                throw new Exception($"Error al procesar respuesta de OpenRouter: {ex.Message}");
            }
            catch (Exception )
            {
                throw;
            }
        }
    }
}