using Assimp;
using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceDance
{
	static class ResourceManager
	{
		static List<Mesh> meshes = new List<Mesh>();
	public	static List<Vector3> BaseGuns { get; internal set; } = new List<Vector3>();

		public static void LoadAll(Device device)
		{
			string path = "..\\Resources\\Models";
			LoadModel(device, Path.Combine(path, "player_ship.fbx"), "ship", "playerShip", (node)=>
			{
				BaseGuns = node.Children.Select((w) =>
				{
					w.Transform.Decompose(out Vector3D scalling, out Assimp.Quaternion rotation, out Vector3D translation);
					return new Vector3(translation.X, translation.Y, translation.Z);
				}).ToList();
			});
			LoadModel(device, Path.Combine(path, "simple_patron.fbx"), "patron", "simplePatron");
		}

		public static void LoadModel(Device device, string filename, string nodename, string name, Action<Node> nodeMaker = null)
		{
			AssimpContext context = new AssimpContext();
			var scene = context.ImportFile(filename);
			var node = scene.RootNode.Find(nodename);

			if (node == null)
				throw new KeyNotFoundException($"File = {filename} => Node = {nodename}");

			var mesh = scene.Meshes[node.MeshIndices[0]];

			var m = new Mesh(device, mesh.Vertices.Select(w => new Vertex { position = new SharpDX.Vector3(w.X, w.Y, w.Z) }).ToArray(), mesh.GetIndices(), name);
			meshes.Add(m);

			nodeMaker?.Invoke(node);
		}

		public static Mesh GetMesh(string name) => meshes.First(w => w.Name == name);

		public static void Dispose()
		{
			foreach (var mesh in meshes)
			{
				mesh?.Dispose();
			}
		}
	}
}
