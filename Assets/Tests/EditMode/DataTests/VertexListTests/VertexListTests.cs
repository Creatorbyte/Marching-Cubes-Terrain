using NUnit.Framework;
using Unity.Mathematics;

namespace Eldemarkki.VoxelTerrain.Data
{
    public class VertexListTests
    {
        [Test]
        public void VertexList_Everything_Initialized_To_0_0_0()
        {
            VertexList vertexList = new VertexList();

            for (int i = 0; i < 12; i++)
            {
                Assert.AreEqual(new float3(0, 0, 0), vertexList[i]);
            }
        }

        [Test]
        public void VertexList_SetGetTests([Range(0, 11)] int index)
        {
            VertexList vertexList = new VertexList();
            float3 expected = new float3(6.8f, 10.02f, -4.94f);

            vertexList[index] = expected;

            var actual = vertexList[index];
            Assert.AreEqual(expected, actual);
        }
    }
}
