using System;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace BasicOpenTk
{
    public class Game : GameWindow
    {
        private int vertexBufferHandle;
        private int shaderProgramHandle;
        private int vertexArrayHandle;

        public Game(int width = 800, int height = 600, string title = "BasicOpenTK")
            : base(
                GameWindowSettings.Default,
                new NativeWindowSettings()
                {
                    Size = new Vector2i(width, height),
                    Title = title,
                    WindowBorder = WindowBorder.Fixed,
                    StartVisible = false,
                    StartFocused = true,
                    API = ContextAPI.OpenGL,
                    Profile = ContextProfile.Core,
                    APIVersion = new Version(3,3)
                })
        {
            this.CenterWindow();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            base.OnResize(e);
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            this.IsVisible = true;

            GL.ClearColor(new Color4(0.3f, 0.4f, 0.5f, 1.0f));

            float[] vertices = new float[]
            {
                0.0f, 0.5f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f,
                0.4f, -0.5f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f,
                -0.4f, -0.5f, 0.0f, 0.0f, 0.0f, 1.0f, 1.0f
            };

            this.vertexBufferHandle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, this.vertexBufferHandle);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            this.vertexArrayHandle = GL.GenVertexArray();
            GL.BindVertexArray(this.vertexArrayHandle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, this.vertexBufferHandle);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 7 * sizeof(float), 0);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 7 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.BindVertexArray(0);


            string vertexShaderCode =
            @"
            #version 330 core

                layout(location = 0) in vec3 aPosition;
                layout(location = 1) in vec4 aColor;
                out vec4 vColor;

                void main(void)
                {
                    gl_Position = vec4(aPosition, 1.0);
                    vColor = aColor;
                }
            ";

            string pixelShaderCode =
            @"
            #version 330
            in vec4 vColor;
            out vec4 outputColor;

            void main()
            {
                outputColor = vColor;
            }
            ";

            int vertexShaderHandle = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShaderHandle, vertexShaderCode);
            GL.CompileShader(vertexShaderHandle);

            int pixelShaderHandle = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(pixelShaderHandle, pixelShaderCode);
            GL.CompileShader(pixelShaderHandle);

            this.shaderProgramHandle = GL.CreateProgram();

            // Após compilar o shader de vértices
            int status;
            GL.GetShader(vertexShaderHandle, ShaderParameter.CompileStatus, out status);
            if (status != 1)
            {
                string infoLog = GL.GetShaderInfoLog(vertexShaderHandle);
                Console.WriteLine("Erro ao compilar o shader de vértices: " + infoLog);
            }

            // Após compilar o shader de pixels
            GL.GetShader(pixelShaderHandle, ShaderParameter.CompileStatus, out status);
            if (status != 1)
            {
                string infoLog = GL.GetShaderInfoLog(pixelShaderHandle);
                Console.WriteLine("Erro ao compilar o shader de pixels: " + infoLog);
            }


            GL.AttachShader(this.shaderProgramHandle, vertexShaderHandle);
            GL.AttachShader(this.shaderProgramHandle, pixelShaderHandle);

            GL.LinkProgram(this.shaderProgramHandle);

            GL.DetachShader(this.shaderProgramHandle, vertexShaderHandle);
            GL.DetachShader(this.shaderProgramHandle, pixelShaderHandle);

            GL.DeleteShader(vertexShaderHandle);
            GL.DeleteShader(pixelShaderHandle);
        }

        protected override void OnUnload()
        {
            GL.BindVertexArray(0);
            GL.DeleteVertexArray(this.vertexArrayHandle);

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
            GL.BindVertexArray(this.vertexArrayHandle);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            this.SwapBuffers();
            base.OnRenderFrame(args);
        }
    }
}
