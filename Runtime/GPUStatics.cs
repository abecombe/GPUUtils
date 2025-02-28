using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Abecombe.GPUUtils
{
    public static class GPUStatics
    {
        // for AsyncCompute
        public static bool SimulationUseBuffer1 { get; private set; } = true;
        public static bool RenderingUseBuffer1 => !SimulationUseBuffer1;

        public static void SwapSimulationRenderingBuffer()
        {
            SimulationUseBuffer1 = !SimulationUseBuffer1;
        }

        // for GPUComputeShader
        internal const string DirectDispatch = "DIRECT_DISPATCH";
        internal const string IndirectDispatch = "INDIRECT_DISPATCH";

        // for GPUKernel
        internal static readonly int DispatchThreadSizeShaderPropertyID = Shader.PropertyToID("_DispatchThreadSize");

        // for GPUBufferBase
        internal const string UtilsShaderName = "GPUUtils";
        internal const string CopyBuffer1KernelName = "CopyBuffer1";
        internal const string CopyBuffer32KernelName = "CopyBuffer32";
        internal const string CopyBuffer128KernelName = "CopyBuffer128";
        internal const string CopyBuffer1024KernelName = "CopyBuffer1024";
        internal const string ClearBuffer1KernelName = "ClearBuffer1";
        internal const string ClearBuffer32KernelName = "ClearBuffer32";
        internal const string ClearBuffer128KernelName = "ClearBuffer128";
        internal const string ClearBuffer1024KernelName = "ClearBuffer1024";
        internal static readonly int BufferCountShaderPropertyID = Shader.PropertyToID("_BufferCount");
        internal static readonly int BufferUIntCountShaderPropertyID = Shader.PropertyToID("_BufferUIntCount");
        internal static readonly int FromBufferUIntStartIndexShaderPropertyID = Shader.PropertyToID("_FromBufferUIntStartIndex");
        internal static readonly int ToBufferUIntStartIndexShaderPropertyID = Shader.PropertyToID("_ToBufferUIntStartIndex");
        internal static readonly int FromBufferShaderPropertyID = Shader.PropertyToID("_FromBuffer");
        internal static readonly int ToBufferShaderPropertyID = Shader.PropertyToID("_ToBuffer");
        internal static readonly int BufferShaderPropertyID = Shader.PropertyToID("_Buffer");

        // for GPUStructuredBuffer
        internal static readonly string[] StructuredBufferConcatNames = { "", "Length", "Size", "StartIndex", "EndIndex", "IndexToPosition", "PositionToIndex", "PositionToFloorIndex" };

        // for GPUIndirectArgumentsBuffer
        internal static readonly string[] IndirectArgumentsBufferConcatNames = { "", "CountBufferOffset", "CountBufferSize" };

        // for GPUDispatchIndirectArgsBuffer
        internal const string BuildDispatchIndirectKernelName = "BuildDispatchIndirect";
        internal static readonly int CountBufferShaderPropertyID = Shader.PropertyToID("_CountBuffer");
        internal static readonly int DispatchThreadSizeBufferShaderPropertyID = Shader.PropertyToID("_DispatchThreadSizeBuffer");
        internal static readonly int DispatchIndirectArgsBufferShaderPropertyID = Shader.PropertyToID("_DispatchIndirectArgsBuffer");
        internal static readonly int CountBufferOffsetShaderPropertyID = Shader.PropertyToID("_CountBufferOffset");
        internal static readonly int CountBufferSizeShaderPropertyID = Shader.PropertyToID("_CountBufferSize");
        internal static readonly int ThreadGroupSizeShaderPropertyID = Shader.PropertyToID("_ThreadGroupSize");

        // for GPUAppendConsumeBuffer
        internal static readonly string[] AppendConsumeBufferConcatNames = { "", "CountBuffer" };

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