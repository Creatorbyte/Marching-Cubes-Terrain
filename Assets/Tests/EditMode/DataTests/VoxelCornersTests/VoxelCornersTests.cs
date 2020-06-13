using NUnit.Framework;
using System;

namespace Eldemarkki.VoxelTerrain.Data
{
    public class VoxelCornersTests
    {
        [Test]
        public void VoxelCorners_Initializes_Values_To_Default_Values()
        {
            VoxelCorners<float> voxelCorners = new VoxelCorners<float>();
            for (int i = 0; i < 8; i++)
            {
                Assert.AreEqual(default(float), voxelCorners[i]);
            }
        }

        [Test]
        public void VoxelCorners_SetGet([Range(0, 7)] int index)
        {
            VoxelCorners<float> voxelCorners = new VoxelCorners<float>();
            float value = 15.6f;

            voxelCorners[index] = value;

            Assert.AreEqual(value, voxelCorners[index]);
        }

        [TestCase(-1)]
        [TestCase(-10)]
        [TestCase(8)]
        [TestCase(20)]
        public void VoxelCorners_Throws_If_Index_Out_Of_Range(int index)
        {
            VoxelCorners<float> voxelCorners = new VoxelCorners<float>();
            float value = 15.6f;

            Assert.Throws<IndexOutOfRangeException>(() => voxelCorners[index] = value);
        }
    }
}
