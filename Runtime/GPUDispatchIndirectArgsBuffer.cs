using System.Runtime.InteropServices;
using Unity.Collections;
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
            using (var array = new NativeArray<uint>(new[] { 1u, 1u, 1u }, Allocator.Temp))
            {
                SetData(array);
            }
            DispatchThreadSizeBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Raw, 3, Marshal.SizeOf(typeof(uint)));
            using (var array = new NativeArray<uint>(new[] { 0u, 0u, 0u }, Allocator.Temp))
            {
                DispatchThreadSizeBuffer.SetData(array);
            }
            CountBuffer = countBuffer;
            CountBufferOffset = countBufferOffset;
            CountBufferSize = math.clamp(countBufferSize, 1, 3);
            Inited = true;
        }

        public void UpdateBuffer(uint3 threadGroupSize)
        {
            var cs = GPUUtilsCs;
            var kernel = cs.FindKernel(GPUStatics.BuildDispatchIndirectKernelName);

            kernel.SetBuffer(GPUStatics.CountBufferShaderPropertyID, CountBuffer);
            kernel.SetBuffer(GPUStatics.DispatchThreadSizeBufferShaderPropertyID, DispatchThreadSizeBuffer);
            kernel.SetBuffer(GPUStatics.DispatchIndirectArgsBufferShaderPropertyID, Data);
            cs.SetInt(GPUStatics.CountBufferOffsetShaderPropertyID, CountBufferOffset);
            cs.SetInt(GPUStatics.CountBufferSizeShaderPropertyID, CountBufferSize);
            cs.SetInts(GPUStatics.ThreadGroupSizeShaderPropertyID, threadGroupSize);

            kernel.DispatchDesired(3);
        }
        public void UpdateBuffer(CommandBuffer cb, uint3 threadGroupSize)
        {
            var cs = GPUUtilsCs;
            var kernel = cs.FindKernel(GPUStatics.BuildDispatchIndirectKernelName);

            kernel.SetBuffer(cb, GPUStatics.CountBufferShaderPropertyID, CountBuffer);
            kernel.SetBuffer(cb, GPUStatics.DispatchThreadSizeBufferShaderPropertyID, DispatchThreadSizeBuffer);
            kernel.SetBuffer(cb, GPUStatics.DispatchIndirectArgsBufferShaderPropertyID, Data);
            cs.SetInt(cb, GPUStatics.CountBufferOffsetShaderPropertyID, CountBufferOffset);
            cs.SetInt(cb, GPUStatics.CountBufferSizeShaderPropertyID, CountBufferSize);
            cs.SetInts(cb, GPUStatics.ThreadGroupSizeShaderPropertyID, threadGroupSize);

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

            kernel.SetBuffer(GPUStatics.DispatchThreadSizeBufferShaderPropertyID, argsBuffer.DispatchThreadSizeBuffer);
            kernel.Cs.DispatchIndirect(kernel, argsBuffer);
        }

        public static void DispatchIndirectDesired(this GPUKernel kernel, CommandBuffer cb, GPUDispatchIndirectArgsBuffer argsBuffer, bool updateBuffer = true)
        {
            if (updateBuffer) argsBuffer.UpdateBuffer(cb, kernel.ThreadGroupSizes);

            kernel.SetBuffer(cb, GPUStatics.DispatchThreadSizeBufferShaderPropertyID, argsBuffer.DispatchThreadSizeBuffer);
            kernel.Cs.DispatchIndirect(cb, kernel, argsBuffer);
        }
    }
}