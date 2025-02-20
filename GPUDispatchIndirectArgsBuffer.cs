using System.Runtime.InteropServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace Abecombe.GPUUtils
{
    public class GPUDispatchIndirectArgsBuffer : GPUBufferBase<uint>
    {
        public override GraphicsBuffer.Target BufferTarget => GraphicsBuffer.Target.IndirectArguments;

        public override int Length => 3;

        public GPUBufferBase<uint> CountBuffer { get; private set; }
        public int CountBufferOffset { get; private set; }
        public int CountBufferSize { get; private set; }

        public GraphicsBuffer DispatchThreadSizeBuffer { get; private set; }

        public void Init(GPUBufferBase<uint> countBuffer, int countBufferOffset, int countBufferSize)
        {
            Dispose();
            InitBufferCs();
            DispatchThreadSizeBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured | GraphicsBuffer.Target.Raw, 1, Marshal.SizeOf(typeof(uint3)));
            CountBuffer = countBuffer;
            CountBufferOffset = countBufferOffset;
            CountBufferSize = math.clamp(countBufferSize, 1, 3);
            Inited = true;
        }

        public void UpdateBuffer(uint3 threadGroupSize)
        {
            var cs = GPUUtilsCs;
            var kernel = cs.FindKernel(GPUStaticValues.BuildDispatchIndirectKernelName);

            kernel.SetBuffer(GPUStaticValues.CountBufferShaderPropertyID, CountBuffer);
            kernel.SetBuffer(GPUStaticValues.DispatchThreadSizeBufferShaderPropertyID, DispatchThreadSizeBuffer);
            kernel.SetBuffer(GPUStaticValues.DispatchIndirectArgsBufferShaderPropertyID, Data);
            cs.SetInt(GPUStaticValues.CountBufferOffsetShaderPropertyID, CountBufferOffset);
            cs.SetInt(GPUStaticValues.CountBufferSizeShaderPropertyID, CountBufferSize);
            cs.SetInts(GPUStaticValues.ThreadGroupSizeShaderPropertyID, threadGroupSize);

            kernel.DispatchDesired(3);
        }
        public void UpdateBuffer(CommandBuffer cb, uint3 threadGroupSize)
        {
            var cs = GPUUtilsCs;
            var kernel = cs.FindKernel(GPUStaticValues.BuildDispatchIndirectKernelName);

            kernel.SetBuffer(cb, GPUStaticValues.CountBufferShaderPropertyID, CountBuffer);
            kernel.SetBuffer(cb, GPUStaticValues.DispatchThreadSizeBufferShaderPropertyID, DispatchThreadSizeBuffer);
            kernel.SetBuffer(cb, GPUStaticValues.DispatchIndirectArgsBufferShaderPropertyID, Data);
            cs.SetInt(cb, GPUStaticValues.CountBufferOffsetShaderPropertyID, CountBufferOffset);
            cs.SetInt(cb, GPUStaticValues.CountBufferSizeShaderPropertyID, CountBufferSize);
            cs.SetInts(cb, GPUStaticValues.ThreadGroupSizeShaderPropertyID, threadGroupSize);

            kernel.DispatchDesired(cb, 3);
        }

        public override void Dispose()
        {
            if (Inited)
            {
                Data.Release();
                Data = null;
                DispatchThreadSizeBuffer.Release();
                DispatchThreadSizeBuffer = null;
            }
            Inited = false;
        }
    }

    public static class GPUDispatchIndirectArgsBufferExtensions
    {
        public static void DispatchIndirectDesired(this GPUKernel kernel, GPUDispatchIndirectArgsBuffer argsBuffer, bool updateBuffer = true)
        {
            if (updateBuffer) argsBuffer.UpdateBuffer(kernel.ThreadGroupSizes);

            kernel.SetBuffer(GPUStaticValues.DispatchThreadSizeBufferShaderPropertyID, argsBuffer.DispatchThreadSizeBuffer);
            kernel.Cs.DispatchIndirect(kernel, argsBuffer);
        }

        public static void DispatchIndirectDesired(this GPUKernel kernel, CommandBuffer cb, GPUDispatchIndirectArgsBuffer argsBuffer, bool updateBuffer = true)
        {
            if (updateBuffer) argsBuffer.UpdateBuffer(cb, kernel.ThreadGroupSizes);

            kernel.SetBuffer(cb, GPUStaticValues.DispatchThreadSizeBufferShaderPropertyID, argsBuffer.DispatchThreadSizeBuffer);
            kernel.Cs.DispatchIndirect(cb, kernel, argsBuffer);
        }
    }
}