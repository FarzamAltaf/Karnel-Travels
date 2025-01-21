using System.ComponentModel.DataAnnotations.Schema;

namespace kernel.Models
{
    public class PackageService
    {
        public int id { get; set; }

        public int packageId { get; set; }
        [ForeignKey("packageId")]
         public virtual Packages Package { get; set; }


        public int serviceId { get; set; }
        [ForeignKey("serviceId")]
        public virtual Services services { get; set; }
    }
}
