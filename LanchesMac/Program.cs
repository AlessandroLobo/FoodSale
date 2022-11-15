
using FastReport.Data;
using LanchesMac.Areas.Admin.Servicos;
using LanchesMac.Context;
using LanchesMac.Models;
using LanchesMac.Repositories;
using LanchesMac.Repositories.Interfaces;
using LanchesMac.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);


FastReport.Utils.RegisteredObjects.AddConnection(typeof(MsSqlDataConnection));

builder.Services.AddDbContext<AppDbContext>(options => options
               .UseSqlServer(builder.Configuration
               .GetConnectionString("DefaultConnection")));

// Configurando Identity pra gerenciar loguin
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<ConfigurationImagens>(builder.Configuration.GetSection("ConfigurationPastaImagens"));
//builder.builder.Services.Configure<IdentityOptions>(options =>
//{
//    // Default Password settings.
//    options.Password.RequireDigit = true;
//    options.Password.RequireLowercase = true;
//    options.Password.RequireNonAlphanumeric = true;
//    options.Password.RequireUppercase = true;
//    options.Password.RequiredLength = 6;
//    options.Password.RequiredUniqueChars = 1;
//});


// Criando instancia da injeção de dependência
builder.Services.AddTransient<ILancheRepository, LancheRepository>();
builder.Services.AddTransient<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddTransient<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<ISeedUserRoleInitial, SeedUserRoleInitial>();
builder.Services.AddScoped<RelatorioVendasService>();
builder.Services.AddScoped<GraficoVendasService>();
builder.Services.AddScoped<RelatorioLanchesService>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin",
        politica =>
        {
            politica.RequireRole("Admin");

        });
});

// Relacionado ao cash
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped(sp => CarrinhoCompra.GetCarrinho(sp));

builder.Services.AddControllersWithViews();

// Configura o pacote ReflectioniT para paginação
builder.Services.AddPaging(options =>
{
    options.ViewName = "Bootstrap4";
    options.PageParameterName = "pageindex";
});

//Habilitando o cash
builder.Services.AddMemoryCache();
builder.Services.AddSession();

// registra serviços
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseFastReport();
app.UseRouting();

CriaPerfisUsuarios(app);

// Cria os perfis
//seedUserRoleInitial.SeedRoles();

// Cria os usuários e atributos ao perfil
//seedUserRoleInitial.SeedUsers();

//Habilitando cash
app.UseSession();

// Configura Identity para gerenciar loguin
app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{


    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}"
    );


    endpoints.MapControllerRoute(
    name: "categoriaFiltro",
    pattern: "Lanche/{action}/{categoria?}",
    defaults: new { Controller = "Lanche", action = "List" });

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

//configura o pipeline do request da aplicação

app.Run();

//substitui serviço seed da antiga classe startup
void CriaPerfisUsuarios(WebApplication app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
    using (var scope = scopedFactory.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<ISeedUserRoleInitial>();
        service.SeedUsers();
        service.SeedRoles();
    }
}