using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace BreakOut
{
    public enum ShaderReference
    {
        tileShader,
        blurShader,
        shadowShader,
        shockShader,
        maskShader,
        editorShader,
    }

    class ShaderContainer
    {
        private static Dictionary<ShaderReference, Effect> shaderDictionary = new Dictionary<ShaderReference, Effect>();
                
        public static void AddShader(ShaderReference shaderRef, Effect effect)
        {
            shaderDictionary.Add(shaderRef, effect);
        }

        public static Effect GetShader(ShaderReference shaderRef)
        {            
            shaderDictionary.TryGetValue(shaderRef, out Effect effect);
            return effect;
        }
    }
}