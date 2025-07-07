using Microsoft.EntityFrameworkCore;
using ChurrascosAPI.Data;
using ChurrascosAPI.Models;
using ChurrascosAPI.Services;
using ChurrascosAPI.Models.Ventas;
using ChurrascosAPI.Models.Inventario;
using ChurrascosAPI.Models.Productos;
using ChurrascosAPI.Models.Enums;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ChurrascosContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()     
                   .AllowAnyHeader()     
                   .AllowAnyMethod();    
        });
});


// Add services
builder.Services.AddScoped<IOpenRouterService, OpenRouterService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddHttpClient();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    options.SerializerOptions.WriteIndented = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Churrascos API V1");
        c.RoutePrefix = "swagger"; // Para acceder en /swagger
    });
}

app.MapGet("/", () => "Churrascos API funcionando correctamente");
app.UseCors("AllowAllOrigins");
app.UseHttpsRedirection();


// los Endpoints

// === CHURRASCOS ===
app.MapGet("/api/churrascos", async (ChurrascosContext context) =>
{
    var churrascos = await context.Churrascos
        .Include(c => c.Guarniciones)
        .ThenInclude(gc => gc.Guarnicion)
        .ToListAsync();

    var response = churrascos.Select(c => new 
    {
        Id = c.Id,
        Nombre = c.Nombre,
        Precio = c.Precio,
        Descripcion = c.Descripcion,
        TipoCarne = c.TipoCarne,
        TerminoCoccion = c.TerminoCoccion,
        TipoPlato = c.TipoPlato,
        CantidadPorciones = c.CantidadPorciones,
        PrecioBase = c.PrecioBase,
        Disponible = c.Disponible,
        FechaCreacion = c.FechaCreacion,
        Guarniciones = c.Guarniciones.Select(gc => new 
        {
            GuarnicionId = gc.GuarnicionId,
            NombreGuarnicion = gc.Guarnicion.Nombre,
            CantidadPorciones = gc.Cantidad, 
            EsExtra = gc.EsExtra,
            PrecioExtra = gc.Guarnicion.PrecioExtra,
            PrecioTotal = gc.EsExtra ? gc.Guarnicion.PrecioExtra * gc.Cantidad : 0
        }).ToList()
    }).ToList();

    return Results.Ok(response);
});

app.MapGet("/api/churrascos/{id}", async (int id, ChurrascosContext context) =>
{
    var churrasco = await context.Churrascos
        .Include(c => c.Guarniciones)
        .ThenInclude(gc => gc.Guarnicion)
        .FirstOrDefaultAsync(c => c.Id == id);
    
    if (churrasco == null) return Results.NotFound();

    var response = new 
    {
        Id = churrasco.Id,
        Nombre = churrasco.Nombre,
        Precio = churrasco.Precio,
        Descripcion = churrasco.Descripcion,
        TipoCarne = churrasco.TipoCarne,
        TerminoCoccion = churrasco.TerminoCoccion,
        TipoPlato = churrasco.TipoPlato,
        CantidadPorciones = churrasco.CantidadPorciones,
        PrecioBase = churrasco.PrecioBase,
        Disponible = churrasco.Disponible,
        FechaCreacion = churrasco.FechaCreacion,
        Guarniciones = churrasco.Guarniciones.Select(gc => new 
        {
            GuarnicionId = gc.GuarnicionId,
            NombreGuarnicion = gc.Guarnicion.Nombre,
            CantidadPorciones = gc.Cantidad, 
            EsExtra = gc.EsExtra,
            PrecioExtra = gc.Guarnicion.PrecioExtra,
            PrecioTotal = gc.EsExtra ? gc.Guarnicion.PrecioExtra * gc.Cantidad : 0
        }).ToList()
    };

    return Results.Ok(response);
});

