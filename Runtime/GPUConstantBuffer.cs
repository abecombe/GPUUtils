using UnityEngine;

namespace Abecombe.GPUUtils
{
    // You should layout the struct correctly to match the hlsl constant buffer layout.
    // See https://maraneshi.github.io/HLSL-ConstantBufferLayoutVisualizer/
    public class GPUConstantBuffer<T> : GPUBufferBase<T>
        where T : struct
    {
        public override GraphicsBuffer.Target BufferTarget => GraphicsBuffer.Target.Constant;

        public override int Length => 1;

        public void Init()
        {
            Dispose();
            InitBufferCs();
            Inited = true;
        }

        public override void Dispose()
        {
            if (Inited)
            {
                Data.Release();
                Data = null;
            }
            Inited = false;
        }
    }
}