using UnityEngine;
using UnityEngine.Rendering;

namespace Abecombe.GPUUtils
{
    public interface IGPUAppendConsumeBuffer : IGPUBuffer
    {
        public GPUIndirectArgumentsBuffer CountBuffer { get; }

        public void Init(int size);

        public void UpdateCountBuffer();
        public void CopyCountTo(GPUBufferBase<uint> dest, int destOffset = 0);
        public void UpdateCountBuffer(CommandBuffer cb);
        public void CopyCountTo(CommandBuffer cb, GPUBufferBase<uint> dest, int destOffset = 0);

        public void SetCounterValue(uint value);
        public void ResetCounter();
        public void SetCounterValue(CommandBuffer cb, uint value);
        public void ResetCounter(CommandBuffer cb);

        public uint GetCounterValue();
    }

    public class GPUAppendConsumeBuffer<T> : GPUBufferBase<T>, IGPUAppendConsumeBuffer
        where T : struct
    {
        public override GraphicsBuffer.Target BufferTarget => GraphicsBuffer.Target.Append;

        public override int Length => _length;
        protected int _length;

        public GPUIndirectArgumentsBuffer CountBuffer { get; } = new();

        public void Init(int size)
        {
            Dispose();
            _length = size;
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
            GraphicsBuffer.CopyCount(Data, dest, destOffset * dest.Stride);
        }
        public void UpdateCountBuffer(CommandBuffer cb)
        {
            cb.CopyCounterValue(Data, CountBuffer, 0);
        }
        public void CopyCountTo(CommandBuffer cb, GPUBufferBase<uint> dest, int destOffset = 0)
        {
            cb.CopyCounterValue(Data, dest, (uint)(destOffset * dest.Stride));
        }

        public void SetCounterValue(uint value)
        {
            Data.SetCounterValue(value);
        }
        public void ResetCounter()
        {
            SetCounterValue(0);
        }
        public void SetCounterValue(CommandBuffer cb, uint value)
        {
            cb.SetBufferCounterValue(Data, value);
        }
        public void ResetCounter(CommandBuffer cb)
        {
            SetCounterValue(cb, 0);
        }

        public uint GetCounterValue()
        {
            UpdateCountBuffer();

            CountBuffer.GetDataToArgs(0, 0, 1);
            return CountBuffer.Args[0];
        }
    }

    public static class GPUAppendConsumeBufferExtensions
    {
        public static void SetGPUAppendBuffer(this GPUComputeShader cs, GPUKernel kernel, string name, IGPUAppendConsumeBuffer buffer, bool resetBuffer = false)
        {
            if (resetBuffer) buffer.ResetCounter();
            cs.SetBuffer(kernel, name, buffer.Data);
        }
        public static void SetGPUAppendBuffer(this GPUKernel kernel, string name, IGPUAppendConsumeBuffer buffer, bool resetBuffer = false)
        {
            kernel.Cs.SetGPUAppendBuffer(kernel, name, buffer, resetBuffer);
        }

        public static void SetGPUAppendBuffer(this GPUComputeShader cs, CommandBuffer cb, GPUKernel kernel, string name, IGPUAppendConsumeBuffer buffer, bool resetBuffer = false)
        {
            if (resetBuffer) buffer.ResetCounter(cb);
            cs.SetBuffer(cb, kernel, name, buffer.Data);
        }
        public static void SetGPUAppendBuffer(this GPUKernel kernel, CommandBuffer cb, string name, IGPUAppendConsumeBuffer buffer, bool resetBuffer = false)
        {
            kernel.Cs.SetGPUAppendBuffer(cb, kernel, name, buffer, resetBuffer);
        }

        public static void SetGPUConsumeBuffer(this GPUComputeShader cs, GPUKernel kernel, string name, IGPUAppendConsumeBuffer buffer)
        {
            var propertyIDs = cs.GetPropertyIDs(name, GPUStatics.AppendConsumeBufferConcatNames);
            int count = 0;
            buffer.UpdateCountBuffer();
            cs.SetBuffer(kernel, propertyIDs[count++], buffer.Data);
            cs.SetBuffer(kernel, propertyIDs[count++], buffer.CountBuffer);
        }
        public static void SetGPUConsumeBuffer(this GPUKernel kernel, string name, IGPUAppendConsumeBuffer buffer)
        {
            kernel.Cs.SetGPUConsumeBuffer(kernel, name, buffer);
        }

        public static void SetGPUConsumeBuffer(this GPUComputeShader cs, CommandBuffer cb, GPUKernel kernel, string name, IGPUAppendConsumeBuffer buffer)
        {
            var propertyIDs = cs.GetPropertyIDs(name, GPUStatics.AppendConsumeBufferConcatNames);
            int count = 0;
            buffer.UpdateCountBuffer(cb);
            cs.SetBuffer(cb, kernel, propertyIDs[count++], buffer.Data);
            cs.SetBuffer(cb, kernel, propertyIDs[count++], buffer.CountBuffer);
        }
        public static void SetGPUConsumeBuffer<T>(this GPUKernel kernel, CommandBuffer cb, string name, IGPUAppendConsumeBuffer buffer) where T : struct
        {
            kernel.Cs.SetGPUConsumeBuffer(cb, kernel, name, buffer);
        }
    }
}