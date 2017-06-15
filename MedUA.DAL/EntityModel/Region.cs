namespace MedUA.DAL.EntityModel
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Region
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }
        public Oblast Oblast { get; set; }

        public virtual ICollection<Hospital> Hospitals { get; set; }
    }
}