app.MapPost("/api/churrascos", async (ChurrascoCreateRequest request, ChurrascosContext context) =>
{
    // Crear el churrasco
    var churrasco = new Churrasco
    {
        Nombre = request.Nombre,
        Precio = request.Precio,
        Descripcion = request.Descripcion,
        TipoCarne = request.TipoCarne,
        TerminoCoccion = request.TerminoCoccion,
        TipoPlato = request.TipoPlato,
        CantidadPorciones = request.CantidadPorciones,
        PrecioBase = request.PrecioBase,
        Disponible = request.Disponible,
        FechaCreacion = DateTime.UtcNow
    };

    context.Churrascos.Add(churrasco);
    await context.SaveChangesAsync();

    if (request.Guarniciones != null && request.Guarniciones.Any())
    {
        foreach (var guarnicionRequest in request.Guarniciones)
        {
            var guarnicionChurrasco = new GuarnicionChurrasco
            {
                ChurrascoId = churrasco.Id,
                GuarnicionId = guarnicionRequest.GuarnicionId,
                Cantidad = guarnicionRequest.CantidadPorciones,
                EsExtra = guarnicionRequest.EsExtra
            };

            context.GuarnicionesChurrasco.Add(guarnicionChurrasco);
        }

        await context.SaveChangesAsync();
    }

    return Results.Created($"/api/churrascos/{churrasco.Id}", new { id = churrasco.Id });
});

app.MapPut("/api/churrascos/{id}", async (int id, ChurrascoUpdateRequest request, ChurrascosContext context) =>
{
    var existingChurrasco = await context.Churrascos
        .Include(c => c.Guarniciones)
        .FirstOrDefaultAsync(c => c.Id == id);
    
    if (existingChurrasco == null) return Results.NotFound();

    existingChurrasco.Nombre = request.Nombre;
    existingChurrasco.Precio = request.Precio;
    existingChurrasco.Descripcion = request.Descripcion;
    existingChurrasco.TipoCarne = request.TipoCarne;
    existingChurrasco.TerminoCoccion = request.TerminoCoccion;
    existingChurrasco.TipoPlato = request.TipoPlato;
    existingChurrasco.CantidadPorciones = request.CantidadPorciones;
    existingChurrasco.PrecioBase = request.PrecioBase;
    existingChurrasco.Disponible = request.Disponible;
    existingChurrasco.FechaModificacion = DateTime.UtcNow;

    context.GuarnicionesChurrasco.RemoveRange(existingChurrasco.Guarniciones);


    if (request.Guarniciones != null && request.Guarniciones.Any())
    {
        foreach (var guarnicionRequest in request.Guarniciones)
        {
            var guarnicionChurrasco = new GuarnicionChurrasco
            {
                ChurrascoId = id,
                GuarnicionId = guarnicionRequest.GuarnicionId,
                Cantidad = guarnicionRequest.CantidadPorciones, 
                EsExtra = guarnicionRequest.EsExtra
            };

            context.GuarnicionesChurrasco.Add(guarnicionChurrasco);
        }
    }

    await context.SaveChangesAsync();
    return Results.Ok(new { message = "Churrasco actualizado correctamente" });
});

app.MapDelete("/api/churrascos/{id}", async (int id, ChurrascosContext context) =>
{
    var churrasco = await context.Churrascos
        .Include(c => c.Guarniciones)
        .FirstOrDefaultAsync(c => c.Id == id);
    
    if (churrasco == null) return Results.NotFound();

    context.Churrascos.Remove(churrasco);
    await context.SaveChangesAsync();
    
    return Results.NoContent();
});       

// === DULCES TÍPICOS ===
app.MapGet("/api/dulces", async (ChurrascosContext context) =>
{
    return await context.DulcesTipicos.ToListAsync();
});

app.MapGet("/api/dulces/{id}", async (int id, ChurrascosContext context) =>
{
    var dulce = await context.DulcesTipicos.FindAsync(id);
    return dulce is not null ? Results.Ok(dulce) : Results.NotFound();
});

app.MapPost("/api/dulces", async (DulceTipico dulce, ChurrascosContext context) =>
{
    context.DulcesTipicos.Add(dulce);
    await context.SaveChangesAsync();
    return Results.Created($"/api/dulces/{dulce.Id}", dulce);
});

