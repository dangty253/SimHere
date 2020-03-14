using System;
using SimhereApp.Portable.Models;

namespace SimhereApp.Portable.Services
{
    public interface IFacebookService
    {
        void Login(Action<FacebookUser, Exception> OnLoginCompleted);
        void Logout();
    }
}
