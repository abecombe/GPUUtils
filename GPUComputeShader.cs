using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Abecombe.GPUUtils
{
    public class GPUComputeShader
    {
        public ComputeShader Data { get; }

        private Dictionary<int, GPUKernel> _kernels = new();

        private Dictionary<int, int> _propertyID = new();
        private Dictionary<int, int[]> _propertyIDs = new();

        private int[] _intArr = new int[4];

        public GPUComputeShader(ComputeShader cs)
        {
            Data = cs;
        }
        public GPUComputeShader(string csName)
        {
            Data = Resources.Load<ComputeShader>(csName);
        }

        public GPUKernel FindKernel(string name)
        {
            int hash = name.GetHashCode();

            if (_kernels.TryGetValue(hash, out var kernel))
                return kernel;

            kernel = new GPUKernel(this, name);
            _kernels.Add(hash, kernel);
            return kernel;
        }

        public int GetPropertyID(string name)
        {
            int hash = name.GetHashCode();

            if (_propertyID.TryGetValue(hash, out var id))
                return id;

            id = Shader.PropertyToID(name);
            _propertyID.Add(hash, id);
            return id;
        }

        public int[] GetPropertyIDs(string name, string[] concatNames)
        {
            int hash = name.GetHashCode() ^ concatNames.GetHashCode();

            if (_propertyIDs.TryGetValue(hash, out var ids))
                return ids;

            ids = new int[concatNames.Length];
            for (int i = 0; i < concatNames.Length; i++)
                ids[i] = Shader.PropertyToID(name + concatNames[i]);
            _propertyIDs.Add(hash, ids);
            return ids;
        }

        #region SetBool
        public void SetBool(int id, bool value)
        {
            Data.SetBool(id, value);
        }

        public void SetBool(string name, bool value)
        {
            Data.SetBool(name, value);
        }
        #endregion

        #region SetInt
        public void SetInt(int id, int value)
        {
            Data.SetInt(id, value);
        }
        public void SetInt(int id, uint value)
        {
            Data.SetInt(id, (int)value);
        }

        public void SetInt(string name, int value)
        {
            Data.SetInt(name, value);
        }
        public void SetInt(string name, uint value)
        {
            Data.SetInt(name, (int)value);
        }
        #endregion

        #region SetInts
        private void SetInts(int id)
        {
            Data.SetInts(id, _intArr);
        }
        public void SetInts(int id, int x, int y)
        {
            _intArr[0] = x;
            _intArr[1] = y;
            SetInts(id);
        }
        public void SetInts(int id, int x, int y, int z)
        {
            _intArr[0] = x;
            _intArr[1] = y;
            _intArr[2] = z;
            SetInts(id);
        }
        public void SetInts(int id, int x, int y, int z, int w)
        {
            _intArr[0] = x;
            _intArr[1] = y;
            _intArr[2] = z;
            _intArr[3] = w;
            SetInts(id);
        }
        public void SetInts(int id, int2 value)
        {
            SetInts(id, value.x, value.y);
        }
        public void SetInts(int id, int3 value)
        {
            SetInts(id, value.x, value.y, value.z);
        }
        public void SetInts(int id, int4 value)
        {
            SetInts(id, value.x, value.y, value.z, value.w);
        }
        public void SetInts(int id, uint x, uint y)
        {
            SetInts(id, (int)x, (int)y);
        }
        public void SetInts(int id, uint x, uint y, uint z)
        {
            SetInts(id, (int)x, (int)y, (int)z);
        }
        public void SetInts(int id, uint x, uint y, uint z, uint w)
        {
            SetInts(id, (int)x, (int)y, (int)z, (int)w);
        }
        public void SetInts(int id, uint2 value)
        {
            SetInts(id, (int)value.x, (int)value.y);
        }
        public void SetInts(int id, uint3 value)
        {
            SetInts(id, (int)value.x, (int)value.y, (int)value.z);
        }
        public void SetInts(int id, uint4 value)
        {
            SetInts(id, (int)value.x, (int)value.y, (int)value.z, (int)value.w);
        }

        private void SetInts(string name)
        {
            Data.SetInts(name, _intArr);
        }
        public void SetInts(string name, int x, int y)
        {
            _intArr[0] = x;
            _intArr[1] = y;
            SetInts(name);
        }
        public void SetInts(string name, int x, int y, int z)
        {
            _intArr[0] = x;
            _intArr[1] = y;
            _intArr[2] = z;
            SetInts(name);
        }
        public void SetInts(string name, int x, int y, int z, int w)
        {
            _intArr[0] = x;
            _intArr[1] = y;
            _intArr[2] = z;
            _intArr[3] = w;
            SetInts(name);
        }
        public void SetInts(string name, int2 value)
        {
            SetInts(name, value.x, value.y);
        }
        public void SetInts(string name, int3 value)
        {
            SetInts(name, value.x, value.y, value.z);
        }
        public void SetInts(string name, int4 value)
        {
            SetInts(name, value.x, value.y, value.z, value.w);
        }
        public void SetInts(string name, uint x, uint y)
        {
            SetInts(name, (int)x, (int)y);
        }
        public void SetInts(string name, uint x, uint y, uint z)
        {
            SetInts(name, (int)x, (int)y, (int)z);
        }
        public void SetInts(string name, uint x, uint y, uint z, uint w)
        {
            SetInts(name, (int)x, (int)y, (int)z, (int)w);
        }
        public void SetInts(string name, uint2 value)
        {
            SetInts(name, (int)value.x, (int)value.y);
        }
        public void SetInts(string name, uint3 value)
        {
            SetInts(name, (int)value.x, (int)value.y, (int)value.z);
        }
        public void SetInts(string name, uint4 value)
        {
            SetInts(name, (int)value.x, (int)value.y, (int)value.z, (int)value.w);
        }
        #endregion

        #region SetFloat
        public void SetFloat(int id, float value)
        {
            Data.SetFloat(id, value);
        }

        public void SetFloat(string name, float value)
        {
            Data.SetFloat(name, value);
        }
        #endregion

        #region SetVector
        public void SetVector(int id, float x, float y)
        {
            Data.SetVector(id, new Vector4(x, y));
        }
        public void SetVector(int id, float x, float y, float z)
        {
            Data.SetVector(id, new Vector4(x, y, z));
        }
        public void SetVector(int id, float x, float y, float z, float w)
        {
            Data.SetVector(id, new Vector4(x, y, z, w));
        }
        public void SetVector(int id, float2 value)
        {
            Data.SetVector(id, new Vector4(value.x, value.y));
        }
        public void SetVector(int id, float3 value)
        {
            Data.SetVector(id, new Vector4(value.x, value.y, value.z));
        }
        public void SetVector(int id, float4 value)
        {
            Data.SetVector(id, new Vector4(value.x, value.y, value.z, value.w));
        }
        public void SetVector(int id, Vector2 value)
        {
            Data.SetVector(id, value);
        }
        public void SetVector(int id, Vector3 value)
        {
            Data.SetVector(id, value);
        }
        public void SetVector(int id, Vector4 value)
        {
            Data.SetVector(id, value);
        }

        public void SetVector(string name, float x, float y)
        {
            Data.SetVector(name, new Vector4(x, y));
        }
        public void SetVector(string name, float x, float y, float z)
        {
            Data.SetVector(name, new Vector4(x, y, z));
        }
        public void SetVector(string name, float x, float y, float z, float w)
        {
            Data.SetVector(name, new Vector4(x, y, z, w));
        }
        public void SetVector(string name, float2 value)
        {
            Data.SetVector(name, new Vector4(value.x, value.y));
        }
        public void SetVector(string name, float3 value)
        {
            Data.SetVector(name, new Vector4(value.x, value.y, value.z));
        }
        public void SetVector(string name, float4 value)
        {
            Data.SetVector(name, new Vector4(value.x, value.y, value.z, value.w));
        }
        public void SetVector(string name, Vector2 value)
        {
            Data.SetVector(name, value);
        }
        public void SetVector(string name, Vector3 value)
        {
            Data.SetVector(name, value);
        }
        public void SetVector(string name, Vector4 value)
        {
            Data.SetVector(name, value);
        }
        #endregion

        #region SetMatrix
        public void SetMatrix(int id, Matrix4x4 matrix)
        {
            Data.SetMatrix(id, matrix);
        }
        public void SetMatrix(int id, float4x4 matrix)
        {
            Data.SetMatrix(id, matrix);
        }

        public void SetMatrix(string name, Matrix4x4 matrix)
        {
            Data.SetMatrix(name, matrix);
        }
        public void SetMatrix(string name, float4x4 matrix)
        {
            Data.SetMatrix(name, matrix);
        }
        #endregion

        #region SetBuffer
        public void SetBuffer(int kernelIndex, int id, GraphicsBuffer buffer)
        {
            Data.SetBuffer(kernelIndex, id, buffer);
        }
        public void SetBuffer(GPUKernel kernel, int id, GraphicsBuffer buffer)
        {
            Data.SetBuffer(kernel.ID, id, buffer);
        }

        public void SetBuffer(int kernelIndex, string name, GraphicsBuffer buffer)
        {
            Data.SetBuffer(kernelIndex, name, buffer);
        }
        public void SetBuffer(GPUKernel kernel, string name, GraphicsBuffer buffer)
        {
            Data.SetBuffer(kernel.ID, name, buffer);
        }
        #endregion

        #region SetTexture
        public void SetTexture(int kernelIndex, int id, Texture tex)
        {
            Data.SetTexture(kernelIndex, id, tex);
        }
        public void SetTexture(GPUKernel kernel, int id, Texture tex)
        {
            Data.SetTexture(kernel.ID, id, tex);
        }

        public void SetTexture(int kernelIndex, string name, Texture tex)
        {
            Data.SetTexture(kernelIndex, name, tex);
        }
        public void SetTexture(GPUKernel kernel, string name, Texture tex)
        {
            Data.SetTexture(kernel.ID, name, tex);
        }
        #endregion

        #region SetKeyword
        public void EnableKeyword(string keyword)
        {
            Data.EnableKeyword(keyword);
        }
        public void DisableKeyword(string keyword)
        {
            Data.DisableKeyword(keyword);
        }
        public void SetKeyword(string keyword, bool enabled)
        {
            if (enabled)
                Data.EnableKeyword(keyword);
            else
                Data.DisableKeyword(keyword);
        }
        #endregion

        #region Dispatch
        public void Dispatch(int kernelIndex, int threadGroupsX, int threadGroupsY = 1, int threadGroupsZ = 1)
        {
            Data.Dispatch(kernelIndex, threadGroupsX, threadGroupsY, threadGroupsZ);
        }
        public void Dispatch(GPUKernel kernel, int threadGroupsX, int threadGroupsY = 1, int threadGroupsZ = 1)
        {
            Data.Dispatch(kernel.ID, threadGroupsX, threadGroupsY, threadGroupsZ);
        }
        #endregion

        #region DispatchIndirect
        public void DispatchIndirect(int kernelIndex, GraphicsBuffer argsBuffer)
        {
            Data.DispatchIndirect(kernelIndex, argsBuffer);
        }
        public void DispatchIndirect(GPUKernel kernel, GraphicsBuffer argsBuffer)
        {
            Data.DispatchIndirect(kernel.ID, argsBuffer);
        }
        #endregion
    }
}