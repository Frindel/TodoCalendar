using EventsCalendar.Data.Directories;
using EventsCalendar.Data.Entities;
using EventsCalendar.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// регистрация сервисов
builder.Services.AddScoped<IDirectory<User>, UsersDirectory>();
builder.Services.AddScoped<IDirectory<Note>, NotesDirectory>();
builder.Services.AddScoped<IDirectory<Category>, CategoriesDirectory>();
builder.Services.AddScoped<IUserServise, UserServise>();
builder.Services.AddScoped<ITokenServise, TokenServise>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// настройка аутентификации
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true, // валидация издателя
			ValidIssuer = builder.Configuration["Jwt:Issuer"], // издатель
			ValidateAudience = true, // валидация потребителя токена
			ValidAudience = builder.Configuration["Jwt:Audience"], // потребитель
			ValidateLifetime = true, // валидация времени существования
			IssuerSigningKey =
				new SymmetricSecurityKey(
					Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"])), // ключ безопасности
			ValidateIssuerSigningKey = true, // валидация ключа безопасности
		};
	});

// настройка swagger
builder.Services.AddSwaggerGen(option =>
{
	option.SwaggerDoc("v1", new OpenApiInfo { Title = "Pathnostics", Version = "v1" });
	option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		In = ParameterLocation.Header,
		Description = "Please enter a valid token",
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		BearerFormat = "JWT",
		Scheme = "Bearer"
	});
	option.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			new string[] { }
		}
	});
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

app.Run();