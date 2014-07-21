using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace SolidEdge.MouseEvents
{
    public class IDispatchHelper
    {
        const int LOCALE_SYSTEM_DEFAULT = 2048;

        /// <summary>
        /// Using IDispatch, determine the managed type of the specified object.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static Type GetManagedType(object o)
        {
            Type type = null;
            IDispatch dispatch = o as IDispatch;
            ITypeInfo typeInfo = null;
            IntPtr pTypeAttr = IntPtr.Zero;
            System.Runtime.InteropServices.ComTypes.TYPEATTR typeAttr = default(System.Runtime.InteropServices.ComTypes.TYPEATTR);

            try
            {
                if (dispatch != null)
                {
                    typeInfo = dispatch.GetTypeInfo(0, LOCALE_SYSTEM_DEFAULT);
                    typeInfo.GetTypeAttr(out pTypeAttr);
                    typeAttr = (System.Runtime.InteropServices.ComTypes.TYPEATTR)Marshal.PtrToStructure(pTypeAttr, typeof(System.Runtime.InteropServices.ComTypes.TYPEATTR));

                    // Type can technically be defined in any loaded assembly.
                    Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

                    // Scan each assembly for a type with a matching GUID.
                    foreach (Assembly assembly in assemblies)
                    {
                        type = assembly.GetTypes().Where(x => x.GUID.Equals(typeAttr.guid)).FirstOrDefault();

                        if (type != null)
                        {
                            // Found what we're looking for so break out of the loop.
                            break;
                        }
                    }
                }
            }
            catch
            {
            }
            finally
            {
                if (typeInfo != null)
                {
                    typeInfo.ReleaseTypeAttr(pTypeAttr);
                    Marshal.ReleaseComObject(typeInfo);
                }
            }

            return type;
        }
    }
}