app.MapPut("/api/dulces/{id}", async (int id, DulceTipico dulce, ChurrascosContext context) =>
{
    var existingDulce = await context.DulcesTipicos.FindAsync(id);
    if (existingDulce is null) return Results.NotFound();

    existingDulce.Nombre = dulce.Nombre;
    existingDulce.Precio = dulce.Precio;
    existingDulce.TipoDulce = dulce.TipoDulce;
    existingDulce.CantidadEnStock = dulce.CantidadEnStock;
    existingDulce.ModalidadVenta = dulce.ModalidadVenta;
    existingDulce.CapacidadCaja = dulce.CapacidadCaja;
    existingDulce.Disponible = dulce.Disponible;

    await context.SaveChangesAsync();
    return Results.Ok(existingDulce);
});

app.MapDelete("/api/dulces/{id}", async (int id, ChurrascosContext context) =>
{
    var dulce = await context.DulcesTipicos.FindAsync(id);
    if (dulce is null) return Results.NotFound();

    context.DulcesTipicos.Remove(dulce);
    await context.SaveChangesAsync();
    return Results.NoContent();
});

// === GUARNICIONES ===
app.MapGet("/api/guarniciones", async (ChurrascosContext context) =>
{
    return await context.Guarniciones.ToListAsync();
});

app.MapPost("/api/guarniciones", async (Guarnicion guarnicion, ChurrascosContext context) =>
{
    context.Guarniciones.Add(guarnicion);
    await context.SaveChangesAsync();
    return Results.Created($"/api/guarniciones/{guarnicion.Id}", guarnicion);
});

app.MapGet("/api/guarniciones/disponibles", async (ChurrascosContext context) =>
{
    var guarniciones = await context.Guarniciones
        .Where(g => g.Disponible && g.CantidadStock > 0)
        .Select(g => new
        {
            g.Id,
            g.Nombre,
            g.PrecioExtra,
            g.CantidadStock,
            g.Descripcion
        })
        .ToListAsync();

    return Results.Ok(guarniciones);
});

app.MapPut("/api/guarniciones/{id}", async (int id, Guarnicion guarnicion, ChurrascosContext context) =>
{
    var existingGuarnicion = await context.Guarniciones.FindAsync(id);
    if (existingGuarnicion is null) return Results.NotFound();

    existingGuarnicion.Nombre = guarnicion.Nombre;
    existingGuarnicion.PrecioExtra = guarnicion.PrecioExtra;
    existingGuarnicion.Disponible = guarnicion.Disponible;
    existingGuarnicion.CantidadStock = guarnicion.CantidadStock;
    existingGuarnicion.StockMinimo = guarnicion.StockMinimo;
    existingGuarnicion.Descripcion = guarnicion.Descripcion;

    await context.SaveChangesAsync();
    return Results.Ok(existingGuarnicion);
});

app.MapDelete("/api/guarniciones/{id}", async (int id, ChurrascosContext context) =>
{
    var guarnicion = await context.Guarniciones.FindAsync(id);
    if (guarnicion is null) return Results.NotFound();

    context.Guarniciones.Remove(guarnicion);
    await context.SaveChangesAsync();
    return Results.NoContent();
});

// === COMBOS ===
app.MapGet("/api/combos", async (ChurrascosContext context) =>
{
    return await context.Combos.ToListAsync();
});

app.MapGet("/api/combos/{id}", async (int id, ChurrascosContext context) =>
{
    var combo = await context.Combos.FindAsync(id);
    return combo is not null ? Results.Ok(combo) : Results.NotFound();
});

app.MapPost("/api/combos", async (Combo combo, ChurrascosContext context) =>
{
    context.Combos.Add(combo);
    await context.SaveChangesAsync();
    return Results.Created($"/api/combos/{combo.Id}", combo);
});

app.MapPut("/api/combos/{id}", async (int id, Combo combo, ChurrascosContext context) =>
{
    var existingCombo = await context.Combos.FindAsync(id);
    if (existingCombo is null) return Results.NotFound();

    existingCombo.Nombre = combo.Nombre;
    existingCombo.Precio = combo.Precio;
    existingCombo.Descripcion = combo.Descripcion;
    existingCombo.TipoCombo = combo.TipoCombo;
    existingCombo.PorcentajeDescuento = combo.PorcentajeDescuento;
    existingCombo.MontoDescuento = combo.MontoDescuento;
    existingCombo.EsTemporada = combo.EsTemporada;
    existingCombo.FechaInicioVigencia = combo.FechaInicioVigencia;
    existingCombo.FechaFinVigencia = combo.FechaFinVigencia;
    existingCombo.Disponible = combo.Disponible;

    await context.SaveChangesAsync();
    return Results.Ok(existingCombo);
});

