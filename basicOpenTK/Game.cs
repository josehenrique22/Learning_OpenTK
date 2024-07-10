using OpenTK;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
namespace BasicOpenTK{
    public class Game : GameWindow
    {
        private int vertexBufferHandle;
        private int shaderProgramHandle;
        private int vertexArrayHandle;
        
        public Game()
            : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            this.CenterWindow(new Vector2i(800, 600));
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            base.OnResize(e);
        }
        
        protected override void OnLoad()
        {
            GL.ClearColor(new Color4(0.3f, 0.4f, 0.5f, 1f));
            
            // Triangle have 3 vertices
            float[] vertices = new float[]
            {
                // Define all the vertices X, Y, Z
                // The graphics cards at the very top is 1+ Y ()max and -1 Y (min),
                // in the ring is +1 X (max)  and left -1 X (min)
                0.0f, 0.5f, 0.0f, // vertex 0
                5.0f, -5.0f, 0.0f, // vertex 1
                -5.0f, -5.0f, 0.0f // vertex 2
            };

            this.vertexBufferHandle = GL.GenBuffer(); // Essential, tell to render
            GL.BindBuffer(BufferTarget.ArrayBuffer, this.vertexBufferHandle); // tell what object is rendering
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw); // Send the data to the graphics cards, need to find the length data in bytes 
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            string vertexShaderCode =
            @"
                #version 330 core

                layout (location = 0) in vec3 aPosition;


                void main()
                {
                    gl_Position = vec4(aPosition, 1f);
                }
             ";

            string pixelShaderCoder =
            @"
                #version 330 core

                out vec4 pixelColor;

                void main()
                {
                    pixelColor = vec4(0.8f, 0.8f, 0.1f, 1f);
                }
             ";

            int vertexShaderHandle = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexBufferHandle, vertexShaderCode);
            GL.CompileShader(vertexBufferHandle);

            int pixelShaderHandle = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(pixelShaderHandle, pixelShaderCoder);
            GL.CompileShader(pixelShaderHandle);

            this.shaderProgramHandle = GL.CreateProgram();

            GL.AttachShader(this.shaderProgramHandle, vertexBufferHandle);
            GL.AttachShader(this.shaderProgramHandle, pixelShaderHandle);

            GL.LinkProgram(this.shaderProgramHandle);

            GL.DetachShader(this.shaderProgramHandle, vertexShaderHandle);
            GL.DetachShader(this.shaderProgramHandle, pixelShaderHandle);

            GL.DeleteShader(vertexShaderHandle);
            GL.DeleteShader(pixelShaderHandle);

            base.OnLoad();
        }

        protected override void OnUnload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(this.vertexBufferHandle);

            GL.UseProgram(0);
            GL.DeleteProgram(this.shaderProgramHandle);

            base.OnUnload();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.UseProgram(this.shaderProgramHandle);

            GL.BindBuffer(BufferTarget.ArrayBuffer, this.vertexBufferHandle);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            this.Context.SwapBuffers();
            base.OnRenderFrame(args);
        }
    }
}