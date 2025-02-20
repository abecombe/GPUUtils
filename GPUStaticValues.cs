using UnityEngine;

namespace Abecombe.GPUUtils
{
    public static class GPUStaticValues
    {
        // for AsyncCompute
        public static bool SimulationUseBuffer1 = true;
        public static bool RenderingUseBuffer1 => !SimulationUseBuffer1;

        public static void SwapSimulationRenderingBuffer()
        {
            SimulationUseBuffer1 = !SimulationUseBuffer1;
        }

        // for GPUComputeShader
        public const string DirectDispatch = "DIRECT_DISPATCH";
        public const string IndirectDispatch = "INDIRECT_DISPATCH";

        // for GPUKernel
        public static readonly int DispatchThreadSizeShaderPropertyID = Shader.PropertyToID("_DispatchThreadSize");

        // for GPUBufferBase
        public const string UtilsShaderName = "GPUUtils";
        public const string CopyBuffer1KernelName = "CopyBuffer1";
        public const string CopyBuffer32KernelName = "CopyBuffer32";
        public const string CopyBuffer128KernelName = "CopyBuffer128";
        public const string CopyBuffer1024KernelName = "CopyBuffer1024";
        public const string ClearBuffer1KernelName = "ClearBuffer1";
        public const string ClearBuffer32KernelName = "ClearBuffer32";
        public const string ClearBuffer128KernelName = "ClearBuffer128";
        public const string ClearBuffer1024KernelName = "ClearBuffer1024";
        public static readonly int BufferCountShaderPropertyID = Shader.PropertyToID("_BufferCount");
        public static readonly int BufferUIntCountShaderPropertyID = Shader.PropertyToID("_BufferUIntCount");
        public static readonly int FromBufferUIntStartIndexShaderPropertyID = Shader.PropertyToID("_FromBufferUIntStartIndex");
        public static readonly int ToBufferUIntStartIndexShaderPropertyID = Shader.PropertyToID("_ToBufferUIntStartIndex");
        public static readonly int FromBufferShaderPropertyID = Shader.PropertyToID("_FromBuffer");
        public static readonly int ToBufferShaderPropertyID = Shader.PropertyToID("_ToBuffer");
        public static readonly int BufferShaderPropertyID = Shader.PropertyToID("_Buffer");

        // for GPUStructuredBuffer
        public static readonly string[] StructuredBufferConcatNames = { "", "Length", "Size", "StartIndex", "EndIndex", "IndexToPosition", "PositionToIndex", "PositionToFloorIndex" };

        // for GPUIndirectArgumentsBuffer
        public static readonly string[] IndirectArgumentsBufferConcatNames = { "", "CountBufferOffset", "CountBufferSize" };

        // for GPUDispatchIndirectArgsBuffer
        public const string BuildDispatchIndirectKernelName = "BuildDispatchIndirect";
        public static readonly int CountBufferShaderPropertyID = Shader.PropertyToID("_CountBuffer");
        public static readonly int DispatchThreadSizeBufferShaderPropertyID = Shader.PropertyToID("_DispatchThreadSizeBuffer");
        public static readonly int DispatchIndirectArgsBufferShaderPropertyID = Shader.PropertyToID("_DispatchIndirectArgsBuffer");
        public static readonly int CountBufferOffsetShaderPropertyID = Shader.PropertyToID("_CountBufferOffset");
        public static readonly int CountBufferSizeShaderPropertyID = Shader.PropertyToID("_CountBufferSize");
        public static readonly int ThreadGroupSizeShaderPropertyID = Shader.PropertyToID("_ThreadGroupSize");

        // for GPUAppendConsumeBuffer
        public static readonly string[] AppendConsumeBufferConcatNames = { "", "CountBuffer" };
    }
}