app.MapDelete("/api/combos/{id}", async (int id, ChurrascosContext context) =>
{
    var combo = await context.Combos.FindAsync(id);
    if (combo is null) return Results.NotFound();

    context.Combos.Remove(combo);
    await context.SaveChangesAsync();
    return Results.NoContent();
});

// === INVENTARIO ===
app.MapGet("/api/inventario", async (ChurrascosContext context) =>
{
    return await context.Inventario.ToListAsync();
});

app.MapGet("/api/inventario/bajo-stock", async (ChurrascosContext context) =>
{
    return await context.Inventario
        .Where(i => i.Cantidad <= i.StockMinimo)
        .ToListAsync();
});

app.MapPut("/api/inventario/{id}", async (int id, InventarioItem item, ChurrascosContext context) =>
{
    var existingItem = await context.Inventario.FindAsync(id);
    if (existingItem is null) return Results.NotFound();

    existingItem.Cantidad = item.Cantidad;
    existingItem.UltimaActualizacion = DateTime.Now;

    await context.SaveChangesAsync();
    return Results.Ok(existingItem);
});


app.MapPut("/api/inventario/{id}/completo", async (int id, InventarioItem item, ChurrascosContext context) =>
{
    var existingItem = await context.Inventario.FindAsync(id);
    if (existingItem is null) return Results.NotFound();

    existingItem.Nombre = item.Nombre;
    existingItem.Tipo = item.Tipo;
    existingItem.Cantidad = item.Cantidad;
    existingItem.StockMinimo = item.StockMinimo;
    existingItem.StockMaximo = item.StockMaximo;
    existingItem.Unidad = item.Unidad;
    existingItem.PrecioUnitario = item.PrecioUnitario;
    existingItem.Proveedor = item.Proveedor;
    existingItem.CodigoProveedor = item.CodigoProveedor;
    existingItem.FechaVencimiento = item.FechaVencimiento;
    existingItem.UbicacionAlmacen = item.UbicacionAlmacen;
    existingItem.PuntoReorden = item.PuntoReorden;
    existingItem.Activo = item.Activo;
    existingItem.Notas = item.Notas;
    existingItem.UltimaActualizacion = DateTime.Now;

    await context.SaveChangesAsync();
    return Results.Ok(existingItem);
});


app.MapPost("/api/inventario", async (InventarioItem item, ChurrascosContext context) =>
{
    item.UltimaActualizacion = DateTime.Now;
    context.Inventario.Add(item);
    await context.SaveChangesAsync();
    return Results.Created($"/api/inventario/{item.Id}", item);
});

app.MapDelete("/api/inventario/{id}", async (int id, ChurrascosContext context) =>
{
    var item = await context.Inventario.FindAsync(id);
    if (item is null) return Results.NotFound();

    context.Inventario.Remove(item);
    await context.SaveChangesAsync();
    return Results.NoContent();
});

// === VENTAS ===
app.MapGet("/api/ventas", async (ChurrascosContext context) =>
{
    var ventas = await context.Ventas
        .Select(v => new
        {
            v.Id,
            v.Fecha,
            v.NombreCliente,
            v.TelefonoCliente,
            v.DireccionEntrega,
            v.TipoVenta,
            v.NumeroMesa,
            v.MetodoPago,
            v.Subtotal,
            v.Impuestos,
            v.Total,
            v.Estado,
            v.NumeroOrden,
            Items = v.Items.Select(i => new
            {
                i.Id,
                i.ProductoId,
                i.Cantidad,
                i.PrecioUnitario,
                i.Subtotal,
                i.NotasEspeciales
            }).ToList()
        })
        .OrderByDescending(v => v.Fecha)
        .ToListAsync();
    
    return ventas;
});

