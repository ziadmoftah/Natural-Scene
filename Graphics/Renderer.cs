﻿using System;
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
        // skybox attributes
        //----------------
        uint skybox_vertexBufferID;
        Texture skybox_BackTexture;
        Texture skybox_BottomTexture;
        Texture skybox_FrontTexture;
        Texture skybox_LeftTexture;
        Texture skybox_RightTexture;
        Texture skybox_TopTexture;
        //-----------------

        int transID;
        int viewID;
        int projID;
        mat4 scaleMat;
        mat4 ProjectionMatrix;
        mat4 ViewMatrix;
        public Camera cam;
        public void Initialize()
        {
            string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            // skybox
            //-----------------
            sh = new Shader(projectPath + "\\Shaders\\SimpleVertexShader.vertexshader", projectPath + "\\Shaders\\SimpleFragmentShader.fragmentshader");
            skybox_FrontTexture = new Texture(projectPath + "\\Textures\\front.jpg", 3);
            skybox_BackTexture = new Texture(projectPath + "\\Textures\\back.jpg", 4);
            skybox_RightTexture = new Texture(projectPath + "\\Textures\\right.jpg", 5);
            skybox_LeftTexture = new Texture(projectPath + "\\Textures\\left.jpg", 6);
            skybox_TopTexture = new Texture(projectPath + "\\Textures\\top.jpg", 7);
            skybox_BottomTexture = new Texture(projectPath + "\\Textures\\bottom.jpg", 8);
            Gl.glClearColor(0, 0, 0.4f, 1);
               float[] skybox_vertices =
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
                1,1,1,     1,1,1,       0,1,
                1,-1,1,    1,1,1,       0,0,
                1,-1,-1,   1,1,1,       1,0,

                1,1,1,     1,1,1,       0,1,
                1,1,-1,    1,1,1,       1,1,
                1,-1,-1,   1,1,1,       1,0,

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
                1,1,1,      0,0,0,       0,1,
                -1,1,1,     0,0,0,       0,0,
                -1,1,-1,    0,0,0,       1,0,

                1,1,1,      0,0,0,       0,1,
                1,1,-1,     0,0,0,       1,1,
                -1,1,-1,    0,0,0,       1,0,

                //bottom
                -1,-1,-1,   0,0,0,       0,1,
                -1,-1,1,    0,0,0,       0,0,
                1,-1,1,     0,0,0,       1,0,

               -1,-1,-1,    0,0,0,       0,1,
                1,-1,-1,   0,0,0,       1,1,
                1,-1,1,    0,0,0,       1,0
            };
            skybox_vertexBufferID = GPU.GenerateBuffer(skybox_vertices);
            //----------------------
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
            // draw skybox
            //-----------------------
            GPU.BindBuffer(skybox_vertexBufferID);
            Gl.glEnableVertexAttribArray(0);
            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), IntPtr.Zero);
            Gl.glEnableVertexAttribArray(1);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), (IntPtr)(3 * sizeof(float)));
            Gl.glEnableVertexAttribArray(2);
            Gl.glVertexAttribPointer(2, 2, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), (IntPtr)(6 * sizeof(float)));

            //front
            skybox_FrontTexture.Bind();
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, 3);
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 3, 3);

            //back
            skybox_BackTexture.Bind();
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 6, 3);
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 9, 3);

            //right
            skybox_RightTexture.Bind();
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 12, 3);
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 15, 3);

            //left
            skybox_LeftTexture.Bind();
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 18, 3);
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 21, 3);

            //top
            skybox_TopTexture.Bind();
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 24, 3);
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 27, 3);

            //bottom
            skybox_BottomTexture.Bind();
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 30, 3);
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 33, 3);

            Gl.glDisableVertexAttribArray(0);
            Gl.glDisableVertexAttribArray(1);
            Gl.glDisableVertexAttribArray(2);
            //-----------------------------
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
