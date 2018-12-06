using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceDance
{
	class Renderer : IDisposable
	{
		private Direct3D direct;
		private Device device;
		private bool enabled;
		private Task task;

		public Device Device => device;

		public delegate void RendererHandler(Renderer renderer);
		public event RendererHandler Rendering;

		public Renderer(Form form)
		{
			direct = new Direct3D();
			device = new Device(direct, 0, DeviceType.Hardware, form.Handle, CreateFlags.HardwareVertexProcessing | CreateFlags.Multithreaded, new PresentParameters
			{
				AutoDepthStencilFormat = Format.D24S8,
				BackBufferCount = 1,
				BackBufferFormat = Format.A8R8G8B8,
				BackBufferHeight = form.ClientSize.Height,
				BackBufferWidth = form.ClientSize.Width,
				DeviceWindowHandle = form.Handle,
				EnableAutoDepthStencil = true,
				SwapEffect = SwapEffect.Discard,
				Windowed = true
			});

			device.SetRenderState(RenderState.Lighting, false);

			enabled = true;
			task = Task.Run(new Action(RenderStart));
		}

		private void RenderStart()
		{
			while (enabled)
			{
				device.Clear(ClearFlags.All, Color.Black, 1.0f, 0);
				device.BeginScene();

				Rendering?.Invoke(this);

				device.EndScene();
				device.Present();

				Thread.Sleep(1);
			}
		}

		public void Dispose()
		{
			enabled = false;
			task.Wait();
			device?.Dispose();
			direct?.Dispose();
		}
	}
}
