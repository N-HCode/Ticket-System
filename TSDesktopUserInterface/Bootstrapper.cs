using AutoMapper;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using TSDesktopUserInterface.Helpers;
using TSDesktopUserInterface.Models;
using TSDesktopUserInterface.ViewModels;
using TSDesktopUserInterfaceLibrary.API;
using TSDesktopUserInterfaceLibrary.Helpers;
using TSDesktopUserInterfaceLibrary.Models;
using TSDesktopUserInterfaceLibray.API;

namespace TSDesktopUserInterface
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer _container = new SimpleContainer();

        public Bootstrapper()
        {
            Initialize();

            ConventionManager.AddElementConvention<PasswordBox>(
            PasswordBoxHelper.BoundPasswordProperty,
            "Password",
            "PasswordChanged");
        }

        private IMapper ConfigureAutomapper()
        {
            //automapper. It is used to transform data in one model to another model
            //Maps the ProductModel to ProductDisplay Model, same with CartItemModel
            //We do this in the Configure because there will be some reflection
            //Once it done the mapping once, it will store how it was mapped in memory,
            //allowing automapper to map things quicker.
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductModel, ProductDisplayModel>();
                cfg.CreateMap<CartItemModel, CartItemDisplayModel>();
            });



            //this is the actual mapper which will use the config we created above.
            var output = config.CreateMapper();

            return output;

        }

        protected override void Configure()
        {


            //Dependency inject the mapper. Making it only have ne instance of the mapper
            //So it will be like a singleton
            _container.Instance(ConfigureAutomapper());


            //when we ask for container, it gives itself
            _container.Instance(_container)
                .PerRequest<IProductEndpoint, ProductEndpoint>()
                .PerRequest<IUserEndpoint, UserEndpoint>()
                .PerRequest<ISaleEndpoint, SaleEndpoint>();


            _container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>()
                .Singleton<IAPIHelper, APIHelper>()
                .Singleton<IConfigHelper, ConfigHelper>()
                .Singleton<ILoggedInUserModel, LoggedInUserModel>();

            GetType().Assembly.GetTypes()
                .Where(type => type.IsClass)
                .Where(type => type.Name.EndsWith("ViewModel"))
                .ToList()
                .ForEach(viewModelType => _container.RegisterPerRequest(
                    viewModelType, viewModelType.ToString(), viewModelType));
        
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            // On startup, launch ShellViewModel as the base View.
            // Not starting the view because the ViewModel will launch the View.
            DisplayRootViewFor<ShellViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }
    }
}
