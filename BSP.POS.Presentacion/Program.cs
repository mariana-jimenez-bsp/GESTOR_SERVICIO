global using Microsoft.AspNetCore.Components.Authorization;

using Blazored.LocalStorage;
using BSP.POS.Presentacion;
using BSP.POS.Presentacion.Interfaces.Actividades;
using BSP.POS.Presentacion.Interfaces.Clientes;
using BSP.POS.Presentacion.Interfaces.Informes;
using BSP.POS.Presentacion.Interfaces.Licencias;
using BSP.POS.Presentacion.Interfaces.Observaciones;
using BSP.POS.Presentacion.Interfaces.Permisos;
using BSP.POS.Presentacion.Interfaces.Proyectos;
using BSP.POS.Presentacion.Interfaces.Usuarios;
using BSP.POS.Presentacion.Services.Actividades;
using BSP.POS.Presentacion.Services.Autorizacion;
using BSP.POS.Presentacion.Services.Clientes;
using BSP.POS.Presentacion.Services.Informes;
using BSP.POS.Presentacion.Services.Licencias;
using BSP.POS.Presentacion.Services.Observaciones;
using BSP.POS.Presentacion.Services.Permisos;
using BSP.POS.Presentacion.Services.Proyectos;
using BSP.POS.Presentacion.Services.Usuarios;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiUrl = "https://localhost:7032/api/";
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiUrl) });
builder.Services.AddScoped<IClientesInterface, ClientesService>();
builder.Services.AddScoped<IInformesInterface, InformesService>();
builder.Services.AddScoped<IUsuariosInterface, UsuariosService>();
builder.Services.AddScoped<IActividadesInterface, ActividadesService>();
builder.Services.AddScoped<IPermisosInterface, PermisosService>();
builder.Services.AddScoped<IProyectosInterface, ProyectosService>();
builder.Services.AddScoped<IObservacionesInterface, ObservacionesService>();
builder.Services.AddScoped<ILicenciasInterface, LicenciasService>();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddAuthorizationCore();

builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
