// 비즈니스 
using BusinessLayer.Services;
using DataAccessLayer.Mappers;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        // 웹 애플리케이션을 생성하는 빌더 객체 생성
        var builder = WebApplication.CreateBuilder(args);

        // appsettings.json의 "SiteInfo" 섹션을 SiteInfo 객체에 바인딩
        builder.Services.Configure<SiteInfo>(builder.Configuration.GetSection("SiteInfo"));

        // MVC패턴에서 컨트롤러와 뷰를 사용할 수 있도록 설정
        builder.Services.AddControllersWithViews();

        // DI 설정 (transient : 매번 새롭게 생성, scopped : 하나의 요청에는 하나의 객체, singleton: 모든 요청에 하나의 객체)
        builder.Services.AddSingleton<IUserService, UserService>(); // dependency injection

        // ADO.NET
        builder.Services.AddSingleton<IMemberMapper, MemberMapper>(); // dependency injection

        // InMemory
        // builder.Services.AddSingleton<IMemberMapper, InMemoryMemberMapper>(); // dependency injection

        // File 방법 1
        //builder.Services.AddSingleton<IMemberMapper>(serviceProvider =>
        //{
        //    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        //    var filePath = configuration["FileSettings:FilePath"];
        //    return new FileMemberMapper(filePath);
        //});

        // File 방법 2
        // builder.Services.AddSingleton<IMemberMapper, FileMemberMapper>();

        // Dapper
        // builder.Services.AddSingleton<IMemberMapper, DapperMemberMapper>(); // dependency injection

        // EntityFrameworkCore
        // appsettings.json에서 "MyDatabase" 키의 연결 문자열을 가져옴
        var connectionString = builder.Configuration.GetConnectionString("MyDatabase");

        // AddDbContext<ApplicationDbContext>: SQL Server를 사용하는 ApplicationDbContext를 등록
        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

        // 설정된 빌더를 사용해서 애플리케이션 객체 생성
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            // 예외처리페이지 활성화
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            // HTTPS 연결 강제 (보안 강화)
            app.UseHsts();
        }

        // HTTP 요청을 HTTPS로 리다이렉션
        app.UseHttpsRedirection();
        // 정적파일을 제공하도록 설정
        app.UseStaticFiles();
        // 라우팅 미들에어 추가하여 URL 요청을 라우팅
        app.UseRouting();
        // 인증된 사용자만 요청을 처리하도록 설정
        app.UseAuthorization();

        // 컨트롤러 기반 라우트 설정 (기본값 설정 User/UserList)
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=User}/{action=UserList}");

        // 애플리케이션 실행
        app.Run();
    }
}