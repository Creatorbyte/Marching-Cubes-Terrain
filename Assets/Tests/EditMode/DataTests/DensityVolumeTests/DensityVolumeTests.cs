using Eldemarkki.VoxelTerrain.Density;
using NUnit.Framework;
using Unity.Collections;
using Unity.Mathematics;

namespace Tests
{
    public class DensityVolumeTests
    {
        private DensityVolume densityVolume;

        [TestCase(0, 0)]
        [TestCase(5, 5)]
        [TestCase(16, 16)]
        public void Test_Int_Size_Constructor_Width(int size, int expectedSize)
        {
            densityVolume = new DensityVolume(size, Allocator.Temp);
            Assert.AreEqual(expectedSize, densityVolume.Width);
        }

        [TestCase(0, 0)]
        [TestCase(5, 5)]
        [TestCase(16, 16)]
        public void Test_Int_Size_Constructor_Height(int size, int expectedSize)
        {
            densityVolume = new DensityVolume(size, Allocator.Temp);
            Assert.AreEqual(expectedSize, densityVolume.Height);

        }

        [TestCase(0, 0)]
        [TestCase(5, 5)]
        [TestCase(16, 16)]
        public void Test_Int_Size_Constructor_Depth(int size, int expectedSize)
        {
            densityVolume = new DensityVolume(size, Allocator.Temp);
            Assert.AreEqual(expectedSize, densityVolume.Depth);
        }

        [TestCase(0, 0)]
        [TestCase(5, 5)]
        [TestCase(16, 16)]
        public void Test_Int_Size_Constructor_Size(int size, int expectedSize)
        {
            densityVolume = new DensityVolume(size, Allocator.Temp);
            Assert.AreEqual(new int3(expectedSize, expectedSize, expectedSize), densityVolume.Size);
        }

        [TestCase(0, 0)]
        [TestCase(5, 5)]
        [TestCase(16, 16)]
        public void Test_Int_Size_Constructor_Length(int size, int expectedSize)
        {
            densityVolume = new DensityVolume(size, Allocator.Temp);
            Assert.AreEqual(expectedSize * expectedSize * expectedSize, densityVolume.Length);
        }

        [TestCase(0)]
        [TestCase(5)]
        [TestCase(16)]
        public void Test_Int_Size_Constructor_IsCreated(int size)
        {
            densityVolume = new DensityVolume(size, Allocator.Temp);
            Assert.AreEqual(true, densityVolume.IsCreated);
        }

        public void Test_Int_Size_Constructor_Negative_Throws(int size)
        {
            Assert.Throws<System.ArgumentException>(() =>
            {
                densityVolume = new DensityVolume(size, Allocator.Temp);
            });
        }

        [Test]
        public void Test_Int_Size_Constructor_IsNotCreated()
        {
            densityVolume = new DensityVolume();
            Assert.AreEqual(false, densityVolume.IsCreated);
        }

        [Test]
        public void Test_Densities_Are_Initialized_To_Negative1()
        {
            densityVolume = new DensityVolume(5, Allocator.Temp);
            for (int i = 0; i < densityVolume.Length; i++)
            {
                Assert.AreEqual(-1, densityVolume.GetDensity(i));
            }
        }

        [Test]
        public void Test_SetGetDensityIndex([Random(-1f, 1f, 5)] float newDensity, [Random(0, 5 * 5 * 5 - 1, 5)] int index)
        {
            densityVolume = new DensityVolume(5, Allocator.Temp);

            densityVolume.SetDensity(newDensity, index);
            float actualDensity = densityVolume.GetDensity(index);

            Assert.IsTrue(AreDensitiesSame(newDensity, actualDensity), $"Expected {newDensity}, actual was {actualDensity}");
        }

        [Test]
        public void Test_SetGetDensityXyz([Random(-1f, 1f, 5)] float newDensity, [Random(0, 4, 3)] int x, [Random(0, 4, 3)] int y, [Random(0, 4, 3)] int z)
        {
            densityVolume = new DensityVolume(5, Allocator.Temp);

            densityVolume.SetDensity(newDensity, x, y, z);
            float actualDensity = densityVolume.GetDensity(x, y, z);

            Assert.IsTrue(AreDensitiesSame(newDensity, actualDensity), $"Expected {newDensity}, actual was {actualDensity}");
        }

        [Test]
        public void Test_SetGetDensityInt3([Random(-1f, 1f, 5)] float newDensity, [Random(0, 4, 3)] int x, [Random(0, 4, 3)] int y, [Random(0, 4, 3)] int z)
        {
            densityVolume = new DensityVolume(5, Allocator.Temp);

            densityVolume.SetDensity(newDensity, new int3(x, y, z));
            float actualDensity = densityVolume.GetDensity(new int3(x, y, z));

            Assert.IsTrue(AreDensitiesSame(newDensity, actualDensity), $"Expected {newDensity}, actual was {actualDensity}");
        }

        // This function tests if two densities are nearly the same. Nearly meaning them remapped from (-1f to 1f) to (0 to 255), 
        // where they are rounded if needed. That rounding causes slight inaccuracy so this function is needed to check for that.
        private bool AreDensitiesSame(float a, float b)
        {
            float aByte = 127.5f * (a + 1);
            float bByte = 127.5f * (b + 1);

            // They may have to be ceil-ed due to floating point imprecision
            if (math.ceil(aByte) - aByte < 0.0001f) aByte = math.ceil(aByte);
            if (math.ceil(bByte) - bByte < 0.0001f) bByte = math.ceil(bByte);

            return math.floor(aByte) == math.floor(bByte);
        }

        [TearDown]
        public void Teardown()
        {
            if (densityVolume.IsCreated)
                densityVolume.Dispose();
        }
    }
}
