// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "S_Link"
{
	Properties
	{
		_bendRatio("bendRatio", Range( 0 , 1)) = 0
		_DistortionValue("DistortionValue", Range( 0.5 , 3)) = 2.55
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
		uniform float _DistortionValue;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float smoothstepResult43 = smoothstep( 0.6 , 1.0 , v.texcoord.xy.x);
			float smoothstepResult46 = smoothstep( 0.6 , 1.0 , ( 1.0 - v.texcoord.xy.x ));
			float2 uv_TexCoord51 = v.texcoord.xy * float2( 3,3 );
			float4 tex2DNode8 = tex2Dlod( _Texture0, float4( uv_TexCoord51, 0, 0.0) );
			float4 appendResult9 = (float4(tex2DNode8.r , tex2DNode8.g , 0.0 , 0.0));
			float4 temp_cast_0 = (0.0).xxxx;
			float4 temp_cast_1 = (1.0).xxxx;
			float4 temp_cast_2 = (-1.0).xxxx;
			float4 temp_cast_3 = (1.0).xxxx;
			v.vertex.xyz += ( ( 1.0 - ( smoothstepResult43 + smoothstepResult46 ) ) * ( (temp_cast_2 + (appendResult9 - temp_cast_0) * (temp_cast_3 - temp_cast_2) / (temp_cast_1 - temp_cast_0)) * _bendRatio * _DistortionValue ) ).xyz;
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
-1920;0;1920;1019;1720.918;-308.8394;1;True;False
Node;AmplifyShaderEditor.TexturePropertyNode;57;-1431.62,-249.8674;Float;True;Property;_Texture0;Texture 0;3;0;Create;True;0;0;False;0;cd460ee4ac5c1e746b7a734cc7cc64dd;cd460ee4ac5c1e746b7a734cc7cc64dd;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;51;-1476.521,1.08873;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;3,3;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;15;-1072.11,516.4548;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;45;-767.5254,608.2532;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;8;-1207.477,-160.5331;Float;True;Property;_TextureSample0;Texture Sample 0;3;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;46;-564.0029,615.4974;Float;True;3;0;FLOAT;0;False;1;FLOAT;0.6;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-791.1301,100.5885;Float;False;Constant;_Float4;Float 4;5;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-772.7351,245.6803;Float;False;Constant;_Float3;Float 3;5;0;Create;True;0;0;False;0;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;9;-855.1569,-126.5102;Float;True;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-786.1952,327.1663;Float;False;Constant;_Float5;Float 5;5;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;43;-569.1204,390.3152;Float;True;3;0;FLOAT;0;False;1;FLOAT;0.6;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;10;-552.2867,-100.9887;Float;True;5;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT4;1,0,0,0;False;3;FLOAT4;0,0,0,0;False;4;FLOAT4;1,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;47;-300.9492,471.176;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;54;-492.5446,-400.6816;Float;True;Property;_DistortionValue;DistortionValue;2;0;Create;True;0;0;False;0;2.55;0;0.5;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-612.7827,190.6484;Float;False;Property;_bendRatio;bendRatio;1;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-145.1662,-74.49013;Float;True;3;3;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.OneMinusNode;50;-83.7641,479.1179;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;58;-610.918,898.8394;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;60;-1056.918,730.8394;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;-804.918,909.8394;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;66.50878,67.17458;Float;True;2;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-1001.378,928.0937;Float;False;Property;_Range;Range;0;0;Create;True;0;0;False;0;16;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;2;802.5193,-130.8173;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;S_Link;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;45;0;15;1
WireConnection;8;0;57;0
WireConnection;8;1;51;0
WireConnection;46;0;45;0
WireConnection;9;0;8;1
WireConnection;9;1;8;2
WireConnection;43;0;15;1
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
WireConnection;58;0;59;0
WireConnection;59;0;60;1
WireConnection;59;1;3;0
WireConnection;49;0;50;0
WireConnection;49;1;17;0
WireConnection;2;11;49;0
ASEEND*/
//CHKSM=EF3EBFECDD34F6DA4193896789E6F9280CC93A2F