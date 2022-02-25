using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HavaDurumu.Models
{
    public class Response
    {
        public Response()
        {
            Result = new List<Result>();
        }

        public bool Success { get; set; }
        public string City { get; set; }

        public virtual ICollection<Result> Result { get; set; }
    }
}
