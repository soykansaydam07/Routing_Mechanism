using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Routing_Mechanism.Extensions;
using Routing_Mechanism.Handlers;
using Routing_Mechanism.Services;
using Routing_Mechanism.Services.Interfaces;
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
        public void ConfigureServices(IServiceCollection services) // Ioc yapılanmasını sağlayan bir yapıdır, IServiceCollection  da  ilgili IoC yapılanması sağlanacaktır 
        {

            //services.Add(new ServiceDescriptor(typeof(ConsoleLog), new ConsoleLog())); //Add fonsiyonu çok kullanmaz bunun yerine daha dedike bir kullanım vardır 
            //services.Add(new ServiceDescriptor(typeof(TextLog), new TextLog()));
            //services.Add(new ServiceDescriptor(typeof(TextLog), new TextLog(), ServiceLifetime.Transient));

            //services.AddSingleton<ConsoleLog>(); //new T() şeklinde baz alınır 
            //services.AddSingleton<ConsoleLog>(p => new ConsoleLog(5)); // Parametre olarak gelicek bir cons yapısı barsa bu şekilde kullanılıcaktır .

            services.AddScoped<ILog>(p => new ConsoleLog(5)); // Artık bağlantılı yapı oluğu için Interface tarafına ait ilgili class tarafının instance için oluşturulması yeterli olucaktır 
            //services.AddScoped<ILog, TextLog>(); // Bu şekilde kullanılması için Ilist den türetilmiş olmalı ve constracter tarafında  bir parametre yoksa default değer varsa  kullanılır 
            
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); //Exception page tarafına yönlendirilmesini istersek yazmaktayız 
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            //app.Run(async c => {async c.Response.WriteAsync("Middleware 1"):})); Kısadevre için oluşturulmuştur

            //app.Use(async (context, task) =>    // Bu kısımdaki task bir sonraki middleware tarafını temsil edicektir invoke yapısı ile bu kısım diğer middleware ayağa kaldırılıcaktır 
            //{
            //await task.invoke()
            //});

            //app.Map("/temp", builder =>  // İf şartı gbi path anlamında yapılandırma kullanmak için kullanılır
            //{
            //  builder.Run(async c => {async c.Response.WriteAsync("Middleware 1"):}))
            //}); 

            //app.MapWhen(c => c.Request.Method == "GET", builder => {    // Sadece path olmayıp daha farklı özelliklere göre filitrelemesi sağlandı.

            //    app.Use(async (context, task) =>
            //    {
            //        await task.Invoke();
            //    });
            //});

            app.UseRouting(); //Route işlemlerinde gelen  rotanın yapılandırılmasını sağlayan middleware

            app.UseAuthorization();


            //MapControllerRoute: Custom olarak bizim istediğimiz şekilde oluşturulmasını sağladığımız route yapılanmasıdır 
            app.UseEndpoints(endpoints =>
            {

                //Route Mekanizması


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


                //Middleware pipelime yapılandırılması 


                //Web uygulamsında requeste verilicek response a kadara arada farklı işlemler ile sürceim gidişatına farklı yön vermek için aralara eklenebilen pipeline mekanizmalrıdır 
                //Mekanizma sarmla şekilde işlem yapmaktadır ilk middleware tetiklendi logic kısmı yapıldı sonrasısında ikinci middleware tarafına gider taki middleware bitene kadar , sonrasında en iç middleware
                //işlemi logic tarafını bitirip bir üst middleware tarafına döndüğünde o da logini yapıp bir üstekine dönücek şekilde işlemlerini yapıcaktır 
                //.net yapılanması middleware yapılanmasını desteklicektir startup ya da program cs tarafında configure sisteminin içindeki tüm yapılanma middleware yapılı bir yaklaşımdır denebilir
                //.net tarafında middleware yapıları use adıyla başlar Configure metodu içinde tanımlıdır tetiklenme sırası derleyici satırına göre yapılıcaktr 

                //Hazır miidleware lar (yani çekirdek anlamında işlem yapılmasını sağlayan defauşt da olan middleware lar  Run ,Use Map ,MapWhen) şeklindedir , .Net tarafında hazır yapılardır

                //Run Metodu : Kendisinden sonra gelen middlewareı tetiklemez , haliyle kendisinden sonraki middleware tarafını tetiklemiyeceği için o anki çıktıyı vericektir bu yapıya shortcircuit denir
                //Use Metodu : Ana middleware yapısı olarakda geçebilir kendisinden bir sonraki metodu invoke ederek devam eden yapıdır 
                //Map Metodu : Talep gönderilen path yapısına göre filtrelemek isteyebiliriz bunun için Use ya da run fonksiyonlarında if kontrolü sağlayabilir ya da map metodu ile yaklaşım sergilenir 
                //MapWhen Metodu : Request tarafında pathe gmre map metodunda filitreleme yaparken mapwhen tarafında gelen requestin herhangi bir çzelliğine göre yapılıcaktır 

                //Bunun yanında custom miidleware oluşturmak için ,
                //Yazılan middleware bir class olup extension olarak IApplicationBuilder a entegre edersek kendi custom middleware yapımız oluşucaktır 
                //İlk olarak Middleware yapısı oluşturulucak daha sonra bu kısma extension eklenebilecek şekle getirilicek aşağıdaki şekilde custom yapı kullanılabilecektir. 
                //app.UseHello();


                //Dependency Injection ve IoC Yapılandırılması (Invercion Of Control)


                //Aslında ikisi farklı tasarım desenleri denilebilir, 
                //Dependancy Injection için, 
                //Bağımlılık oluşturacak parçaların ayrılıp , bunların dışarıdan verilmesiyle sistem içerisine dahil etme durumudur , dependancy Invercion yapıs bir prensip olp bu prensibi tam olara kuygulamaya çalışan patter dir aslında 
                //Yapı da bağımlılık new operetörü ile diğer sınıftan instance oluşturmaktan oluyor , bunun yerine bu sınıf bir ana Interface den türetilse ve cons tarafında parametre olarak bu interface parametre olarak alınırsa bu bağımlılık ortadan kalkıcaktır 
                //IoC Yapılandırması için , 
                //Bizim dependancyInjection ile yaptığımız yapıda bu yapının çok karmaşık haline geldiğini düşünürsek , yani A class ı hem b hem c hem d,e,f gibi bir çok kısımdan inject alıyorsa  karmaşayı engellemek için oluştuurlan conteynerlardır
                //Bu konteynerlarda genel olarak , dictonary bir kolleksiyon yapısında bağımlılıklar vardır ,  oluşan instanceların nasıl oluşması gerektiği gibi konuların configüre edilmesi durumları buraya eklenir 
                //.net tarafında bu ioc yapılanması sistemin içinde zaten bulunmaktadır ve ismide built-in şeklindedir , içerisine koyulacak değerleri 3 farklı davranışla içeri alabilmektedir 
                //Singleton : Uygulama bazında tekil nesne oluşturur ve tüm taleplere bu nesne ile cevap verir
                //Scoped : Her request için bir nesne üretiler ve orequest de olan tüm isteklere o nesneyi gönderir 
                //Transient : Her requestin her talebine karşılık  bir nesne üretilir ve en maliyetli olan kısım budur
                //default olarak eklenen servislerin yaşaç döngüleri singleton olarak eklenicektir 


                //Area Nedir ve ne amaçla kullanılır ?


                //Web uygulamasında, farklı işlevsellikleri ayırmak için kullanılan özelliktir Genellikle bu yapı , kullanıcı ve admin panel olarak ayarlanma anlamında kullanılabilir
                //Her bir area view ve modal katmanı olucaktır . Yöetim panellerinde , farklı role kısımlarında mantıksal olarak ayrılabilen durumlarda kullanılabilir
                //Mvc yaklaşımında kullanılmaktadır 
                //Area içindeki controller area attribute yapısı ile işaretlenmelidir 
                //Sonrasında Area yapılarına route belirlemesi yapmak gerekmektedir. Aşağıdaki şekilde bir rota tanımlaması yapılmış olmaktadır 

                endpoints.MapControllerRoute( //Default area rotası belirlememizi sağlamaktadır
                    name:"AdminPanel",
                    pattern:"{area:exists}/{controller=Home}/{action=Index}/{id?}"
                    );
                //MapAreaControllerRoute fonsiyonu ile özel route yapıları area için tanımlana bilecektir.
                endpoints.MapAreaControllerRoute(
                    name: "AdminPanel",
                    areaName: "AdminPanel",
                    pattern: "AdminPanel/{controller=Home}/{action=Index}/{id?}"

                    );
                //Arealar arası bağlantı oluşturma  için TagHelpers ve Html Helpers ile ActionLink veya asp-area yapılandırması kullanılarak bu yapı sağlanmaktadır 

                //ViewModal yapısı : genel olarak front tarafına gönderim sırasında çoklu obje oluşturma , ve istenen verieri ön tarafa gönderme gibi konularda kullanılır
                //ViewModal içinde function yapısıda olabilir 
            });
        }
    }
}
