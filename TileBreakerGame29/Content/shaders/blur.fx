#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

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


static const float xPixel = 1.0f / 1080.0f;
static const float yPixel = 1.0f / 1920.0f;
//static const float weight[] = { 0.000003f,	0.000229f,	0.005977f,	0.060598f,	0.24173f,	0.382925f,	0.24173f,	0.060598f,	0.005977f,	0.000229f,	0.000003f};

static const float weight[] = { 0.018061f,	0.0235f,	0.029742f,	0.036614f,	0.043841f,	0.051059f,	0.057841f,	0.063733f,	0.068305f, 0.071204f,	0.072198f,
							  0.071204f,	0.068305f,	0.063733f,	0.057841f,	0.051059f,	0.043841f,	0.036614f,	0.029742f,	0.0235f,	0.018061f };

static const float pp = 0.0062f;


float4 HorizontalPS(VertexShaderOutput input) : COLOR
{
	
	//float4 p = tex2D(SpriteTextureSampler, input.TextureCoordinates);
	float4 result=float4(0,0,0,0);
	
	for (int i = 0; i < 21; i++)
	{
		result += weight[i] * (tex2D(SpriteTextureSampler, float2(input.TextureCoordinates.x + xPixel * (i - 10), input.TextureCoordinates.y + yPixel * 0)));
	}
	
	return result * input.Color;
}

float4 VerticalPS(VertexShaderOutput input) : COLOR
{

	//float4 p = tex2D(SpriteTextureSampler, input.TextureCoordinates);
	float4 result = float4(0,0,0,0);
	for (int i = 0; i < 21; i++)
	{
		result += weight[i] * (tex2D(SpriteTextureSampler, float2(input.TextureCoordinates.x + xPixel * 0, input.TextureCoordinates.y + yPixel * (i - 10))));
	}
	return result * input.Color;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL HorizontalPS();
	}
	pass P1
	{
		PixelShader = compile PS_SHADERMODEL VerticalPS();
	}
	
	
};