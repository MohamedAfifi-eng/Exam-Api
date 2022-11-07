using Exam_Api.Model;
using Exam_Api.Repos;
using Exam_Api.Repos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.Text;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
string cors = "cors";
// Add services to the container.
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers(options =>
{
    var jsonInputFormatter = options.InputFormatters
        .OfType<Microsoft.AspNetCore.Mvc.Formatters.SystemTextJsonInputFormatter>()
        .Single();
    jsonInputFormatter.SupportedMediaTypes.Add("multipart/form-data");
    jsonInputFormatter.SupportedMediaTypes.Add("application/csp-report");

});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(cors,
    builder =>
    {
        builder.WithOrigins("*");
        builder.WithHeaders("*");
        builder.WithMethods("*");
        //builder.AllowAnyOrigin();
        //builder.AllowAnyMethod();
        //builder.AllowAnyHeader();
    });
});
// For Entity Framework  
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("def1")));

builder.Services.AddTransient<IExamService, ExamService>();
builder.Services.AddTransient<IQuestionService, QuestionService>();

// For Identity  
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(op =>
{
    op.Password.RequireNonAlphanumeric = false;
    op.Password.RequireUppercase = false;
    op.Password.RequireLowercase = false;
    op.Password.RequireDigit = false;
    op.Password.RequiredLength = 5;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Adding Authentication  
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Adding Jwt Bearer  
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateLifetime = false,
        ValidateIssuer = false,
        ValidateAudience = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(cors);
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
