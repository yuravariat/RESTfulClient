using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTfulClient
{
    public interface IReturn
    {
    }
    public interface IReturn<T> : IReturn
    {
    }
}
