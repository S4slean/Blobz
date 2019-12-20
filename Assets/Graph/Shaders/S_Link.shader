// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "S_Link"
{
	Properties
	{
		_bendRatio("bendRatio", Range( 0 , 1)) = 0
		_DeformationValue("DeformationValue", Range( 0.5 , 3)) = 2.55
		_Texture0("Texture 0", 2D) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			half filler;
		};

		uniform sampler2D _Texture0;
		uniform float _bendRatio;
		uniform float _DeformationValue;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float2 uv_TexCoord15 = v.texcoord.xy + float2( -0.04,0 );
			float smoothstepResult43 = smoothstep( 0.5 , 0.95 , uv_TexCoord15.x);
			float smoothstepResult46 = smoothstep( 0.5 , 0.95 , ( 1.0 - uv_TexCoord15.x ));
			float2 uv_TexCoord51 = v.texcoord.xy * float2( 3,3 );
			float4 tex2DNode8 = tex2Dlod( _Texture0, float4( uv_TexCoord51, 0, 0.0) );
			float4 appendResult9 = (float4(tex2DNode8.r , tex2DNode8.g , 0.0 , 0.0));
			float4 temp_cast_0 = (0.0).xxxx;
			float4 temp_cast_1 = (1.0).xxxx;
			float4 temp_cast_2 = (-1.0).xxxx;
			float4 temp_cast_3 = (1.0).xxxx;
			v.vertex.xyz += ( ( 1.0 - ( smoothstepResult43 + smoothstepResult46 ) ) * ( (temp_cast_2 + (appendResult9 - temp_cast_0) * (temp_cast_3 - temp_cast_2) / (temp_cast_1 - temp_cast_0)) * _bendRatio * _DeformationValue ) ).xyz;
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15900
-1920;0;1920;1019;2307.709;934.5236;2.058977;True;False
Node;AmplifyShaderEditor.CommentaryNode;71;-1561.965,-362.3968;Float;False;1854.281;652.6954;Base deformation ;12;54;6;10;49;17;11;12;13;9;8;57;51;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;70;-1103.251,340.3152;Float;False;1217.487;545.6828;Deformation Restriction ;9;15;68;67;45;66;46;43;47;50;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;15;-1053.251,729.998;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;-0.04,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;57;-1557.928,-328.4839;Float;True;Property;_Texture0;Texture 0;2;0;Create;True;0;0;False;0;cd460ee4ac5c1e746b7a734cc7cc64dd;cd460ee4ac5c1e746b7a734cc7cc64dd;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;51;-1547.764,-129.9063;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;3,3;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;8;-1235.743,-319.7318;Float;True;Property;_TextureSample0;Texture Sample 0;3;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;68;-813.8987,499.2319;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;67;-739.296,546.6302;Float;False;Constant;_Float1;Float 1;4;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;45;-768.6608,753.5112;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;66;-745.8602,617.3441;Float;False;Constant;_Float0;Float 0;4;0;Create;True;0;0;False;0;0.95;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-874.4603,-119.0471;Float;False;Constant;_Float4;Float 4;5;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-870.8683,17.54724;Float;False;Constant;_Float5;Float 5;5;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;9;-918.3414,-320.6281;Float;True;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SmoothstepOpNode;46;-564.0029,615.4974;Float;True;3;0;FLOAT;0;False;1;FLOAT;0.6;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-873.5245,-51.85151;Float;False;Constant;_Float3;Float 3;5;0;Create;True;0;0;False;0;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;43;-569.1204,390.3152;Float;True;3;0;FLOAT;0;False;1;FLOAT;0.6;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;10;-629.527,-314.446;Float;True;5;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT4;1,0,0,0;False;3;FLOAT4;0,0,0,0;False;4;FLOAT4;1,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;54;-627.6563,26.62595;Float;True;Property;_DeformationValue;DeformationValue;1;0;Create;True;0;0;False;0;2.55;0;0.5;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;47;-300.9492,471.176;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-642.7672,-63.78389;Float;False;Property;_bendRatio;bendRatio;0;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-216.4091,-205.4851;Float;True;3;3;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.OneMinusNode;50;-83.7641,479.1179;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;-4.734145,-63.82048;Float;True;2;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;2;379.8105,83.125;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;S_Link;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;8;0;57;0
WireConnection;8;1;51;0
WireConnection;68;0;15;1
WireConnection;45;0;15;1
WireConnection;9;0;8;1
WireConnection;9;1;8;2
WireConnection;46;0;45;0
WireConnection;46;1;67;0
WireConnection;46;2;66;0
WireConnection;43;0;68;0
WireConnection;43;1;67;0
WireConnection;43;2;66;0
WireConnection;10;0;9;0
WireConnection;10;1;13;0
WireConnection;10;2;11;0
WireConnection;10;3;12;0
WireConnection;10;4;11;0
WireConnection;47;0;43;0
WireConnection;47;1;46;0
WireConnection;17;0;10;0
WireConnection;17;1;6;0
WireConnection;17;2;54;0
WireConnection;50;0;47;0
WireConnection;49;0;50;0
WireConnection;49;1;17;0
WireConnection;2;11;49;0
ASEEND*/
//CHKSM=B62C4E5E18EBC1C1835307AFEFAF0E449FEF3782