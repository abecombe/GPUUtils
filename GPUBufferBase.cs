using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;

namespace Abecombe.GPUUtils
{
    public abstract class GPUBufferBase<T> : IGPUBuffer, IDisposable
    {
        public GraphicsBuffer Data { get; protected set; }
        public abstract GraphicsBuffer.Target BufferTarget { get; }

        public abstract int Length { get; }
        public int Stride => Data.stride;
        public int Bytes => Length * Stride;

        protected GPUComputeShader GPUUtilsCs;

        public bool Inited = false;

        protected void InitBufferCs()
        {
            Data = new GraphicsBuffer(BufferTarget, Length, Marshal.SizeOf(typeof(T)));
            GPUUtilsCs = new GPUComputeShader("GPUUtils");
        }

        public virtual void Dispose()
        {
            if (Inited)
            {
                Data.Release();
                Data = null;
            }
            Inited = false;
        }

        public void SetData(T[] data)
        {
            Data.SetData(data);
        }
        public void SetData(T[] data, int managedBufferStartIndex, int graphicsBufferStartIndex, int count)
        {
            Data.SetData(data, managedBufferStartIndex, graphicsBufferStartIndex, count);
        }
        public void SetData(CommandBuffer cb, T[] data)
        {
            cb.SetBufferData(Data, data);
        }
        public void SetData(CommandBuffer cb, T[] data, int managedBufferStartIndex, int graphicsBufferStartIndex, int count)
        {
            cb.SetBufferData(Data, data, managedBufferStartIndex, graphicsBufferStartIndex, count);
        }
        public void GetData(T[] data)
        {
            Data.GetData(data);
        }
        public void GetData(T[] data, int managedBufferStartIndex, int graphicsBufferStartIndex, int count)
        {
            Data.GetData(data, managedBufferStartIndex, graphicsBufferStartIndex, count);
        }

        public void CopyTo(GPUBufferBase<T> toBuffer, int fromBufferStartIndex = 0, int toBufferStartIndex = 0, int count = -1)
        {
            if (!BufferTarget.HasFlag(GraphicsBuffer.Target.Raw) || !toBuffer.BufferTarget.HasFlag(GraphicsBuffer.Target.Raw))
            {
                Debug.LogError("Copy kernel only supports Raw buffers, please use your own copy method");
                return;
            }

            if (count == -1)
            {
                if (Length != toBuffer.Length)
                {
                    Debug.LogError("Buffer length mismatch, please specify count");
                    return;
                }
                count = Length;
            }
            switch (count)
            {
                case <= 0:
                    return;
                case > 1024 * GPUConstants.MaxDispatchSize:
                    Debug.LogError("Buffer copy count exceeds maximum dispatch size");
                    return;
            }

            var cs = GPUUtilsCs;
            var kernel = cs.FindKernel(count == 1 ? "CopyBuffer1" : count <= 32 ? "CopyBuffer32" : count <= 128 * GPUConstants.MaxDispatchSize ? "CopyBuffer128" : "CopyBuffer1024");

            int uintScaling = Stride / sizeof(uint);

            cs.SetInt("_BufferCopyCount", count);
            cs.SetInt("_BufferCopyUIntCount", count * uintScaling);
            cs.SetInt("_FromBufferUIntStartIndex", fromBufferStartIndex * uintScaling);
            cs.SetInt("_ToBufferUIntStartIndex", toBufferStartIndex * uintScaling);
            kernel.SetBuffer("_FromBuffer", Data);
            kernel.SetBuffer("_ToBuffer", toBuffer);

            kernel.DispatchDesired(count);
        }
        public void CopyFrom(GPUBufferBase<T> fromBuffer, int fromBufferStartIndex = 0, int toBufferStartIndex = 0, int count = -1)
        {
            fromBuffer.CopyTo(this, fromBufferStartIndex, toBufferStartIndex, count);
        }

