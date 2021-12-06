﻿#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;
Texture2D RefractionMap;



sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

sampler2D SpriteRefractionSampler = sampler_state
{
	Texture = <RefractionMap>;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float4 normalColor = tex2D(SpriteRefractionSampler,input.TextureCoordinates);

	float2 texCoord = input.TextureCoordinates;
	
	texCoord.x -= (normalColor.r * 2 - 1)*0.02;

	texCoord.y -= (normalColor.g * 2 - 1)*0.02;

	return tex2D(SpriteTextureSampler,texCoord) * input.Color;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};