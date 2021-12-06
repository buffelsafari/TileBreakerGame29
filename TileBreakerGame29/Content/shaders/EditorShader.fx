#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

// Viewport size needed for VS
float2 ViewportSize;

// Vertex color
float4 TopLeftColor;
float4 TopRightColor;
float4 BottomLeftColor;
float4 BottomRightColor;
float2 CenterTexCoord;


Texture2D SpriteTexture;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

void SpriteVertexShader(inout float4 color : COLOR0, inout float2 texCoord : TEXCOORD0, inout float4 position : POSITION0)
{
    // Half pixel offset for correct texel centering.
    position.xy -= 0.5;

    // Viewport adjustment.
    position.xy = position.xy / ViewportSize;
    position.xy *= float2(2, -2);
    position.xy -= float2(1, -1);

    // Determine which corner we're dealing with and set the corresponding color
    if (texCoord.x < CenterTexCoord.x)
    {
        if (texCoord.y < CenterTexCoord.y)
        {
            color = TopLeftColor;
        }
        else
        {
            color = BottomLeftColor;
        }
    }
    else
    {
        if (texCoord.y < CenterTexCoord.y)
        {
            color = TopRightColor;
        }
        else
        {
            color = BottomRightColor;
        }
    }
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
	return tex2D(SpriteTextureSampler,input.TextureCoordinates) * input.Color;
}

technique SpriteDrawing
{
	pass P0
	{
        VertexShader = compile vs_2_0 SpriteVertexShader();
		//PixelShader = compile PS_SHADERMODEL MainPS();
	}
};