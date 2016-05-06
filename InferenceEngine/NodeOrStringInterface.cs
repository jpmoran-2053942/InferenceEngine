using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    interface NodeOrStringInterface
    {
        Node GetNode();
        bool IsEqualTo(string checkValue);
        bool IsANode();
    }
}
