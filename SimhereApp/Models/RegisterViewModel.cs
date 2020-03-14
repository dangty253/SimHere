using System;
using SimHere.Entities;
using SimhereApp.Portable.ViewModels;

namespace SimhereApp.Portable.Models
{
    public class RegisterViewModel : BaseViewModel
    {
        private string _fullName;
        public string FullName
        {
            get => _fullName;
            set
            {
                if (_fullName != value)
                {
                    _fullName = value;
                    OnPropertyChanged(nameof(FullName));
                }
            }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        private string _phone;
        public string Phone
        {
            get => _phone;
            set
            {
                if (_phone != value)
                {
                    _phone = value;
                    OnPropertyChanged(nameof(Phone));
                }
            }
        }

        private DateTime? _birthday;
        public DateTime? Birthday
        {
            get => _birthday;
            set
            {
                if (_birthday != value)
                {
                    _birthday = value;
                    OnPropertyChanged(nameof(Birthday));
                }
            }
        }

        public int? Sex;


        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        private OptionSet _sexOption;
        public OptionSet SexOption
        {
            get => _sexOption;
            set
            {
                if (_sexOption != value)
                {
                    _sexOption = value;
                    OnPropertyChanged(nameof(SexOption));
                }
            }
        }
    }
}
