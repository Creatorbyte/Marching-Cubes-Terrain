﻿using System;
using Eldemarkki.VoxelTerrain.Utilities;
using Unity.Collections;
using Unity.Mathematics;

namespace Eldemarkki.VoxelTerrain.VoxelData
{
    /// <summary>
    /// A 3-dimensional volume of voxel data
    /// </summary>
    public struct VoxelDataVolume : IDisposable
    {
        /// <summary>
        /// The native array which contains the voxel data. Voxel data is stored as bytes (0 to 255), and later mapped to go from 0 to 1
        /// </summary>
        private NativeArray<byte> _voxelData;

        /// <summary>
        /// The width of the volume
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// The height of the volume
        /// </summary>
        public int Height { get; }
        
        /// <summary>
        /// The depth of the volume
        /// </summary>
        public int Depth { get; }

        /// <summary>
        /// The size of this volume
        /// </summary>
        public int3 Size => new int3(Width, Height, Depth);

        /// <summary>
        /// How many voxel data points does this volume contain
        /// </summary>
        public int Length => Width * Height * Depth;

        /// <summary>
        /// Is the voxel data array allocated in memory
        /// </summary>
        public bool IsCreated => _voxelData.IsCreated;

        /// <summary>
        /// Creates a <see cref="VoxelDataVolume"/> with a persistent allocator
        /// </summary>
        /// <param name="width">The width of the volume</param>
        /// <param name="height">The height of the volume</param>
        /// <param name="depth">The depth of the volume</param>
        public VoxelDataVolume(int width, int height, int depth) : this(width, height, depth, Allocator.Persistent) { }

        /// <summary>
        /// Creates a <see cref="VoxelDataVolume"/>
        /// </summary>
        /// <param name="width">The width of the volume</param>
        /// <param name="height">The height of the volume</param>
        /// <param name="depth">The depth of the volume</param>
        /// <param name="allocator">How the memory should be allocated</param>
        public VoxelDataVolume(int width, int height, int depth, Allocator allocator)
        {
            _voxelData = new NativeArray<byte>(width * height * depth, allocator);

            Width = width;
            Height = height;
            Depth = depth;
        }

        /// <summary>
        /// Creates a <see cref="VoxelDataVolume"/> with a persistent allocator
        /// </summary>
        /// <param name="size">Amount of items in 1 dimension of this volume</param>
        public VoxelDataVolume(int size) : this(size, Allocator.Persistent) { }

        /// <summary>
        /// Creates a <see cref="VoxelDataVolume"/>
        /// </summary>
        /// <param name="size">Amount of items in 1 dimension of this volume</param>
        /// <param name="allocator">How the memory should be allocated</param>
        public VoxelDataVolume(int size, Allocator allocator) : this(size, size, size, allocator) { }

        /// <summary>
        /// Creates a <see cref="VoxelDataVolume"/> with a persistent allocator
        /// </summary>
        /// <param name="size">The 3-dimensional size of this volume</param>
        public VoxelDataVolume(int3 size) : this(size.x, size.y, size.z, Allocator.Persistent) { }

        /// <summary>
        /// Creates a <see cref="VoxelDataVolume"/>
        /// </summary>
        /// <param name="size">The 3-dimensional size of this volume</param>
        /// <param name="allocator">How the memory should be allocated</param>
        public VoxelDataVolume(int3 size, Allocator allocator) : this(size.x, size.y, size.z, allocator) { }

        /// <summary>
        /// Disposes the native voxel data array
        /// </summary>
        public void Dispose()
        {
            _voxelData.Dispose();
        }

        /// <summary>
        /// Stores the <paramref name="voxelData"/> at <paramref name="localPosition"/>. The <paramref name="voxelData"/> will be clamped to be in range [0, 1]
        /// </summary>
        /// <param name="voxelData">The new voxel data</param>
        /// <param name="localPosition">The location of that voxel data</param>
        public void SetVoxelData(float voxelData, int3 localPosition)
        {
            int index = IndexUtilities.XyzToIndex(localPosition, Width, Height);
            SetVoxelData(voxelData, index);
        }

