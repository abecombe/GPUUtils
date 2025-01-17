﻿using System;
using System.Runtime.InteropServices;
using UnityEngine;

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
        protected const int MaxDispatchSize = 65535;

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
            if (count == -1)
            {
                if (this.Length != toBuffer.Length)
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
                case > 1024 * MaxDispatchSize:
                    Debug.LogError("Buffer copy count exceeds maximum dispatch size");
                    return;
            }

            var cs = GPUUtilsCs;
            var kernel = cs.FindKernel(count == 1 ? "CopyBuffer1" : count <= 32 ? "CopyBuffer32" : count <= 128 * MaxDispatchSize ? "CopyBuffer128" : "CopyBuffer1024");

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

        public static implicit operator GraphicsBuffer(GPUBufferBase<T> buffer)
        {
            return buffer.Data;
        }
    }
}