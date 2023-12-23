using System.ComponentModel.DataAnnotations;

namespace Transportmk2.Models
{
    public class Storage
    {
        [Key]
        public int Storage_id { get; set; }
        public string Storage_name { get; set; }

        public string Longitude { get; set; }

        public string Latitude { get; set; }

        public int Capacity { get; set; }

        public int Needed { get; set; }
    }
}
