using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TUA.API.Dev
{
    class ReflectionExtension
    {
        public static void MethodSwap(Type OriginalMethodType, string OriginalMethodName, Type NewMethodType, string NewMethodName)
        {
            if (IntPtr.Size == 4)
            {
                MethodSwap32Bit(OriginalMethodType, OriginalMethodName, NewMethodType, NewMethodName);
                return;
            }
            MethodSwap64bit(OriginalMethodType, OriginalMethodName, NewMethodType, NewMethodName);
        }


        public static void MethodSwap32Bit(Type OriginalMethodType, string OriginalMethodName, Type NewMethodType, string NewMethodName)
        {
            MethodInfo OriginalMethod =
                OriginalMethodType.GetMethod(OriginalMethodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);
            MethodInfo NewMethod =
                NewMethodType.GetMethod(NewMethodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);

            RuntimeHelpers.PrepareMethod(OriginalMethod.MethodHandle);
            RuntimeHelpers.PrepareMethod(NewMethod.MethodHandle);

            IntPtr ptr = OriginalMethod.MethodHandle.Value + IntPtr.Size * 2;
            IntPtr ptr2 = NewMethod.MethodHandle.Value + IntPtr.Size * 2;
            int value;
            
            value = ptr.ToInt32();
            Marshal.WriteInt32(ptr, Marshal.ReadInt32(ptr2));
            Marshal.WriteInt32(ptr2, Marshal.ReadInt32(new IntPtr(value)));
            
        }

        public static unsafe void MethodSwap64bit(Type OriginalMethodType, string OriginalMethodName, Type NewMethodType, string NewMethodName)
        {
            MethodInfo OriginalMethod =
                OriginalMethodType.GetMethod(OriginalMethodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);
            MethodInfo NewMethod =
                NewMethodType.GetMethod(NewMethodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);
            RuntimeHelpers.PrepareMethod(OriginalMethod.MethodHandle);
            RuntimeHelpers.PrepareMethod(NewMethod.MethodHandle);

            long* inj = (long*)NewMethod.MethodHandle.Value.ToPointer() + 1;
            long* tar = (long*)OriginalMethod.MethodHandle.Value.ToPointer() + 1;
            *tar = *inj;
        }

        public static Delegate GetEventDelegate(Object obj, string evt)
        {
            Delegate del = null;
            FieldInfo fi = obj.GetType().GetField("OnClick",
                BindingFlags.NonPublic |
                BindingFlags.Static |
                BindingFlags.Instance |
                BindingFlags.FlattenHierarchy |
                BindingFlags.IgnoreCase);

            if (fi != null)
            {
                Object value = fi.GetValue(obj);
                if (value is Delegate)
                {
                    del = (Delegate)value;
                }
            }

            return del;
        }
    }
}
