using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceDance
{
	class Mesh : IDisposable
	{
		VertexBuffer vertexBuffer;
		IndexBuffer indexBuffer;
		readonly int indexCount;

		public string Name { get; }

		public Mesh(Device device, Vertex[] vertices, int[] indices, string name)
		{
			vertexBuffer = new VertexBuffer(device, Vertex.Size * vertices.Length, Usage.WriteOnly, Vertex.Format, Pool.Managed);
			indexBuffer = new IndexBuffer(device, sizeof(int) * indices.Length, Usage.WriteOnly, Pool.Managed, false);
			indexCount = indices.Length;

			var stream = vertexBuffer.Lock(0, 0, LockFlags.None);
			stream.WriteRange(vertices);
			vertexBuffer.Unlock();

			var istream = indexBuffer.Lock(0, 0, LockFlags.None);
			istream.WriteRange(indices);
			indexBuffer.Unlock();
			Name = name;
		}

		public void Dispose()
		{
			indexBuffer?.Dispose();
			vertexBuffer?.Dispose();
		}

		public void Render(Device device)
		{
			device.VertexFormat = Vertex.Format;
			device.SetStreamSource(0, vertexBuffer, 0, Vertex.Size);
			device.Indices = indexBuffer;
			device.DrawIndexedPrimitive(PrimitiveType.TriangleList, 0, 0, indexCount, 0, indexCount / 3);
		}
	}
}
