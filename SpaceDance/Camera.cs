using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceDance
{
	class Camera
	{
		public void Render(Device device, float width, float height)
		{
			device.SetTransform(TransformState.View, Matrix.LookAtLH(new Vector3(0.0f, 0.0f, -100.0f), new Vector3(0.0f), new Vector3(0.0f, 1.0f, 0.0f)));

			device.SetTransform(TransformState.Projection, Matrix.OrthoLH(width, height, 0.1f, 1000.0f));
		}
	}
}
