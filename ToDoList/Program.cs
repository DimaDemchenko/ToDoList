using Microsoft.AspNetCore.CookiePolicy;
using System.Data;
using System.Data.SqlClient;
using ToDoList.AutoMapper;
using ToDoList.Repository;
using ToDoList.Services;
using GraphQL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.Strict;
    options.HttpOnly = HttpOnlyPolicy.Always;
    options.Secure = CookieSecurePolicy.Always;
});

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<CookieService>();
builder.Services.AddTransient<IDbConnection>((sp) =>
new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<TaskProvider>();
builder.Services.AddAutoMapper(typeof(AppMappingProfile));

builder.Services.AddGraphQL((options) =>
{
    options.AddSchema<TaskSchema>();
    options.AddGraphTypes();
    options.AddErrorInfoProvider(e => e.ExposeExceptionDetails = true);
    options.AddSystemTextJson();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCookiePolicy();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ToDo}/{action=Index}/{id?}");



app.UseGraphQLAltair();

app.Run();

