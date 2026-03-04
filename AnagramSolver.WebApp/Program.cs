using AnagramSolver.BusinessLogic;
using AnagramSolver.Contracts;
using AnagramSolver.EF.CodeFirst.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AnagramDbContext>(options =>
    options.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=AnagramSolver_CF;Trusted_Connection=True;TrustServerCertificate=True;"));
builder.Services.AddTransient<IWordRepository, DbWordRepository>();

builder.Services.AddScoped<MultipleAnagramFinder>();
builder.Services.AddScoped<IAnagramSolver>(provider =>
{
    var solver = provider.GetRequiredService<MultipleAnagramFinder>();
    var solverCached = new AnagramCacheDecorator(solver);
    var solverLogged = new AnagramLoggingDecorator(solverCached);
    return solverLogged;
});

builder.Services.AddSingleton<ChatHistoryStorage>();

builder.Services.AddScoped<Kernel>(sp =>
{
    var kernelBuilder = Kernel.CreateBuilder();

    kernelBuilder.AddOpenAIChatCompletion(
        modelId: builder.Configuration["OpenAI:Model"]!,
        apiKey: builder.Configuration["OpenAI:ApiKey"]!);

    var anagramSolver = sp.GetRequiredService<IAnagramSolver>();

    kernelBuilder.Plugins.AddFromObject(new AnagramPlugin(anagramSolver), "AnagramPlugin");
    kernelBuilder.Plugins.AddFromObject(new MathPlugin(), "MathPlugin");
    kernelBuilder.Plugins.AddFromObject(new RandomNrPlugin(), "RandomNrPlugin");

    return kernelBuilder.Build();
});

builder.Services.AddScoped<IAiChatService, AiChatService>();
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