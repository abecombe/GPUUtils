using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Abecombe.GPUUtils
{
    // You should layout the struct correctly to match the hlsl constant buffer layout.
    // See https://maraneshi.github.io/HLSL-ConstantBufferLayoutVisualizer/
    public class GPUConstantBuffer<T> : GPUBufferBase<T>
    {
        public override GraphicsBuffer.Target BufferTarget => GraphicsBuffer.Target.Constant;

        public override int Length => 1;

        public void Init()
        {
            Dispose();
            InitBufferCs();
            Inited = true;
        }

        public override void Dispose()
        {
            if (Inited)
            {
                Data.Release();
                Data = null;
            }
            Inited = false;
        }
    }

    public static class GPUConstantBufferExtensions
    {
        /// This method must be called inside an `unsafe` block.
        ///
        /// Preconditions:
        /// - The byte size of `srcArray` must be equal to the size of `fixedByteArray`.
        ///
        /// Example usage:
        /// ```csharp
        /// unsafe
        /// {
        ///     fixed (byte* destBytesPtr = fixedByteArray)
        ///     {
        ///         srcArray.CopyToFixedBytesWithPtr(destBytesPtr, 0, srcArray.Length);
        ///     }
        /// }
        /// ```
        public static unsafe void CopyToFixedBytesWithPtr<T>(this T[] srcArray, byte* destBytesPtr, int offset = 0, int count = -1) where T : struct
        {
            if (count == -1)
            {
                count = srcArray.Length - offset;
            }
            if (count <= 0 || offset < 0 || offset + count > srcArray.Length)
            {
                Debug.LogError("Invalid offset or count");
                return;
            }

            int structByteSize = Marshal.SizeOf(typeof(T));
            IntPtr ptr = Marshal.AllocHGlobal(structByteSize);

            try
            {
                byte* src = (byte*)ptr;
                for (int i = offset; i < count + offset; i++)
                {
                    Marshal.StructureToPtr(srcArray[i], ptr, false);
                    byte* dest = destBytesPtr + i * structByteSize;

                    for (int j = 0; j < structByteSize; j++)
                    {
                        dest[j] = src[j];
                    }
                }
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }
    }
}