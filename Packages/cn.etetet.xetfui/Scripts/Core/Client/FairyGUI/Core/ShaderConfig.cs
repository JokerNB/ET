using ET;
using UnityEngine;

namespace FairyGUI
{
    /// <summary>
    /// 
    /// </summary>
    public static class ShaderConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public delegate Shader GetFunction(string name);

        /// <summary>
        /// 
        /// </summary>
        [StaticField]
        public static GetFunction Get = Shader.Find;

        /// <summary>
        /// 
        /// </summary>
        [StaticField]
        public static string imageShader = "FairyGUI/Image";

        /// <summary>
        /// 
        /// </summary>
        [StaticField]
        public static string textShader = "FairyGUI/Text";

        /// <summary>
        /// 
        /// </summary>
        [StaticField]
        public static string bmFontShader = "FairyGUI/BMFont";

        /// <summary>
        /// 
        /// </summary>
        [StaticField]
        public static string TMPFontShader = "FairyGUI/TMP";

        [StaticField]
        public static int ID_ClipBox;
        [StaticField]
        public static int ID_ClipSoftness;
        [StaticField]
        public static int ID_AlphaTex;
        [StaticField]
        public static int ID_StencilComp;
        [StaticField]
        public static int ID_Stencil;
        [StaticField]
        public static int ID_StencilOp;
        [StaticField]
        public static int ID_StencilReadMask;
        [StaticField]
        public static int ID_ColorMask;
        [StaticField]
        public static int ID_ColorMatrix;
        [StaticField]
        public static int ID_ColorOffset;
        [StaticField]
        public static int ID_BlendSrcFactor;
        [StaticField]
        public static int ID_BlendDstFactor;
        [StaticField]
        public static int ID_ColorOption;
        [StaticField]
        public static int ID_Stencil2;

#if UNITY_2019_3_OR_NEWER
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void InitializeOnLoad()
        {
            Get = Shader.Find;
        }
#endif

        static ShaderConfig()
        {
            ID_ClipBox = Shader.PropertyToID("_ClipBox");
            ID_ClipSoftness = Shader.PropertyToID("_ClipSoftness");
            ID_AlphaTex = Shader.PropertyToID("_AlphaTex");
            ID_StencilComp = Shader.PropertyToID("_StencilComp");
            ID_Stencil = Shader.PropertyToID("_Stencil");
            ID_StencilOp = Shader.PropertyToID("_StencilOp");
            ID_StencilReadMask = Shader.PropertyToID("_StencilReadMask");
            ID_ColorMask = Shader.PropertyToID("_ColorMask");
            ID_ColorMatrix = Shader.PropertyToID("_ColorMatrix");
            ID_ColorOffset = Shader.PropertyToID("_ColorOffset");
            ID_BlendSrcFactor = Shader.PropertyToID("_BlendSrcFactor");
            ID_BlendDstFactor = Shader.PropertyToID("_BlendDstFactor");
            ID_ColorOption = Shader.PropertyToID("_ColorOption");

            ID_Stencil2 = Shader.PropertyToID("_StencilRef");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Shader GetShader(string name)
        {
            Shader shader = Get(name);
            if (shader == null)
            {
                Debug.LogWarning("FairyGUI: shader not found: " + name);
                shader = Shader.Find("UI/Default");
            }
            shader.hideFlags = DisplayObject.hideFlags;

            return shader;
        }
    }
}