app.MapPost("/api/ventas", async (Venta venta, ChurrascosContext context) =>
{
    venta.Total = venta.Items.Sum(i => i.Subtotal) - venta.Descuento;
    context.Ventas.Add(venta);
    await context.SaveChangesAsync();
    return Results.Created($"/api/ventas/{venta.Id}", venta);
});

// === DASHBOARD ===
app.MapGet("/api/dashboard", async (IDashboardService dashboardService) =>
{
    return await dashboardService.GetDashboardDataAsync();
});


app.MapGet("/api/ia/dashboard-insights", async (IOpenRouterService openRouterService, IDashboardService dashboardService, ILogger<Program> logger) =>
{
    try
    {

        
        var dashboardData = await dashboardService.GetDashboardDataAsync();
       
        
        var insights = await openRouterService.AnalizeDataAsync(
            "Analiza estos datos del dashboard de mi restaurante de churrascos y dulces típicos guatemaltecos. Proporciona 3 insights clave y 3 recomendaciones específicas para mejorar el negocio. Sé específico y práctico."
        );
        
     
        
        return Results.Ok(new { 
            success = true, 
            dashboardData = dashboardData,
            aiInsights = insights,
            timestamp = DateTime.Now 
        });
    }
    catch (InvalidOperationException ex) when (ex.Message.Contains("OpenRouter API Key"))
    {
       
        return Results.Problem(
            title: "Configuración faltante",
            detail: "La API Key de OpenRouter no está configurada. Revisa tu appsettings.json y asegúrate de tener: \"OpenRouter\": { \"ApiKey\": \"sk-or-v1-tu-key-aqui\" }",
            statusCode: 500
        );
    }
    catch (HttpRequestException ex)
    {
        
        return Results.Problem(
            title: "Error de conexión",
            detail: $"No se pudo conectar con OpenRouter. Verifica tu conexión a internet. Error: {ex.Message}",
            statusCode: 503
        );
    }
    catch (Exception ex)
    {
        return Results.Problem(
            title: "Error del servidor",
            detail: $"Error inesperado: {ex.Message}. Revisa los logs para más detalles.",
            statusCode: 500
        );
    }
});

app.MapPost("/api/ia/chat", async (ChatMessage message, IOpenRouterService openRouterService, ILogger<Program> logger) =>
{
    try
    {
        
        var response = await openRouterService.ChatBotResponseAsync(message.Message, "");
        
        
        return Results.Ok(new { 
            success = true, 
            response = response, 
            conversationId = message.ConversationId,
            timestamp = DateTime.Now 
        });
    }
    catch (Exception ex)
    {
        return Results.Problem(
            title: "Error en chatbot",
            detail: $"Error al procesar mensaje: {ex.Message}",
            statusCode: 500
        );
    }
});

// Endpoint para probar la configuración
app.MapGet("/api/ia/test-config", async (IConfiguration config, ILogger<Program> logger) =>
{
    try
    {
        var apiKey = config["OpenRouter:ApiKey"];

        var result = new
        {
            HasApiKey = !string.IsNullOrEmpty(apiKey),
            ApiKeyPrefix = !string.IsNullOrEmpty(apiKey) ? apiKey.Substring(0, Math.Min(10, apiKey.Length)) + "..." : "No configurada",
            ConfigurationSections = config.GetChildren().Select(c => c.Key).ToList(),
            OpenRouterSection = config.GetSection("OpenRouter").Exists(),
            Timestamp = DateTime.Now
        };


        return Results.Ok(result);
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error al verificar configuración: {ex.Message}");
    }
});

app.MapGet("/api/sucursales", async (ChurrascosContext context) =>
{
    var sucursales = await context.Sucursales
        .ToListAsync();

    return Results.Ok(sucursales);
});

app.MapGet("/api/sucursales/{id}", async (int id, ChurrascosContext context) =>
{
    var sucursal = await context.Sucursales
        .FirstOrDefaultAsync(s => s.Id == id);
    
    if (sucursal == null) return Results.NotFound();

    return Results.Ok(sucursal);
});

