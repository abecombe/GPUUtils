using UnityEngine;

namespace Abecombe.GPUUtils
{
    public class GPUAppendConsumeBuffer<T> : GPUBufferBase<T>
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

        public uint GetCounterValue()
        {
            UpdateCountBuffer();

            CountBuffer.GetDataToArgs(0, 0, 1);
            return CountBuffer.Args[0];
        }
    }

    public static class GPUAppendConsumeBufferExtensions
    {
        public static void SetGPUAppendBuffer<T>(this GPUComputeShader cs, GPUKernel kernel, string name, GPUAppendConsumeBuffer<T> buffer, bool resetBuffer = false)
        {
            if (resetBuffer) buffer.ResetCounter();
            cs.SetBuffer(kernel, name, buffer);
        }

        private static readonly string[] BufferConcatNames = { "", "CountBuffer" };

        public static void SetGPUConsumeBuffer<T>(this GPUComputeShader cs, GPUKernel kernel, string name, GPUAppendConsumeBuffer<T> buffer)
        {
            var propertyIDs = cs.GetPropertyIDs(name, BufferConcatNames);
            int count = 0;
            buffer.UpdateCountBuffer();
            cs.SetBuffer(kernel, propertyIDs[count++], buffer);
            cs.SetBuffer(kernel, propertyIDs[count++], buffer.CountBuffer);
        }

        public static void SetGPUAppendBuffer<T>(this GPUKernel kernel, string name, GPUAppendConsumeBuffer<T> buffer, bool resetBuffer = false)
        {
            kernel.Cs.SetGPUAppendBuffer(kernel, name, buffer, resetBuffer);
        }

        public static void SetGPUConsumeBuffer<T>(this GPUKernel kernel, string name, GPUAppendConsumeBuffer<T> buffer)
        {
            kernel.Cs.SetGPUConsumeBuffer(kernel, name, buffer);
        }
    }
}