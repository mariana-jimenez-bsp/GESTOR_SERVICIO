using BSP.POS.Presentacion;
using BSP.POS.Presentacion.Interfaces.Clientes;
using BSP.POS.Presentacion.Interfaces.Informes;
using BSP.POS.Presentacion.Services.Clientes;
using BSP.POS.Presentacion.Services.Informes;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IClientesInterface, ClientesService>();
builder.Services.AddScoped<IInformesInterface, InformesService>();

await builder.Build().RunAsync();
