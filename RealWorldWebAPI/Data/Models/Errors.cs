using System.Collections;
using System.Collections.Generic;

namespace RealWorldWebAPI.Data.Models
{
    public class Errors
    {
        public IEnumerable<string> Body { get; } = new List<string>();
    }
}