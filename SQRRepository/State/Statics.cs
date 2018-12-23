using System;
using System.Collections.Generic;
using System.Text;
using Joha.Interfaces;

namespace SQRRepository.State
{
public static class Statics
    {
        public static Dictionary<string, Dictionary<MethodType, UInt64>> Stat = new Dictionary<string, Dictionary<MethodType, ulong>>();
    }
}
