using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ChurrascosAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Guarniciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PrecioExtra = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Disponible = table.Column<bool>(type: "bit", nullable: false),
                    CantidadStock = table.Column<int>(type: "int", nullable: false),
                    StockMinimo = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TiempoPreparacionMinutos = table.Column<int>(type: "int", nullable: false),
                    CaloriasPorPorcion = table.Column<int>(type: "int", nullable: true),
                    EsPremium = table.Column<bool>(type: "bit", nullable: false),
                    UnidadMedida = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guarniciones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Inventario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    Unidad = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StockMinimo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StockMaximo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    UltimaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Proveedor = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CodigoProveedor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FechaVencimiento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UbicacionAlmacen = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CostoPromedio = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PuntoReorden = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    Notas = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Producto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Disponible = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProductoTipo = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    TipoCarne = table.Column<int>(type: "int", nullable: true),
                    TerminoCoccion = table.Column<int>(type: "int", nullable: true),
                    TipoPlato = table.Column<int>(type: "int", nullable: true),
                    CantidadPorciones = table.Column<int>(type: "int", nullable: true),
                    PrecioBase = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    TemperaturaRecomendada = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TiempoPreparacion = table.Column<TimeSpan>(type: "time", nullable: true),
                    NotasEspeciales = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TipoCombo = table.Column<int>(type: "int", nullable: true),
                    PorcentajeDescuento = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    MontoDescuento = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    EsTemporada = table.Column<bool>(type: "bit", nullable: true),
                    FechaInicioVigencia = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaFinVigencia = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CantidadMinimaPedido = table.Column<int>(type: "int", nullable: true),
                    CantidadMaximaPedido = table.Column<int>(type: "int", nullable: true),
                    CondicionesEspeciales = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TipoDulce = table.Column<int>(type: "int", nullable: true),
                    CantidadEnStock = table.Column<int>(type: "int", nullable: true),
                    ModalidadVenta = table.Column<int>(type: "int", nullable: true),
                    CapacidadCaja = table.Column<int>(type: "int", nullable: true),
                    PrecioUnidad = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    FechaVencimiento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Proveedor = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Ingredientes = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    PesoGramos = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ventas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Impuestos = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Descuento = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TipoVenta = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    NotasEspeciales = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    NombreCliente = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TelefonoCliente = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DireccionEntrega = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NumeroMesa = table.Column<int>(type: "int", nullable: true),
                    AreaRestaurante = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HoraInicioPedido = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HoraInicioPreparacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HoraFinalizacionPreparacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HoraEntrega = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MetodoPago = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Propina = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Mesero = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Cocinero = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Repartidor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FechaEstimadaEntrega = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CostoEnvio = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CalificacionServicio = table.Column<int>(type: "int", nullable: true),
                    ComentariosCliente = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioCreacion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UsuarioModificacion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NumeroOrden = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ventas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MovimientosInventario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InventarioItemId = table.Column<int>(type: "int", nullable: false),
                    TipoMovimiento = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CantidadAnterior = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CantidadNueva = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Motivo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UsuarioResponsable = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReferenciaId = table.Column<int>(type: "int", nullable: true),
                    TipoReferencia = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimientosInventario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovimientosInventario_Inventario_InventarioItemId",
                        column: x => x.InventarioItemId,
                        principalTable: "Inventario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComboItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComboId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    EsObligatorio = table.Column<bool>(type: "bit", nullable: false),
                    PermiteAlternativas = table.Column<bool>(type: "bit", nullable: false),
                    OrdenPresentacion = table.Column<int>(type: "int", nullable: false),
                    CategoriaCombo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DescripcionEnCombo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    OpcionesPersonalizacion = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    PorcentajeDescuentoItem = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecioOriginal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComboItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComboItems_Producto_ComboId",
                        column: x => x.ComboId,
                        principalTable: "Producto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComboItems_Producto_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Producto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GuarnicionesChurrasco",
                columns: table => new
                {
                    ChurrascoId = table.Column<int>(type: "int", nullable: false),
                    GuarnicionId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    EsExtra = table.Column<bool>(type: "bit", nullable: false),
                    PrecioEspecial = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    NotasPreparacion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OrdenPreferencia = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuarnicionesChurrasco", x => new { x.ChurrascoId, x.GuarnicionId });
                    table.ForeignKey(
                        name: "FK_GuarnicionesChurrasco_Guarniciones_GuarnicionId",
                        column: x => x.GuarnicionId,
                        principalTable: "Guarniciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GuarnicionesChurrasco_Producto_ChurrascoId",
                        column: x => x.ChurrascoId,
                        principalTable: "Producto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VentasItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VentaId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PorcentajeDescuento = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MontoDescuento = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NotasEspeciales = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TerminoCoccionSolicitado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GuarnicionesSeleccionadas = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EstadoItem = table.Column<int>(type: "int", nullable: false),
                    HoraInicioPreparacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HoraFinPreparacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TiempoPreparacionMinutos = table.Column<int>(type: "int", nullable: true),
                    TemperaturaServicio = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CaloriasCalculadas = table.Column<int>(type: "int", nullable: true),
                    CostoUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VentasItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VentasItems_Producto_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Producto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VentasItems_Ventas_VentaId",
                        column: x => x.VentaId,
                        principalTable: "Ventas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Guarniciones",
                columns: new[] { "Id", "CaloriasPorPorcion", "CantidadStock", "Descripcion", "Disponible", "EsPremium", "FechaCreacion", "FechaModificacion", "Nombre", "PrecioExtra", "StockMinimo", "TiempoPreparacionMinutos", "UnidadMedida" },
                values: new object[,]
                {
                    { 1, null, 100, null, true, false, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Frijoles", 0m, 20, 5, "porción" },
                    { 2, null, 50, null, true, false, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Chile de Árbol", 0m, 10, 5, "porción" },
                    { 3, null, 30, null, true, false, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Cebollín", 0m, 10, 5, "porción" },
                    { 4, null, 200, null, true, false, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Tortillas", 0m, 50, 5, "porción" },
                    { 5, null, 40, null, true, false, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Chirmol", 0m, 15, 5, "porción" }
                });

            migrationBuilder.InsertData(
                table: "Inventario",
                columns: new[] { "Id", "Activo", "Cantidad", "CodigoProveedor", "CostoPromedio", "FechaVencimiento", "Nombre", "Notas", "PrecioUnitario", "Proveedor", "PuntoReorden", "StockMaximo", "StockMinimo", "Tipo", "UbicacionAlmacen", "UltimaActualizacion", "Unidad" },
                values: new object[,]
                {
                    { 1, true, 50m, null, 20.00m, null, "Carne Puyazo", null, 25.00m, null, 15m, 100m, 10m, 0, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "libras" },
                    { 2, true, 40m, null, 24.00m, null, "Carne Culotte", null, 30.00m, null, 12m, 80m, 8m, 0, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "libras" },
                    { 3, true, 35m, null, 18.00m, null, "Costilla", null, 22.00m, null, 10m, 60m, 5m, 0, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "libras" },
                    { 4, true, 20m, null, 6.00m, null, "Carbón", null, 8.00m, null, 8m, 50m, 5m, 4, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "sacos" },
                    { 5, true, 15m, null, 4.50m, null, "Leña", null, 6.00m, null, 5m, 30m, 3m, 4, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "sacos" },
                    { 6, true, 100m, null, 0.30m, null, "Cajas 6 dulces", null, 0.50m, null, 30m, 200m, 20m, 3, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "unidades" },
                    { 7, true, 80m, null, 0.50m, null, "Cajas 12 dulces", null, 0.75m, null, 25m, 150m, 15m, 3, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "unidades" },
                    { 8, true, 60m, null, 0.70m, null, "Cajas 24 dulces", null, 1.00m, null, 20m, 120m, 10m, 3, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "unidades" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComboItems_ComboId",
                table: "ComboItems",
                column: "ComboId");

            migrationBuilder.CreateIndex(
                name: "IX_ComboItems_ProductoId",
                table: "ComboItems",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_GuarnicionesChurrasco_GuarnicionId",
                table: "GuarnicionesChurrasco",
                column: "GuarnicionId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventario_Cantidad",
                table: "Inventario",
                column: "Cantidad");

            migrationBuilder.CreateIndex(
                name: "IX_Inventario_Tipo",
                table: "Inventario",
                column: "Tipo");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientosInventario_InventarioItemId",
                table: "MovimientosInventario",
                column: "InventarioItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_Disponible",
                table: "Producto",
                column: "Disponible");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_Nombre",
                table: "Producto",
                column: "Nombre");

            migrationBuilder.CreateIndex(
                name: "IX_Ventas_Estado",
                table: "Ventas",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_Ventas_Fecha",
                table: "Ventas",
                column: "Fecha");

            migrationBuilder.CreateIndex(
                name: "IX_Ventas_NumeroOrden",
                table: "Ventas",
                column: "NumeroOrden",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ventas_TipoVenta",
                table: "Ventas",
                column: "TipoVenta");

            migrationBuilder.CreateIndex(
                name: "IX_VentasItems_ProductoId",
                table: "VentasItems",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_VentasItems_VentaId",
                table: "VentasItems",
                column: "VentaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComboItems");

            migrationBuilder.DropTable(
                name: "GuarnicionesChurrasco");

            migrationBuilder.DropTable(
                name: "MovimientosInventario");

            migrationBuilder.DropTable(
                name: "VentasItems");

            migrationBuilder.DropTable(
                name: "Guarniciones");

            migrationBuilder.DropTable(
                name: "Inventario");

            migrationBuilder.DropTable(
                name: "Producto");

            migrationBuilder.DropTable(
                name: "Ventas");
        }
    }
}
