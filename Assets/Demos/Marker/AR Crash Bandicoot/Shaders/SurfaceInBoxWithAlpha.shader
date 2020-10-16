Shader "TNTC/SurfaceInBoxWithAlpha"{

	Properties{
		_Color("Tint", Color) = (0, 0, 0, 1)
		_MainTex("Texture", 2D) = "white" {}
		_Smoothness("Smoothness", Range(0, 1)) = 0
		_Metallic("Metalness", Range(0, 1)) = 0
		[HDR] _Emission("Emission", color) = (0,0,0)
	}

		SubShader{
			Tags{
				"RenderType" = "Transparent"
				"Queue" = "Transparent"
			}

			CGPROGRAM

			#pragma vertex vert
			#pragma surface surf Standard addshadow alpha
			#pragma target 3.0

			sampler2D _MainTex;
			fixed4 _Color;

			half _Smoothness;
			half _Metallic;
			half3 _Emission;
			float4 _Center;
			float4 _Dimensions;
			float4x4 _RotationMatrix;

			struct Input {
				float3 worldPos;
				float2 uv_MainTex;
			};

			void vert(inout appdata_full v, out Input o) {
				UNITY_INITIALIZE_OUTPUT(Input, o);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
			}

			float isInside(float3 p, float3 center, float3 dimensions) {

				float3 pos = mul(_RotationMatrix, p - center) + center;
				float3 minExt = center - dimensions;
				float3 maxExt = center + dimensions;

				if (pos.x < (maxExt.x) && pos.x >(minExt.x) && pos.y < (maxExt.y) && pos.y >(minExt.y) && pos.z < (maxExt.z) && pos.z >(minExt.z)) {
					return 1;
				}
	else {
	   return 0;
   }

}


void surf(Input i, inout SurfaceOutputStandard o) {
	fixed4 col = tex2D(_MainTex, i.uv_MainTex);
	col *= _Color;
	o.Albedo = col.rgb;
	o.Metallic = _Metallic;
	o.Smoothness = _Smoothness;
	o.Emission = _Emission;
	o.Alpha = col.a;

	if (!isInside(i.worldPos, _Center, _Dimensions / 2)) {
		discard;
	}

}
ENDCG
		}

}
