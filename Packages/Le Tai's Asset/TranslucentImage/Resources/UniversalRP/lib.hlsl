#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

struct minimalVertexInput
{
    half4 position  : POSITION;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct minimalVertexOutput
{
    half4 position  : POSITION;
    half2 texcoord : TEXCOORD0;
    UNITY_VERTEX_OUTPUT_STEREO
};

#if defined(UNITY_SINGLE_PASS_STEREO)
float4 UnityStereoAdjustedTexelSize(float4 texelSize)
{
    texelSize.x = texelSize.x * 2.0; // texelSize.x = 1/w. For a double-wide texture, the true resolution is given by 2/w.
    texelSize.z = texelSize.z * 0.5; // texelSize.z = w. For a double-wide texture, the true size of the eye texture is given by w/2.
    return texelSize;
}
#else
float4 UnityStereoAdjustedTexelSize(float4 texelSize)
{
    return texelSize;
}
#endif

half2 VertexToUV(half2 vertex)
{
    half2 texcoord = (vertex + 1.0) * 0.5; // triangle vert to uv
    #if UNITY_UV_STARTS_AT_TOP
    texcoord = texcoord * half2(1.0, -1.0) + half2(0.0, 1.0);
    #endif
    return texcoord;
}

SAMPLER(sampler_LinearClamp);
#define SAMPLE_SCREEN_TEX(tex, uv) SAMPLE_TEXTURE2D_X(tex, sampler_LinearClamp, UnityStereoTransformScreenSpaceTex(uv))
