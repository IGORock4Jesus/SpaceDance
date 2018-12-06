using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceDance
{
	static class Scene
	{
		static List<GameObject> gameObjects = new List<GameObject>();

		public static void Render(Device device)
		{
			foreach (var go in gameObjects.ToArray())
			{
				go.Render(device);
			}
		}

		public static void Update(float time)
		{
			foreach (var go in gameObjects.ToArray())
			{
				go.Update(time);
			}
		}

		public static T Find<T>() where T : GameObject
		{
			return gameObjects.First(w => w is T) as T;
		}

		internal static void Add(GameObject gameObject)
		{
			gameObjects.Add(gameObject);
		}
	}
}
