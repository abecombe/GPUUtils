using UnityEngine;
using UnityEngine.Rendering;

namespace Abecombe.GPUUtils
{
    public interface IGPUBuffer
    {
        public GraphicsBuffer Data { get; }
        public GraphicsBuffer.Target BufferTarget { get; }

        public int Length { get; }
        public int Stride { get; }
        public int Bytes { get; }
    }

    public static class GPUBufferExtensions
    {
        public static void SetGPUBuffer(this GPUComputeShader cs, GPUKernel kernel, string name, IGPUBuffer buffer)
        {
            cs.SetBuffer(kernel, name, buffer.Data);
        }
        public static void SetGPUBuffer(this GPUKernel kernel, string name, IGPUBuffer buffer)
        {
            kernel.Cs.SetGPUBuffer(kernel, name, buffer);
        }

        public static void SetGPUBuffer(this GPUComputeShader cs, CommandBuffer cb, GPUKernel kernel, string name, IGPUBuffer buffer)
        {
            cs.SetBuffer(cb, kernel, name, buffer.Data);
        }
        public static void SetGPUBuffer(this GPUKernel kernel, CommandBuffer cb, string name, IGPUBuffer buffer)
        {
            kernel.Cs.SetGPUBuffer(cb, kernel, name, buffer);
        }
    }
}