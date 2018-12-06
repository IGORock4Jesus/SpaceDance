using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct3D9;

namespace SpaceDance
{
	class BasePatron : GameObject
	{
		public float ChargingTime { get; }
		public Matrix Transform { get; internal set; }

		float speed = 1000.0f;


		Mesh mesh;

		public BasePatron(string mesh, float chargingTime)
		{
			this.mesh = ResourceManager.GetMesh(mesh);
			ChargingTime = chargingTime;
		}

		public override void Render(Device device)
		{
			device.SetTransform(TransformState.World,Matrix.RotationY((90.0f).ToRadians()) * Transform);
			mesh.Render(device);
		}

		public override void Update(float time)
		{
			Transform = Matrix.Translation(new Vector3(speed * time, 0.0f, 0.0f))*Transform;
		}
	}
}
