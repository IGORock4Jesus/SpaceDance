using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceDance
{
	class Input : IDisposable
	{
		DirectInput direct;
		Keyboard keyboard;
		private bool enabled;
		private Task task;

		public delegate void KeyHandler(Key key);
		public event KeyHandler KeyPressed;


		public Input(Form form)
		{
			direct = new DirectInput();
			keyboard = new Keyboard(direct);
			keyboard.SetCooperativeLevel(form.Handle, CooperativeLevel.Background | CooperativeLevel.NonExclusive);
			enabled = true;
			task = Task.Run(new Action(StartUpdate));
		}

		private void StartUpdate()
		{
			keyboard.Acquire();
			while (enabled)
			{
				var state = keyboard.GetCurrentState();
				foreach (var pressed in state.PressedKeys)
				{
					KeyPressed?.Invoke(pressed);
				}

				Thread.Sleep(10);
			}
			keyboard.Unacquire();
		}

		public void Dispose()
		{
			enabled = false;
			task.Wait();
			keyboard?.Dispose();
			direct?.Dispose();
		}
	}
}
