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
		_rangeDivision("rangeDivision", Float) = 2
		_TranstionRatio("TranstionRatio", Range( 0 , 1)) = 0.3919734
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
			float4 appendResult101 = (float4(( 1.0 - ( _rangeDivision * ( ( _TranstionRatio * ( 1.0 / _rangeDivision ) ) + _TranstionRatio ) ) ) , 0.0 , 0.0 , 0.0));
			float2 uv_TexCoord94 = i.uv_texcoord * (appendResult96).xy + (appendResult101).xy;
			float2 uv_TexCoord15 = i.uv_texcoord + float2( -0.01,0 );
			float smoothstepResult43 = smoothstep( 0.46 , 1.2 , uv_TexCoord15.x);
			float smoothstepResult46 = smoothstep( 0.46 , 1.2 , ( 1.0 - uv_TexCoord15.x ));
			float temp_output_50_0 = ( 1.0 - ( smoothstepResult43 + smoothstepResult46 ) );
			float2 uv_T_OpaLink = i.uv_texcoord * _T_OpaLink_ST.xy + _T_OpaLink_ST.zw;
			float temp_output_81_0 = ( ( tex2D( _T_OpaLinkCercle, uv_TexCoord94 ).r * ( temp_output_50_0 * 1.5 ) ) + tex2D( _T_OpaLink, uv_T_OpaLink ).b );
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
-1920;0;1920;1019;175.8389;255.3118;1.621661;True;False
Node;AmplifyShaderEditor.RangedFloatNode;103;-964.0837,1014.572;Float;False;Constant;_Depart;Depart;8;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;98;-967.5177,912.3409;Float;False;Property;_rangeDivision;rangeDivision;7;0;Create;True;0;0;False;0;2;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;108;-864.0837,1129.572;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;99;-1006.281,1321.613;Float;False;Property;_TranstionRatio;TranstionRatio;8;0;Create;True;0;0;False;0;0.3919734;0.08621052;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;109;-706.0837,1134.572;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;112;-646.0837,1062.572;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;110;-642.0837,1304.572;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;70;-896.1846,345.1873;Float;False;1217.487;545.6828;Deformation Restriction ;9;15;68;67;45;66;46;43;47;50;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;15;-847.2654,734.8702;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;-0.01,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;111;-647.0837,1062.572;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;105;-556.0837,1125.572;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;66;-538.7938,622.2163;Float;False;Constant;_Float0;Float 0;4;0;Create;True;0;0;False;0;1.2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;67;-532.2296,551.5024;Float;False;Constant;_Float1;Float 1;4;0;Create;True;0;0;False;0;0.46;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;104;-419.0837,1192.572;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;68;-606.8322,504.1041;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;45;-561.5944,758.3834;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;101;-193.1826,1213.32;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;96;-600.9321,925.731;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SmoothstepOpNode;46;-356.9364,620.3696;Float;True;3;0;FLOAT;0;False;1;FLOAT;0.6;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;43;-362.0539,395.1873;Float;True;3;0;FLOAT;0;False;1;FLOAT;0.6;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;71;-1561.965,-362.3968;Float;False;1854.281;652.6954;Base deformation ;15;54;6;10;11;12;13;9;8;57;51;72;74;75;76;77;;1,1,1,1;0;0
Node;AmplifyShaderEditor.ComponentMaskNode;97;-414.9322,944.731;Float;False;True;True;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;47;-93.88269,476.0481;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;102;-39.18259,1220.32;Float;False;True;True;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;51;-1547.764,-129.9063;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;50;123.3024,483.99;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;94;-72.2641,995.3853;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0.5,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;80;361.9977,959.6365;Float;True;Property;_T_OpaLinkCercle;T_OpaLinkCercle;4;0;Create;True;0;0;False;0;0df572ab14ba54c45bcf969a94c77a72;0df572ab14ba54c45bcf969a94c77a72;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;114;423.1075,732.7221;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;1.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;75;-1275.568,-117.4613;Float;False;3;0;FLOAT2;1,1;False;2;FLOAT2;-1,0.5;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;57;-1557.928,-328.4839;Float;True;Property;_Texture0;Texture 0;2;0;Create;True;0;0;False;0;cd460ee4ac5c1e746b7a734cc7cc64dd;b2d4cc75b5d9b6541b0ba621a261eccc;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.PannerNode;74;-1390.631,73.5387;Float;False;3;0;FLOAT2;1,1;False;2;FLOAT2;0.5,-0.5;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;8;-1235.743,-319.7318;Float;True;Property;_TextureSample0;Texture Sample 0;3;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;113;678.2642,976.808;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;79;524.7849,1232.935;Float;True;Property;_T_OpaLink;T_OpaLink;3;0;Create;True;0;0;False;0;54f8f6fb2374ff1488d002a3d1217446;54f8f6fb2374ff1488d002a3d1217446;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;72;-1126.568,31.5387;Float;True;Property;_TextureSample1;Texture Sample 1;3;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;76;-701.7046,-142.6312;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;81;908.9977,1024.713;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;77;-543.7046,28.36877;Float;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.ClampOpNode;116;1098.785,1281.687;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;88;1280.508,1279.032;Float;True;2;0;FLOAT;0.52;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;9;-486.3414,-289.6281;Float;True;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-266.1396,-108.744;Float;False;Constant;_Float4;Float 4;5;0;Create;True;0;0;False;0;-0.6;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-254.2037,-42.5484;Float;False;Constant;_Float3;Float 3;5;0;Create;True;0;0;False;0;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-251.5475,26.85034;Float;False;Constant;_Float5;Float 5;5;0;Create;True;0;0;False;0;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;85;1247.982,1035.015;Float;True;2;0;FLOAT;0.68;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-20.67911,-54.69689;Float;False;Property;_bendRatio;bendRatio;0;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;10;-4.679114,-310.6969;Float;True;5;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT4;1,0,0,0;False;3;FLOAT4;0,0,0,0;False;4;FLOAT4;1,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;89;1594.775,1286.032;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;83;1785.204,758.5747;Float;False;Property;_InnerPlayer;InnerPlayer;5;0;Create;True;0;0;False;0;1,0.7926845,0.4292453,0;0.8391602,1,0.8254717,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;54;-4.679114,41.30311;Float;True;Property;_DeformationValue;DeformationValue;1;0;Create;True;0;0;False;0;1;1.5;0.5;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;84;1560.204,1526.317;Float;False;Property;_OuterColor;OuterColor;6;0;Create;True;0;0;False;0;0.5660378,0.5660378,0.5660378,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;87;1617.581,1044.258;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;402.9117,-196.182;Float;True;3;3;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;90;1955.775,1401.032;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;78;403.6088,452.714;Float;True;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;614.5868,-54.51737;Float;True;2;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.WireNode;115;1617.015,452.6684;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;91;2110.658,1042.483;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;2;2405.743,61.4244;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;S_Link;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;108;0;103;0
WireConnection;108;1;98;0
WireConnection;109;0;99;0
WireConnection;109;1;108;0
WireConnection;112;0;98;0
WireConnection;110;0;109;0
WireConnection;110;1;99;0
WireConnection;111;0;103;0
WireConnection;105;0;112;0
WireConnection;105;1;110;0
WireConnection;104;0;111;0
WireConnection;104;1;105;0
WireConnection;68;0;15;1
WireConnection;45;0;15;1
WireConnection;101;0;104;0
WireConnection;96;0;98;0
WireConnection;46;0;45;0
WireConnection;46;1;67;0
WireConnection;46;2;66;0
WireConnection;43;0;68;0
WireConnection;43;1;67;0
WireConnection;43;2;66;0
WireConnection;97;0;96;0
WireConnection;47;0;43;0
WireConnection;47;1;46;0
WireConnection;102;0;101;0
WireConnection;50;0;47;0
WireConnection;94;0;97;0
WireConnection;94;1;102;0
WireConnection;80;1;94;0
WireConnection;114;0;50;0
WireConnection;75;0;51;0
WireConnection;74;0;51;0
WireConnection;8;0;57;0
WireConnection;8;1;75;0
WireConnection;113;0;80;1
WireConnection;113;1;114;0
WireConnection;72;0;57;0
WireConnection;72;1;74;0
WireConnection;76;0;8;0
WireConnection;76;1;72;0
WireConnection;81;0;113;0
WireConnection;81;1;79;3
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
WireConnection;87;0;85;0
WireConnection;87;1;83;0
WireConnection;17;0;10;0
WireConnection;17;1;6;0
WireConnection;17;2;54;0
WireConnection;90;0;89;0
WireConnection;90;1;84;0
WireConnection;78;0;50;0
WireConnection;49;0;78;0
WireConnection;49;1;17;0
WireConnection;115;0;88;0
WireConnection;91;0;87;0
WireConnection;91;1;90;0
WireConnection;2;2;91;0
WireConnection;2;9;115;0
WireConnection;2;11;49;0
ASEEND*/
//CHKSM=2ED0A90A610B62F722B264021C6731C972054A6B