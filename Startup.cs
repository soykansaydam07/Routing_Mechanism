using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Routing_Mechanism.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routing_Mechanism
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();


            //MapControllerRoute: Custom olarak bizim istediğimiz şekilde oluşturulmasını sağladığımız route yapılanmasıdır 
            app.UseEndpoints(endpoints =>
            {
                //Default olarak verilen değer MapDefaultControllerRoute tarafı ile aynı şekilde olduğu gözükmekte
                //endpoints.MapControllerRoute(
                //    name: "default",
                //    pattern: "{controller=Home}/{action=Index}/{id?}");

                //Rota şablon varsa {} parametrelere karşılık gelmektedir , controller ve action parametreleri MapDefaultControllerRoute içinde tanmlanmıştır diğerleri ise custom anımlanmış anlamındadır
                //MapDefaultControllerRoute için hiç bir parametre olmadan bir tetikleme yapmak istiyorsam  controller home olarak action da index olarak tetiklenecektir 

                //MapDefaultControllerRoute: Default route oluşturmak için kullanılan yapıdır 
                //endpoints.MapDefaultControllerRoute();

                //Her bir controller route, default da bir ismi olması gerekmektedir name parametresi bundan kullanılır, Diğer pattern route bazında nasıl yönlenme yapılcağına dair verilen parametrelerdir 
                //endpoints.MapControllerRoute("Default", "{controller=Personel}/{action=Index}");

                //Bunun yanında parametre olarak default değer anasayfa olarak gelmesini istersek yapılması gereken ,3 parametre bir instance şeklinde  default değeri doldurulacaktır böylece farklı bir isimde default parametresi verilebilecektir  
                //Anasayfa parametresi verildiğinde default parametre çalıştırılıcaktır 
                //endpoints.MapControllerRoute("DefaultSecond", "anasayfa",new {controller = "Home", action = "Index"});

                //Birden fazla route yapılandırması oluşturucaksınız bu yapılanma özelden geneel doğru aşağıya yazılarak gitmesi sağlanmaktadir çünkü özel olan bu yapılandırmada kullanılamıyorsa genel olan çalıştırılarak sistemin hata vermesi önlenir 

                //Bu kısımda Route Constraints dediğimiz bir yapı ile parametre olarak tanımlanan verilerin  sadece int gelmesini sağlamak gibi durumlar için kullanılan yapıdır 
                //Mesala Aşağıdaki id parametresinini sadece id gelmesini sağlamak için bu yapı kullanılıcaktır , bildiğim kadarıyla string ifade yerine int kullanımı için genellikle yapılandırılır, diğer tarzlarda kullanımı mevcuttur
                //Bunun yanında route parametrelerine değerler verilebilmektedir  x gelen verinin  12 karakterli olmasını istedik aşağıdaki örnekte mesala min max gibi yapılar kullanılabilir 
                //endpoints.MapControllerRoute("DefaultThird", "{controller=Home}/{action=Index}/{id:int?}/{x:alpha:length(12)}/{y?}");

                //Bunun yanında CustomConstraint ile custom route yapılandırnalırı yapılabiliyor CustomContrait sınıfı oluşturulup IRouteConstrait interface ile Match metodu override edilir

                endpoints.MapDefaultControllerRoute();

                //Attribute Routing:  Geleneksel yaklaşım olarak startup cs tarafında yönetim sağladığımız merkezi hale getirdiğimiz durumlar , birde Attribute bazlı yaklaşım da controller bazlı hareket ile yapılandırma sağlama olayı
                /*endpoints.MapControllers(); */
                //Route için gelen isteği attribute taraflı  controller verileri ile eşleştir , bunu yapmadan controller daki rouelarda çalışma olmaz 
                //Controller bazlı tanımlamalar ["[controller]/[action]"] şeklinde tanımlı ifadeler köşeli tanımsızlar için süslü parantez, route için şablonlarda değişken için doğrudan süslü parantez kullanımı bulunmaktadır 

                //Api yapılandırmalırında  Controlelr bazlı Route kullanlırken mvc tarafında konvensiyonel bir yaklaşım sergilenmektedir 


                // CustomRouteHandler : Biz yapı olarak alınan request isteğini ilgili yapıda bir controller için göndermemiz gerekmekte  , fakat bunun yerine controller işlemi yapmadan bir şekilde handle edebilme olayına  denmektedir 
                //Belirli başlı işlemlerde controller yormak yerine resmi resize etmek gibi mesala durumlar controller yollanmadan da yapılabilir bu iş 

                //CustomRouteHandler tarafı aşağıdaki şekilde gibi olmaktadir
                //endpoints.Map("example-route",async c => 
                //{
                //    //https://localhost:5001/example-route endpoint'e gelen herhangi bir istek controller dan ziyade direk olarak buradaki fonksiyon tarafından karşılancaktır.
                //});

                //Yukarıdaki kısmı yazmak yerine bunu handler tarafında oluşturup devam etmekteyiz
                //endpoints.Map("example-route", new ExampleHandler().Handler());
                //Yukarıdaki şekilde artık işlemlerini yapmaya devam edeceğinden dolayı  Handler sınıfna  nasıl bir işlem yapması gerektiği yazıldığı işlem burada biticektir 
            });
        }
    }
}
