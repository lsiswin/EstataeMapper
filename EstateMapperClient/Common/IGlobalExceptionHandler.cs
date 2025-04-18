using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateMapperClient.Common
{
    public interface IGlobalExceptionHandler
    {
        void HandleException(Exception ex);
    }
}
