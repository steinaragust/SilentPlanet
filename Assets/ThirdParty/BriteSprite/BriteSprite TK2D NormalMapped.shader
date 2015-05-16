Shader "BriteSprite/TK2D Blend Vertex Color" 
{
Properties {
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_BumpMap ("Normalmap", 2D) = "bump" {}
}

SubShader {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	LOD 100
	Blend SrcAlpha OneMinusSrcAlpha
	CGPROGRAM
	#pragma surface surf Lambert alpha
	
	sampler2D _MainTex;
	sampler2D _BumpMap;
	
	struct Input {
		float2 uv_MainTex;
		float2 uv_BumpMap;
	};
	
	void surf (Input IN, inout SurfaceOutput o) 
	{
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
		o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
		o.Albedo = c.rgb;
		o.Alpha = c.a;
	}
	ENDCG
}

Fallback "Transparent/Diffuse"
}
