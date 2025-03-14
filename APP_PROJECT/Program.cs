// ����Ͻ� 
using BusinessLayer.Services;
using DataAccessLayer.Mappers;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        // �� ���ø����̼��� �����ϴ� ���� ��ü ����
        var builder = WebApplication.CreateBuilder(args);

        // appsettings.json�� "SiteInfo" ������ SiteInfo ��ü�� ���ε�
        builder.Services.Configure<SiteInfo>(builder.Configuration.GetSection("SiteInfo"));

        // MVC���Ͽ��� ��Ʈ�ѷ��� �並 ����� �� �ֵ��� ����
        builder.Services.AddControllersWithViews();

        // DI ���� (transient : �Ź� ���Ӱ� ����, scopped : �ϳ��� ��û���� �ϳ��� ��ü, singleton: ��� ��û�� �ϳ��� ��ü)
        builder.Services.AddSingleton<IUserService, UserService>(); // dependency injection

        // ADO.NET
        builder.Services.AddSingleton<IMemberMapper, MemberMapper>(); // dependency injection

        // InMemory
        // builder.Services.AddSingleton<IMemberMapper, InMemoryMemberMapper>(); // dependency injection

        // File ��� 1
        //builder.Services.AddSingleton<IMemberMapper>(serviceProvider =>
        //{
        //    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        //    var filePath = configuration["FileSettings:FilePath"];
        //    return new FileMemberMapper(filePath);
        //});

        // File ��� 2
        // builder.Services.AddSingleton<IMemberMapper, FileMemberMapper>();

        // Dapper
        // builder.Services.AddSingleton<IMemberMapper, DapperMemberMapper>(); // dependency injection

        // EntityFrameworkCore
        // appsettings.json���� "MyDatabase" Ű�� ���� ���ڿ��� ������
        var connectionString = builder.Configuration.GetConnectionString("MyDatabase");

        // AddDbContext<ApplicationDbContext>: SQL Server�� ����ϴ� ApplicationDbContext�� ���
        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

        // ������ ������ ����ؼ� ���ø����̼� ��ü ����
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            // ����ó�������� Ȱ��ȭ
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            // HTTPS ���� ���� (���� ��ȭ)
            app.UseHsts();
        }

        // HTTP ��û�� HTTPS�� �����̷���
        app.UseHttpsRedirection();
        // ���������� �����ϵ��� ����
        app.UseStaticFiles();
        // ����� �̵鿡�� �߰��Ͽ� URL ��û�� �����
        app.UseRouting();
        // ������ ����ڸ� ��û�� ó���ϵ��� ����
        app.UseAuthorization();

        // ��Ʈ�ѷ� ��� ���Ʈ ���� (�⺻�� ���� User/UserList)
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=User}/{action=UserList}");

        // ���ø����̼� ����
        app.Run();
    }
}