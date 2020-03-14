using System;
using SimhereApp.Portable.Services;

namespace SimhereApp.iOS.Services
{
    public class AppService : IAppService
    {
        public AppService()
        {
        }

        public event EventHandler OnReult;
    }
}
