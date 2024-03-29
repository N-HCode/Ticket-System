﻿using System.Net.Http;
using System.Threading.Tasks;
using TSDesktopUserInterfaceLibrary.Models;

namespace TSDesktopUserInterfaceLibray.API
{
    public interface IAPIHelper
    {
        Task<AuthenticatedUser> Authenticate(string username, string password);

        void LogOffUser();

        Task GetLoggedInUserInfo(string token);

        HttpClient ApiClient { get; }
    }
}