        public void CopyTo(CommandBuffer cb, GPUBufferBase<T> toBuffer, int fromBufferStartIndex = 0, int toBufferStartIndex = 0, int count = -1)
        {
            if (!BufferTarget.HasFlag(GraphicsBuffer.Target.Raw) || !toBuffer.BufferTarget.HasFlag(GraphicsBuffer.Target.Raw))
            {
                Debug.LogError("Copy kernel only supports Raw buffers, please use your own copy method");
                return;
            }

            if (count == -1)
            {
                if (Length != toBuffer.Length)
                {
                    Debug.LogError("Buffer length mismatch, please specify count");
                    return;
                }
                count = Length;
            }
            switch (count)
            {
                case <= 0:
                    return;
                case > 1024 * GPUConstants.MaxDispatchSize:
                    Debug.LogError("Buffer copy count exceeds maximum dispatch size");
                    return;
            }

            var cs = GPUUtilsCs;
            var kernel = cs.FindKernel(count == 1 ? "CopyBuffer1" : count <= 32 ? "CopyBuffer32" : count <= 128 * GPUConstants.MaxDispatchSize ? "CopyBuffer128" : "CopyBuffer1024");

            int uintScaling = Stride / sizeof(uint);

            cs.SetInt(cb, "_BufferCopyCount", count);
            cs.SetInt(cb, "_BufferCopyUIntCount", count * uintScaling);
            cs.SetInt(cb, "_FromBufferUIntStartIndex", fromBufferStartIndex * uintScaling);
            cs.SetInt(cb, "_ToBufferUIntStartIndex", toBufferStartIndex * uintScaling);
            kernel.SetBuffer(cb, "_FromBuffer", Data);
            kernel.SetBuffer(cb, "_ToBuffer", toBuffer);

            kernel.DispatchDesired(cb, count);
        }
        public void CopyFrom(CommandBuffer cb, GPUBufferBase<T> fromBuffer, int fromBufferStartIndex = 0, int toBufferStartIndex = 0, int count = -1)
        {
            fromBuffer.CopyTo(cb, this, fromBufferStartIndex, toBufferStartIndex, count);
        }

        public void Clear()
        {
            if (!BufferTarget.HasFlag(GraphicsBuffer.Target.Raw))
            {
                Debug.LogError("Clear kernel only supports Raw buffers, please use your own clear method");
                return;
            }

            var count = Length;

            var cs = GPUUtilsCs;
            var kernel = cs.FindKernel(count == 1 ? "ClearBuffer1" : count <= 32 ? "ClearBuffer32" : count <= 128 * GPUConstants.MaxDispatchSize ? "ClearBuffer128" : "ClearBuffer1024");

            int uintScaling = Stride / sizeof(uint);

            cs.SetInt("_BufferClearCount", count);
            cs.SetInt("_BufferClearUIntCount", count * uintScaling);
            kernel.SetBuffer("_Buffer", Data);

            kernel.DispatchDesired(count);
        }
        public void Clear(CommandBuffer cb)
        {
            if (!BufferTarget.HasFlag(GraphicsBuffer.Target.Raw))
            {
                Debug.LogError("Clear kernel only supports Raw buffers, please use your own clear method");
                return;
            }

            var count = Length;

            var cs = GPUUtilsCs;
            var kernel = cs.FindKernel(count == 1 ? "ClearBuffer1" : count <= 32 ? "ClearBuffer32" : count <= 128 * GPUConstants.MaxDispatchSize ? "ClearBuffer128" : "ClearBuffer1024");

            int uintScaling = Stride / sizeof(uint);

            cs.SetInt(cb, "_BufferClearCount", count);
            cs.SetInt(cb, "_BufferClearUIntCount", count * uintScaling);
            kernel.SetBuffer(cb, "_Buffer", Data);

            kernel.DispatchDesired(cb, count);
        }

        public static implicit operator GraphicsBuffer(GPUBufferBase<T> buffer)
        {
            return buffer.Data;
        }
    }
}