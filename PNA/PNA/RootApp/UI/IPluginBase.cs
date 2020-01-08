using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RootApp.UI
{
    public interface IPluginBase
    {
        void Init();

        void RunCommand(string strCommand);
    }
}
