using System.Linq;
using System.Reflection;

namespace Routing_Mechanism.Business
{
    public static class TypeConversion<T,T2>
    {
        public static TResult Conversion<T, TResult>(T model)  where TResult:class ,new() //where parametresi ile class geliceğine dair veri oluşturulmuş olucaktır ve  bu parametre ile işlem sağlanıcaktır
        {
            TResult result = new TResult();
            typeof(T).GetProperties().ToList().ForEach(p =>
            {
                PropertyInfo property = typeof(TResult).GetProperty(p.Name);
                property.SetValue(result,p.GetValue(model));

            });

            return result;
        }
    }
}
