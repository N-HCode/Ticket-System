using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSDesktopUserInterface.EventModels;
using TSDesktopUserInterface.Helpers;
using TSDesktopUserInterfaceLibray.API;

namespace TSDesktopUserInterface.ViewModels
{
    public class LoginViewModel : Screen
    {
        private string _userName = "Test@Test.Test";
        private string _password = "Test1234$";
        private IAPIHelper _apiHelper;
        private IEventAggregator _events;

        public LoginViewModel(IAPIHelper apiHelper, IEventAggregator events)
        {
            _apiHelper = apiHelper;
            _events = events;
        }

        public string UserName
        {
            get { return _userName; }
            set 
            { 
                _userName = value;
                NotifyOfPropertyChange(() => UserName);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }


        public string Password
        {
            get { return _password; }
            set 
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }

  

        public bool IsErrorVisible
        {
            get 
            {
                bool output = false;

                if(ErrorMessage?.Length > 0)
                {
                    output = true;
                }

                return output; 
            }

        }

        private string _errorMessage;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set 
            {
                _errorMessage = value;
                NotifyOfPropertyChange(() => IsErrorVisible);
                NotifyOfPropertyChange(() => ErrorMessage);
            }
        }



        //CanLogIn references the LogIn button name in the LoginView
        public bool CanLogIn
        {
            get
            {

                bool output = false;

                if (UserName?.Length > 0 && Password?.Length >0)
                {
                    output = true;
                }

                return output;
            }
        }

        public async Task LogIn()
        {
            try
            {
                ErrorMessage = "";
                var result = await _apiHelper.Authenticate(UserName, Password);

                //Capture more information about the user
                await _apiHelper.GetLoggedInUserInfo(result.Access_Token);

                //Pusblish anything (in this case a class) and see if anyone is looking
                //for that published item. Using a class will differiate it from other
                //calls making it obvious which event is happening.
                _events.PublishOnUIThread(new LogOnEvent());
            }
            catch (Exception ex)
            {

                ErrorMessage = ex.Message;
            }
        }


    }
}
