using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace Abecombe.GPUUtils
{
    public interface IGPUStructuredBuffer : IGPUBuffer
    {
        public int3 Size { get; }
        public int3 StartIndex { get; }
        public int3 EndIndex { get; }
        public float3 PositionOffset { get; }

        public void Init(int size);
        public void Init(int2 size);
        public void Init(int3 size);

        public void SetStartIndex(int startIndex);
        public void SetStartIndex(int2 startIndex);
        public void SetStartIndex(int3 startIndex);

        public void SetPositionOffset(float offset);
        public void SetPositionOffset(float2 offset);
        public void SetPositionOffset(float3 offset);
    }
    public interface IGPUDoubleStructuredBuffer : IDisposable
    {
        public IGPUStructuredBuffer Read { get; }
        public IGPUStructuredBuffer Write { get; }
        public IGPUStructuredBuffer SimulationBuffer { get; }
        public IGPUStructuredBuffer RenderingBuffer { get; }

        public int Length { get; }
        public int Stride { get; }
        public int Bytes { get; }
        public int3 Size { get; }
        public int3 StartIndex { get; }
        public int3 EndIndex { get; }
        public float3 PositionOffset { get; }

        public bool Inited { get; }

        public void Init(int size);
        public void Init(int2 size);
        public void Init(int3 size);

        public void SetStartIndex(int startIndex);
        public void SetStartIndex(int2 startIndex);
        public void SetStartIndex(int3 startIndex);

        public void SetPositionOffset(float offset);
        public void SetPositionOffset(float2 offset);
        public void SetPositionOffset(float3 offset);

        public void Swap();

        public void CopyFromReadToWrite();
        public void CopyFromReadToWrite(CommandBuffer cb);
    }

    public class GPUStructuredBuffer<T> : GPUBufferBase<T>, IGPUStructuredBuffer
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

    public class GPUDoubleStructuredBuffer<T> : IGPUDoubleStructuredBuffer
        where T : struct
    {
        public GPUStructuredBuffer<T> Buffer1 { get; protected set; } = new();
        public GPUStructuredBuffer<T> Buffer2 { get; protected set; } = new();

        public IGPUStructuredBuffer Read => Buffer1;
        public IGPUStructuredBuffer Write => Buffer2;
        // for AsyncCompute
        public IGPUStructuredBuffer SimulationBuffer => GPUStatics.SimulationUseBuffer1 ? Buffer1 : Buffer2;
        public IGPUStructuredBuffer RenderingBuffer => GPUStatics.RenderingUseBuffer1 ? Buffer1 : Buffer2;

        public int Length => Read.Length;
        public int Stride => Read.Stride;
        public int Bytes => Read.Bytes;
        public int3 Size => Read.Size;
        public int3 StartIndex => Read.StartIndex;
        public int3 EndIndex => Read.EndIndex;
        public float3 PositionOffset => Read.PositionOffset;

        public bool Inited { get; private set; } = false;

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
            Buffer1.Init(size);
            Buffer2.Init(size);
            Inited = true;
        }

        public void Dispose()
        {
            if (Inited)
            {
                Buffer1.Dispose();
                Buffer2.Dispose();
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
            Buffer1.SetStartIndex(startIndex);
            Buffer2.SetStartIndex(startIndex);
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
            Buffer1.SetPositionOffset(offset);
            Buffer2.SetPositionOffset(offset);
        }

        public void Swap()
        {
            (Buffer1, Buffer2) = (Buffer2, Buffer1);
        }

        public void CopyFromReadToWrite()
        {
            Buffer1.CopyTo(Buffer2);
        }
        public void CopyFromReadToWrite(CommandBuffer cb)
        {
            Buffer1.CopyTo(cb, Buffer2);
        }
    }

    public static class GPUStructuredBufferExtensions
    {
        public static void SetGPUStructuredBuffer(this GPUComputeShader cs, GPUKernel kernel, string name, IGPUStructuredBuffer buffer)
        {
            var propertyIDs = cs.GetPropertyIDs(name, GPUStatics.StructuredBufferConcatNames);
            int count = 0;
            cs.SetBuffer(kernel, propertyIDs[count++], buffer.Data);
            cs.SetInt(propertyIDs[count++], buffer.Length);
            cs.SetInts(propertyIDs[count++], buffer.Size);
            cs.SetInts(propertyIDs[count++], buffer.StartIndex);
            cs.SetInts(propertyIDs[count++], buffer.EndIndex);
            cs.SetVector(propertyIDs[count++], buffer.PositionOffset);
            cs.SetVector(propertyIDs[count++], (float3)0.5f - buffer.PositionOffset);
            cs.SetVector(propertyIDs[count++], -buffer.PositionOffset);
        }
        public static void SetGPUStructuredBuffer(this GPUKernel kernel, string name, IGPUStructuredBuffer buffer)
        {
            kernel.Cs.SetGPUStructuredBuffer(kernel, name, buffer);
        }

        public static void SetGPUStructuredBuffer(this GPUComputeShader cs, CommandBuffer cb, GPUKernel kernel, string name, IGPUStructuredBuffer buffer)
        {
            var propertyIDs = cs.GetPropertyIDs(name, GPUStatics.StructuredBufferConcatNames);
            int count = 0;
            cs.SetBuffer(cb, kernel, propertyIDs[count++], buffer.Data);
            cs.SetInt(cb, propertyIDs[count++], buffer.Length);
            cs.SetInts(cb, propertyIDs[count++], buffer.Size);
            cs.SetInts(cb, propertyIDs[count++], buffer.StartIndex);
            cs.SetInts(cb, propertyIDs[count++], buffer.EndIndex);
            cs.SetVector(cb, propertyIDs[count++], buffer.PositionOffset);
            cs.SetVector(cb, propertyIDs[count++], (float3)0.5f - buffer.PositionOffset);
            cs.SetVector(cb, propertyIDs[count++], -buffer.PositionOffset);
        }
        public static void SetGPUStructuredBuffer(this GPUKernel kernel, CommandBuffer cb, string name, IGPUStructuredBuffer buffer)
        {
            kernel.Cs.SetGPUStructuredBuffer(cb, kernel, name, buffer);
        }
    }
}