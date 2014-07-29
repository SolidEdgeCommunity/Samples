using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace ApiSamples
{
    [ComImport()]
    [Guid("00020400-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IDispatch
    {
        int GetTypeInfoCount();
        ITypeInfo GetTypeInfo(int iTInfo, int lcid);
    }
}
