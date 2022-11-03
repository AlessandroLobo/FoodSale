namespace LanchesMac
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //criamos o m todo de extens o CreateAdminRole
            CreateHostBuilder(args)
               .Build()
               //.CreateAdminRole()
               .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
