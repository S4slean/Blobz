// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "S_Link"
{
	Properties
	{
		_bendRatio("bendRatio", Range( 0 , 1)) = 0
		_DeformationValue("DeformationValue", Range( 0.5 , 10)) = 1
		_Texture0("Texture 0", 2D) = "white" {}
		_T_OpaLink("T_OpaLink", 2D) = "white" {}
		_T_OpaLinkCercle("T_OpaLinkCercle", 2D) = "white" {}
		_InnerPlayer("InnerPlayer", Color) = (1,0.7926845,0.4292453,0)
		_OuterColor("OuterColor", Color) = (0.5660378,0.5660378,0.5660378,0)
		_rangeDivision("rangeDivision", Range( 0 , 100)) = 10
		_TranstionRatio("TranstionRatio", Range( 0 , 1)) = 0
		_blobNumber("_blobNumber", Range( 1 , 10)) = 2
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Texture0;
		uniform float _bendRatio;
		uniform float _DeformationValue;
		uniform sampler2D _T_OpaLinkCercle;
		uniform float _rangeDivision;
		uniform float _TranstionRatio;
		uniform float _blobNumber;
		uniform sampler2D _T_OpaLink;
		uniform float4 _T_OpaLink_ST;
		uniform float4 _InnerPlayer;
		uniform float4 _OuterColor;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float2 uv_TexCoord15 = v.texcoord.xy + float2( -0.01,0 );
			float smoothstepResult43 = smoothstep( 0.46 , 1.2 , uv_TexCoord15.x);
			float smoothstepResult46 = smoothstep( 0.46 , 1.2 , ( 1.0 - uv_TexCoord15.x ));
			float temp_output_50_0 = ( 1.0 - ( smoothstepResult43 + smoothstepResult46 ) );
			float2 panner75 = ( 1.0 * _Time.y * float2( -1,0.5 ) + v.texcoord.xy);
			float2 panner74 = ( 1.0 * _Time.y * float2( 0.5,-0.5 ) + v.texcoord.xy);
			float4 break77 = ( tex2Dlod( _Texture0, float4( panner75, 0, 0.0) ) * tex2Dlod( _Texture0, float4( panner74, 0, 0.0) ) );
			float4 appendResult9 = (float4(break77.r , break77.g , 0.0 , 0.0));
			float4 temp_cast_0 = (-0.6).xxxx;
			float4 temp_cast_1 = (2.0).xxxx;
			float4 temp_cast_2 = (-1.0).xxxx;
			float4 temp_cast_3 = (2.0).xxxx;
			v.vertex.xyz += ( pow( temp_output_50_0 , 2.0 ) * ( (temp_cast_2 + (appendResult9 - temp_cast_0) * (temp_cast_3 - temp_cast_2) / (temp_cast_1 - temp_cast_0)) * _bendRatio * _DeformationValue ) ).xyz;
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float4 appendResult96 = (float4(_rangeDivision , 1.0 , 0.0 , 0.0));
			float temp_output_108_0 = ( 1.0 / _rangeDivision );
			float temp_output_130_0 = ( temp_output_108_0 * _blobNumber );
			float4 appendResult101 = (float4(( 1.0 - ( _rangeDivision * ( ( ( _TranstionRatio * temp_output_108_0 ) + _TranstionRatio + ( _TranstionRatio * temp_output_130_0 ) ) - temp_output_130_0 ) ) ) , 0.0 , 0.0 , 0.0));
			float2 uv_TexCoord94 = i.uv_texcoord * (appendResult96).xy + (appendResult101).xy;
			float2 uv_TexCoord15 = i.uv_texcoord + float2( -0.01,0 );
			float smoothstepResult43 = smoothstep( 0.46 , 1.2 , uv_TexCoord15.x);
			float smoothstepResult46 = smoothstep( 0.46 , 1.2 , ( 1.0 - uv_TexCoord15.x ));
			float temp_output_50_0 = ( 1.0 - ( smoothstepResult43 + smoothstepResult46 ) );
			float2 uv_T_OpaLink = i.uv_texcoord * _T_OpaLink_ST.xy + _T_OpaLink_ST.zw;
			float temp_output_81_0 = ( ( tex2D( _T_OpaLinkCercle, uv_TexCoord94 ).r * ( temp_output_50_0 * 1.5 ) * ( 1.0 - ( step( uv_TexCoord94.x , 0.0 ) + step( _blobNumber , uv_TexCoord94.x ) ) ) ) + tex2D( _T_OpaLink, uv_T_OpaLink ).b );
			float temp_output_85_0 = step( 0.68 , temp_output_81_0 );
			float clampResult116 = clamp( temp_output_81_0 , 0.0 , 1.0 );
			float temp_output_88_0 = step( 0.52 , clampResult116 );
			o.Emission = ( ( temp_output_85_0 * _InnerPlayer ) + ( ( temp_output_88_0 - temp_output_85_0 ) * _OuterColor ) ).rgb;
			o.Alpha = temp_output_88_0;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Unlit alpha:fade keepalpha fullforwardshadows vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15900
-1920;0;1920;1029;1898.127;-488.8233;1.747629;True;True
Node;AmplifyShaderEditor.RangedFloatNode;103;-1182.084,1121.572;Float;False;Constant;_Depart;Depart;8;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;98;-1179.518,934.3409;Float;False;Property;_rangeDivision;rangeDivision;7;0;Create;False;0;0;False;0;10;4;0;100;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;108;-965.2829,1149.372;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;131;-1007.166,1675.643;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;126;-954.1466,1652.618;Float;False;Property;_blobNumber;_blobNumber;9;0;Create;True;0;0;False;0;2;1;1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;132;-664.2375,1810.021;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;99;-939.7296,1446.543;Float;False;Property;_TranstionRatio;TranstionRatio;8;0;Create;True;0;0;False;0;0;0.08621052;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;130;-630.5342,1649.021;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;109;-773.2836,1146.072;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;133;-712.8302,1537.799;Float;False;2;2;0;FLOAT;1.5;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;110;-610.9835,1427.672;Float;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;134;-490.8491,1429.02;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;111;-993.0837,1763.572;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;112;-646.0837,1062.572;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;127;-343.2422,1803.639;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;105;-353.0837,1405.572;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;70;-896.1846,345.1873;Float;False;1217.487;545.6828;Deformation Restriction ;9;15;68;67;45;66;46;43;47;50;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;104;-259.4739,1404.685;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;101;-117.5779,1404.432;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;96;-457.9322,933.5309;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;15;-847.2654,734.8702;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;-0.01,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;67;-532.2296,551.5024;Float;False;Constant;_Float1;Float 1;4;0;Create;True;0;0;False;0;0.46;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;68;-606.8322,504.1041;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;97;-277.1322,926.5311;Float;True;True;True;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;66;-538.7938,622.2163;Float;False;Constant;_Float0;Float 0;4;0;Create;True;0;0;False;0;1.2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;102;19.99603,1394.897;Float;True;True;True;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;45;-561.5944,758.3834;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;129;-622.9301,2077.218;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;128;324.2698,2006.817;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;94;100.1155,937.6859;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;43;-362.0539,395.1873;Float;True;3;0;FLOAT;0;False;1;FLOAT;0.6;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;46;-356.9364,620.3696;Float;True;3;0;FLOAT;0;False;1;FLOAT;0.6;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;122;452.176,1210.931;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;123;449.5761,1425.431;Float;True;2;0;FLOAT;2;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;71;-1561.965,-362.3968;Float;False;1854.281;652.6954;Base deformation ;15;54;6;10;11;12;13;9;8;57;51;72;74;75;76;77;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;47;-93.88269,476.0481;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;124;684.9762,1230.431;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;50;123.3024,483.99;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;51;-1547.764,-129.9063;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;114;423.1075,732.7221;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;1.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;80;361.9977,959.6365;Float;True;Property;_T_OpaLinkCercle;T_OpaLinkCercle;4;0;Create;True;0;0;False;0;0df572ab14ba54c45bcf969a94c77a72;0df572ab14ba54c45bcf969a94c77a72;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;57;-1557.928,-328.4839;Float;True;Property;_Texture0;Texture 0;2;0;Create;True;0;0;False;0;cd460ee4ac5c1e746b7a734cc7cc64dd;b2d4cc75b5d9b6541b0ba621a261eccc;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.PannerNode;74;-1390.631,73.5387;Float;False;3;0;FLOAT2;1,1;False;2;FLOAT2;0.5,-0.5;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;75;-1275.568,-117.4613;Float;False;3;0;FLOAT2;1,1;False;2;FLOAT2;-1,0.5;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;125;908.5761,1361.732;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;72;-1126.568,31.5387;Float;True;Property;_TextureSample1;Texture Sample 1;3;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;79;682.4958,1512.587;Float;True;Property;_T_OpaLink;T_OpaLink;3;0;Create;True;0;0;False;0;54f8f6fb2374ff1488d002a3d1217446;54f8f6fb2374ff1488d002a3d1217446;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;8;-1235.743,-319.7318;Float;True;Property;_TextureSample0;Texture Sample 0;3;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;113;678.2642,976.808;Float;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;81;908.9977,1024.713;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;76;-701.7046,-142.6312;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BreakToComponentsNode;77;-543.7046,28.36877;Float;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.ClampOpNode;116;1098.785,1281.687;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;88;1280.508,1279.032;Float;True;2;0;FLOAT;0.52;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-266.1396,-108.744;Float;False;Constant;_Float4;Float 4;5;0;Create;True;0;0;False;0;-0.6;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;9;-486.3414,-289.6281;Float;True;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-254.2037,-42.5484;Float;False;Constant;_Float3;Float 3;5;0;Create;True;0;0;False;0;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-251.5475,26.85034;Float;False;Constant;_Float5;Float 5;5;0;Create;True;0;0;False;0;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;85;1247.982,1035.015;Float;True;2;0;FLOAT;0.68;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;10;-4.679114,-310.6969;Float;True;5;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT4;1,0,0,0;False;3;FLOAT4;0,0,0,0;False;4;FLOAT4;1,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;83;1785.204,758.5747;Float;False;Property;_InnerPlayer;InnerPlayer;5;0;Create;True;0;0;False;0;1,0.7926845,0.4292453,0;0.8391602,1,0.8254717,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;84;1560.204,1526.317;Float;False;Property;_OuterColor;OuterColor;6;0;Create;True;0;0;False;0;0.5660378,0.5660378,0.5660378,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;54;-4.679114,41.30311;Float;True;Property;_DeformationValue;DeformationValue;1;0;Create;True;0;0;False;0;1;1.5;0.5;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;89;1594.775,1286.032;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-20.67911,-54.69689;Float;False;Property;_bendRatio;bendRatio;0;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;402.9117,-196.182;Float;True;3;3;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.PowerNode;78;403.6088,452.714;Float;True;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;87;1617.581,1044.258;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;90;1955.775,1401.032;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;614.5868,-54.51737;Float;True;2;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;91;2110.658,1042.483;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;115;1617.015,452.6684;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;2;2405.743,61.4244;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;S_Link;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;108;0;103;0
WireConnection;108;1;98;0
WireConnection;131;0;108;0
WireConnection;132;0;131;0
WireConnection;130;0;132;0
WireConnection;130;1;126;0
WireConnection;109;0;99;0
WireConnection;109;1;108;0
WireConnection;133;0;99;0
WireConnection;133;1;130;0
WireConnection;110;0;109;0
WireConnection;110;1;99;0
WireConnection;110;2;133;0
WireConnection;134;0;110;0
WireConnection;134;1;130;0
WireConnection;111;0;103;0
WireConnection;112;0;98;0
WireConnection;127;0;111;0
WireConnection;105;0;112;0
WireConnection;105;1;134;0
WireConnection;104;0;127;0
WireConnection;104;1;105;0
WireConnection;101;0;104;0
WireConnection;96;0;98;0
WireConnection;68;0;15;1
WireConnection;97;0;96;0
WireConnection;102;0;101;0
WireConnection;45;0;15;1
WireConnection;129;0;126;0
WireConnection;128;0;129;0
WireConnection;94;0;97;0
WireConnection;94;1;102;0
WireConnection;43;0;68;0
WireConnection;43;1;67;0
WireConnection;43;2;66;0
WireConnection;46;0;45;0
WireConnection;46;1;67;0
WireConnection;46;2;66;0
WireConnection;122;0;94;1
WireConnection;123;0;128;0
WireConnection;123;1;94;1
WireConnection;47;0;43;0
WireConnection;47;1;46;0
WireConnection;124;0;122;0
WireConnection;124;1;123;0
WireConnection;50;0;47;0
WireConnection;114;0;50;0
WireConnection;80;1;94;0
WireConnection;74;0;51;0
WireConnection;75;0;51;0
WireConnection;125;0;124;0
WireConnection;72;0;57;0
WireConnection;72;1;74;0
WireConnection;8;0;57;0
WireConnection;8;1;75;0
WireConnection;113;0;80;1
WireConnection;113;1;114;0
WireConnection;113;2;125;0
WireConnection;81;0;113;0
WireConnection;81;1;79;3
WireConnection;76;0;8;0
WireConnection;76;1;72;0
WireConnection;77;0;76;0
WireConnection;116;0;81;0
WireConnection;88;1;116;0
WireConnection;9;0;77;0
WireConnection;9;1;77;1
WireConnection;85;1;81;0
WireConnection;10;0;9;0
WireConnection;10;1;13;0
WireConnection;10;2;11;0
WireConnection;10;3;12;0
WireConnection;10;4;11;0
WireConnection;89;0;88;0
WireConnection;89;1;85;0
WireConnection;17;0;10;0
WireConnection;17;1;6;0
WireConnection;17;2;54;0
WireConnection;78;0;50;0
WireConnection;87;0;85;0
WireConnection;87;1;83;0
WireConnection;90;0;89;0
WireConnection;90;1;84;0
WireConnection;49;0;78;0
WireConnection;49;1;17;0
WireConnection;91;0;87;0
WireConnection;91;1;90;0
WireConnection;115;0;88;0
WireConnection;2;2;91;0
WireConnection;2;9;115;0
WireConnection;2;11;49;0
ASEEND*/
//CHKSM=6B3C6AC5F3C3776ACDDD7149CFE52DC5B1FE3533