using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace SolidEdge.Common
{
    public static class ApplicationHelper
    {
        /// <summary>
        /// Creates and returns a new instance of Solid Edge.
        /// </summary>
        /// <returns>
        /// An object of type SolidEdgeFramework.Application.
        /// </returns>
        public static SolidEdgeFramework.Application Start()
        {
            // On a system where Solid Edge is installed, the COM ProgID will be
            // defined in registry: HKEY_CLASSES_ROOT\SolidEdge.Application
            Type t = Type.GetTypeFromProgID(progID: "SolidEdge.Application", throwOnError: true);

            // Using the discovered Type, create and return a new instance of Solid Edge.
            return (SolidEdgeFramework.Application)Activator.CreateInstance(type: t);
        }

        /// <summary>
        /// Connects to a running instance of Solid Edge.
        /// </summary>
        /// <returns>
        /// An object of type SolidEdgeFramework.Application.
        /// </returns>
        public static SolidEdgeFramework.Application Connect()
        {
            return Connect(startIfNotRunning: false);
        }

        /// <summary>
        /// Connects to or starts a new instance of Solid Edge.
        /// </summary>
        /// <param name="startIfNotRunning"></param>
        /// <returns>
        /// An object of type SolidEdgeFramework.Application.
        /// </returns>
        public static SolidEdgeFramework.Application Connect(bool startIfNotRunning)
        {
            try
            {
                // Attempt to connect to a running instance of Solid Edge.
                return (SolidEdgeFramework.Application)Marshal.GetActiveObject(progID: "SolidEdge.Application");
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                switch (ex.ErrorCode)
                {
                    // Solid Edge is not running.
                    case NativeMethods.MK_E_UNAVAILABLE:
                        if (startIfNotRunning)
                        {
                            // Start Solid Edge.
                            return Start();
                        }
                        else
                        {
                            // Rethrow exception.
                            throw;
                        }
                    default:
                        // Rethrow exception.
                        throw;
                }
            }
            catch
            {
                // Rethrow exception.
                throw;
            }
        }

        /// <summary>
        /// Connects to or starts a new instance of Solid Edge.
        /// </summary>
        /// <param name="startIfNotRunning"></param>
        /// <param name="ensureVisible"></param>
        /// <returns>
        /// An object of type SolidEdgeFramework.Application.
        /// </returns>
        public static SolidEdgeFramework.Application Connect(bool startIfNotRunning, bool ensureVisible)
        {
            SolidEdgeFramework.Application application = null;

            try
            {
                // Attempt to connect to a running instance of Solid Edge.
                application = (SolidEdgeFramework.Application)Marshal.GetActiveObject(progID: "SolidEdge.Application");
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                switch (ex.ErrorCode)
                {
                    // Solid Edge is not running.
                    case NativeMethods.MK_E_UNAVAILABLE:
                        if (startIfNotRunning)
                        {
                            // Start Solid Edge.
                            application = Start();
                            break;
                        }
                        else
                        {
                            // Rethrow exception.
                            throw;
                        }
                    default:
                        // Rethrow exception.
                        throw;
                }
            }
            catch
            {
                // Rethrow exception.
                throw;
            }

            if ((application != null) && (ensureVisible))
            {
                application.Visible = true;
            }

            return application;
        }
    }

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

    public class InstallDataHelper
    {
        public static string GetInstalledPath()
        {
            /* Get path to Solid Edge program directory.  Typically, 'C:\Program Files\Solid Edge XXX\Program'. */
            DirectoryInfo programDirectory = new DirectoryInfo(GetProgramFolderPath());

            /* Get path to Solid Edge installation directory.  Typically, 'C:\Program Files\Solid Edge XXX'. */
            DirectoryInfo installationDirectory = programDirectory.Parent;

            return installationDirectory.FullName;
        }

        public static string GetProgramFolderPath()
        {
            SEInstallDataLib.SEInstallData installData = new SEInstallDataLib.SEInstallData();

            /* Get path to Solid Edge program directory.  Typically, 'C:\Program Files\Solid Edge XXX\Program'. */
            return installData.GetInstalledPath();
        }

        public static string GetTrainingFolderPath()
        {
            /* Get path to Solid Edge training directory.  Typically, 'C:\Program Files\Solid Edge XXX\Training'. */
            DirectoryInfo trainingDirectory = new DirectoryInfo(Path.Combine(GetInstalledPath(), "Training"));

            return trainingDirectory.FullName;
        }

        public static Version GetVersion()
        {
            SEInstallDataLib.SEInstallData installData = new SEInstallDataLib.SEInstallData();

            return new Version(installData.GetMajorVersion(), installData.GetMinorVersion(), installData.GetServicePackVersion(), installData.GetBuildNumber());
        }
    }

    public static class ReflectionHelper
    {
        /// <summary>
        /// Returns the Solid Edge object type by invoking the 'Name' property.
        /// </summary>
        /// <param name="o"></param>
        /// <returns>System.String</returns>
        public static string GetPropertyValueAsString(object o, string propertyName)
        {
            // Using .NET reflection, attempt to obtain the Name value.
            var val = o.GetType().InvokeMember(propertyName, BindingFlags.GetProperty, null, o, null);

            return val.ToString();
        }

        /// <summary>
        /// Returns the Solid Edge object type by invoking the 'Type' property.
        /// </summary>
        /// <param name="o"></param>
        /// <returns>SolidEdgeFramework.ObjectType</returns>
        public static SolidEdgeFramework.ObjectType GetObjectType(object o)
        {
            // Using .NET reflection, attempt to obtain the Type value.
            var val = o.GetType().InvokeMember("Type", BindingFlags.GetProperty, null, o, null);

            return (SolidEdgeFramework.ObjectType)val;
        }

        /// <summary>
        /// Returns the Solid Edge Part feature modeling mode by invoking the 'ModelingModeType' property.
        /// </summary>
        /// <param name="o"></param>
        /// <returns>SolidEdgePart.ModelingModeConstants</returns>
        public static SolidEdgePart.ModelingModeConstants GetPartFeatureModelingMode(object o)
        {
            // Using .NET reflection, attempt to obtain the ModelingModeType value.
            var val = o.GetType().InvokeMember("ModelingModeType", BindingFlags.GetProperty, null, o, null);

            return (SolidEdgePart.ModelingModeConstants)val;
        }

        /// <summary>
        /// Returns the Solid Edge Part feature type by invoking the 'Type' property.
        /// </summary>
        /// <param name="o"></param>
        /// <returns>SolidEdgePart.FeatureTypeConstants</returns>
        public static SolidEdgePart.FeatureTypeConstants GetPartFeatureType(object o)
        {
            // Using .NET reflection, attempt to obtain the Type value.
            var val = o.GetType().InvokeMember("Type", BindingFlags.GetProperty, null, o, null);

            return (SolidEdgePart.FeatureTypeConstants)val;
        }

        /// <summary>
        /// Returns the Solid Edge value of the object by invoking the 'Value' property.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static object GetObjectValue(object o)
        {
            // Using .NET reflection, attempt to obtain the Value value.
            return o.GetType().InvokeMember("Value", BindingFlags.GetProperty, null, o, null);
        }
    }
}
