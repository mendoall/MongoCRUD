using MongoDB.Bson;

namespace MongoBackEnd.Models
{
    public class User
    {       
        public ObjectId? _id { get; set; }
        public string? name { get; set; }
        public string? email { get; set; }
        public Int32? phone { get; set; }
        public string? address { get; set; }
        public DateTime? dateIn { get; set; }

        /// <summary>
        /// Guarda el valor del _id para enviarlo en el formulario
        /// </summary>
        public string idStr { get; set; }
    }
}
