Shader "3DAutosports/Specular Red Chan (1 Dir Light)" {
Properties {
	_Shininess ("Shininess", Range (0.03, 0.8)) = 0.078125	//	_Shininess ("Shininess", Range (0.03, 1)) = 0.078125
	_MainTex ("Base (RGB)", 2D) = "white" {}	//	_MainTex ("Base (RGB) Emission (A)", 2D) = "white" {}
}
SubShader { 
	Tags { "RenderType"="Opaque" }
	LOD 250
	
CGPROGRAM
#pragma surface surf MobileBlinnPhong exclude_path:prepass nolightmap noforwardadd halfasview novertexlights

inline fixed4 LightingMobileBlinnPhong (SurfaceOutput s, fixed3 lightDir, fixed3 halfDir, fixed atten)
{
	fixed diff = max (0, dot (s.Normal, lightDir));
	fixed nh = max (0, dot (s.Normal, halfDir));
	fixed spec = pow (nh, s.Specular*128) * s.Gloss;
	
	fixed4 c;
	c.rgb = (s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * spec) * (atten*2);
	c.a = 0.0;
	return c;
}

sampler2D _MainTex;
half _Shininess;

struct Input {
	float2 uv_MainTex;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
	o.Albedo = tex.rgb;
	o.Gloss = tex.r;
	o.Specular = _Shininess;
}
ENDCG
}

FallBack "Mobile/VertexLit"
}
