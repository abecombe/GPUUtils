﻿using UnityEngine;

namespace Abecombe.GPUUtils
{
    public class GPUCounterBuffer : GPUBufferBase<uint>
    {
        public override GraphicsBuffer.Target BufferTarget => GraphicsBuffer.Target.Counter;

        public override int Length => 1;

        public GPUIndirectArgumentsBuffer CountBuffer { get; } = new();

        public void Init()
        {
            Dispose();
            InitBufferCs();
            ResetCounter();
            CountBuffer.Init(1);
            Inited = true;
        }

        public override void Dispose()
        {
            if (Inited)
            {
                Data.Release();
                Data = null;
                CountBuffer.Dispose();
            }
            Inited = false;
        }

        public void UpdateCountBuffer()
        {
            GraphicsBuffer.CopyCount(Data, CountBuffer, 0);
        }
        public void CopyCountTo(GPUBufferBase<uint> dest, int destOffset = 0)
        {
            GraphicsBuffer.CopyCount(this, dest, destOffset * dest.Stride);
        }

        public void SetCounterValue(uint value)
        {
            Data.SetCounterValue(value);
        }
        public void ResetCounter()
        {
            SetCounterValue(0);
        }

        public uint GetCountValue()
        {
            UpdateCountBuffer();

            CountBuffer.GetDataToArgs(0, 0, 1);
            return CountBuffer.Args[0];
        }
    }

    public static class GPUCounterBufferExtensions
    {
        public static void SetGPUCounterBuffer(this GPUComputeShader cs, GPUKernel kernel, string name, GPUCounterBuffer buffer, bool resetCounter = false)
        {
            if (resetCounter)
            {
                buffer.ResetCounter();
            }
            cs.SetBuffer(kernel, name, buffer.Data);
        }
        public static void SetGPUCounterCountBuffer(this GPUComputeShader cs, GPUKernel kernel, string name, GPUCounterBuffer buffer)
        {
            buffer.UpdateCountBuffer();
            cs.SetBuffer(kernel, name, buffer.CountBuffer);
        }

        public static void SetGPUCounterBuffer(this GPUKernel kernel, string name, GPUCounterBuffer buffer, bool resetCounter = false)
        {
            kernel.Cs.SetGPUCounterBuffer(kernel, name, buffer, resetCounter);
        }
        public static void SetGPUCounterCountBuffer(this GPUKernel kernel, string name, GPUCounterBuffer buffer)
        {
            kernel.Cs.SetGPUCounterCountBuffer(kernel, name, buffer);
        }
    }
}