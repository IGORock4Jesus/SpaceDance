using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceDance
{
	class Player : GameObject
	{
		Mesh mesh;
		private Matrix[] guns;
		Vector3 position = new Vector3();
		float rotation = 0.0f;
		const float secondsPerTurn = 2.0f;
		float timeLastRechage = 0.0f;
		float shutSpeed = 0.7f; // стреляет раз в секунду
		private Type gunType = typeof(SimplePatron);
		private Matrix transform;

		public Player(Mesh mesh, Vector3[] gunPositions)
		{
			this.mesh = mesh;
			this.guns = gunPositions.Select(w=>Matrix.Translation(w)).ToArray();
		}

		public override void Render(Device device)
		{
			transform =  Matrix.RotationX(rotation.ToRadians()) * Matrix.Translation(position);
			device.SetTransform(TransformState.World, Matrix.RotationY((90.0f).ToRadians()) * transform);
			//device.SetTransform(TransformState.World, Matrix.Identity);
			mesh.Render(device);
			
		}

		public void Move(Vector2 vector)
		{
			position += new Vector3(vector, 0.0f);
		}

		public override void Update(float time)
		{
			if (time == 0.0f) return;
			rotation += 360.0f / secondsPerTurn * time;

			timeLastRechage += time;
			if (timeLastRechage >= shutSpeed)
			{
				timeLastRechage -= shutSpeed;

				// рождаем пулю
				foreach (var baseGun in guns)
				{
					var patron = (BasePatron)Activator.CreateInstance(gunType);
					patron.Transform = transform * baseGun;
					Scene.Add(patron);
				}
			}
		}
	}
}
