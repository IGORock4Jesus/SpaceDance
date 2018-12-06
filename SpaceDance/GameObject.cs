using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceDance
{
	class GameObject
	{
		public virtual void Render(Device device) { }
		public virtual void Update(float time) { }
	}
}
