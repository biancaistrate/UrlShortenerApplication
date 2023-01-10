using Domain.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public interface ITinyUrlCreator
    {
        Uri Create(Uri domain, string alias = null);
    }
}
