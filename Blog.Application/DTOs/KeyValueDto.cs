using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.DTOs
{
    public class KeyValueDto<T>
    {
        public T Id { get; set; }
        public string Name { get; set; }
    }
}
