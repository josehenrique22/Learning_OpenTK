using OpenTK;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
namespace BasicOpenTK{
    public class Game : GameWindow
    {
        public Game()
            : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            this.CenterWindow(new Vector2i(800, 600));
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.ClearColor(new Color4(0.3f, 0.4f, 0.5f, 1f));
            GL.Clear(ClearBufferMask.ColorBufferBit);

            this.Context.SwapBuffers();
            base.OnRenderFrame(args);
        }
    }
}