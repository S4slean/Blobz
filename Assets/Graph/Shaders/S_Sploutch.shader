// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Blobz/Cell/Sploutch"
{
	Properties
	{
		_Noise("Noise", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit alpha:fade keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Noise;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 panner7 = ( 1.0 * _Time.y * float2( 0.3,0.3 ) + i.uv_texcoord);
			float2 panner8 = ( 1.0 * _Time.y * float2( 0.3,0.6 ) + i.uv_texcoord);
			float temp_output_12_0 = ( tex2D( _Noise, panner7 ).r * tex2D( _Noise, panner8 ).r );
			float3 temp_cast_0 = (( step( 0.3 , temp_output_12_0 ) - step( 0.4 , temp_output_12_0 ) )).xxx;
			o.Emission = temp_cast_0;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15900
-1920;0;1920;1019;816.6404;383.0835;1;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;9;-1126.924,-124.0723;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;3;-1118.924,-388.0723;Float;True;Property;_Noise;Noise;0;0;Create;True;0;0;False;0;e6f9ade421a7a3446b784a8d2602c822;None;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.PannerNode;7;-735.9244,-109.0723;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.3,0.3;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;8;-759.9244,42.92771;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.3,0.6;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;10;-550.9244,-352.0723;Float;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;11;-509.9244,-98.07228;Float;True;Property;_TextureSample1;Texture Sample 1;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-181.9245,-225.0723;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;13;98.87332,-188.5202;Float;True;2;0;FLOAT;0.3;False;1;FLOAT;0.4;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;14;106.8708,27.12524;Float;True;2;0;FLOAT;0.4;False;1;FLOAT;0.4;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;15;365.8708,-174.8748;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;2;638.5,44.6;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;Blobz/Cell/Sploutch;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;7;0;9;0
WireConnection;8;0;9;0
WireConnection;10;0;3;0
WireConnection;10;1;7;0
WireConnection;11;0;3;0
WireConnection;11;1;8;0
WireConnection;12;0;10;1
WireConnection;12;1;11;1
WireConnection;13;1;12;0
WireConnection;14;1;12;0
WireConnection;15;0;13;0
WireConnection;15;1;14;0
WireConnection;2;2;15;0
ASEEND*/
//CHKSM=3BD3FBFA79D4FE13B3F7F24E11301ECDECCA6F6F