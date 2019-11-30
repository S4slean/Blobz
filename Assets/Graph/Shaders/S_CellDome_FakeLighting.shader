// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Blobz/Cell/DomeFakeLighing"
{
	Properties
	{
		_CellColor("CellColor", Color) = (0,0,0,0)
		_CellColorIntensity("CellColorIntensity", Range( 0.05 , 0.3)) = 0
		_DistortionMap("DistortionMap", 2D) = "white" {}
		_DistortionValue("DistortionValue", Range( 0 , 0.15)) = 0.011
		_SideDeformationMap("SideDeformationMap", 2D) = "white" {}
		_Min("Min", Range( 0 , 1)) = 0
		_Max("Max", Range( 0.4 , 1)) = 1
		_DeformationValue("DeformationValue", Range( -1 , 1)) = 0
		_SideDeformation("SideDeformation", Range( 0 , 0.2)) = 0
		_ShapeFactor("ShapeFactor", Range( 0 , 1)) = 0
		_Ramp("Ramp", 2D) = "white" {}
		_LigthAttenuation("LigthAttenuation", Range( 0 , 2)) = 0.7529412
		_LigthLevelMin("LigthLevelMin", Range( 0 , 1)) = 0
		_LigthLevelMax("LigthLevelMax", Range( 0.4 , 1)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		GrabPass{ }
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit alpha:fade keepalpha noshadow vertex:vertexDataFunc 
		struct Input
		{
			float4 screenPos;
			float2 uv_texcoord;
			float3 worldPos;
		};

		uniform float _Min;
		uniform float _Max;
		uniform float _DeformationValue;
		uniform sampler2D _SideDeformationMap;
		uniform float _SideDeformation;
		uniform float _ShapeFactor;
		uniform sampler2D _GrabTexture;
		uniform sampler2D _DistortionMap;
		uniform float _DistortionValue;
		uniform float _LigthAttenuation;
		uniform sampler2D _Ramp;
		uniform float _LigthLevelMin;
		uniform float _LigthLevelMax;
		uniform float _CellColorIntensity;
		uniform float4 _CellColor;


		inline float4 ASE_ComputeGrabScreenPos( float4 pos )
		{
			#if UNITY_UV_STARTS_AT_TOP
			float scale = -1.0;
			#else
			float scale = 1.0;
			#endif
			float4 o = pos;
			o.y = pos.w * 0.5f;
			o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
			return o;
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float smoothstepResult25 = smoothstep( _Min , _Max , ase_vertex3Pos.z);
			float smoothstepResult134 = smoothstep( 0.56 , 1.0 , ( 1.0 - smoothstepResult25 ));
			float2 panner143 = ( 1.0 * _Time.y * float2( 3,3 ) + v.texcoord.xy);
			float4 temp_cast_1 = (0.0).xxxx;
			float4 temp_cast_2 = (1.0).xxxx;
			float4 temp_cast_3 = (-1.0).xxxx;
			float4 temp_cast_4 = (1.0).xxxx;
			float3 ase_vertexNormal = v.normal.xyz;
			v.vertex.xyz += ( float4( ( float3(0,0,-1) * smoothstepResult25 * _DeformationValue ) , 0.0 ) + ( smoothstepResult134 * ( (temp_cast_3 + (tex2Dlod( _SideDeformationMap, float4( panner143, 0, 0.0) ) - temp_cast_1) * (temp_cast_4 - temp_cast_3) / (temp_cast_2 - temp_cast_1)) * float4( ase_vertexNormal , 0.0 ) ) * _SideDeformation * _ShapeFactor ) ).rgb;
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
			float2 panner60 = ( 1.0 * _Time.y * float2( 0.2,0.2 ) + i.uv_texcoord);
			float4 tex2DNode53 = tex2D( _DistortionMap, panner60 );
			float4 appendResult54 = (float4(tex2DNode53.r , tex2DNode53.g , 0.0 , 0.0));
			float4 temp_cast_0 = (0.0).xxxx;
			float4 temp_cast_1 = (1.0).xxxx;
			float4 temp_cast_2 = (-1.0).xxxx;
			float4 temp_cast_3 = (1.0).xxxx;
			float4 screenColor50 = tex2D( _GrabTexture, ( ase_grabScreenPosNorm + ( (temp_cast_2 + (appendResult54 - temp_cast_0) * (temp_cast_3 - temp_cast_2) / (temp_cast_1 - temp_cast_0)) * _DistortionValue ) ).xy );
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float smoothstepResult170 = smoothstep( _LigthLevelMin , _LigthLevelMax , ase_vertex3Pos.z);
			float4 appendResult160 = (float4(smoothstepResult170 , 0.0 , 0.0 , 0.0));
			float4 clampResult84 = clamp( ( ( _LigthAttenuation * tex2D( _Ramp, (appendResult160).xy ) ) + ( _CellColorIntensity * _CellColor ) ) , float4( 0,0,0,0 ) , float4( 0.9803922,0.9803922,0.9803922,0 ) );
			float4 clampResult87 = clamp( ( screenColor50 + clampResult84 ) , float4( 0,0,0,0 ) , float4( 0.9803922,0.9803922,0.9803922,0 ) );
			o.Emission = clampResult87.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15900
-1920;0;1920;1019;3424.423;1759.148;2.293288;True;False
Node;AmplifyShaderEditor.CommentaryNode;66;-2379.51,-1799.446;Float;False;2602.49;869.9062;Displacement;14;50;53;54;55;60;61;62;63;51;65;56;58;57;64;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;88;-2359.591,-905.5003;Float;False;2136.212;607.6041;Ligth;9;78;79;83;160;163;169;170;171;172;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;172;-1846.759,-558.1956;Float;False;Property;_LigthLevelMin;LigthLevelMin;12;0;Create;True;0;0;False;0;0;0.2;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;62;-2326.269,-1308.805;Float;False;Constant;_Vector0;Vector 0;5;0;Create;True;0;0;False;0;0.2,0.2;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;61;-2329.51,-1441.192;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosVertexDataNode;169;-1858.695,-866.4219;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;171;-2075.24,-657.731;Float;False;Property;_LigthLevelMax;LigthLevelMax;13;0;Create;True;0;0;False;0;1;0.84;0.4;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;170;-1611.649,-851.3384;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;174;-1392.805,-194.0604;Float;False;1954.713;1522.637;Physics;2;159;139;;1,1,1,1;0;0
Node;AmplifyShaderEditor.PannerNode;60;-2033.549,-1397.15;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;160;-1284.658,-679.5988;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;53;-1784.549,-1465.15;Float;True;Property;_DistortionMap;DistortionMap;2;0;Create;True;0;0;False;0;b2d4cc75b5d9b6541b0ba621a261eccc;b2d4cc75b5d9b6541b0ba621a261eccc;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;159;-1350.558,382.8678;Float;False;1680.153;882.2336;SideDeformation;15;133;134;141;142;143;140;158;157;156;155;154;151;152;147;173;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;58;-1475.538,-1126.026;Float;False;Constant;_Float2;Float 2;5;0;Create;True;0;0;False;0;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;163;-1056.607,-689.3899;Float;False;True;True;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;54;-1425.549,-1444.15;Float;True;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.Vector2Node;141;-1297.318,782.2998;Float;False;Constant;_Vector2;Vector 2;5;0;Create;True;0;0;False;0;3,3;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;142;-1300.559,649.9128;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;57;-1488.998,-1044.54;Float;False;Constant;_Float1;Float 1;5;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;56;-1458.933,-1195.118;Float;False;Constant;_Float0;Float 0;5;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;139;-1363.5,-106.9453;Float;False;1839.225;466.205;Deformation Verticale;7;35;32;33;25;28;7;27;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;64;-1167.741,-1141.518;Float;False;Property;_DistortionValue;DistortionValue;3;0;Create;True;0;0;False;0;0.011;0.0117;0;0.15;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;78;-779.5205,-751.7832;Float;True;Property;_Ramp;Ramp;10;0;Create;True;0;0;False;0;ad863236c94ec5741b53d30db16a2f73;c6530a438a17f174bb7ae70dcda2a0fc;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosVertexDataNode;7;-1120.831,50.22425;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;143;-1004.597,693.9548;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-1142.585,244.2596;Float;False;Property;_Min;Min;5;0;Create;True;0;0;False;0;0;0.2;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;79;-772.2001,-831.0221;Float;False;Property;_LigthAttenuation;LigthAttenuation;11;0;Create;True;0;0;False;0;0.7529412;0.114;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;115;-197.5698,-559.8876;Float;False;Property;_CellColor;CellColor;0;0;Create;True;0;0;False;0;0,0,0,0;0.5215687,0.6930056,0.9607843,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;55;-1091.549,-1423.15;Float;True;5;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT4;1,0,0,0;False;3;FLOAT4;0,0,0,0;False;4;FLOAT4;1,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;114;-201.1309,-643.1038;Float;False;Property;_CellColorIntensity;CellColorIntensity;1;0;Create;True;0;0;False;0;0;0.2055;0.05;0.3;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-1178.259,-50.01888;Float;False;Property;_Max;Max;6;0;Create;True;0;0;False;0;1;0.84;0.4;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;-727.95,-1416.325;Float;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;156;-773.5259,1013.205;Float;False;Constant;_Float3;Float 3;5;0;Create;True;0;0;False;0;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;83;-461.3506,-831.5739;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;117;75.43018,-577.8876;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;157;-756.9209,944.1133;Float;False;Constant;_Float4;Float 4;5;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;25;-516.7352,102.7283;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;140;-747.9191,721.0227;Float;True;Property;_SideDeformationMap;SideDeformationMap;4;0;Create;True;0;0;False;0;None;b2d4cc75b5d9b6541b0ba621a261eccc;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GrabScreenPosition;51;-780.0224,-1749.446;Float;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;158;-786.986,1094.691;Float;False;Constant;_Float5;Float 5;5;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;155;-372.3781,706.5394;Float;True;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.NormalVertexDataNode;154;-297.5993,992.5044;Float;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;133;-1025.613,432.868;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;116;70.43018,-826.8876;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;65;-386.95,-1550.325;Float;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.Vector3Node;32;-131.037,-60.59718;Float;False;Constant;_DeformationVersleBas;DeformationVersleBas;2;0;Create;True;0;0;False;0;0,0,-1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SmoothstepOpNode;134;-725.0544,435.0825;Float;True;3;0;FLOAT;0;False;1;FLOAT;0.56;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-148.2151,112.4034;Float;False;Property;_DeformationValue;DeformationValue;7;0;Create;True;0;0;False;0;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;173;-409.4697,595.4476;Float;False;Property;_ShapeFactor;ShapeFactor;9;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;152;-84.38309,703.8073;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ScreenColorNode;50;20.9797,-1538.604;Float;False;Global;_GrabScreen0;Grab Screen 0;4;0;Create;True;0;0;False;0;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;84;240.4416,-720.7656;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0.9803922,0.9803922,0.9803922,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;151;-392.4359,502.3198;Float;False;Property;_SideDeformation;SideDeformation;8;0;Create;True;0;0;False;0;0;0;0;0.2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;147;77.15671,443.3091;Float;False;4;4;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;85;472.668,-744.3512;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;306.7263,53.11069;Float;False;3;3;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ClampOpNode;87;766.8232,-742.666;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0.9803922,0.9803922,0.9803922,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;148;740.4261,9.229095;Float;False;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;95;935.8036,-251.7018;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;Blobz/Cell/DomeFakeLighing;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;170;0;169;3
WireConnection;170;1;172;0
WireConnection;170;2;171;0
WireConnection;60;0;61;0
WireConnection;60;2;62;0
WireConnection;160;0;170;0
WireConnection;53;1;60;0
WireConnection;163;0;160;0
WireConnection;54;0;53;1
WireConnection;54;1;53;2
WireConnection;78;1;163;0
WireConnection;143;0;142;0
WireConnection;143;2;141;0
WireConnection;55;0;54;0
WireConnection;55;1;56;0
WireConnection;55;2;57;0
WireConnection;55;3;58;0
WireConnection;55;4;57;0
WireConnection;63;0;55;0
WireConnection;63;1;64;0
WireConnection;83;0;79;0
WireConnection;83;1;78;0
WireConnection;117;0;114;0
WireConnection;117;1;115;0
WireConnection;25;0;7;3
WireConnection;25;1;28;0
WireConnection;25;2;27;0
WireConnection;140;1;143;0
WireConnection;155;0;140;0
WireConnection;155;1;157;0
WireConnection;155;2;158;0
WireConnection;155;3;156;0
WireConnection;155;4;158;0
WireConnection;133;0;25;0
WireConnection;116;0;83;0
WireConnection;116;1;117;0
WireConnection;65;0;51;0
WireConnection;65;1;63;0
WireConnection;134;0;133;0
WireConnection;152;0;155;0
WireConnection;152;1;154;0
WireConnection;50;0;65;0
WireConnection;84;0;116;0
WireConnection;147;0;134;0
WireConnection;147;1;152;0
WireConnection;147;2;151;0
WireConnection;147;3;173;0
WireConnection;85;0;50;0
WireConnection;85;1;84;0
WireConnection;33;0;32;0
WireConnection;33;1;25;0
WireConnection;33;2;35;0
WireConnection;87;0;85;0
WireConnection;148;0;33;0
WireConnection;148;1;147;0
WireConnection;95;2;87;0
WireConnection;95;11;148;0
ASEEND*/
//CHKSM=F370212340CCA9D447C1AE0749E68C7F10FAF600