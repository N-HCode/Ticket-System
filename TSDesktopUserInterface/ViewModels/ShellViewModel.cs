using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TSDesktopUserInterface.EventModels;
using TSDesktopUserInterfaceLibrary.Models;
using TSDesktopUserInterfaceLibray.API;

namespace TSDesktopUserInterface.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        
        private IEventAggregator _events;
        private SalesViewModel _salesVM;
        private ILoggedInUserModel _user;
        private IAPIHelper _apiHelper;



        public ShellViewModel( IEventAggregator events,
            SalesViewModel salesView, ILoggedInUserModel user,
            IAPIHelper apiHelper)
        {
            _events = events;
            _salesVM = salesView;
            _user = user;
            _apiHelper = apiHelper;
            
            //This is needed to be added to know that this listen for events
            _events.SubscribeOnPublishedThread(this);

            //Every time a LoginViewModel is requested a new one is create.
            //This make sure that no information is stored for the login screnen.
            //IoC is from caliburn micro that let us talk to an instance.
            ActivateItemAsync(IoC.Get<LoginViewModel>(), new CancellationToken());


        }

        public bool IsLoggedIn
        {
            get
            {
                bool output = false;

                if (string.IsNullOrWhiteSpace(_user.Token) == false)
                {
                    output = true;
                }

                return output;
            }

        }

        public void ExitApplication()
        {
            TryCloseAsync();
        }

        public async Task UserManagement()
        {
            await ActivateItemAsync(IoC.Get<UserDisplayViewModel>(), new CancellationToken());
        }

        public async Task LogOut()
        {
            _user.ResetUserModel();
            _apiHelper.LogOffUser();
            await ActivateItemAsync(IoC.Get<LoginViewModel>(), new CancellationToken());
            NotifyOfPropertyChange(() => IsLoggedIn);
            
        }

        ////This handles the event after it is subscribed.
        //public void Handle(LogOnEvent message)
        //{
        //    ActivateItem(_salesVM);
        //    NotifyOfPropertyChange(() => IsLoggedIn);
        //}

        public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {

            await ActivateItemAsync(_salesVM, cancellationToken);
            NotifyOfPropertyChange(() => IsLoggedIn);
        }
    }
}
