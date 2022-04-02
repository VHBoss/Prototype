using UnityEngine;
using System.Collections;

namespace Wacki.IndentSurface
{

    public class IndentDraw : MonoBehaviour
    {
        public Texture2D texture;      
        public Texture2D stampTexture;  
        public RenderTexture tempTestRenderTexture;
        public int rtWidth = 512;
        public int rtHeight = 512;

        private RenderTexture targetTexture;
        private RenderTexture auxTexture;

        public Material mat;

        // mouse debug draw
        private Vector3 _prevMousePosition;
        private Vector4 tempVec;
        private Camera m_Camera;
        // setup rect for our indent texture stamp to draw into
        private Rect screenRect = new Rect();
        private Plane m_Plane = new Plane(Vector3.up, Vector3.zero);
        private readonly int SourceTexCoords = Shader.PropertyToID("_SourceTexCoords");

        void Awake()
        {
            m_Camera = Camera.main;

            targetTexture = new RenderTexture(rtWidth, rtHeight, 32);

            // temporarily use a given render texture to be able to see how it looks
            targetTexture = tempTestRenderTexture;
            auxTexture = new RenderTexture(rtWidth, rtHeight, 32);

            GetComponent<Renderer>().material.SetTexture("_Indentmap", targetTexture);
            Graphics.Blit(texture, targetTexture);

            mat.SetTexture("_MainTex", stampTexture);
            mat.SetTexture("_SurfaceTex", auxTexture);
        }
        
        // add an indentation at a raycast hit position
        public void IndentAt(RaycastHit hit)
        {
            if (hit.collider.gameObject != this.gameObject)
                return;
            
            float x = hit.textureCoord.x;
            float y = hit.textureCoord.y;

            DrawAt(x * targetTexture.width, y * targetTexture.height, 1.0f);
        }

        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                if (Physics.Raycast(m_Camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
                {
                    if (hit.collider.gameObject != gameObject)
                    {
                        return;
                    }

                    if ((hit.point - _prevMousePosition).sqrMagnitude > 0.001f)
                    {
                        _prevMousePosition = hit.point;
                        IndentAt(hit);
                    }
                }
            }
        }

        /// <summary>
        /// todo:   it would probably be a bit more straight forward if we didn't use Graphics.DrawTexture
        ///         and instead handle everything ourselves. This way we could directly provide multiple 
        ///         texture coordinates to each vertex.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="alpha"></param>
        void DrawAt(float x, float y, float alpha)
        {
            Graphics.Blit(targetTexture, auxTexture);

            // activate our render texture
            RenderTexture.active = targetTexture; 

            GL.PushMatrix();
            GL.LoadPixelMatrix(0, targetTexture.width, targetTexture.height, 0);

            x = Mathf.Round(x);
            y = Mathf.Round(y);

            // put the center of the stamp at the actual draw position
            screenRect.x = x - stampTexture.width * 0.5f;
            screenRect.y = (targetTexture.height - y) - stampTexture.height * 0.5f;
            screenRect.width = stampTexture.width;
            screenRect.height = stampTexture.height;

            tempVec.x = screenRect.x / ((float)targetTexture.width);
            tempVec.y = 1 - (screenRect.y / (float)targetTexture.height);
            tempVec.z = screenRect.width / targetTexture.width;
            tempVec.w = screenRect.height / targetTexture.height;
            tempVec.y -= tempVec.w;

            mat.SetVector(SourceTexCoords, tempVec);

            // Draw the texture
            Graphics.DrawTexture(screenRect, stampTexture, mat);

            GL.PopMatrix();
            RenderTexture.active = null;
        }
    }

}