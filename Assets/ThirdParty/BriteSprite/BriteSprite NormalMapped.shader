Shader "BriteSprite/Unity Sprite Normal Mapped"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		_BumpMap ("Normal Map", 2D) = "bump" {}
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting On
		ZWrite Off
		Fog { Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
		#pragma surface surf SpriteLambert alpha vertex:vert
		#pragma multi_compile DUMMY PIXELSNAP_ON
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _BumpMap;
		fixed4 _Color;

		struct Input
		{
			float3 localPos;
			float2 uv_MainTex;
			fixed4 color;
		};
		
		
		void vert (inout appdata_full v, out Input o)
		{
			#if defined(PIXELSNAP_ON) && !defined(SHADER_API_FLASH)
			v.vertex = UnityPixelSnap (v.vertex);
			#endif
			
			UNITY_INITIALIZE_OUTPUT(Input, o);
			
			//these are sort of hacky until unity gives us normal/tangent support on sprites
			v.normal = float3(0,0,-1);
			v.tangent = float4(1,0,0,-1);
			
			o.localPos = v.vertex.xyz;
			o.color = _Color;
		}

		half4 LightingSpriteLambert (SurfaceOutput s, half3 lightDir, half atten)
		{
			half NdotL = dot(s.Normal, lightDir);
			
			half4 c;
			c.rgb = s.Albedo * _LightColor0.rgb * NdotL * atten * 2;
			c.a = s.Alpha;
			
			return c;
		}
		
		inline fixed3 CustomUnpackNormal(fixed4 packednormal)
		{
			#if defined(SHADER_API_GLES) && defined(SHADER_API_MOBILE)
				return packednormal.xyz * 2 - 1;
			#else
				fixed3 normal;
				normal.xy = packednormal.wy * 2 - 1;
				normal.z = sqrt(1 - normal.x*normal.x - normal.y * normal.y);
				return normal;
			#endif
		}

		void surf (Input IN, inout SurfaceOutput o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			
			o.Normal = CustomUnpackNormal(tex2D(_BumpMap, IN.uv_MainTex) ) ;
		}
		ENDCG
	}

Fallback "Transparent/VertexLit"
}

