// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "LowPolyPainterDetail"
{
	Properties
	{
		_MainColor("Main Color", Color) = (1,1,1,1)
		_Diffuse("Diffuse", 2D) = "white" {}
		_Reflective("Reflective", 2D) = "white" {}
		_Emission("Emission", 2D) = "white" {}
		_EmissionAmount("Emission Amount", Range( 0 , 1000)) = 0
		_DetailTextureA("Detail Texture A", 2D) = "white" {}
		_MixAmountA("MixAmountA", Range( 0 , 10)) = 1
		_DetailTextureB("Detail Texture B", 2D) = "white" {}
		_MixAmountB("MixAmountB", Range( 0 , 10)) = 1
		_DetailTextureC("Detail Texture C", 2D) = "white" {}
		_MixAmountC("MixAmountC", Range( 0 , 10)) = 1
		_DetailTextureD("Detail Texture D", 2D) = "white" {}
		_MixAmountD("MixAmountD", Range( 0 , 10)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
		[Header(Forward Rendering Options)]
		[ToggleOff] _SpecularHighlights("Specular Highlights", Float) = 1.0
		[ToggleOff] _GlossyReflections("Reflections", Float) = 1.0
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma shader_feature _SPECULARHIGHLIGHTS_OFF
		#pragma shader_feature _GLOSSYREFLECTIONS_OFF
		#define ASE_TEXTURE_PARAMS(textureName) textureName

		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
			float4 vertexColor : COLOR;
			float2 uv_texcoord;
		};

		uniform float4 _MainColor;
		uniform sampler2D _DetailTextureA;
		uniform float4 _DetailTextureA_ST;
		uniform float _MixAmountA;
		uniform sampler2D _DetailTextureB;
		uniform float4 _DetailTextureB_ST;
		uniform float _MixAmountB;
		uniform sampler2D _DetailTextureC;
		uniform float4 _DetailTextureC_ST;
		uniform float _MixAmountC;
		uniform sampler2D _DetailTextureD;
		uniform float4 _DetailTextureD_ST;
		uniform float _MixAmountD;
		uniform sampler2D _Diffuse;
		uniform float4 _Diffuse_ST;
		uniform sampler2D _Emission;
		uniform float4 _Emission_ST;
		uniform float _EmissionAmount;
		uniform sampler2D _Reflective;
		uniform float4 _Reflective_ST;


		inline float4 TriplanarSamplingSF( sampler2D topTexMap, float3 worldPos, float3 worldNormal, float falloff, float2 tiling, float3 normalScale, float3 index )
		{
			float3 projNormal = ( pow( abs( worldNormal ), falloff ) );
			projNormal /= ( projNormal.x + projNormal.y + projNormal.z ) + 0.00001;
			float3 nsign = sign( worldNormal );
			half4 xNorm; half4 yNorm; half4 zNorm;
			xNorm = ( tex2D( ASE_TEXTURE_PARAMS( topTexMap ), tiling * worldPos.zy * float2( nsign.x, 1.0 ) ) );
			yNorm = ( tex2D( ASE_TEXTURE_PARAMS( topTexMap ), tiling * worldPos.xz * float2( nsign.y, 1.0 ) ) );
			zNorm = ( tex2D( ASE_TEXTURE_PARAMS( topTexMap ), tiling * worldPos.xy * float2( -nsign.z, 1.0 ) ) );
			return xNorm * projNormal.x + yNorm * projNormal.y + zNorm * projNormal.z;
		}


		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			o.Normal = float3(0,0,1);
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 ase_vertexNormal = mul( unity_WorldToObject, float4( ase_worldNormal, 0 ) );
			float4 triplanar26 = TriplanarSamplingSF( _DetailTextureA, ase_vertex3Pos, ase_vertexNormal, 10.0, _DetailTextureA_ST.xy, 1.0, 0 );
			float ifLocalVar74 = 0;
			if( i.vertexColor.r >= 0.85 )
				ifLocalVar74 = i.vertexColor.r;
			else
				ifLocalVar74 = 0.0;
			float4 temp_cast_0 = (ifLocalVar74).xxxx;
			float4 blendOpSrc27 = triplanar26;
			float4 blendOpDest27 = temp_cast_0;
			float4 temp_output_27_0 = ( saturate( 	max( blendOpSrc27, blendOpDest27 ) ));
			float4 color83 = IsGammaSpace() ? float4(1,1,1,1) : float4(1,1,1,1);
			float4 blendOpSrc91 = temp_output_27_0;
			float4 blendOpDest91 = color83;
			float4 lerpBlendMode91 = lerp(blendOpDest91,( blendOpSrc91 * blendOpDest91 ),_MixAmountA);
			float4 triplanar31 = TriplanarSamplingSF( _DetailTextureB, ase_vertex3Pos, ase_vertexNormal, 10.0, _DetailTextureB_ST.xy, 1.0, 0 );
			float ifLocalVar75 = 0;
			if( i.vertexColor.g >= 0.85 )
				ifLocalVar75 = i.vertexColor.g;
			else
				ifLocalVar75 = 0.0;
			float4 temp_cast_2 = (ifLocalVar75).xxxx;
			float4 blendOpSrc32 = triplanar31;
			float4 blendOpDest32 = temp_cast_2;
			float4 temp_output_32_0 = ( saturate( 	max( blendOpSrc32, blendOpDest32 ) ));
			float4 blendOpSrc90 = temp_output_32_0;
			float4 blendOpDest90 = color83;
			float4 lerpBlendMode90 = lerp(blendOpDest90,( blendOpSrc90 * blendOpDest90 ),_MixAmountB);
			float4 triplanar35 = TriplanarSamplingSF( _DetailTextureC, ase_vertex3Pos, ase_vertexNormal, 10.0, _DetailTextureC_ST.xy, 1.0, 0 );
			float ifLocalVar76 = 0;
			if( i.vertexColor.b >= 0.85 )
				ifLocalVar76 = i.vertexColor.b;
			else
				ifLocalVar76 = 0.0;
			float4 temp_cast_4 = (ifLocalVar76).xxxx;
			float4 blendOpSrc36 = triplanar35;
			float4 blendOpDest36 = temp_cast_4;
			float4 temp_output_36_0 = ( saturate( 	max( blendOpSrc36, blendOpDest36 ) ));
			float4 blendOpSrc89 = temp_output_36_0;
			float4 blendOpDest89 = color83;
			float4 lerpBlendMode89 = lerp(blendOpDest89,( blendOpSrc89 * blendOpDest89 ),_MixAmountC);
			float4 triplanar39 = TriplanarSamplingSF( _DetailTextureD, ase_vertex3Pos, ase_vertexNormal, 10.0, _DetailTextureD_ST.xy, 1.0, 0 );
			float ifLocalVar71 = 0;
			if( i.vertexColor.a >= 0.5 )
				ifLocalVar71 = i.vertexColor.a;
			else
				ifLocalVar71 = 0.0;
			float4 temp_cast_6 = (ifLocalVar71).xxxx;
			float4 blendOpSrc40 = triplanar39;
			float4 blendOpDest40 = temp_cast_6;
			float4 temp_output_40_0 = ( saturate( 	max( blendOpSrc40, blendOpDest40 ) ));
			float4 blendOpSrc84 = temp_output_40_0;
			float4 blendOpDest84 = color83;
			float4 lerpBlendMode84 = lerp(blendOpDest84,( blendOpSrc84 * blendOpDest84 ),_MixAmountD);
			float2 uv_Diffuse = i.uv_texcoord * _Diffuse_ST.xy + _Diffuse_ST.zw;
			float4 tex2DNode1 = tex2D( _Diffuse, uv_Diffuse );
			o.Albedo = ( _MainColor * ( ( ( saturate( lerpBlendMode91 )) * ( saturate( lerpBlendMode90 )) * ( saturate( lerpBlendMode89 )) * ( saturate( lerpBlendMode84 )) ) * tex2DNode1 ) ).rgb;
			float2 uv_Emission = i.uv_texcoord * _Emission_ST.xy + _Emission_ST.zw;
			float4 tex2DNode3 = tex2D( _Emission, uv_Emission );
			o.Emission = ( _MainColor * ( tex2DNode3 * _EmissionAmount * tex2DNode3.a ) ).rgb;
			float2 uv_Reflective = i.uv_texcoord * _Reflective_ST.xy + _Reflective_ST.zw;
			float4 tex2DNode2 = tex2D( _Reflective, uv_Reflective );
			o.Specular = ( _MainColor * ( tex2DNode2 * tex2DNode2.a ) ).rgb;
			o.Smoothness = tex2DNode2.a;
			o.Alpha = tex2DNode1.a;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardSpecular keepalpha fullforwardshadows 

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
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
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
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				half4 color : COLOR0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.color = v.color;
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
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				surfIN.vertexColor = IN.color;
				SurfaceOutputStandardSpecular o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandardSpecular, o )
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
}
/*ASEBEGIN
Version=17000
588;273;1061;730;1580.485;2521.883;3.51794;True;False
Node;AmplifyShaderEditor.TexturePropertyNode;33;-1527.83,-934.0665;Float;True;Property;_DetailTextureC;Detail Texture C;9;0;Create;True;0;0;False;0;beab9c2b8b811224ca1a097c36a2d76b;0ec466566f5bb9444937395faa118b4f;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;37;-1526.281,-708.5567;Float;True;Property;_DetailTextureD;Detail Texture D;11;0;Create;True;0;0;False;0;9faefc29de297db4fba42672e05da67d;2b1d6d8193767624f8f84125a6db0c9d;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;29;-1531.114,-1181.66;Float;True;Property;_DetailTextureB;Detail Texture B;7;0;Create;True;0;0;False;0;226216e59be77be42b555a19788f8cfb;ef2f3f9948e71cb4aa3cf65477fe2d12;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;25;-1533.674,-1423.024;Float;True;Property;_DetailTextureA;Detail Texture A;5;0;Create;True;0;0;False;0;c2ad71b30c7d51840bb921257ea29101;c68296334e691ed45b62266cbc716628;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.VertexColorNode;82;-1679.784,-505.9942;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;73;-1440.244,121.4149;Float;False;Constant;_Float1;Float 1;10;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;72;-1455.047,-86.88381;Float;False;Constant;_Float0;Float 0;10;0;Create;True;0;0;False;0;0.85;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureTransformNode;30;-1145.644,-1074.019;Float;False;-1;1;0;SAMPLER2D;;False;2;FLOAT2;0;FLOAT2;1
Node;AmplifyShaderEditor.TextureTransformNode;38;-1139.411,-651.0762;Float;False;-1;1;0;SAMPLER2D;;False;2;FLOAT2;0;FLOAT2;1
Node;AmplifyShaderEditor.TextureTransformNode;24;-1131.569,-1305.403;Float;False;-1;1;0;SAMPLER2D;;False;2;FLOAT2;0;FLOAT2;1
Node;AmplifyShaderEditor.TextureTransformNode;34;-1162.324,-813.1174;Float;False;-1;1;0;SAMPLER2D;;False;2;FLOAT2;0;FLOAT2;1
Node;AmplifyShaderEditor.RangedFloatNode;81;-1441.877,9.821033;Float;False;Constant;_Float2;Float 2;10;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TriplanarNode;39;-885.2855,-739.5115;Float;True;Spherical;Object;False;Top Texture 3;_TopTexture3;white;-1;None;Mid Texture 3;_MidTexture3;white;-1;None;Bot Texture 3;_BotTexture3;white;-1;None;Triplanar Sampler;False;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT;1;False;3;FLOAT2;1,1;False;4;FLOAT;10;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ConditionalIfNode;75;-975.8975,-497.1628;Float;True;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;74;-1278.047,-491.8359;Float;True;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TriplanarNode;31;-888.7687,-1138.075;Float;True;Spherical;Object;False;Top Texture 1;_TopTexture1;white;-1;None;Mid Texture 1;_MidTexture1;white;-1;None;Bot Texture 1;_BotTexture1;white;-1;None;Triplanar Sampler;False;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT;1;False;3;FLOAT2;1,1;False;4;FLOAT;10;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TriplanarNode;26;-894.6563,-1359.475;Float;True;Spherical;Object;False;Top Texture 0;_TopTexture0;white;-1;None;Mid Texture 0;_MidTexture0;white;-1;None;Bot Texture 0;_BotTexture0;white;-1;None;Triplanar Sampler;False;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT;1;False;3;FLOAT2;1,1;False;4;FLOAT;10;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TriplanarNode;35;-885.4856,-937.0624;Float;True;Spherical;Object;False;Top Texture 2;_TopTexture2;white;-1;None;Mid Texture 2;_MidTexture2;white;-1;None;Bot Texture 2;_BotTexture2;white;-1;None;Triplanar Sampler;False;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT;1;False;3;FLOAT2;1,1;False;4;FLOAT;10;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ConditionalIfNode;76;-699.175,-500.8802;Float;True;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;71;-968.7141,-138.1043;Float;True;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;27;-176.0779,-1358.079;Float;False;Lighten;True;3;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.BlendOpsNode;36;-192.9049,-903.5579;Float;False;Lighten;True;3;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.BlendOpsNode;32;-162.7599,-1120.089;Float;False;Lighten;True;3;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.BlendOpsNode;40;-189.8289,-702.7905;Float;False;Lighten;True;3;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;83;-181.0474,-470.44;Float;False;Constant;_Color0;Color 0;20;0;Create;True;0;0;False;0;1,1,1,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;85;-246.1837,-564.8878;Float;False;Property;_MixAmountD;MixAmountD;12;0;Create;True;0;0;False;0;1;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;86;-242.927,-778.2095;Float;False;Property;_MixAmountC;MixAmountC;10;0;Create;True;0;0;False;0;1;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;87;-226.6428,-991.5307;Float;False;Property;_MixAmountB;MixAmountB;8;0;Create;True;0;0;False;0;1;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;88;-211.9872,-1208.109;Float;False;Property;_MixAmountA;MixAmountA;6;0;Create;True;0;0;False;0;1;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;91;95.78165,-1317.212;Float;False;Multiply;True;3;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.TexturePropertyNode;23;-2063.653,-253.0503;Float;True;Property;_Diffuse;Diffuse;1;0;Create;True;0;0;False;0;31d1a56ab37626643bfe31b8e4dd1a27;31d1a56ab37626643bfe31b8e4dd1a27;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.BlendOpsNode;84;81.12608,-680.5048;Float;False;Multiply;True;3;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendOpsNode;89;71.35551,-900.34;Float;False;Multiply;True;3;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendOpsNode;90;84.38273,-1108.776;Float;False;Multiply;True;3;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;1;-680.9472,-36.33194;Float;True;Property;_DiffuseOld;DiffuseOld;1;0;Create;True;0;0;False;0;31d1a56ab37626643bfe31b8e4dd1a27;31d1a56ab37626643bfe31b8e4dd1a27;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-811.2415,224.6881;Float;True;Property;_Reflective;Reflective;2;0;Create;True;0;0;False;0;None;e8d9d9388c8fa954a82880f276c69599;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;371.9456,-712.153;Float;False;4;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;3;-829.4773,463.906;Float;True;Property;_Emission;Emission;3;0;Create;True;0;0;False;0;None;045d94ef7b920174fb7054628f7b7196;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;4;-572.1758,765.1087;Float;False;Property;_EmissionAmount;Emission Amount;4;0;Create;True;0;0;False;0;0;20;0;1000;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;456.2286,-419.5258;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-22.61802,-57.98154;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;8;-457.6304,-293.0034;Float;False;Property;_MainColor;Main Color;0;0;Create;True;0;0;False;0;1,1,1,1;1,1,1,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-175.1532,410.4412;Float;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;379.4227,-99.42786;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;66;379.6475,-996.7899;Float;False;4;4;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT4;0,0,0,0;False;3;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TFHCGrayscale;77;-432.2574,-1290.945;Float;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;28;-1739.394,-317.9585;Float;True;Property;_TextureSample0;Texture Sample 0;9;0;Create;True;0;0;False;0;None;None;True;3;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCGrayscale;78;-453.0576,-1114.945;Float;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;585.8793,-303.7351;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;379.7338,93.0555;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCGrayscale;80;-446.6569,-714.9456;Float;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCGrayscale;79;-459.4573,-903.7457;Float;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;720.4403,-20.91668;Float;False;True;2;Float;;0;0;StandardSpecular;LowPolyPainterDetail;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;True;True;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Transparent;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;5;False;-1;10;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;13;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;30;0;29;0
WireConnection;38;0;37;0
WireConnection;24;0;25;0
WireConnection;34;0;33;0
WireConnection;39;0;37;0
WireConnection;39;3;38;0
WireConnection;75;0;82;2
WireConnection;75;1;72;0
WireConnection;75;2;82;2
WireConnection;75;3;82;2
WireConnection;75;4;73;0
WireConnection;74;0;82;1
WireConnection;74;1;72;0
WireConnection;74;2;82;1
WireConnection;74;3;82;1
WireConnection;74;4;73;0
WireConnection;31;0;29;0
WireConnection;31;3;30;0
WireConnection;26;0;25;0
WireConnection;26;3;24;0
WireConnection;35;0;33;0
WireConnection;35;3;34;0
WireConnection;76;0;82;3
WireConnection;76;1;72;0
WireConnection;76;2;82;3
WireConnection;76;3;82;3
WireConnection;76;4;73;0
WireConnection;71;0;82;4
WireConnection;71;1;81;0
WireConnection;71;2;82;4
WireConnection;71;3;82;4
WireConnection;71;4;73;0
WireConnection;27;0;26;0
WireConnection;27;1;74;0
WireConnection;36;0;35;0
WireConnection;36;1;76;0
WireConnection;32;0;31;0
WireConnection;32;1;75;0
WireConnection;40;0;39;0
WireConnection;40;1;71;0
WireConnection;91;0;27;0
WireConnection;91;1;83;0
WireConnection;91;2;88;0
WireConnection;84;0;40;0
WireConnection;84;1;83;0
WireConnection;84;2;85;0
WireConnection;89;0;36;0
WireConnection;89;1;83;0
WireConnection;89;2;86;0
WireConnection;90;0;32;0
WireConnection;90;1;83;0
WireConnection;90;2;87;0
WireConnection;1;0;23;0
WireConnection;22;0;91;0
WireConnection;22;1;90;0
WireConnection;22;2;89;0
WireConnection;22;3;84;0
WireConnection;18;0;22;0
WireConnection;18;1;1;0
WireConnection;9;0;2;0
WireConnection;9;1;2;4
WireConnection;5;0;3;0
WireConnection;5;1;4;0
WireConnection;5;2;3;4
WireConnection;11;0;8;0
WireConnection;11;1;9;0
WireConnection;66;0;27;0
WireConnection;66;1;32;0
WireConnection;66;2;36;0
WireConnection;66;3;40;0
WireConnection;77;0;26;0
WireConnection;28;0;23;0
WireConnection;78;0;31;0
WireConnection;10;0;8;0
WireConnection;10;1;18;0
WireConnection;12;0;8;0
WireConnection;12;1;5;0
WireConnection;80;0;39;0
WireConnection;79;0;35;0
WireConnection;0;0;10;0
WireConnection;0;2;12;0
WireConnection;0;3;11;0
WireConnection;0;4;2;4
WireConnection;0;9;1;4
ASEEND*/
//CHKSM=607AD2BA01A024DCADF0954E75E40162187D58F9