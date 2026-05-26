using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace Abecombe.GPUUtils
{
    public interface IGPUBuffer : IDisposable
    {
        public GraphicsBuffer Data { get; }
        public GraphicsBuffer.Target BufferTarget { get; }

        public int Length { get; }
        public int Stride { get; }
        public int Bytes { get; }

        public bool Inited { get; }

        public void SetData<U>(U[] data) where U : struct;
        public void SetData<U>(U[] data, int managedBufferStartIndex, int graphicsBufferStartIndex, int count) where U : struct;
        public void SetData<U>(List<U> data) where U : struct;
        public void SetData<U>(List<U> data, int managedBufferStartIndex, int graphicsBufferStartIndex, int count) where U : struct;
        public void SetData<U>(NativeArray<U> data) where U : struct;
        public void SetData<U>(NativeArray<U> data, int managedBufferStartIndex, int graphicsBufferStartIndex, int count) where U : struct;
        public void SetData<U>(CommandBuffer cb, U[] data) where U : struct;
        public void SetData<U>(CommandBuffer cb, U[] data, int managedBufferStartIndex, int graphicsBufferStartIndex, int count) where U : struct;
        public void SetData<U>(CommandBuffer cb, List<U> data) where U : struct;
        public void SetData<U>(CommandBuffer cb, List<U> data, int managedBufferStartIndex, int graphicsBufferStartIndex, int count) where U : struct;
        public void SetData<U>(CommandBuffer cb, NativeArray<U> data) where U : struct;
        public void SetData<U>(CommandBuffer cb, NativeArray<U> data, int managedBufferStartIndex, int graphicsBufferStartIndex, int count) where U : struct;
        public void SetData<U>(IComputeCommandBuffer cb, U[] data) where U : struct;
        public void SetData<U>(IComputeCommandBuffer cb, U[] data, int managedBufferStartIndex, int graphicsBufferStartIndex, int count) where U : struct;
        public void SetData<U>(IComputeCommandBuffer cb, List<U> data) where U : struct;
        public void SetData<U>(IComputeCommandBuffer cb, List<U> data, int managedBufferStartIndex, int graphicsBufferStartIndex, int count) where U : struct;
        public void SetData<U>(IComputeCommandBuffer cb, NativeArray<U> data) where U : struct;
        public void SetData<U>(IComputeCommandBuffer cb, NativeArray<U> data, int managedBufferStartIndex, int graphicsBufferStartIndex, int count) where U : struct;
        public void GetData<U>(U[] data) where U : struct;
        public void GetData<U>(U[] data, int managedBufferStartIndex, int graphicsBufferStartIndex, int count) where U : struct;

        public void CopyTo(IGPUBuffer toBuffer, int fromBufferStartIndex = 0, int toBufferStartIndex = 0, int count = -1);
        public void CopyFrom(IGPUBuffer fromBuffer, int fromBufferStartIndex = 0, int toBufferStartIndex = 0, int count = -1);
        public void CopyTo(CommandBuffer cb, IGPUBuffer toBuffer, int fromBufferStartIndex = 0, int toBufferStartIndex = 0, int count = -1);
        public void CopyFrom(CommandBuffer cb, IGPUBuffer fromBuffer, int fromBufferStartIndex = 0, int toBufferStartIndex = 0, int count = -1);
        public void CopyTo(IComputeCommandBuffer cb, IGPUBuffer toBuffer, int fromBufferStartIndex = 0, int toBufferStartIndex = 0, int count = -1);
        public void CopyFrom(IComputeCommandBuffer cb, IGPUBuffer fromBuffer, int fromBufferStartIndex = 0, int toBufferStartIndex = 0, int count = -1);

        public void Clear();
        public void Clear(CommandBuffer cmd);
        public void Clear(IComputeCommandBuffer cmd);
    }

    public abstract class GPUBufferBase<T> : IGPUBuffer
        where T : struct
    {
        public GraphicsBuffer Data { get; protected set; }
        public abstract GraphicsBuffer.Target BufferTarget { get; }

        public abstract int Length { get; }
        public int Stride => Data.stride;
        public int Bytes => Length * Stride;

        protected GPUComputeShader GPUUtilsCs = new();

        public bool Inited { get; protected set; } = false;

        protected void InitBufferCs()
        {
            Data = new GraphicsBuffer(BufferTarget, Length, Marshal.SizeOf(typeof(T)));
            GPUUtilsCs.Init(GPUStatics.UtilsShaderName);
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

        public void SetData<U>(U[] data) where U : struct
        {
            Data.SetData(data);
        }
        public void SetData<U>(U[] data, int managedBufferStartIndex, int graphicsBufferStartIndex, int count) where U : struct
        {
            Data.SetData(data, managedBufferStartIndex, graphicsBufferStartIndex, count);
        }
        public void SetData<U>(List<U> data) where U : struct
        {
            Data.SetData(data);
        }
        public void SetData<U>(List<U> data, int managedBufferStartIndex, int graphicsBufferStartIndex, int count) where U : struct
        {
            Data.SetData(data, managedBufferStartIndex, graphicsBufferStartIndex, count);
        }
        public void SetData<U>(NativeArray<U> data) where U : struct
        {
            Data.SetData(data);
        }
        public void SetData<U>(NativeArray<U> data, int managedBufferStartIndex, int graphicsBufferStartIndex, int count) where U : struct
        {
            Data.SetData(data, managedBufferStartIndex, graphicsBufferStartIndex, count);
        }
        public void SetData<U>(CommandBuffer cb, U[] data) where U : struct
        {
            cb.SetBufferData(Data, data);
        }
        public void SetData<U>(CommandBuffer cb, U[] data, int managedBufferStartIndex, int graphicsBufferStartIndex, int count) where U : struct
        {
            cb.SetBufferData(Data, data, managedBufferStartIndex, graphicsBufferStartIndex, count);
        }
        public void SetData<U>(CommandBuffer cb, List<U> data) where U : struct
        {
            cb.SetBufferData(Data, data);
        }
        public void SetData<U>(CommandBuffer cb, List<U> data, int managedBufferStartIndex, int graphicsBufferStartIndex, int count) where U : struct
        {
            cb.SetBufferData(Data, data, managedBufferStartIndex, graphicsBufferStartIndex, count);
        }
        public void SetData<U>(CommandBuffer cb, NativeArray<U> data) where U : struct
        {
            cb.SetBufferData(Data, data);
        }
        public void SetData<U>(CommandBuffer cb, NativeArray<U> data, int managedBufferStartIndex, int graphicsBufferStartIndex, int count) where U : struct
        {
            cb.SetBufferData(Data, data, managedBufferStartIndex, graphicsBufferStartIndex, count);
        }
        public void SetData<U>(IComputeCommandBuffer cb, U[] data) where U : struct
        {
            cb.SetBufferData(Data, data);
        }
        public void SetData<U>(IComputeCommandBuffer cb, U[] data, int managedBufferStartIndex, int graphicsBufferStartIndex, int count) where U : struct
        {
            cb.SetBufferData(Data, data, managedBufferStartIndex, graphicsBufferStartIndex, count);
        }
        public void SetData<U>(IComputeCommandBuffer cb, List<U> data) where U : struct
        {
            cb.SetBufferData(Data, data);
        }
        public void SetData<U>(IComputeCommandBuffer cb, List<U> data, int managedBufferStartIndex, int graphicsBufferStartIndex, int count) where U : struct
        {
            cb.SetBufferData(Data, data, managedBufferStartIndex, graphicsBufferStartIndex, count);
        }
        public void SetData<U>(IComputeCommandBuffer cb, NativeArray<U> data) where U : struct
        {
            cb.SetBufferData(Data, data);
        }
        public void SetData<U>(IComputeCommandBuffer cb, NativeArray<U> data, int managedBufferStartIndex, int graphicsBufferStartIndex, int count) where U : struct
        {
            cb.SetBufferData(Data, data, managedBufferStartIndex, graphicsBufferStartIndex, count);
        }
        public void GetData<U>(U[] data) where U : struct
        {
            Data.GetData(data);
        }
        public void GetData<U>(U[] data, int managedBufferStartIndex, int graphicsBufferStartIndex, int count) where U : struct
        {
            Data.GetData(data, managedBufferStartIndex, graphicsBufferStartIndex, count);
        }

        public void CopyTo(IGPUBuffer toBuffer, int fromBufferStartIndex = 0, int toBufferStartIndex = 0, int count = -1)
        {
            if (Stride != toBuffer.Stride)
            {
                Debug.LogError($"Stride mismatch, cannot copy from stride {Stride} to {toBuffer.Stride}.");
                return;
            }
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
                    Debug.LogError("Buffer copy count exceeds maximum dispatch size, please use your own copy method");
                    return;
            }

            var cs = GPUUtilsCs;
            var kernel = cs.FindKernel(count == 1 ? GPUStatics.CopyBuffer1KernelName : count <= 32 ? GPUStatics.CopyBuffer32KernelName : count <= 128 * GPUConstants.MaxDispatchSize ? GPUStatics.CopyBuffer128KernelName : GPUStatics.CopyBuffer1024KernelName);

            int uintScaling = Stride / sizeof(uint);

            cs.SetInt(GPUStatics.BufferCountShaderPropertyID, count);
            cs.SetInt(GPUStatics.BufferUIntCountShaderPropertyID, count * uintScaling);
            cs.SetInt(GPUStatics.FromBufferUIntStartIndexShaderPropertyID, fromBufferStartIndex * uintScaling);
            cs.SetInt(GPUStatics.ToBufferUIntStartIndexShaderPropertyID, toBufferStartIndex * uintScaling);
            kernel.SetBuffer(GPUStatics.FromBufferShaderPropertyID, Data);
            kernel.SetBuffer(GPUStatics.ToBufferShaderPropertyID, toBuffer.Data);

            kernel.DispatchDesired(count);
        }
        public void CopyFrom(IGPUBuffer fromBuffer, int fromBufferStartIndex = 0, int toBufferStartIndex = 0, int count = -1)
        {
            fromBuffer.CopyTo(this, fromBufferStartIndex, toBufferStartIndex, count);
        }

        public void CopyTo(CommandBuffer cb, IGPUBuffer toBuffer, int fromBufferStartIndex = 0, int toBufferStartIndex = 0, int count = -1)
        {
            if (Stride != toBuffer.Stride)
            {
                Debug.LogError($"Stride mismatch, cannot copy from stride {Stride} to {toBuffer.Stride}.");
                return;
            }
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
                    Debug.LogError("Buffer copy count exceeds maximum dispatch size, please use your own copy method");
                    return;
            }

            var cs = GPUUtilsCs;
            var kernel = cs.FindKernel(count == 1 ? GPUStatics.CopyBuffer1KernelName : count <= 32 ? GPUStatics.CopyBuffer32KernelName : count <= 128 * GPUConstants.MaxDispatchSize ? GPUStatics.CopyBuffer128KernelName : GPUStatics.CopyBuffer1024KernelName);

            int uintScaling = Stride / sizeof(uint);

            cs.SetInt(cb, GPUStatics.BufferCountShaderPropertyID, count);
            cs.SetInt(cb, GPUStatics.BufferUIntCountShaderPropertyID, count * uintScaling);
            cs.SetInt(cb, GPUStatics.FromBufferUIntStartIndexShaderPropertyID, fromBufferStartIndex * uintScaling);
            cs.SetInt(cb, GPUStatics.ToBufferUIntStartIndexShaderPropertyID, toBufferStartIndex * uintScaling);
            kernel.SetBuffer(cb, GPUStatics.FromBufferShaderPropertyID, Data);
            kernel.SetBuffer(cb, GPUStatics.ToBufferShaderPropertyID, toBuffer.Data);

            kernel.DispatchDesired(cb, count);
        }
        public void CopyFrom(CommandBuffer cb, IGPUBuffer fromBuffer, int fromBufferStartIndex = 0, int toBufferStartIndex = 0, int count = -1)
        {
            fromBuffer.CopyTo(cb, this, fromBufferStartIndex, toBufferStartIndex, count);
        }

        public void CopyTo(IComputeCommandBuffer cb, IGPUBuffer toBuffer, int fromBufferStartIndex = 0, int toBufferStartIndex = 0, int count = -1)
        {
            if (Stride != toBuffer.Stride)
            {
                Debug.LogError($"Stride mismatch, cannot copy from stride {Stride} to {toBuffer.Stride}.");
                return;
            }
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
                    Debug.LogError("Buffer copy count exceeds maximum dispatch size, please use your own copy method");
                    return;
            }

            var cs = GPUUtilsCs;
            var kernel = cs.FindKernel(count == 1 ? GPUStatics.CopyBuffer1KernelName : count <= 32 ? GPUStatics.CopyBuffer32KernelName : count <= 128 * GPUConstants.MaxDispatchSize ? GPUStatics.CopyBuffer128KernelName : GPUStatics.CopyBuffer1024KernelName);

            int uintScaling = Stride / sizeof(uint);

            cs.SetInt(cb, GPUStatics.BufferCountShaderPropertyID, count);
            cs.SetInt(cb, GPUStatics.BufferUIntCountShaderPropertyID, count * uintScaling);
            cs.SetInt(cb, GPUStatics.FromBufferUIntStartIndexShaderPropertyID, fromBufferStartIndex * uintScaling);
            cs.SetInt(cb, GPUStatics.ToBufferUIntStartIndexShaderPropertyID, toBufferStartIndex * uintScaling);
            kernel.SetBuffer(cb, GPUStatics.FromBufferShaderPropertyID, Data);
            kernel.SetBuffer(cb, GPUStatics.ToBufferShaderPropertyID, toBuffer.Data);

            kernel.DispatchDesired(cb, count);
        }
        public void CopyFrom(IComputeCommandBuffer cb, IGPUBuffer fromBuffer, int fromBufferStartIndex = 0, int toBufferStartIndex = 0, int count = -1)
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
            if (Length > 1024 * GPUConstants.MaxDispatchSize)
            {
                Debug.LogError("Buffer clear count exceeds maximum dispatch size, please use your own clear method");
            }

            var count = Length;

            var cs = GPUUtilsCs;
            var kernel = cs.FindKernel(count == 1 ? GPUStatics.ClearBuffer1KernelName : count <= 32 ? GPUStatics.ClearBuffer32KernelName : count <= 128 * GPUConstants.MaxDispatchSize ? GPUStatics.ClearBuffer128KernelName : GPUStatics.ClearBuffer1024KernelName);

            int uintScaling = Stride / sizeof(uint);

            cs.SetInt(GPUStatics.BufferCountShaderPropertyID, count);
            cs.SetInt(GPUStatics.BufferUIntCountShaderPropertyID, count * uintScaling);
            kernel.SetBuffer(GPUStatics.BufferShaderPropertyID, Data);

            kernel.DispatchDesired(count);
        }
        public void Clear(CommandBuffer cb)
        {
            if (!BufferTarget.HasFlag(GraphicsBuffer.Target.Raw))
            {
                Debug.LogError("Clear kernel only supports Raw buffers, please use your own clear method");
                return;
            }
            if (Length > 1024 * GPUConstants.MaxDispatchSize)
            {
                Debug.LogError("Buffer clear count exceeds maximum dispatch size, please use your own clear method");
            }

            var count = Length;

            var cs = GPUUtilsCs;
            var kernel = cs.FindKernel(count == 1 ? GPUStatics.ClearBuffer1KernelName : count <= 32 ? GPUStatics.ClearBuffer32KernelName : count <= 128 * GPUConstants.MaxDispatchSize ? GPUStatics.ClearBuffer128KernelName : GPUStatics.ClearBuffer1024KernelName);

            int uintScaling = Stride / sizeof(uint);

            cs.SetInt(cb, GPUStatics.BufferCountShaderPropertyID, count);
            cs.SetInt(cb, GPUStatics.BufferUIntCountShaderPropertyID, count * uintScaling);
            kernel.SetBuffer(cb, GPUStatics.BufferShaderPropertyID, Data);

            kernel.DispatchDesired(cb, count);
        }
        public void Clear(IComputeCommandBuffer cb)
        {
            if (!BufferTarget.HasFlag(GraphicsBuffer.Target.Raw))
            {
                Debug.LogError("Clear kernel only supports Raw buffers, please use your own clear method");
                return;
            }
            if (Length > 1024 * GPUConstants.MaxDispatchSize)
            {
                Debug.LogError("Buffer clear count exceeds maximum dispatch size, please use your own clear method");
            }

            var count = Length;

            var cs = GPUUtilsCs;
            var kernel = cs.FindKernel(count == 1 ? GPUStatics.ClearBuffer1KernelName : count <= 32 ? GPUStatics.ClearBuffer32KernelName : count <= 128 * GPUConstants.MaxDispatchSize ? GPUStatics.ClearBuffer128KernelName : GPUStatics.ClearBuffer1024KernelName);

            int uintScaling = Stride / sizeof(uint);

            cs.SetInt(cb, GPUStatics.BufferCountShaderPropertyID, count);
            cs.SetInt(cb, GPUStatics.BufferUIntCountShaderPropertyID, count * uintScaling);
            kernel.SetBuffer(cb, GPUStatics.BufferShaderPropertyID, Data);

            kernel.DispatchDesired(cb, count);
        }

        public static implicit operator GraphicsBuffer(GPUBufferBase<T> buffer)
        {
            return buffer.Data;
        }
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

        public static void SetGPUBuffer(this GPUComputeShader cs, IComputeCommandBuffer cb, GPUKernel kernel, string name, IGPUBuffer buffer)
        {
            cs.SetBuffer(cb, kernel, name, buffer.Data);
        }
        public static void SetGPUBuffer(this GPUKernel kernel, IComputeCommandBuffer cb, string name, IGPUBuffer buffer)
        {
            kernel.Cs.SetGPUBuffer(cb, kernel, name, buffer);
        }
    }
}