app.MapPost("/api/sucursales", async (SucursalCreateRequest request, ChurrascosContext context) =>
{
    var sucursal = new Sucursal
    {
        Nombre = request.Nombre,
        Direccion = request.Direccion,
        Telefono = request.Telefono,
        Email = request.Email,
        Gerente = request.Gerente,
        Activa = request.Activa,
        Latitud = request.Latitud,
        Longitud = request.Longitud,
        HorarioApertura = request.HorarioApertura,
        HorarioCierre = request.HorarioCierre,
        DiasLaborales = request.DiasLaborales,
        FechaCreacion = DateTime.UtcNow
    };

    context.Sucursales.Add(sucursal);
    await context.SaveChangesAsync();

    return Results.Created($"/api/sucursales/{sucursal.Id}", new { id = sucursal.Id });
});

app.MapPut("/api/sucursales/{id}", async (int id, SucursalCreateRequest request, ChurrascosContext context) =>
{
    var existingSucursal = await context.Sucursales
        .FirstOrDefaultAsync(s => s.Id == id);
    
    if (existingSucursal == null) return Results.NotFound();

    existingSucursal.Nombre = request.Nombre;
    existingSucursal.Direccion = request.Direccion;
    existingSucursal.Telefono = request.Telefono;
    existingSucursal.Email = request.Email;
    existingSucursal.Gerente = request.Gerente;
    existingSucursal.Activa = request.Activa;
    existingSucursal.Latitud = request.Latitud;
    existingSucursal.Longitud = request.Longitud;
    existingSucursal.HorarioApertura = request.HorarioApertura;
    existingSucursal.HorarioCierre = request.HorarioCierre;
    existingSucursal.DiasLaborales = request.DiasLaborales;
    existingSucursal.FechaModificacion = DateTime.UtcNow;

    await context.SaveChangesAsync();
    return Results.Ok(new { message = "Sucursal actualizada correctamente" });
});

app.MapDelete("/api/sucursales/{id}", async (int id, ChurrascosContext context) =>
{
    var sucursal = await context.Sucursales
        .FirstOrDefaultAsync(s => s.Id == id);
    
    if (sucursal == null) return Results.NotFound();

    context.Sucursales.Remove(sucursal);
    await context.SaveChangesAsync();
    
    return Results.NoContent();
});

app.MapGet("/api/sucursales/{id}/estadisticas", async (int id, ChurrascosContext context) =>
{
    var sucursal = await context.Sucursales
        .FirstOrDefaultAsync(s => s.Id == id);
    
    if (sucursal == null) return Results.NotFound();

    var estadisticas = new
    {
        SucursalId = id,
        Nombre = sucursal.Nombre,
        Activa = sucursal.Activa,
        FechaCreacion = sucursal.FechaCreacion,
    };

    return Results.Ok(estadisticas);
});


app.Run();

public class SucursalCreateRequest
{
    public string Nombre { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Gerente { get; set; }
    public bool Activa { get; set; } = true;
    public double? Latitud { get; set; }
    public double? Longitud { get; set; }
    public string? HorarioApertura { get; set; }
    public string? HorarioCierre { get; set; }
    public List<string>? DiasLaborales { get; set; }
}
public class ChatMessage
{
    public string Message { get; set; } = string.Empty;
    public string ConversationId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
}

public class RecommendationRequest
{
    public string TipoAnalisis { get; set; } = string.Empty;
    public object Datos { get; set; } = new();
}

public class ChurrascoCreateRequest
{
    public string Nombre { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public string? Descripcion { get; set; }
    public TipoCarne TipoCarne { get; set; }
    public TerminoCoccion TerminoCoccion { get; set; }
    public TipoPlato TipoPlato { get; set; }
    public int CantidadPorciones { get; set; } = 1;
    public decimal PrecioBase { get; set; }
    public bool Disponible { get; set; } = true;
    public List<GuarnicionChurrascoRequest>? Guarniciones { get; set; }
}

public class ChurrascoUpdateRequest : ChurrascoCreateRequest
{
}

public class GuarnicionChurrascoRequest
{
    public int GuarnicionId { get; set; }
    public int CantidadPorciones { get; set; }
    public bool EsExtra { get; set; }
}