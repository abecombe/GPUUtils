﻿using System;
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
            GPUUtilsCs = new GPUComputeShader(GPUStaticValues.UtilsShaderName);
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
                    Debug.LogError("Buffer copy count exceeds maximum dispatch size, please use your own copy method");
                    return;
            }

            var cs = GPUUtilsCs;
            var kernel = cs.FindKernel(count == 1 ? GPUStaticValues.CopyBuffer1KernelName : count <= 32 ? GPUStaticValues.CopyBuffer32KernelName : count <= 128 * GPUConstants.MaxDispatchSize ? GPUStaticValues.CopyBuffer128KernelName : GPUStaticValues.CopyBuffer1024KernelName);

            int uintScaling = Stride / sizeof(uint);

            cs.SetInt(GPUStaticValues.BufferCountShaderPropertyID, count);
            cs.SetInt(GPUStaticValues.BufferUIntCountShaderPropertyID, count * uintScaling);
            cs.SetInt(GPUStaticValues.FromBufferUIntStartIndexShaderPropertyID, fromBufferStartIndex * uintScaling);
            cs.SetInt(GPUStaticValues.ToBufferUIntStartIndexShaderPropertyID, toBufferStartIndex * uintScaling);
            kernel.SetBuffer(GPUStaticValues.FromBufferShaderPropertyID, Data);
            kernel.SetBuffer(GPUStaticValues.ToBufferShaderPropertyID, toBuffer);

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
                    Debug.LogError("Buffer copy count exceeds maximum dispatch size, please use your own copy method");
                    return;
            }

            var cs = GPUUtilsCs;
            var kernel = cs.FindKernel(count == 1 ? GPUStaticValues.CopyBuffer1KernelName : count <= 32 ? GPUStaticValues.CopyBuffer32KernelName : count <= 128 * GPUConstants.MaxDispatchSize ? GPUStaticValues.CopyBuffer128KernelName : GPUStaticValues.CopyBuffer1024KernelName);

            int uintScaling = Stride / sizeof(uint);

            cs.SetInt(cb, GPUStaticValues.BufferCountShaderPropertyID, count);
            cs.SetInt(cb, GPUStaticValues.BufferUIntCountShaderPropertyID, count * uintScaling);
            cs.SetInt(cb, GPUStaticValues.FromBufferUIntStartIndexShaderPropertyID, fromBufferStartIndex * uintScaling);
            cs.SetInt(cb, GPUStaticValues.ToBufferUIntStartIndexShaderPropertyID, toBufferStartIndex * uintScaling);
            kernel.SetBuffer(cb, GPUStaticValues.FromBufferShaderPropertyID, Data);
            kernel.SetBuffer(cb, GPUStaticValues.ToBufferShaderPropertyID, toBuffer);

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
            if (Length > 1024 * GPUConstants.MaxDispatchSize)
            {
                Debug.LogError("Buffer clear count exceeds maximum dispatch size, please use your own clear method");
            }

            var count = Length;

            var cs = GPUUtilsCs;
            var kernel = cs.FindKernel(count == 1 ? GPUStaticValues.ClearBuffer1KernelName : count <= 32 ? GPUStaticValues.ClearBuffer32KernelName : count <= 128 * GPUConstants.MaxDispatchSize ? GPUStaticValues.ClearBuffer128KernelName : GPUStaticValues.ClearBuffer1024KernelName);

            int uintScaling = Stride / sizeof(uint);

            cs.SetInt(GPUStaticValues.BufferCountShaderPropertyID, count);
            cs.SetInt(GPUStaticValues.BufferUIntCountShaderPropertyID, count * uintScaling);
            kernel.SetBuffer(GPUStaticValues.BufferShaderPropertyID, Data);

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
            var kernel = cs.FindKernel(count == 1 ? GPUStaticValues.ClearBuffer1KernelName : count <= 32 ? GPUStaticValues.ClearBuffer32KernelName : count <= 128 * GPUConstants.MaxDispatchSize ? GPUStaticValues.ClearBuffer128KernelName : GPUStaticValues.ClearBuffer1024KernelName);

            int uintScaling = Stride / sizeof(uint);

            cs.SetInt(cb, GPUStaticValues.BufferCountShaderPropertyID, count);
            cs.SetInt(cb, GPUStaticValues.BufferUIntCountShaderPropertyID, count * uintScaling);
            kernel.SetBuffer(cb, GPUStaticValues.BufferShaderPropertyID, Data);

            kernel.DispatchDesired(cb, count);
        }

        public static implicit operator GraphicsBuffer(GPUBufferBase<T> buffer)
        {
            return buffer.Data;
        }
    }
}