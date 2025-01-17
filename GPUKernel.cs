using Unity.Mathematics;
using UnityEngine;

namespace Abecombe.GPUUtils
{
    public class GPUKernel
    {
        public GPUComputeShader Cs { get; }
        public string Name { get; }
        public int ID { get; }
        public uint3 ThreadGroupSizes;
        public uint ThreadGroupSizeX => ThreadGroupSizes.x;
        public uint ThreadGroupSizeY => ThreadGroupSizes.y;
        public uint ThreadGroupSizeZ => ThreadGroupSizes.z;

        public GPUKernel(GPUComputeShader cs, string name)
        {
            Cs = cs;
            Name = name;
            ID = cs.Data.FindKernel(name);
            cs.Data.GetKernelThreadGroupSizes(ID, out var threadGroupSizeX, out var threadGroupSizeY, out var threadGroupSizeZ);
            ThreadGroupSizes = new uint3(threadGroupSizeX, threadGroupSizeY, threadGroupSizeZ);
        }

        #region SetBuffer
        public void SetBuffer(int id, GraphicsBuffer buffer)
        {
            Cs.SetBuffer(this, id, buffer);
        }
        public void SetBuffer(string name, GraphicsBuffer buffer)
        {
            Cs.SetBuffer(this, name, buffer);
        }
        #endregion

        #region SetTexture
        public void SetTexture(int id, Texture tex)
        {
            Cs.SetTexture(this, id, tex);
        }
        public void SetTexture(string name, Texture tex)
        {
            Cs.SetTexture(this, name, tex);
        }
        #endregion

        #region Dispatch
        public void DispatchDesired(int sizeX, int sizeY = 1, int sizeZ = 1)
        {
            Cs.EnableKeyword("DIRECT_DISPATCH");
            Cs.DisableKeyword("INDIRECT_DISPATCH");
            int groupSizeX = Mathf.Max(1, (sizeX + (int)ThreadGroupSizeX - 1) / (int)ThreadGroupSizeX);
            int groupSizeY = Mathf.Max(1, (sizeY + (int)ThreadGroupSizeY - 1) / (int)ThreadGroupSizeY);
            int groupSizeZ = Mathf.Max(1, (sizeZ + (int)ThreadGroupSizeZ - 1) / (int)ThreadGroupSizeZ);
            Cs.SetInts("_DispatchThreadSize", sizeX, sizeY, sizeZ);
            Cs.Dispatch(this, groupSizeX, groupSizeY, groupSizeZ);
        }
        public void DispatchDesired(int2 size)
        {
            DispatchDesired(size.x, size.y);
        }
        public void DispatchDesired(int3 size)
        {
            DispatchDesired(size.x, size.y, size.z);
        }
        #endregion
    }
}