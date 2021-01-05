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
        uint vertexBufferID2;
        int transID;
        int viewID;
        int projID;
        mat4 scaleMat;

        mat4 ProjectionMatrix;
        mat4 ViewMatrix;


        public Camera cam;

        Texture tex1;
        Texture tex2;
        SkyBox skybox;

        public void Initialize()
        {
            string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            sh = new Shader(projectPath + "\\Shaders\\SimpleVertexShader.vertexshader", projectPath + "\\Shaders\\SimpleFragmentShader.fragmentshader");
            //tex1 = new Texture(projectPath + "\\Textures\\crate.jpg", 1);
            //tex2 = new Texture(projectPath + "\\Textures\\Ground.jpg", 2);


            Gl.glClearColor(0, 0, 0.4f, 1);
            skybox = new SkyBox();

            //float[] verts = {
            //    -1.0f, -1.0f, 0.0f,
            //     1,1,1,
            //     0,0,
            //
            //     1.0f, -1.0f, 0.0f,
            //     1,1,1,
            //     0,1,
            //
            //     0.0f,  1.0f, 0.0f,
            //     1,1,1,
            //     1,0
            //
            //};
            //
            //
            //float[] ground = {
            //    -5.0f, -1.0f, 5.0f,//1
            //     0,0,1,
            //     0,0,
            //
            //     5.0f, -1.0f, -5.0f,//2
            //     0,0,1,
            //     1,1,
            //
            //    -5.0f, -1.0f, -5.0f,
            //     0,0,1,
            //     1,0,
            //
            //     5.0f, -1.0f, 5.0f,
            //     0,0,1,
            //     0,1,
            //
            //    -5.0f, -1.0f, 5.0f,//1
            //     0,0,1,
            //     0,0,
            //
            //     5.0f, -1.0f, -5.0f,//2
            //     0,0,1,
            //     1,1
            //};

            //vertexBufferID = GPU.GenerateBuffer(verts);
            //vertexBufferID2 = GPU.GenerateBuffer(ground);

            //scaleMat = glm.scale(new mat4(1),new vec3(2f, 2f, 2.0f));

            cam = new Camera();
            
            ProjectionMatrix = cam.GetProjectionMatrix();
            ViewMatrix = cam.GetViewMatrix();

            //transID = Gl.glGetUniformLocation(sh.ID, "model");
            //projID = Gl.glGetUniformLocation(sh.ID, "projection");
            //viewID = Gl.glGetUniformLocation(sh.ID, "view");

        }

        public void Draw()
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
            sh.UseShader();

            Gl.glUniformMatrix4fv(transID, 1, Gl.GL_FALSE, scaleMat.to_array());
            Gl.glUniformMatrix4fv(projID, 1, Gl.GL_FALSE, ProjectionMatrix.to_array());
            Gl.glUniformMatrix4fv(viewID, 1, Gl.GL_FALSE, ViewMatrix.to_array());

            GPU.BindBuffer(skybox.vertexbuffer);
            Gl.glEnableVertexAttribArray(0);
            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), IntPtr.Zero);
            Gl.glEnableVertexAttribArray(1);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), (IntPtr)(3 * sizeof(float)));
            Gl.glEnableVertexAttribArray(2);
            Gl.glVertexAttribPointer(2, 2, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), (IntPtr)(6 * sizeof(float)));

            //front
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, 3);
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 3, 3);

            //back
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 6, 3);
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 9, 3);

            //right
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 12, 3);
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 15, 3);

            //left
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 18, 3);
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 21, 3);

            //top
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 24, 3);
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 27, 3);

            //bottom
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 30, 3);
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 33, 3);
            
            //GPU.BindBuffer(vertexBufferID2);
            //Gl.glEnableVertexAttribArray(0);
            //Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), IntPtr.Zero);
            //Gl.glEnableVertexAttribArray(1);
            //Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), (IntPtr)(3 * sizeof(float)));
            //Gl.glEnableVertexAttribArray(2);
            //Gl.glVertexAttribPointer(2, 2, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), (IntPtr)(6 * sizeof(float)));

            //tex2.Bind();
            //Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, 6);
            //
            //GPU.BindBuffer(vertexBufferID);
            //Gl.glEnableVertexAttribArray(0);
            //Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), IntPtr.Zero);
            //Gl.glEnableVertexAttribArray(1);
            //Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), (IntPtr)(3 * sizeof(float)));
            //Gl.glEnableVertexAttribArray(2);
            //Gl.glVertexAttribPointer(2, 2, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), (IntPtr)(6 * sizeof(float)));

            //tex1.Bind();
            //Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, 3);

            //Gl.glDisableVertexAttribArray(0);
            //Gl.glDisableVertexAttribArray(1);
            //Gl.glDisableVertexAttribArray(2);
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
