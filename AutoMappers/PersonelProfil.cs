using AutoMapper;

namespace Routing_Mechanism.AutoMappers
{
    public class PersonelProfil : Profile
    {
        public PersonelProfil() 
        {
            //CreateMap<Personel,PersonelCreateVM>(); Her iki nesne içerisinde ortak olan propertilerin arasında atama yapıcaktır 
            //CreateMap<PersonelCreateVM,Personel>();
        }
    }
}
