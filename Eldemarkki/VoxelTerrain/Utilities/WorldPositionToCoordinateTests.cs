using Eldemarkki.VoxelTerrain.Utilities;
using NUnit.Framework;
using Unity.Mathematics;

namespace Eldemarkki.VoxelTerrain.Utilities
{
    public class WorldPositionToCoordinateTests
    {
        [TestCase(0, 0, 0, 16, 0, 0, 0)]
        [TestCase(17, 14, 9, 16, 1, 0, 0)]
        [TestCase(120, -64, 0, 16, 7, -4, 0)]
        [TestCase(120, -64, -0.1f, 16, 7, -4, -1)]
        [TestCase(120, -64.1f, -0.1f, 16, 7, -5, -1)]
        [TestCase(17, 14, -90, 4, 4, 3, -23)]
        public void TestWorldPositionToCoordinate(float worldPositionX, float worldPositionY, float worldPositionZ, int chunkSize, int expectedX, int expectedY, int expectedZ)
        {
            Assert.AreEqual(new int3(expectedX, expectedY, expectedZ), VectorUtilities.WorldPositionToCoordinate(new float3(worldPositionX, worldPositionY, worldPositionZ), chunkSize));
        }
    }
}
