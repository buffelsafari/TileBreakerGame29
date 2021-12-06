using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace BreakOut
{
    class TextureContainer
    {
        public enum TextureReference
        {
            spriteSheetDiffuse,
            spriteSheetNormal,
            enviroment,
            floor,
            renderTarget,
            refractionMap,            
            lightball,
            editorSprites,
        }
        
        private static Dictionary<TextureReference, Texture> textureDictionary=new Dictionary<TextureReference, Texture>();
                
        public static void AddTexture(TextureReference texRef, Texture texture)
        {
            textureDictionary.Add(texRef, texture);
        }

        public static Texture GetTexture(TextureReference texRef)
        {            
            textureDictionary.TryGetValue(texRef, out Texture texture);
            return texture;
        }
    }
}