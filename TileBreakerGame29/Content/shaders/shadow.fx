#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

matrix WorldViewProjection;
float2 LightPosition;

struct VertexShaderInput
{
	float4 Position : POSITION0;
	//float4 Color : COLOR0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

	output.Position = mul(input.Position, WorldViewProjection);
	//float3 p= mul(float3(LightPosition,0), WorldViewProjection);
	//float d = distance(output.Position,p);
	//output.Color = float4(d*0.3,0,0,0);

	return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
	return float4(0.0, 0.0, 0.0, 1.0);
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
				
	}
};