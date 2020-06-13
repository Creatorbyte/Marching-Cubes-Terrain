﻿using System;
using Eldemarkki.VoxelTerrain.Utilities;
using Unity.Collections;
using Unity.Mathematics;

namespace Eldemarkki.VoxelTerrain.Data
{
    /// <summary>
    /// A 3-dimensional volume of densities
    /// </summary>
    public struct DensityVolume : IDisposable
    {
        /// <summary>
        /// The native array which contains the density values. Densities are stored as bytes (0 to 255), and later mapped to go from -1 to 1
        /// </summary>
        private NativeArray<byte> _densities;

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
        /// How many density values this volume contains
        /// </summary>
        public int Length => Width * Height * Depth;

        /// <summary>
        /// Is the densities array allocated in memory
        /// </summary>
        public bool IsCreated => _densities.IsCreated;

        /// <summary>
        /// Constructor to create a DensityVolume
        /// </summary>
        /// <param name="width">The width of the volume</param>
        /// <param name="height">The height of the volume</param>
        /// <param name="depth">The depth of the volume</param>
        /// <param name="allocator">How the memory should be allocated</param>
        public DensityVolume(int width, int height, int depth, Allocator allocator = Allocator.Persistent)
        {
            if (width < 0 || height < 0 || depth < 0) throw new ArgumentException("Width, height, and depth should all be positive!");

            _densities = new NativeArray<byte>(width * height * depth, allocator);

            Width = width;
            Height = height;
            Depth = depth;
        }

        /// <summary>
        /// Constructor to create a DensityVolume
        /// </summary>
        /// <param name="size">Amount of items in 1 dimension of this volume</param>
        /// <param name="allocator">How the memory should be allocated</param>
        public DensityVolume(int size, Allocator allocator = Allocator.Persistent) : this(size, size, size, allocator) { }

        /// <summary>
        /// Constructor to create a DensityVolume
        /// </summary>
        /// <param name="size">The 3-dimensional size of this volume</param>
        /// <param name="allocator">How the memory should be allocated</param>
        public DensityVolume(int3 size, Allocator allocator = Allocator.Persistent) : this(size.x, size.y, size.z, allocator) { }

        /// <summary>
        /// Disposes the native densities array
        /// </summary>
        public void Dispose()
        {
            _densities.Dispose();
        }

        /// <summary>
        /// Sets the density in the specified location. Density is clamped to go from -1 to 1
        /// </summary>
        /// <param name="density">The new density</param>
        /// <param name="localPosition">The density location</param>
        public void SetDensity(float density, int3 localPosition)
        {
            int index = IndexUtilities.XyzToIndex(localPosition, Width, Height);
            SetDensity(density, index);
        }

        /// <summary>
        /// Stores the density in the specified location. Density is clamped to go from -1 to 1
        /// </summary>
        /// <param name="density">The new density</param>
        /// <param name="x">The x value of the density location</param>
        /// <param name="y">The y value of the density location</param>
        /// <param name="z">The z value of the density location</param>
        public void SetDensity(float density, int x, int y, int z)
        {
            int index = IndexUtilities.XyzToIndex(x, y, z, Width, Height);
            SetDensity(density, index);
        }

        /// <summary>
        /// Stores the density to the specified index. Density is clamped to go from -1 to 1
        /// </summary>
        /// <param name="density">The new density</param>
        /// <param name="index">The index in the native array</param>
        public void SetDensity(float density, int index)
        {
            _densities[index] = (byte) (127.5 * (math.clamp(density, -1, 1) + 1));
        }

        /// <summary>
        /// Gets the density value from the local position (x,y,z) in the range from -1 to 1
        /// </summary>
        /// <param name="localPosition"></param>
        /// <returns></returns>
        public float GetDensity(int3 localPosition)
        {
            return GetDensity(localPosition.x, localPosition.y, localPosition.z);
        }

        /// <summary>
        /// Gets the density value from the local position (x,y,z) in the range from -1 to 1
        /// </summary>
        /// <param name="x">The x value of the density location</param>
        /// <param name="y">The y value of the density location</param>
        /// <param name="z">The z value of the density location</param>
        /// <returns>A density value in the range from -1 to 1 in the specified location</returns>
        public float GetDensity(int x, int y, int z)
        {
            int index = IndexUtilities.XyzToIndex(x, y, z, Width, Height);
            return GetDensity(index);
        }

        /// <summary>
        /// Gets the density value from an index in the native array, return value is in the range from -1 to 1
        /// </summary>
        /// <param name="index">The index in the native array</param>
        /// <returns>A density value in the range from -1 to 1 at the specified index</returns>
        public float GetDensity(int index)
        {
            return _densities[index] / 127.5f - 1;
        }

        /// <summary>
        /// Copies the densities from the source volume if the volumes are the same size
        /// </summary>
        /// <param name="sourceVolume">The source volume, which should be the same size as this volume</param>
        public void CopyFrom(DensityVolume sourceVolume)
        {
            if (Width == sourceVolume.Width && Height == sourceVolume.Height && Depth == sourceVolume.Depth)
            {
                _densities.CopyFrom(sourceVolume._densities);
            }
            else
            {
                throw new Exception(
                    $"The chunks are not the same size! Width: {Width}/{sourceVolume.Width}, Height: {Height}/{sourceVolume.Height}, Depth: {Depth}/{sourceVolume.Depth}");
            }
        }
    }
}