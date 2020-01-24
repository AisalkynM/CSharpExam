using System.ComponentModel.DataAnnotations;

namespace Exam.Models
{
    public class Participation
    {
        [Key]
        public int ParticipationId {get; set;}
        public int UserId {get; set; }
        public int BananaId {get; set;}

        public User Guest {get; set;}
        public Banana Attending {get; set;}
    }
}