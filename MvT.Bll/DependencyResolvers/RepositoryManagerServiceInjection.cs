using Microsoft.Extensions.DependencyInjection;
using MvT.Bll.Abstract;
using MvT.Bll.Concretes;
using MvT.Dal.Repositories.Abstract;
using MvT.Dal.Repositories.Abstract.Stok;
using MvT.Dal.Repositories.Concretes.MySqlAdoRepostory.Main;
using MvT.Dal.Repositories.Concretes.MySqlAdoRepostory.Stok;
using Org.BouncyCastle.Asn1.X509.Qualified;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvT.Bll.DependencyResolvers
{
    public static class RepositoryManagerServiceInjection
    {
        public static IServiceCollection AddRepositoryManagerServices(this IServiceCollection services)
        {
          
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddScoped(typeof(IManager<>), typeof(BaseManager<>));
            services.AddScoped(typeof(ICategoryRepository),typeof (CategoryRepostory));
            return services;
        }

    }
}
