// ����Ͻ� 
using BusinessLayer.Services;
using DataAccessLayer.Mappers;

internal class Program
{
    private static void Main(string[] args)
    {
        // ������ü ����
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // MVC���Ͽ��� ��Ʈ�ѷ��� �並 ����� �� �ֵ��� ����
        builder.Services.AddControllersWithViews();
        // DI ���� (transient : �Ź� ���Ӱ� ����, scopped : �ϳ��� ��û���� �ϳ��� ��ü, singleton: ��� ��û�� �ϳ��� ��ü)
        builder.Services.AddScoped<IUserService, UserService>(); // dependency injection

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