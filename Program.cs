namespace ERP;

public class Program
{
    public static string WebRootPath;
    public static string ContentRootPath;
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var env = builder.Environment;
        var app = builder.Build();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        // 5.添加配置中间件
        app.UseStaticFiles();

        WebRootPath = env.WebRootPath;
        ContentRootPath = env.ContentRootPath;
        app.Use(async (context, next) =>
        {
            try
            {
                await next.Invoke();
            }
            catch (System.Exception ex)
            {
                context.Response.WriteAsync(ex.ToString());
            }
        });

        // app.Use((context, next) =>
        // {
        //     context.Request.EnableBuffering();
        //     return next();
        // });
        // app.UseRouting();

        app.Map("/Home", HandleMapHome);
        app.Map("/API", HandleMapAPI);
        app.Map("", ProcessRequest);


        app.Run();
    }

    // 2.添加公用路径过滤
    private static void ProcessRequest(IApplicationBuilder app)
    {
        app.Run(async context =>
        {
            string url = "http://" + context.Request.Host + "/Home";
            context.Response.StatusCode = 307;
            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            context.Response.Headers.Add("Location", url);
        });
    }

    // 3.添加API中间件
    private static void HandleMapAPI(IApplicationBuilder app)
    {
        app.Run(async context =>
        {
            context.Response.Headers.Add("Content-Type", "text/html; charset=utf-8");
            await BC_APICommand.ProcessAPIResult(context, app);
        });
    }

    // 4.添加Home中间件
    private static void HandleMapHome(IApplicationBuilder app)
    {
        app.Run(async context =>
        {
            context.Response.Headers.Add("Content-Type", "text/html; charset=utf-8");
            await BC_Home.ProcessRequest(context, app);
        });
    }

}
