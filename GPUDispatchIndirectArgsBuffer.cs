using System.Runtime.InteropServices;
using Unity.Mathematics;
using UnityEngine;

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
            DispatchThreadSizeBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, 1, Marshal.SizeOf(typeof(uint3)));
            CountBuffer = countBuffer;
            CountBufferOffset = countBufferOffset;
            CountBufferSize = math.clamp(countBufferSize, 1, 3);
            Inited = true;
        }

        public void UpdateBuffer(uint3 threadGroupSize)
        {
            var cs = GPUUtilsCs;
            var kernel = cs.FindKernel("BuildDispatchIndirect");

            kernel.SetBuffer("_CountBuffer", CountBuffer);
            kernel.SetBuffer("_DispatchIndirectArgsBuffer", this);
            kernel.SetBuffer("_DispatchThreadSizeBuffer", DispatchThreadSizeBuffer);
            cs.SetInt("_CountBufferOffset", CountBufferOffset);
            cs.SetInt("_CountBufferSize", CountBufferSize);
            cs.SetInts("_ThreadGroupSize", threadGroupSize);

            kernel.DispatchDesired(3);
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

            kernel.Cs.DisableKeyword("DIRECT_DISPATCH");
            kernel.Cs.EnableKeyword("INDIRECT_DISPATCH");
            kernel.SetBuffer("_DispatchThreadSizeBuffer", argsBuffer.DispatchThreadSizeBuffer);
            kernel.Cs.DispatchIndirect(kernel, argsBuffer);
        }
    }
}