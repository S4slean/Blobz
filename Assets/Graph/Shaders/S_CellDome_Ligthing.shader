// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Blobz/Cell/DomeLighing"
{
	Properties
	{
		_CellColor("CellColor", Color) = (0,0,0,0)
		_CellColorIntensity("CellColorIntensity", Range( 0.05 , 0.3)) = 0
		_DistortionMap("DistortionMap", 2D) = "white" {}
		_SideDeformationMap("SideDeformationMap", 2D) = "white" {}
		_DistortionValue("DistortionValue", Range( 0 , 0.15)) = 0.011
		_Ramp("Ramp", 2D) = "white" {}
		_LigthAttenuation("LigthAttenuation", Range( 0 , 2)) = 0.7529412
		_Min("Min", Range( 0 , 1)) = 0
		_Max("Max", Range( 0.4 , 1)) = 1
		_DeformationValue("DeformationValue", Range( -1 , 1)) = 0
		_SideDeformation("SideDeformation", Range( 0 , 0.2)) = 0
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
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit alpha:fade keepalpha noshadow vertex:vertexDataFunc 
		struct Input
		{
			float4 screenPos;
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform float _Min;
		uniform float _Max;
		uniform float _DeformationValue;
		uniform sampler2D _SideDeformationMap;
		uniform float _SideDeformation;
		uniform sampler2D _GrabTexture;
		uniform sampler2D _DistortionMap;
		uniform float _DistortionValue;
		uniform float _LigthAttenuation;
		uniform sampler2D _Ramp;
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
			v.vertex.xyz += ( float4( ( float3(0,0,-1) * smoothstepResult25 * _DeformationValue ) , 0.0 ) + ( smoothstepResult134 * ( (temp_cast_3 + (tex2Dlod( _SideDeformationMap, float4( panner143, 0, 0.0) ) - temp_cast_1) * (temp_cast_4 - temp_cast_3) / (temp_cast_2 - temp_cast_1)) * float4( ase_vertexNormal , 0.0 ) ) * _SideDeformation ) ).rgb;
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			o.Normal = float3(0,0,1);
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
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float3 normalizeResult93 = normalize( ase_worldlightDir );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_vertexNormal = mul( unity_WorldToObject, float4( ase_worldNormal, 0 ) );
			float3 worldToObj119 = mul( unity_WorldToObject, float4( float3( 0,0,0 ), 1 ) ).xyz;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float smoothstepResult25 = smoothstep( _Min , _Max , ase_vertex3Pos.z);
			float smoothstepResult134 = smoothstep( 0.56 , 1.0 , ( 1.0 - smoothstepResult25 ));
			float3 lerpResult130 = lerp( ( ( float3(0,0.5,0) + worldToObj119 ) - ase_vertex3Pos ) , ase_vertexNormal , smoothstepResult134);
			float3 normalizeResult105 = normalize( lerpResult130 );
			float3 lerpResult106 = lerp( ase_vertexNormal , normalizeResult105 , _DeformationValue);
			float3 normalizeResult94 = normalize( (WorldNormalVector( i , lerpResult106 )) );
			float dotResult68 = dot( normalizeResult93 , normalizeResult94 );
			float temp_output_72_0 = (0.0 + (pow( ( 1.0 - dotResult68 ) , 2.0 ) - -1.0) * (1.0 - 0.0) / (1.0 - -1.0));
			float4 appendResult76 = (float4(temp_output_72_0 , 1.0 , 0.0 , 0.0));
			float4 clampResult84 = clamp( ( ( ( _LigthAttenuation * temp_output_72_0 ) * tex2D( _Ramp, (appendResult76).xy ) ) + ( _CellColorIntensity * _CellColor ) ) , float4( 0,0,0,0 ) , float4( 0.9803922,0.9803922,0.9803922,0 ) );
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
-1920;0;1920;1019;2609.924;1091.21;1.349835;True;False
Node;AmplifyShaderEditor.CommentaryNode;139;-1244.614,-189.6481;Float;False;1839.225;466.205;Deformation Verticale;7;35;32;33;25;28;27;7;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-1023.699,161.5568;Float;False;Property;_Min;Min;7;0;Create;True;0;0;False;0;0;0.2;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-1017.685,244.3396;Float;False;Property;_Max;Max;8;0;Create;True;0;0;False;0;1;0.84;0.4;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;7;-1218.395,-23.4598;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;159;-735.4558,413.8814;Float;False;1680.153;882.2336;SideDeformation;16;133;134;137;136;141;142;143;140;158;157;156;155;154;151;152;147;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SmoothstepOpNode;25;-397.8498,20.0255;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;133;-410.5112,463.8814;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TransformPositionNode;119;-3104.485,864.2864;Float;False;World;Object;False;Fast;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector3Node;99;-3124.336,701.7896;Float;False;Constant;_Vector1;Vector 1;8;0;Create;True;0;0;False;0;0,0.5,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SmoothstepOpNode;134;-109.9522,466.0959;Float;True;3;0;FLOAT;0;False;1;FLOAT;0.56;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;137;65.25113,1214.115;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;100;-2841.883,713.8138;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PosVertexDataNode;102;-2821.219,1078.912;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;104;-2587.951,759.8608;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-29.32982,29.70053;Float;False;Property;_DeformationValue;DeformationValue;9;0;Create;True;0;0;False;0;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;128;-2533.416,1031.745;Float;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;138;-2104.428,1491.131;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;136;75.35828,1207.377;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;130;-2278.79,982.0336;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NormalizeNode;105;-2030.156,981.2684;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WireNode;135;-1605.812,1392.38;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;107;-1982.061,812.059;Float;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;106;-1677.766,865.5773;Float;True;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WireNode;109;-2116.54,1274.099;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WireNode;123;-2566.825,1067.687;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WireNode;108;-2407.217,67.56011;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;88;-2359.591,-905.5003;Float;False;2136.212;607.6041;Ligth;13;67;68;72;80;76;81;78;79;83;93;94;125;131;;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldNormalVector;70;-2277.711,-260.6554;Float;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;67;-2467.267,-691.4173;Float;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NormalizeNode;94;-2053.337,-470.9484;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NormalizeNode;93;-2203.337,-690.8483;Float;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DotProductOpNode;68;-1813.779,-683.7472;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;125;-1632.535,-620.2108;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;66;-2379.51,-1799.446;Float;False;2602.49;869.9062;Displacement;14;50;53;54;55;60;61;62;63;51;65;56;58;57;64;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;61;-2329.51,-1441.192;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;62;-2326.269,-1308.805;Float;False;Constant;_Vector0;Vector 0;5;0;Create;True;0;0;False;0;0.2,0.2;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PowerNode;131;-1478.871,-490.071;Float;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;72;-1400.175,-703.1476;Float;True;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;60;-2033.549,-1397.15;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;76;-1155.677,-481.5585;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;53;-1784.549,-1465.15;Float;True;Property;_DistortionMap;DistortionMap;2;0;Create;True;0;0;False;0;b2d4cc75b5d9b6541b0ba621a261eccc;b2d4cc75b5d9b6541b0ba621a261eccc;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;81;-927.1403,-456.3079;Float;True;True;True;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;79;-1586.774,-855.5003;Float;False;Property;_LigthAttenuation;LigthAttenuation;6;0;Create;True;0;0;False;0;0.7529412;0.114;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;54;-1425.549,-1444.15;Float;True;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.Vector2Node;141;-682.2148,813.3134;Float;False;Constant;_Vector2;Vector 2;5;0;Create;True;0;0;False;0;3,3;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;58;-1475.538,-1126.026;Float;False;Constant;_Float2;Float 2;5;0;Create;True;0;0;False;0;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;57;-1488.998,-1044.54;Float;False;Constant;_Float1;Float 1;5;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;142;-685.4558,680.9264;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;56;-1458.933,-1195.118;Float;False;Constant;_Float0;Float 0;5;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;78;-565.1005,-572.6411;Float;True;Property;_Ramp;Ramp;5;0;Create;True;0;0;False;0;ad863236c94ec5741b53d30db16a2f73;c6530a438a17f174bb7ae70dcda2a0fc;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;114;-201.1309,-643.1038;Float;False;Property;_CellColorIntensity;CellColorIntensity;1;0;Create;True;0;0;False;0;0;0.2055;0.05;0.3;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;55;-1091.549,-1423.15;Float;True;5;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT4;1,0,0,0;False;3;FLOAT4;0,0,0,0;False;4;FLOAT4;1,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;64;-1167.741,-1141.518;Float;False;Property;_DistortionValue;DistortionValue;4;0;Create;True;0;0;False;0;0.011;0.0117;0;0.15;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;143;-389.4948,724.9684;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;80;-1134.918,-838.4685;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;115;-197.5698,-559.8876;Float;False;Property;_CellColor;CellColor;0;0;Create;True;0;0;False;0;0,0,0,0;0.5215687,0.6930056,0.9607843,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;-727.95,-1416.325;Float;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;158;-171.8838,1125.705;Float;False;Constant;_Float5;Float 5;5;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GrabScreenPosition;51;-780.0224,-1749.446;Float;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;140;-132.8169,752.0363;Float;True;Property;_SideDeformationMap;SideDeformationMap;3;0;Create;True;0;0;False;0;None;b2d4cc75b5d9b6541b0ba621a261eccc;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;156;-158.4237,1044.219;Float;False;Constant;_Float3;Float 3;5;0;Create;True;0;0;False;0;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;83;-458.379,-833.4608;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;157;-141.8187,975.1271;Float;False;Constant;_Float4;Float 4;5;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;117;75.43018,-577.8876;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.NormalVertexDataNode;154;317.5029,1023.518;Float;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;65;-386.95,-1550.325;Float;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TFHCRemapNode;155;242.7242,737.553;Float;True;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;116;70.43018,-826.8876;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ScreenColorNode;50;20.9797,-1538.604;Float;False;Global;_GrabScreen0;Grab Screen 0;4;0;Create;True;0;0;False;0;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;84;240.4416,-720.7656;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0.9803922,0.9803922,0.9803922,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;151;320.697,561.5029;Float;False;Property;_SideDeformation;SideDeformation;10;0;Create;True;0;0;False;0;0;0;0;0.2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;152;530.7192,734.8209;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector3Node;32;-12.15166,-278.5844;Float;False;Constant;_DeformationVersleBas;DeformationVersleBas;2;0;Create;True;0;0;False;0;0,0,-1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;85;472.668,-744.3512;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;147;692.259,474.3225;Float;False;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;425.6116,-29.59209;Float;False;3;3;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NormalVertexDataNode;97;296.8019,-343.064;Float;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;148;740.4261,9.229095;Float;False;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BreakToComponentsNode;153;-1830.472,77.51611;Float;False;FLOAT;1;0;FLOAT;0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.PosVertexDataNode;101;40.66473,2185.85;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;87;766.8232,-742.666;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0.9803922,0.9803922,0.9803922,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;95;935.8036,-251.7018;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;Blobz/Cell/DomeLighing;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;25;0;7;3
WireConnection;25;1;28;0
WireConnection;25;2;27;0
WireConnection;133;0;25;0
WireConnection;134;0;133;0
WireConnection;137;0;134;0
WireConnection;100;0;99;0
WireConnection;100;1;119;0
WireConnection;104;0;100;0
WireConnection;104;1;102;0
WireConnection;138;0;137;0
WireConnection;136;0;35;0
WireConnection;130;0;104;0
WireConnection;130;1;128;0
WireConnection;130;2;138;0
WireConnection;105;0;130;0
WireConnection;135;0;136;0
WireConnection;106;0;107;0
WireConnection;106;1;105;0
WireConnection;106;2;135;0
WireConnection;109;0;106;0
WireConnection;123;0;109;0
WireConnection;108;0;123;0
WireConnection;70;0;108;0
WireConnection;94;0;70;0
WireConnection;93;0;67;0
WireConnection;68;0;93;0
WireConnection;68;1;94;0
WireConnection;125;0;68;0
WireConnection;131;0;125;0
WireConnection;72;0;131;0
WireConnection;60;0;61;0
WireConnection;60;2;62;0
WireConnection;76;0;72;0
WireConnection;53;1;60;0
WireConnection;81;0;76;0
WireConnection;54;0;53;1
WireConnection;54;1;53;2
WireConnection;78;1;81;0
WireConnection;55;0;54;0
WireConnection;55;1;56;0
WireConnection;55;2;57;0
WireConnection;55;3;58;0
WireConnection;55;4;57;0
WireConnection;143;0;142;0
WireConnection;143;2;141;0
WireConnection;80;0;79;0
WireConnection;80;1;72;0
WireConnection;63;0;55;0
WireConnection;63;1;64;0
WireConnection;140;1;143;0
WireConnection;83;0;80;0
WireConnection;83;1;78;0
WireConnection;117;0;114;0
WireConnection;117;1;115;0
WireConnection;65;0;51;0
WireConnection;65;1;63;0
WireConnection;155;0;140;0
WireConnection;155;1;157;0
WireConnection;155;2;158;0
WireConnection;155;3;156;0
WireConnection;155;4;158;0
WireConnection;116;0;83;0
WireConnection;116;1;117;0
WireConnection;50;0;65;0
WireConnection;84;0;116;0
WireConnection;152;0;155;0
WireConnection;152;1;154;0
WireConnection;85;0;50;0
WireConnection;85;1;84;0
WireConnection;147;0;134;0
WireConnection;147;1;152;0
WireConnection;147;2;151;0
WireConnection;33;0;32;0
WireConnection;33;1;25;0
WireConnection;33;2;35;0
WireConnection;148;0;33;0
WireConnection;148;1;147;0
WireConnection;87;0;85;0
WireConnection;95;2;87;0
WireConnection;95;11;148;0
ASEEND*/
//CHKSM=A9F54DD5D9A9043B0CDA83E97889FF07AC9C802C