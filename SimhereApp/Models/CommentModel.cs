using SimHere.Entities;
using SimhereApp.Portable.Helpers;

namespace SimhereApp.Portable.Models
{
    public class CommentModel : Comment
    {
        public string TimeAgo => DateTimeHelper.TimeAgo(CreatedOn);
    }
    
}
