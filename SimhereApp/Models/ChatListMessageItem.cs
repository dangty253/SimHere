using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SimHere.Entities;
using SimhereApp.Portable.Helpers;
using SimhereApp.Portable.Settings;

namespace SimhereApp.Portable.Models
{
    public class ChatListMessageItem : MessageItem, INotifyPropertyChanged
    {
        public Users Sender { get; set; }
        public Users Receive { get; set; }

        public Users UserTo
        {
            get
            {
                Users UserToChat = null;
                if (SenderId == UserLogged.Id)
                {
                    UserToChat = Receive;
                }
                else
                {
                    UserToChat = Sender;
                }

                return UserToChat;
            }
        }


        public void FireMessageChange()
        {
            OnPropertyChanged(nameof(MessageContent));
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
