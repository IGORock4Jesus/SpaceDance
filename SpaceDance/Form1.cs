using SharpDX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceDance
{
	public partial class Form1 : Form
	{
		private Renderer renderer;
		Camera camera;
		private bool enabled;
		private Task updateTask;
		Input input;
		readonly List<IDisposable> disposables = new List<IDisposable>();
		float playerMoveSpeed = 7.0f;

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			renderer = new Renderer(this);
			renderer.Rendering += Renderer_Rendering;
			disposables.Add(renderer);

			input = new Input(this);
			input.KeyPressed += Input_KeyPressed;
			disposables.Add(input);

			ResourceManager.LoadAll(renderer.Device);

			var playerShip = ResourceManager.GetMesh("playerShip");

			Scene.Add(new Player(playerShip, ResourceManager.BaseGuns.ToArray()));

			camera = new Camera();

			enabled = true;
			updateTask = Task.Run(new Action(StartUpdate));
		}

		private void Input_KeyPressed(SharpDX.DirectInput.Key key)
		{
			if (key == SharpDX.DirectInput.Key.W)
				Scene.Find<Player>().Move(new Vector2(0.0f, playerMoveSpeed));
			if (key == SharpDX.DirectInput.Key.S)
				Scene.Find<Player>().Move(new Vector2(0.0f, -playerMoveSpeed));
			if (key == SharpDX.DirectInput.Key.D)
				Scene.Find<Player>().Move(new Vector2(playerMoveSpeed, 0.0f));
			if (key == SharpDX.DirectInput.Key.A)
				Scene.Find<Player>().Move(new Vector2(-playerMoveSpeed, 0.0f));
		}

		private void StartUpdate()
		{
			int oldTime = Environment.TickCount;
			while (enabled)
			{
				int newTime = Environment.TickCount;
				float elapsed = (newTime - oldTime) * 0.001f;
				oldTime = newTime;

				Scene.Update(elapsed);
			}
		}

		private void Renderer_Rendering(Renderer renderer)
		{
			SharpDX.Direct3D9.Device device = renderer.Device;
			camera?.Render(device, ClientSize.Width, ClientSize.Height);

			Scene.Render(device);
		}

		private void Form1_FormClosed(object sender, FormClosedEventArgs e)
		{
			enabled = false;
			updateTask.Wait();
			foreach (var d in disposables)
			{
				d.Dispose();
			}
			ResourceManager.Dispose();
		}
	}
}
