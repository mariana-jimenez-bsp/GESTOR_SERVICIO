global using Microsoft.AspNetCore.Components.Authorization;

using Blazored.LocalStorage;
using BSP.POS.Presentacion;
using BSP.POS.Presentacion.Interfaces.Actividades;
using BSP.POS.Presentacion.Interfaces.Clientes;
using BSP.POS.Presentacion.Interfaces.Informes;
using BSP.POS.Presentacion.Interfaces.ItemsCliente;
using BSP.POS.Presentacion.Interfaces.Licencias;
using BSP.POS.Presentacion.Interfaces.Lugares;
using BSP.POS.Presentacion.Interfaces.Observaciones;
using BSP.POS.Presentacion.Interfaces.Permisos;
using BSP.POS.Presentacion.Interfaces.Proyectos;
using BSP.POS.Presentacion.Interfaces.Reportes;
using BSP.POS.Presentacion.Interfaces.Usuarios;
using BSP.POS.Presentacion.Interfaces.CodigoTelefonoPais;
using BSP.POS.Presentacion.Interfaces.Departamentos;
using BSP.POS.Presentacion.Interfaces.Alertas;
using BSP.POS.Presentacion.Services.Actividades;
using BSP.POS.Presentacion.Services.Autorizacion;
using BSP.POS.Presentacion.Services.Clientes;
using BSP.POS.Presentacion.Services.Informes;
using BSP.POS.Presentacion.Services.ItemsCliente;
using BSP.POS.Presentacion.Services.Licencias;
using BSP.POS.Presentacion.Services.Lugares;
using BSP.POS.Presentacion.Services.Observaciones;
using BSP.POS.Presentacion.Services.Permisos;
using BSP.POS.Presentacion.Services.Proyectos;
using BSP.POS.Presentacion.Services.Reportes;
using BSP.POS.Presentacion.Services.Usuarios;
using BSP.POS.Presentacion.Services.CodigoTelefonoPais;
using BSP.POS.Presentacion.Services.Departamentos;
using BSP.POS.Presentacion.Services.Alertas;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiUrl = "https://localhost:7032/api/";
//var apiUrl = "https://localhost/POS_Prueba_API_Gestor_Servicios/Api/";
//var apiUrl = "https://192.168.2.21/POS_Prueba_API_Gestor_Servicios/Api/";

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiUrl) });
builder.Services.AddScoped<IClientesInterface, ClientesService>();
builder.Services.AddScoped<IInformesInterface, InformesService>();
builder.Services.AddScoped<IUsuariosInterface, UsuariosService>();
builder.Services.AddScoped<ILoginInterface, LoginService>();
builder.Services.AddScoped<IActividadesInterface, ActividadesService>();
builder.Services.AddScoped<IPermisosInterface, PermisosService>();
builder.Services.AddScoped<IProyectosInterface, ProyectosService>();
builder.Services.AddScoped<IObservacionesInterface, ObservacionesService>();
builder.Services.AddScoped<ILicenciasInterface, LicenciasService>();
builder.Services.AddScoped<ILugaresInterface, LugaresService>();
builder.Services.AddScoped<IItemsClienteInterface, ItemsClienteService>();
builder.Services.AddScoped<IReportesInterface, ReportesService>();
builder.Services.AddScoped<ICodigoTelefonoPaisInterface, CodigoTelefonoPaisService>();
builder.Services.AddScoped<IDepartamentosInterface, DepartamentosService>();
builder.Services.AddScoped<IAlertasInterface, AlertasService>();
builder.Services.AddSweetAlert2();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddAuthorizationCore();

builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
