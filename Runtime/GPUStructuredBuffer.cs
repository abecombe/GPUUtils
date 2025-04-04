using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace Abecombe.GPUUtils
{
    public class GPUStructuredBuffer<T> : GPUBufferBase<T>
        where T : struct
    {
        public override GraphicsBuffer.Target BufferTarget => GraphicsBuffer.Target.Structured | GraphicsBuffer.Target.Raw;

        public override int Length => Size.x * Size.y * Size.z;
        public int3 Size { get; protected set; }
        public int3 StartIndex { get; protected set; }
        public int3 EndIndex => StartIndex + Size - 1;
        public float3 PositionOffset { get; protected set; }

        public void Init(int size)
        {
            Init(new int3(size, 1, 1));
        }
        public void Init(int2 size)
        {
            Init(new int3(size, 1));
        }
        public void Init(int3 size)
        {
            Dispose();
            Size = size;
            InitBufferCs();
            Clear();
            SetStartIndex(int3.zero);
            SetPositionOffset(new float3(0.5f, 0.5f, 0.5f));
            Inited = true;
        }

        public void SetStartIndex(int startIndex)
        {
            SetStartIndex(new int3(startIndex, 0, 0));
        }
        public void SetStartIndex(int2 startIndex)
        {
            SetStartIndex(new int3(startIndex, 0));
        }
        public void SetStartIndex(int3 startIndex)
        {
            StartIndex = startIndex;
        }

        public void SetPositionOffset(float offset)
        {
            SetPositionOffset(new float3(offset, 0.5f, 0.5f));
        }
        public void SetPositionOffset(float2 offset)
        {
            SetPositionOffset(new float3(offset, 0.5f));
        }
        public void SetPositionOffset(float3 offset)
        {
            PositionOffset = offset;
        }
    }

    public class GPUDoubleStructuredBuffer<T> : IDisposable
        where T : struct
    {
        public GPUStructuredBuffer<T> Read { get; protected set; } = new();
        public GPUStructuredBuffer<T> Write { get; protected set; } = new();
        public GPUStructuredBuffer<T> Buffer1 => Read;
        public GPUStructuredBuffer<T> Buffer2 => Write;
        // for AsyncCompute
        public GPUStructuredBuffer<T> SimulationBuffer => GPUStatics.SimulationUseBuffer1 ? Buffer1 : Buffer2;
        public GPUStructuredBuffer<T> RenderingBuffer => GPUStatics.RenderingUseBuffer1 ? Buffer1 : Buffer2;

        public int Length => Read.Length;
        public int Stride => Read.Stride;
        public int Bytes => Read.Bytes;
        public int3 Size => Read.Size;
        public int3 StartIndex => Read.StartIndex;
        public int3 EndIndex => Read.EndIndex;
        public float3 PositionOffset => Read.PositionOffset;

        public bool Inited = false;

        public void Init(int size)
        {
            Init(new int3(size, 1, 1));
        }
        public void Init(int2 size)
        {
            Init(new int3(size, 1));
        }
        public void Init(int3 size)
        {
            Dispose();
            Read.Init(size);
            Write.Init(size);
            Inited = true;
        }

        public void Dispose()
        {
            if (Inited)
            {
                Read.Dispose();
                Write.Dispose();
            }
            Inited = false;
        }

        public void SetStartIndex(int startIndex)
        {
            SetStartIndex(new int3(startIndex, 0, 0));
        }
        public void SetStartIndex(int2 startIndex)
        {
            SetStartIndex(new int3(startIndex, 0));
        }
        public void SetStartIndex(int3 startIndex)
        {
            Read.SetStartIndex(startIndex);
            Write.SetStartIndex(startIndex);
        }

        public void SetPositionOffset(float offset)
        {
            SetPositionOffset(new float3(offset, 0.5f, 0.5f));
        }
        public void SetPositionOffset(float2 offset)
        {
            SetPositionOffset(new float3(offset, 0.5f));
        }
        public void SetPositionOffset(float3 offset)
        {
            Read.SetPositionOffset(offset);
            Write.SetPositionOffset(offset);
        }

        public void Swap()
        {
            (Read, Write) = (Write, Read);
        }

        public void CopyFromReadToWrite()
        {
            Read.CopyTo(Write);
        }
        public void CopyFromReadToWrite(CommandBuffer cb)
        {
            Read.CopyTo(cb, Write);
        }
    }

    public static class GPUStructuredBufferExtensions
    {
        public static void SetGPUStructuredBuffer<T>(this GPUComputeShader cs, GPUKernel kernel, string name, GPUStructuredBuffer<T> buffer) where T : struct
        {
            var propertyIDs = cs.GetPropertyIDs(name, GPUStatics.StructuredBufferConcatNames);
            int count = 0;
            cs.SetBuffer(kernel, propertyIDs[count++], buffer);
            cs.SetInt(propertyIDs[count++], buffer.Length);
            cs.SetInts(propertyIDs[count++], buffer.Size);
            cs.SetInts(propertyIDs[count++], buffer.StartIndex);
            cs.SetInts(propertyIDs[count++], buffer.EndIndex);
            cs.SetVector(propertyIDs[count++], buffer.PositionOffset);
            cs.SetVector(propertyIDs[count++], (float3)0.5f - buffer.PositionOffset);
            cs.SetVector(propertyIDs[count++], -buffer.PositionOffset);
        }
        public static void SetGPUStructuredBuffer<T>(this GPUKernel kernel, string name, GPUStructuredBuffer<T> buffer) where T : struct
        {
            kernel.Cs.SetGPUStructuredBuffer(kernel, name, buffer);
        }

        public static void SetGPUStructuredBuffer<T>(this GPUComputeShader cs, CommandBuffer cb, GPUKernel kernel, string name, GPUStructuredBuffer<T> buffer) where T : struct
        {
            var propertyIDs = cs.GetPropertyIDs(name, GPUStatics.StructuredBufferConcatNames);
            int count = 0;
            cs.SetBuffer(cb, kernel, propertyIDs[count++], buffer);
            cs.SetInt(cb, propertyIDs[count++], buffer.Length);
            cs.SetInts(cb, propertyIDs[count++], buffer.Size);
            cs.SetInts(cb, propertyIDs[count++], buffer.StartIndex);
            cs.SetInts(cb, propertyIDs[count++], buffer.EndIndex);
            cs.SetVector(cb, propertyIDs[count++], buffer.PositionOffset);
            cs.SetVector(cb, propertyIDs[count++], (float3)0.5f - buffer.PositionOffset);
            cs.SetVector(cb, propertyIDs[count++], -buffer.PositionOffset);
        }
        public static void SetGPUStructuredBuffer<T>(this GPUKernel kernel, CommandBuffer cb, string name, GPUStructuredBuffer<T> buffer) where T : struct
        {
            kernel.Cs.SetGPUStructuredBuffer(cb, kernel, name, buffer);
        }
    }
}