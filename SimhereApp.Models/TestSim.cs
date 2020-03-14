using System;
namespace SimhereApp.Models
{
    public class TestSim
    {
        public string Id { get; set; }
        public string OwnerId { get; set; }
        public string DisplayNumber { get; set; }
        public string PrefixNumber { get; set; }
        public Int16 CarrierId { get; set; }
        public Int16? SubcribeTypeId { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        //public ICollection<Sim_NumberType> Sim_NumberTypes { get; set; }
        //public ICollection<FollowSim> FollowSims { get; set; }
        //public Users Owner { get; set; }
        public int? Status { get; set; }
        public string Description { get; set; }
    }
}
