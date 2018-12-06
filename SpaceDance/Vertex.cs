using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SpaceDance
{
	struct Vertex
	{
		public Vector3 position;
		public Vector2 texel;
		public static int Size => Marshal.SizeOf(typeof(Vertex));
		public static VertexFormat Format => VertexFormat.Position | VertexFormat.Texture1;
	}
}
