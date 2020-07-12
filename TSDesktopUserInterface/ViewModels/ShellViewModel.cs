using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSDesktopUserInterface.EventModels;
using TSDesktopUserInterfaceLibrary.Models;

namespace TSDesktopUserInterface.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        
        private IEventAggregator _events;
        private SalesViewModel _salesVM;
        private ILoggedInUserModel _user;



        public ShellViewModel( IEventAggregator events,
            SalesViewModel salesView, ILoggedInUserModel user)
        {
            _events = events;
            _salesVM = salesView;
            _user = user;
            
            //This is needed to be added to know that this listen for events
            _events.Subscribe(this);

            //Every time a LoginViewModel is requested a new one is create.
            //This make sure that no information is stored for the login screnen.
            //IoC is from caliburn micro that let us talk to an instance.
            ActivateItem(IoC.Get<LoginViewModel>());


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
            TryClose();
        }

        public void LogOut()
        {
            _user.LogOffUser();
            ActivateItem(IoC.Get<LoginViewModel>());
            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        //This handles the event after it is subscribed.
        public void Handle(LogOnEvent message)
        {
            ActivateItem(_salesVM);
            NotifyOfPropertyChange(() => IsLoggedIn);
        }
    }
}
