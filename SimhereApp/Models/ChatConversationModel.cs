using System;
using SimhereApp.Portable.ViewModels;

namespace SimhereApp.Portable.Models
{
    public class ChatConversationModel : BaseViewModel
    {
        string _id;
        public string Id { get => _id; set { _id = value; OnPropertyChanged(nameof(Id)); } }
        UserLite _receiver;
        public UserLite Receiver { get => _receiver; set { _receiver = value; OnPropertyChanged(nameof(Receiver)); } }
        string _latestContent;
        public string LatestContent { get => _latestContent; set { _latestContent = value; OnPropertyChanged(nameof(LatestContent)); } }
        DateTime _createdOn;
        public DateTime CreatedOn { get => _createdOn; set { _createdOn = value; OnPropertyChanged(nameof(CreatedOn)); } }
        DateTime _modifiedOn;
        public DateTime ModifiedOn { get => _modifiedOn; set { _modifiedOn = value; OnPropertyChanged(nameof(ModifiedOn)); } }
    }
}
