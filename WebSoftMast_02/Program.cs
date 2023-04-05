using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebSoftMast_02;
using WebSoftMast_02.Models;
using WebSoftMast_02.Tools;


#region ConfigureServices

    var builder = WebApplication.CreateBuilder(args);


    builder.Services.AddRazorPages();

    builder.Services.AddControllers();

    builder.Services.AddDbContext<DataContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("softMasterConnect")));


    builder.Services.AddDbContext<IdentityContext>(opts =>
        opts.UseSqlServer(builder.Configuration[
            "ConnectionStrings:IdentityConnection"]));

    builder.Services.AddIdentity<IdentityUser, IdentityRole>()
        .AddEntityFrameworkStores<IdentityContext>();

    builder.Services.Configure<IdentityOptions>(opts => {
        opts.Password.RequiredLength = 6;
        opts.Password.RequireNonAlphanumeric = false;
        opts.Password.RequireLowercase = false;
        opts.Password.RequireUppercase = false;
        opts.Password.RequireDigit = false;
        opts.User.RequireUniqueEmail = true;
        opts.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz";
    });


    builder.Services.AddAuthentication(opts =>
    {
        opts.DefaultScheme =
            CookieAuthenticationDefaults.AuthenticationScheme;
        opts.DefaultChallengeScheme =
            CookieAuthenticationDefaults.AuthenticationScheme;
    }).AddCookie(opts =>
    {
        opts.Events.DisableRedirectForPath(e => e.OnRedirectToLogin,
            "/api", StatusCodes.Status401Unauthorized);
        opts.Events.DisableRedirectForPath(e => e.OnRedirectToAccessDenied,
            "/api", StatusCodes.Status403Forbidden);

    });

#endregion


#region Configure

    var app = builder.Build();


    if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
        }


    app.UseDefaultFiles();
    app.UseStaticFiles();

    //app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();



    app.MapRazorPages();

    app.MapControllers();


    await SeedData.Load_SeedDataAsync();

    IdentitySeedData.CreateAdminAccount(app.Services, app.Configuration);

app.Run();


#endregion