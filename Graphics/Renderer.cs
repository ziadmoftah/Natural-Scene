using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;
using GlmNet;
using System.IO;
namespace Graphics
{
    class Renderer
    {
        Shader sh;
        uint vertexBufferID;
        int transID;
        int viewID;
        int projID;
        mat4 scaleMat;
        mat4 ProjectionMatrix;
        mat4 ViewMatrix;
        public Camera cam;
        Texture BackTexture;
        Texture BottomTexture;
        Texture FrontTexture;
        Texture LeftTexture;
        Texture RightTexture;
        Texture TopTexture;
        public void Initialize()
        {
            string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            sh = new Shader(projectPath + "\\Shaders\\SimpleVertexShader.vertexshader", projectPath + "\\Shaders\\SimpleFragmentShader.fragmentshader");
            FrontTexture = new Texture(projectPath + "\\Textures\\front1.png", 3);
            BackTexture = new Texture(projectPath + "\\Textures\\back1.png", 4);
            RightTexture = new Texture(projectPath + "\\Textures\\right1.png", 5);
            LeftTexture = new Texture(projectPath + "\\Textures\\left1.png", 6);
            TopTexture = new Texture(projectPath + "\\Textures\\top1.png", 7);
            BottomTexture = new Texture(projectPath + "\\Textures\\bottom1.png", 8);
            Gl.glClearColor(0, 0, 0.4f, 1);
               float[] vertices =
                {
                //pos       //color     //uv
            
                // front
                -1,1,1,     1,1,1,       0,1,
                -1,-1,1,    1,1,1,       0,0,
                -1,-1,-1,   1,1,1,       1,0,

                -1,1,1,     1,1,1,       0,1,
                -1,1,-1,    1,1,1,       1,1,
                -1,-1,-1,   1,1,1,       1,0,

                //back
                1,1,-1,     1,1,1,       0,1,
                1,-1,-1,    1,1,1,       0,0,
                1,-1,1,     1,1,1,       1,0,

                1,1,-1,     1,1,1,       0,1,
                1,1,1,      1,1,1,       1,1,
                1,-1,1,     1,1,1,       1,0,

                //right
                -1,1,-1,    1,1,1,       0,1,
                -1,-1,-1,   1,1,1,       0,0,
                1,-1,-1,    1,1,1,       1,0,

                -1,1,-1,    1,1,1,       0,1,
                1,1,-1,     1,1,1,       1,1,
                1,-1,-1,    1,1,1,       1,0,
            
                //left
                1,1,1,      1,1,1,       0,1,
                1,-1,1,     1,1,1,       0,0,
                -1,-1,1,    1,1,1,       1,0,

                1,1,1,      1,1,1,       0,1,
                -1,1,1,     1,1,1,       1,1,
                -1,-1,1,    1,1,1,       1,0,

                //top
                1,1,1,     0,0,0,       0,1,
               -1,1,1,     0,0,0,       0,0,
               -1,1,-1,    0,0,0,       1,0,

                1,1,1,      0,0,0,       0,1,
                1,1,-1,     0,0,0,       1,1,
               -1,1,-1,     0,0,0,       1,0,

                //bottom
                -1,-1,1,    0,0,0,       0,1,
                1,-1,1,     0,0,0,       0,0,
                1,-1,-1,    0,0,0,       1,0,

                -1,-1,1,    0,0,0,       0,1,
                -1,-1,-1,   0,0,0,       1,1,
                1,-1,-1,    0,0,0,       1,0
            };
            vertexBufferID = GPU.GenerateBuffer(vertices);
            scaleMat = glm.scale(new mat4(1),new vec3(2f, 2f, 2.0f));
            cam = new Camera();
            ProjectionMatrix = cam.GetProjectionMatrix();
            ViewMatrix = cam.GetViewMatrix();
            transID = Gl.glGetUniformLocation(sh.ID, "model");
            projID = Gl.glGetUniformLocation(sh.ID, "projection");
            viewID = Gl.glGetUniformLocation(sh.ID, "view");
        }

        public void Draw()
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            sh.UseShader();
            Gl.glUniformMatrix4fv(transID, 1, Gl.GL_FALSE, scaleMat.to_array());
            Gl.glUniformMatrix4fv(projID, 1, Gl.GL_FALSE, ProjectionMatrix.to_array());
            Gl.glUniformMatrix4fv(viewID, 1, Gl.GL_FALSE, ViewMatrix.to_array());
            GPU.BindBuffer(vertexBufferID);
            Gl.glEnableVertexAttribArray(0);
            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), IntPtr.Zero);
            Gl.glEnableVertexAttribArray(1);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), (IntPtr)(3 * sizeof(float)));
            Gl.glEnableVertexAttribArray(2);
            Gl.glVertexAttribPointer(2, 2, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), (IntPtr)(6 * sizeof(float)));

            //front
            FrontTexture.Bind();
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, 3);
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 3, 3);

            //back
            BackTexture.Bind();
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 6, 3);
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 9, 3);

            //right
            RightTexture.Bind();
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 12, 3);
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 15, 3);

            //left
            LeftTexture.Bind();
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 18, 3);
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 21, 3);

            //top
            TopTexture.Bind();
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 24, 3);
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 27, 3);

            //bottom

            BottomTexture.Bind();
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 30, 3);
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 33, 3);

            Gl.glDisableVertexAttribArray(0);
            Gl.glDisableVertexAttribArray(1);
            Gl.glDisableVertexAttribArray(2);
        }
        public void Update()
        {
            cam.UpdateViewMatrix();
            ProjectionMatrix = cam.GetProjectionMatrix();
            ViewMatrix = cam.GetViewMatrix();
        }
        public void CleanUp()
        {
            sh.DestroyShader();
        }
    }
}
