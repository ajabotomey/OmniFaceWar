﻿// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Heatmap"
{
    Properties
    {
		_HeatTex("Texture", 2D) = "gray" {}
    }
    SubShader
    {
		Tags {"Queue"="Transparent"}
		Blend SrcAlpha OneMinusSrcAlpha

		Pass {
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			struct vertInput {
				float4 pos : POSITION;
			};

			struct vertOutput {
				float4 pos : POSITION;
				fixed3 worldPos : TEXCOORD1;
			};

			vertOutput vert(vertInput input) {
				vertOutput o;
				o.pos = UnityObjectToClipPos(input.pos);
				o.worldPos = mul(unity_ObjectToWorld, input.pos).xyz;
				return o;
			}

			uniform int _Points_Length = 0;
			uniform float3 _Points[500];
			uniform float2 _Properties[500];
			sampler2D _HeatTex;

			half4 frag(vertOutput output) : COLOR{
				half h = 0;
				for (int i = 0; i < _Points_Length; i++) {
					// Calculate the contribution of each point
					half di = distance(output.worldPos, _Points[i].xyz);
					half ri = _Properties[i].x;
					half hi = 1 - saturate(di / ri);

					h += hi * _Properties[i].y;
				}

				// Converts (0-1) according to heat texture
				h = saturate(h);
				half4 color = tex2D(_HeatTex, fixed2(h, 0.5));
				return color;
			}

			ENDCG
		}
    }
    FallBack "Diffuse"
}
