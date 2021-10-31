using System.Collections.Generic;

namespace Business.Models
{
    public record ReadResult
    {
        public List<Line>? Lines { get; set; }
    }
}