        /// <summary>
        /// Stores the <paramref name="voxelData"/> at <paramref name="x"/>, <paramref name="y"/>, <paramref name="z"/>. The <paramref name="voxelData"/> will be clamped to be in range [0, 1]
        /// </summary>
        /// <param name="voxelData">The new voxel data</param>
        /// <param name="x">The x value of the voxel data location</param>
        /// <param name="y">The y value of the voxel data location</param>
        /// <param name="z">The z value of the voxel data location</param>
        public void SetVoxelData(float voxelData, int x, int y, int z)
        {
            int index = IndexUtilities.XyzToIndex(x, y, z, Width, Height);
            SetVoxelData(voxelData, index);
        }

        /// <summary>
        /// Stores the <paramref name="voxelData"/> at <paramref name="index"/>. The <paramref name="voxelData"/> will be clamped to be in range [0, 1]
        /// </summary>
        /// <param name="voxelData">The new voxel data</param>
        /// <param name="index">The index in the native array</param>
        public void SetVoxelData(float voxelData, int index)
        {
            _voxelData[index] = (byte) (255f * math.saturate(voxelData));
        }

        /// <summary>
        /// Tries to get the voxel data at <paramref name="localPosition"/>. If the data exists at <paramref name="localPosition"/>, true will be returned and <paramref name="voxelData"/> will be set to the value (range [0, 1]). If it doesn't exist, false will be returned and <paramref name="voxelData"/> will be set to 0.
        /// </summary>
        /// <param name="localPosition">The local position of the voxel data to get</param>
        /// <param name="voxelData">A voxel data in the range [0, 1] at <paramref name="localPosition"/></param>
        /// <returns>Does a voxel data point exist at <paramref name="localPosition"/></returns>
        public bool TryGetVoxelData(int3 localPosition, out float voxelData)
        {
            return TryGetVoxelData(localPosition.x, localPosition.y, localPosition.z, out voxelData);
        }

        /// <summary>
        /// Tries to get the voxel data at <paramref name="x"/>, <paramref name="y"/>, <paramref name="z"/>. If the data exists at <paramref name="x"/>, <paramref name="y"/>, <paramref name="z"/>, true will be returned and <paramref name="voxelData"/> will be set to the value (range [0, 1]). If it doesn't exist, false will be returned and <paramref name="voxelData"/> will be set to 0.
        /// </summary>
        /// <param name="x">The x value of the voxel data location</param>
        /// <param name="y">The y value of the voxel data location</param>
        /// <param name="z">The z value of the voxel data location</param>
        /// <param name="voxelData">A voxel data in the range [0, 1] at <paramref name="x"/>, <paramref name="y"/>, <paramref name="z"/></param>
        /// <returns>Does a voxel data point exist at <paramref name="x"/>, <paramref name="y"/>, <paramref name="z"/></returns>
        public bool TryGetVoxelData(int x, int y, int z, out float voxelData)
        {
            int index = IndexUtilities.XyzToIndex(x, y, z, Width, Height);
            return TryGetVoxelData(index, out voxelData);
        }

        /// <summary>
        /// Gets the voxel data at <paramref name="index"/>. If the data exists at <paramref name="index"/>, true will be returned and <paramref name="voxelData"/> will be set to the value (range [0, 1]). If it doesn't exist, false will be returned and <paramref name="voxelData"/> will be set to 0.
        /// </summary>
        /// <param name="index">The index in the native array</param>
        /// <param name="voxelData">A voxel data in the range [0, 1] at <paramref name="index"/></param>
        /// <returns>Does a voxel data point exist at <paramref name="index"/></returns>
        public bool TryGetVoxelData(int index, out float voxelData)
        {
            if (index >= 0 && index < _voxelData.Length)
            {
                voxelData = _voxelData[index] / 255f;
                return true;
            }
            
            voxelData = 0;
            return false;
        }

        /// <summary>
        /// Copies the voxel data from the source volume if the volumes are the same size
        /// </summary>
        /// <param name="sourceVolume">The source volume, which should be the same size as this volume</param>
        public void CopyFrom(VoxelDataVolume sourceVolume)
        {
            if (Width == sourceVolume.Width && Height == sourceVolume.Height && Depth == sourceVolume.Depth)
            {
                _voxelData.CopyFrom(sourceVolume._voxelData);
            }
            else
            {
                throw new ArgumentException($"The chunks are not the same size! Width: {Width.ToString()}/{sourceVolume.Width.ToString()}, Height: {Height.ToString()}/{sourceVolume.Height.ToString()}, Depth: {Depth.ToString()}/{sourceVolume.Depth.ToString()}");
            }
        }
    }
}