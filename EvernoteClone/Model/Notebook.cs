using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvernoteClone.Model
{
    public class Notebook : IHasId<string>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public virtual User User { get; set; }
        public string Name { get; set; }
    }
}