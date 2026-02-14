using AnagramSolver.Contracts;
using AnagramSolver.BusinessLogic;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IWordRepository>(_ =>new WordRepository("zodynas.txt"));

builder.Services.AddSingleton<MultipleAnagramFinder>();
builder.Services.AddSingleton<IAnagramSolver>(provider =>
{
    var solver = provider.GetRequiredService<MultipleAnagramFinder>();
    var solverCached = new AnagramCacheDecorator(solver);
    var solverLogged = new AnagramLoggingDecorator(solverCached);    
    return solverLogged;
});
builder.Services.AddSingleton<InputValidationPipeline>();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
