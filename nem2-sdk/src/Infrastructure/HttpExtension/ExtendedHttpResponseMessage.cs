using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace io.nem2.sdk.src.Infrastructure.HttpExtension
{
    public class ExtendedHttpResponseMessege<T>
    {
        public HttpResponseMessage Response { get; set; }
        public T ComposedResponse { get; set; }
    }
}
