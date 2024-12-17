var builder = WebApplication.CreateBuilder(args);

// Configurar servicios para controladores
builder.Services.AddControllers();

// Configurar Swagger (opcional, solo para desarrollo)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCorsPolicy", policy =>
    {
        policy.WithOrigins("http://127.0.0.1:5500") // URL donde esté el frontend
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("DevCorsPolicy");

if (app.Environment.IsDevelopment())
{
    // Swagger solo en entorno de desarrollo
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
