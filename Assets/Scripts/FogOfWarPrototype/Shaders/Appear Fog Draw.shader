Shader "Toon/Lit Dissolve Appear Fog Draw" {
	Properties{
		_Color("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Ramp("Toon Ramp (RGB)", 2D) = "gray" {}
		_NoiseTex("Dissolve Noise", 2D) = "white"{}
		_NScale("Noise Scale", Range(0, 10)) = 1
		_DisAmount("Noise Texture Opacity", Range(0.0001, 1)) = 0
		_Radius("Radius", Range(0, 20)) = 0
		_DisLineWidth("Line Width", Range(0, 2)) = 0
		_DisLineColor("Line Tint", Color) = (1,1,1,1)
		[Toggle(ALPHA)] _ALPHA("No Shadows on Transparent", Float) = 0
		_PaintMap("RevealMap", 2D) = "white" {} // texture to paint on

	}

		SubShader{
			Tags { "RenderType" = "Transparent" }
			LOD 200
			Cull Off


		Blend SrcAlpha OneMinusSrcAlpha // transparency
CGPROGRAM



#pragma shader_feature LIGHTMAP
#pragma surface surf ToonRamp keepalpha addshadow// transparency

sampler2D _Ramp;

	// custom lighting function that uses a texture ramp based
	// on angle between light direction and normal
	#pragma lighting ToonRamp exclude_path:prepass
	inline half4 LightingToonRamp(SurfaceOutput s, half3 lightDir, half atten)
	{
		#ifndef USING_DIRECTIONAL_LIGHT
		lightDir = normalize(lightDir);
		#endif

		half d = dot(s.Normal, lightDir) * 0.5 + 0.5;
		half3 ramp = tex2D(_Ramp, float2(d,d)).rgb;

		half4 c;
		c.rgb = s.Albedo * _LightColor0.rgb * ramp * (atten * 2);
		//c.a = 0; we don't want the alpha
		c.a = s.Alpha; // use the alpha of the surface output
		return c;
	}
	uniform float3 _Position;
	uniform float _ArrayLength;
	//uniform float3 _Positions[100];
	uniform float3 _FogPositions[1000];
	uniform float _FogRadius[1000];
	float3 _Distances[100];
	float _Radius;

	sampler2D _MainTex;
	float4 _Color;
	sampler2D _NoiseTex;//
	float _DisAmount, _NScale;//
	float _DisLineWidth;//
	float4 _DisLineColor;//
	float4 _DisLineColorEx;//

	 sampler2D _PaintMap;

	struct Input {
		float2 uv_MainTex : TEXCOORD0;
		float2 uv2_NoiseTex : TEXCOORD1;
		float3 worldPos;// built in value to use the world space position
		float3 worldNormal;

	};

		void surf(Input IN, inout SurfaceOutput o)
		{
			half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			half4 paint = (tex2D(_PaintMap, IN.uv2_NoiseTex)); // painted on texture
			float4 endresult = (1 - paint.r) * _Color; // reveal main texture where painted black
			// triplanar noise
			float3 blendNormal = saturate(pow(IN.worldNormal * 1.4,4));
			half4 nSide1 = tex2D(_NoiseTex, (IN.worldPos.xy + _Time.x) * _NScale);
			half4 nSide2 = tex2D(_NoiseTex, (IN.worldPos.xz + _Time.x) * _NScale);
			half4 nTop = tex2D(_NoiseTex, (IN.worldPos.yz + _Time.x) * _NScale);

			float3 noisetexture = nSide1;
			noisetexture = lerp(noisetexture, nTop, blendNormal.x);
			noisetexture = lerp(noisetexture, nSide2, blendNormal.y);

			// distance influencer position to world position and sphere radius
			float3 sphereRNoise;
			for (int i = 0; i < _FogPositions.Length; i++) 
			{
			//float3 dis =  distance(_Positions[i], IN.worldPos);
			float3 dis = distance(_FogPositions[i], IN.worldPos);

			//float3 sphereR = 1-  saturate(dis / _Radius);
			float3 sphereR = 1 - saturate(dis / _FogRadius[i]);
			sphereR *= noisetexture;
			sphereRNoise += (sphereR);
			}

		float3 DissolveLineIn = step(sphereRNoise - _DisLineWidth, _DisAmount);

		float3 NoDissolve = float3(1, 1, 1) - DissolveLineIn;
		// c.rgb = (DissolveLineIn * _DisLineColor) + (NoDissolve * c.rgb);


		 c.a = step(sphereRNoise, _DisAmount);

		 o.Albedo = c.rgb * (paint);
		 o.Alpha = clamp(c.a/* - ((1 - paint) * 0.01)*/, 0,1);



		}
	 ENDCG

	}

		Fallback "Diffuse"
}