#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif



Texture2D diffuseTexture;
Texture2D NormalMap;


Texture SkyBoxTexture;



cbuffer ConstantBuffer : register(b0)
{
	matrix WorldViewProjection;
	float4x4 World;
	//float3 CameraPosition;
	
}

float3 lightPosition[8];
float4 lightColor[8];

sampler2D diffuseTextureSampler = sampler_state
{
	Texture = <diffuseTexture>;	
	
};

sampler2D NormalMapSampler = sampler_state
{
	Texture = <NormalMap>;
	
};




samplerCUBE SkyBoxSampler = sampler_state
{
	texture = <SkyBoxTexture>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU = Mirror;
	AddressV = Mirror;
};



struct VertexShaderInput
{
	float4 Position : POSITION0;
	float4 Color : COLOR0;
	float2 TextureCoordinates: TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
	float2 Posi: TEXCOORD1;
	
	
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

	output.Position = mul(input.Position, WorldViewProjection);
	output.Color = input.Color;
	output.TextureCoordinates = input.TextureCoordinates;
	output.Posi = float2(input.Position.x, input.Position.y);
	

	//float4 VertexPosition = mul(input.Position, World);
	
	return output;
}


float4 ThreeLightPS(VertexShaderOutput input) : COLOR
{
	float4 diffuse = tex2D(diffuseTextureSampler, input.TextureCoordinates);
	float3 nc = tex2D(NormalMapSampler, input.TextureCoordinates).xyz * 2 - 1; // texture normal
	
//	float3 camDir = normalize(float3(input.Posi, 0) - CameraPosition);

	float4 bigpp = float4(0, 0, 0, 1); // normal mapping light
	float4 bigspec = float4(0,0,0,1);  // specular	

	
	float3 V = normalize(float3(input.Posi, 0) - float3(540, 960,1000));  // "camera position" for lights

	[unroll]
	for (int counter = 0; counter < 8; counter++)
	{
		float3 lightVect = float3(input.Posi, 0) - lightPosition[counter];
		float3 lightPositionDir = normalize(lightVect);
		float4 slamp = float4(lightColor[counter])* (100.0f / length(lightVect));
		
		float dd = dot(-lightPositionDir, nc);
		bigpp += (max(float4(dd, dd, dd, 1.0f), 0.0f)*slamp);
		
		float3 R = reflect(-lightPositionDir, nc);
		bigspec += pow(saturate(dot(R, V)), 2.0f)*slamp;
		
	}

	float3 refl = reflect(-V, nc);
	float4 sky = float4(texCUBE(SkyBoxSampler, refl));

	
	return (sky + bigpp + bigspec)*diffuse*input.Color+ input.Color*float4(diffuse.xyz, 0)*0.2f;

}




technique LightShader//BasicColorDrawing
{
	

	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL ThreeLightPS();
	}

	
};