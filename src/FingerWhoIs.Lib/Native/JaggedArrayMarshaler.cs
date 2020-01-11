using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FingerWhoIs.Lib.Native
{
    internal class JaggedArrayMarshaler : ICustomMarshaler
    {
        
        static JaggedArrayMarshaler marshaler;

        private class MarshalInfo
        {
            internal IntPtr[] InnerArrayPointers;
            internal GCHandle InnerArrayPointersHandle;
            internal GCHandle[] InnerArrayHandles;
        }

        readonly Dictionary<IntPtr, MarshalInfo> marshalMap = new Dictionary<IntPtr, MarshalInfo>();

        public object MarshalNativeToManaged(IntPtr pNativeData)
        {
            throw new NotImplementedException();
        }

        public IntPtr MarshalManagedToNative(object managedObj)
        {
            if (managedObj == null)
                return IntPtr.Zero;
            if (!(managedObj is byte[][]))
                throw new MarshalDirectiveException("This custom marshaler must be used on a double[][].");

            Array[] array = (Array[])managedObj;


            MarshalInfo mi = new MarshalInfo();

            mi.InnerArrayPointers = new IntPtr[array.Length];

            mi.InnerArrayPointersHandle = GCHandle.Alloc(mi.InnerArrayPointers, GCHandleType.Pinned);

            mi.InnerArrayHandles = new GCHandle[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                mi.InnerArrayHandles[i] = GCHandle.Alloc(array[i], GCHandleType.Pinned);
                mi.InnerArrayPointers[i] = mi.InnerArrayHandles[i].AddrOfPinnedObject();
            }

            IntPtr pointer = mi.InnerArrayPointersHandle.AddrOfPinnedObject();

            lock (marshalMap)
            {
                marshalMap.Add(pointer, mi);
            }

            return pointer;
        }


        public void CleanUpNativeData(IntPtr pNativeData)
        {
            MarshalInfo mi;
            lock (marshalMap)
            {
                mi = marshalMap[pNativeData];
                marshalMap.Remove(pNativeData);
            }

            for (int i = 0; i < mi.InnerArrayHandles.Length; i++)
            {
                mi.InnerArrayHandles[i].Free();
            }

            mi.InnerArrayPointersHandle.Free();
        }

        public void CleanUpManagedData(object managedObj){}

        public int GetNativeDataSize()
        {
            return -1;
        }

        public static ICustomMarshaler GetInstance(string cookie)
        {
            if (marshaler == null)
                return marshaler = new JaggedArrayMarshaler();
            return marshaler;
        }
    }
}