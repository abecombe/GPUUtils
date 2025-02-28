using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

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

        #region SetBool
        public void SetBool(int id, bool value)
        {
            Cs.SetBool(id, value);
        }
        public void SetBool(string name, bool value)
        {
            Cs.SetBool(name, value);
        }

        public void SetBool(CommandBuffer cb, int id, bool value)
        {
            Cs.SetBool(cb, id, value);
        }
        public void SetBool(CommandBuffer cb, string name, bool value)
        {
            Cs.SetBool(cb, name, value);
        }
        #endregion

        #region SetInt
        public void SetInt(int id, int value)
        {
            Cs.SetInt(id, value);
        }
        public void SetInt(int id, uint value)
        {
            Cs.SetInt(id, value);
        }
        public void SetInt(string name, int value)
        {
            Cs.SetInt(name, value);
        }
        public void SetInt(string name, uint value)
        {
            Cs.SetInt(name, value);
        }

        public void SetInt(CommandBuffer cb, int id, int value)
        {
            Cs.SetInt(cb, id, value);
        }
        public void SetInt(CommandBuffer cb, int id, uint value)
        {
            Cs.SetInt(cb, id, value);
        }
        public void SetInt(CommandBuffer cb, string name, int value)
        {
            Cs.SetInt(cb, name, value);
        }
        public void SetInt(CommandBuffer cb, string name, uint value)
        {
            Cs.SetInt(cb, name, value);
        }
        #endregion

        #region SetInts
        public void SetInts(int id, int2 value)
        {
            Cs.SetInts(id, value);
        }
        public void SetInts(int id, int3 value)
        {
            Cs.SetInts(id, value);
        }
        public void SetInts(int id, int4 value)
        {
            Cs.SetInts(id, value);
        }
        public void SetInts(int id, uint x, uint y)
        {
            Cs.SetInts(id, x, y);
        }
        public void SetInts(int id, uint x, uint y, uint z)
        {
            Cs.SetInts(id, x, y, z);
        }
        public void SetInts(int id, uint x, uint y, uint z, uint w)
        {
            Cs.SetInts(id, x, y, z, w);
        }
        public void SetInts(int id, uint2 value)
        {
            Cs.SetInts(id, value);
        }
        public void SetInts(int id, uint3 value)
        {
            Cs.SetInts(id, value);
        }
        public void SetInts(int id, uint4 value)
        {
            Cs.SetInts(id, value);
        }
        public void SetInts(string name, int2 value)
        {
            Cs.SetInts(name, value);
        }
        public void SetInts(string name, int3 value)
        {
            Cs.SetInts(name, value);
        }
        public void SetInts(string name, int4 value)
        {
            Cs.SetInts(name, value);
        }
        public void SetInts(string name, uint x, uint y)
        {
            Cs.SetInts(name, x, y);
        }
        public void SetInts(string name, uint x, uint y, uint z)
        {
            Cs.SetInts(name, x, y, z);
        }
        public void SetInts(string name, uint x, uint y, uint z, uint w)
        {
            Cs.SetInts(name, x, y, z, w);
        }
        public void SetInts(string name, uint2 value)
        {
            Cs.SetInts(name, value);
        }
        public void SetInts(string name, uint3 value)
        {
            Cs.SetInts(name, value);
        }
        public void SetInts(string name, uint4 value)
        {
            Cs.SetInts(name, value);
        }

        public void SetInts(CommandBuffer cb, int id, int2 value)
        {
            Cs.SetInts(cb, id, value);
        }
        public void SetInts(CommandBuffer cb, int id, int3 value)
        {
            Cs.SetInts(cb, id, value);
        }
        public void SetInts(CommandBuffer cb, int id, int4 value)
        {
            Cs.SetInts(cb, id, value);
        }
        public void SetInts(CommandBuffer cb, int id, uint x, uint y)
        {
            Cs.SetInts(cb, id, x, y);
        }
        public void SetInts(CommandBuffer cb, int id, uint x, uint y, uint z)
        {
            Cs.SetInts(cb, id, x, y, z);
        }
        public void SetInts(CommandBuffer cb, int id, uint x, uint y, uint z, uint w)
        {
            Cs.SetInts(cb, id, x, y, z, w);
        }
        public void SetInts(CommandBuffer cb, int id, uint2 value)
        {
            Cs.SetInts(cb, id, value);
        }
        public void SetInts(CommandBuffer cb, int id, uint3 value)
        {
            Cs.SetInts(cb, id, value);
        }
        public void SetInts(CommandBuffer cb, int id, uint4 value)
        {
            Cs.SetInts(cb, id, value);
        }
        public void SetInts(CommandBuffer cb, string name, int2 value)
        {
            Cs.SetInts(cb, name, value);
        }
        public void SetInts(CommandBuffer cb, string name, int3 value)
        {
            Cs.SetInts(cb, name, value);
        }
        public void SetInts(CommandBuffer cb, string name, int4 value)
        {
            Cs.SetInts(cb, name, value);
        }
        public void SetInts(CommandBuffer cb, string name, uint x, uint y)
        {
            Cs.SetInts(cb, name, x, y);
        }
        public void SetInts(CommandBuffer cb, string name, uint x, uint y, uint z)
        {
            Cs.SetInts(cb, name, x, y, z);
        }
        public void SetInts(CommandBuffer cb, string name, uint x, uint y, uint z, uint w)
        {
            Cs.SetInts(cb, name, x, y, z, w);
        }
        public void SetInts(CommandBuffer cb, string name, uint2 value)
        {
            Cs.SetInts(cb, name, value);
        }
        public void SetInts(CommandBuffer cb, string name, uint3 value)
        {
            Cs.SetInts(cb, name, value);
        }
        public void SetInts(CommandBuffer cb, string name, uint4 value)
        {
            Cs.SetInts(cb, name, value);
        }
        #endregion

        #region SetFloat
        public void SetFloat(int id, float value)
        {
            Cs.SetFloat(id, value);
        }
        public void SetFloat(string name, float value)
        {
            Cs.SetFloat(name, value);
        }

        public void SetFloat(CommandBuffer cb, int id, float value)
        {
            Cs.SetFloat(cb, id, value);
        }
        public void SetFloat(CommandBuffer cb, string name, float value)
        {
            Cs.SetFloat(cb, name, value);
        }
        #endregion

        #region SetVector
        public void SetVector(int id, float x, float y)
        {
            Cs.SetVector(id, x, y);
        }
        public void SetVector(int id, float x, float y, float z)
        {
            Cs.SetVector(id, x, y, z);
        }
        public void SetVector(int id, float x, float y, float z, float w)
        {
            Cs.SetVector(id, x, y, z, w);
        }
        public void SetVector(int id, float2 value)
        {
            Cs.SetVector(id, value);
        }
        public void SetVector(int id, float3 value)
        {
            Cs.SetVector(id, value);
        }
        public void SetVector(int id, float4 value)
        {
            Cs.SetVector(id, value);
        }
        public void SetVector(int id, Vector2 value)
        {
            Cs.SetVector(id, value);
        }
        public void SetVector(int id, Vector3 value)
        {
            Cs.SetVector(id, value);
        }
        public void SetVector(int id, Vector4 value)
        {
            Cs.SetVector(id, value);
        }
        public void SetVector(string name, float x, float y)
        {
            Cs.SetVector(name, x, y);
        }
        public void SetVector(string name, float x, float y, float z)
        {
            Cs.SetVector(name, x, y, z);
        }
        public void SetVector(string name, float x, float y, float z, float w)
        {
            Cs.SetVector(name, x, y, z, w);
        }
        public void SetVector(string name, float2 value)
        {
            Cs.SetVector(name, value);
        }
        public void SetVector(string name, float3 value)
        {
            Cs.SetVector(name, value);
        }
        public void SetVector(string name, float4 value)
        {
            Cs.SetVector(name, value);
        }
        public void SetVector(string name, Vector2 value)
        {
            Cs.SetVector(name, value);
        }
        public void SetVector(string name, Vector3 value)
        {
            Cs.SetVector(name, value);
        }
        public void SetVector(string name, Vector4 value)
        {
            Cs.SetVector(name, value);
        }

        public void SetVector(CommandBuffer cb, int id, float x, float y)
        {
            Cs.SetVector(cb, id, x, y);
        }
        public void SetVector(CommandBuffer cb, int id, float x, float y, float z)
        {
            Cs.SetVector(cb, id, x, y, z);
        }
        public void SetVector(CommandBuffer cb, int id, float x, float y, float z, float w)
        {
            Cs.SetVector(cb, id, x, y, z, w);
        }
        public void SetVector(CommandBuffer cb, int id, float2 value)
        {
            Cs.SetVector(cb, id, value);
        }
        public void SetVector(CommandBuffer cb, int id, float3 value)
        {
            Cs.SetVector(cb, id, value);
        }
        public void SetVector(CommandBuffer cb, int id, float4 value)
        {
            Cs.SetVector(cb, id, value);
        }
        public void SetVector(CommandBuffer cb, int id, Vector2 value)
        {
            Cs.SetVector(cb, id, value);
        }
        public void SetVector(CommandBuffer cb, int id, Vector3 value)
        {
            Cs.SetVector(cb, id, value);
        }
        public void SetVector(CommandBuffer cb, int id, Vector4 value)
        {
            Cs.SetVector(cb, id, value);
        }
        public void SetVector(CommandBuffer cb, string name, float x, float y)
        {
            Cs.SetVector(cb, name, x, y);
        }
        public void SetVector(CommandBuffer cb, string name, float x, float y, float z)
        {
            Cs.SetVector(cb, name, x, y, z);
        }
        public void SetVector(CommandBuffer cb, string name, float x, float y, float z, float w)
        {
            Cs.SetVector(cb, name, x, y, z, w);
        }
        public void SetVector(CommandBuffer cb, string name, float2 value)
        {
            Cs.SetVector(cb, name, value);
        }
        public void SetVector(CommandBuffer cb, string name, float3 value)
        {
            Cs.SetVector(cb, name, value);
        }
        public void SetVector(CommandBuffer cb, string name, float4 value)
        {
            Cs.SetVector(cb, name, value);
        }
        public void SetVector(CommandBuffer cb, string name, Vector2 value)
        {
            Cs.SetVector(cb, name, value);
        }
        public void SetVector(CommandBuffer cb, string name, Vector3 value)
        {
            Cs.SetVector(cb, name, value);
        }
        public void SetVector(CommandBuffer cb, string name, Vector4 value)
        {
            Cs.SetVector(cb, name, value);
        }
        #endregion

        #region SetMatrix
        public void SetMatrix(int id, Matrix4x4 matrix)
        {
            Cs.SetMatrix(id, matrix);
        }
        public void SetMatrix(int id, float4x4 matrix)
        {
            Cs.SetMatrix(id, matrix);
        }
        public void SetMatrix(string name, Matrix4x4 matrix)
        {
            Cs.SetMatrix(name, matrix);
        }
        public void SetMatrix(string name, float4x4 matrix)
        {
            Cs.SetMatrix(name, matrix);
        }

        public void SetMatrix(CommandBuffer cb, int id, Matrix4x4 matrix)
        {
            Cs.SetMatrix(cb, id, matrix);
        }
        public void SetMatrix(CommandBuffer cb, int id, float4x4 matrix)
        {
            Cs.SetMatrix(cb, id, matrix);
        }
        public void SetMatrix(CommandBuffer cb, string name, Matrix4x4 matrix)
        {
            Cs.SetMatrix(cb, name, matrix);
        }
        public void SetMatrix(CommandBuffer cb, string name, float4x4 matrix)
        {
            Cs.SetMatrix(cb, name, matrix);
        }
        #endregion

        #region SetBuffer
        public void SetBuffer(int id, GraphicsBuffer buffer)
        {
            Cs.SetBuffer(this, id, buffer);
        }
        public void SetBuffer(string name, GraphicsBuffer buffer)
        {
            Cs.SetBuffer(this, name, buffer);
        }

        public void SetBuffer(CommandBuffer cb, int id, GraphicsBuffer buffer)
        {
            Cs.SetBuffer(cb, this, id, buffer);
        }
        public void SetBuffer(CommandBuffer cb, string name, GraphicsBuffer buffer)
        {
            Cs.SetBuffer(cb, this, name, buffer);
        }
        #endregion

        #region SetConstantBuffer
        public void SetConstantBuffer(int id, GraphicsBuffer buffer, int offset = 0, int size = -1)
        {
            Cs.SetConstantBuffer(id, buffer, offset, size);
        }
        public void SetConstantBuffer(string name, GraphicsBuffer buffer, int offset = 0, int size = -1)
        {
            Cs.SetConstantBuffer(name, buffer, offset, size);
        }

        public void SetConstantBuffer(CommandBuffer cb, int id, GraphicsBuffer buffer, int offset = 0, int size = -1)
        {
            Cs.SetConstantBuffer(cb, id, buffer, offset, size);
        }
        public void SetConstantBuffer(CommandBuffer cb, string name, GraphicsBuffer buffer, int offset = 0, int size = -1)
        {
            Cs.SetConstantBuffer(cb, name, buffer, offset, size);
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

        public void SetTexture(CommandBuffer cb, int id, Texture tex)
        {
            Cs.SetTexture(cb, this, id, tex);
        }
        public void SetTexture(CommandBuffer cb, string name, Texture tex)
        {
            Cs.SetTexture(cb, this, name, tex);
        }
        #endregion

        #region Dispatch
        public void DispatchDesired(int sizeX, int sizeY = 1, int sizeZ = 1)
        {
            int groupSizeX = Mathf.Max(1, (sizeX + (int)ThreadGroupSizeX - 1) / (int)ThreadGroupSizeX);
            int groupSizeY = Mathf.Max(1, (sizeY + (int)ThreadGroupSizeY - 1) / (int)ThreadGroupSizeY);
            int groupSizeZ = Mathf.Max(1, (sizeZ + (int)ThreadGroupSizeZ - 1) / (int)ThreadGroupSizeZ);
            if (groupSizeX > GPUConstants.MaxDispatchSize || groupSizeY > GPUConstants.MaxDispatchSize || groupSizeZ > GPUConstants.MaxDispatchSize)
            {
                Debug.LogError("Dispatch size exceeds maximum dispatch size");
                return;
            }
            Cs.SetInts(GPUStaticValues.DispatchThreadSizeShaderPropertyID, sizeX, sizeY, sizeZ);
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

        public void DispatchDesired(CommandBuffer cb, int sizeX, int sizeY = 1, int sizeZ = 1)
        {
            int groupSizeX = Mathf.Max(1, (sizeX + (int)ThreadGroupSizeX - 1) / (int)ThreadGroupSizeX);
            int groupSizeY = Mathf.Max(1, (sizeY + (int)ThreadGroupSizeY - 1) / (int)ThreadGroupSizeY);
            int groupSizeZ = Mathf.Max(1, (sizeZ + (int)ThreadGroupSizeZ - 1) / (int)ThreadGroupSizeZ);
            if (groupSizeX > GPUConstants.MaxDispatchSize || groupSizeY > GPUConstants.MaxDispatchSize || groupSizeZ > GPUConstants.MaxDispatchSize)
            {
                Debug.LogError("Dispatch size exceeds maximum dispatch size");
                return;
            }
            Cs.SetInts(cb, GPUStaticValues.DispatchThreadSizeShaderPropertyID, sizeX, sizeY, sizeZ);
            Cs.Dispatch(cb, this, groupSizeX, groupSizeY, groupSizeZ);
        }
        public void DispatchDesired(CommandBuffer cb, int2 size)
        {
            DispatchDesired(cb, size.x, size.y);
        }
        public void DispatchDesired(CommandBuffer cb, int3 size)
        {
            DispatchDesired(cb, size.x, size.y, size.z);
        }
        #endregion
    }
}