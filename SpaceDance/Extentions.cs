using Assimp;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceDance
{
	static class Extentions
	{
		public static Node Find(this Node node, string name)
		{
			if (node.Name == name)
				return node;

			foreach (var child in node.Children)
			{
				var result = Find(child, name);
				if (result != null)
					return result;
			}

			return null;
		}

		public static float ToRadians(this float degree)
		{
			return MathUtil.DegreesToRadians(degree);
		}
	}
}
