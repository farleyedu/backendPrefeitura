using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using PortalTCMSP.Api.Middleware;
using PortalTCMSP.Api.Swagger;
using PortalTCMSP.Domain.DTOs.Requests.Keycloak;
using PortalTCMSP.Infra.Ioc;

var builder = WebApplication.CreateBuilder(args);

string[] allowedOrigins =
	(builder.Configuration["AllowAllOrigins"] ?? "")
	.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

builder.Services.AddControllers()
	.AddJsonOptions(o =>
		o.JsonSerializerOptions.Converters.Add(
			new System.Text.Json.Serialization.JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddTransient<IClaimsTransformation, MyClaimsTransformation>();

builder.Services.AddSwaggerGen(o =>
{
	o.SwaggerDoc("home", new OpenApiInfo { Title = "Home", Version = "1.0.0" });
	o.SwaggerDoc("noticia", new OpenApiInfo { Title = "Noticia", Version = "1.0.0" });
	o.SwaggerDoc("institucional", new OpenApiInfo { Title = "Institucional", Version = "1.0.0" });
	o.SwaggerDoc("sessoes-plenarias", new OpenApiInfo { Title = "Sessões Plenárias", Version = "1.0.0" });
	o.SwaggerDoc("fiscalizacao", new OpenApiInfo { Title = "Fiscalização", Version = "1.0.0" });
    o.SwaggerDoc("servicos", new OpenApiInfo { Title = "Servicos", Version = "1.0.0" });
    o.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "API de PortalTCMSP",
		Version = "1.0.0",
		Description = "Doc geral para endpoints sem GroupName."
	});

	string ToSlug(string g) => g switch
	{
		"Home" => "home",
		"Noticia" => "noticia",
		"Institucional" => "institucional",
		"SessoesPlenarias" => "sessoes-plenarias",
		"Fiscalizacao" => "fiscalizacao",
        "Servicos" => "servicos",
        _ => g.ToLowerInvariant()
	};

	o.DocInclusionPredicate((doc, api) =>
	{
		var g = api.GroupName;
		if (!string.IsNullOrWhiteSpace(g) && ToSlug(g) == doc)
			return true;

		if (string.IsNullOrWhiteSpace(g) && doc == "v1")
			return true;

		if (api.ActionDescriptor is ControllerActionDescriptor cad)
		{
			var actionGroups = cad.MethodInfo
				.GetCustomAttributes(typeof(ApiGroupsAttribute), true)
				.OfType<ApiGroupsAttribute>()
				.SelectMany(a => a.Groups)
				.Select(ToSlug)
				.ToHashSet();

			var controllerGroups = cad.ControllerTypeInfo
				.GetCustomAttributes(typeof(ApiGroupsAttribute), true)
				.OfType<ApiGroupsAttribute>()
				.SelectMany(a => a.Groups)
				.Select(ToSlug)
				.ToHashSet();

			if (actionGroups.Contains(doc) || controllerGroups.Contains(doc))
				return true;
		}

		return false;
	});

	o.TagActionsBy(api =>
	{
		if (api.ActionDescriptor is ControllerActionDescriptor cad)
			return new[] { cad.ControllerName };
		return new[] { "Outros" };
	});

	o.CustomSchemaIds(t => t.ToString());
	o.EnableAnnotations();
	o.UseAllOfToExtendReferenceSchemas();

    var jwtScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT como: Bearer {seu_token}"
    };
    o.AddSecurityDefinition("Bearer", jwtScheme);
    o.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        [jwtScheme] = Array.Empty<string>()
    });
});

builder.Services.AddCors(options =>
{
	options.AddPolicy("ConfiguredOrigins", p =>
	   p.WithOrigins(allowedOrigins)
		.AllowAnyHeader()
		.AllowAnyMethod()
   // .AllowCredentials() // se usar cookies/autenticação do browser
   );
});

builder.Services.ConfigureApplicationContext(builder.Configuration);
builder.Services.AddRouting(o =>
{
	o.LowercaseUrls = true;
	o.LowercaseQueryStrings = true;
});

builder.Services.AddOptions<AuthenticationConfigurationOptions>()
    .Bind(builder.Configuration.GetSection(AuthenticationConfigurationOptions.SectionPath))
    .ValidateDataAnnotations()
    .Validate(o => !string.IsNullOrWhiteSpace(o.Authority), "Authority is required")
    .ValidateOnStart();

builder.Services.AddCustomAuthentication(builder.Configuration);

var app = builder.Build();

app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
	c.SwaggerEndpoint("/swagger/v1/swagger.json", "PortalTCMSP - Geral");
	c.SwaggerEndpoint("/swagger/home/swagger.json", "Home");
	c.SwaggerEndpoint("/swagger/noticia/swagger.json", "Noticia");
	c.SwaggerEndpoint("/swagger/institucional/swagger.json", "Institucional");
	c.SwaggerEndpoint("/swagger/sessoes-plenarias/swagger.json", "Sessões Plenárias");
	c.SwaggerEndpoint("/swagger/fiscalizacao/swagger.json", "Fiscalização");
    c.SwaggerEndpoint("/swagger/servicos/swagger.json", "Servicos");

    c.RoutePrefix = "swagger";
	c.DefaultModelsExpandDepth(-1);
});

app.UseCors("ConfiguredOrigins");
app.UseAuthorization();
app.MapControllers();
app.Run();
