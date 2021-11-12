Shader "Unlit/test"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Color("Color", Color) = (.25, .5, .5, 1)
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float3 normal : TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

			sampler2D _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;

			//float4 LitPassFragment(v2f input) : SV_TARGET
			//{
			//	UNITY_SETUP_INSTANCE_ID(input);
			//	input.normal = normalize(input.normal);
			//	
			//	
			//	//float3 albedo = UNITY_ACCESS_INSTANCED_PROP(PerInstance, _Color).rgb;
			//
			//	float3 color = input.normal;
			//	return float4(color, 1);
			//}

			v2f vert(appdata v)
			{
				v2f o;
				o.normal = mul((float3x3)UNITY_MATRIX_M, v.normal);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				i.normal = normalize(i.normal);

				//float3 albedo = UNITY_ACCESS_INSTANCED_PROP(PerInstance, _Color).rgb;

				float3 color = i.normal;
				return float4(color, 1);

				//// sample the texture
				//fixed4 col = tex2D(_MainTex, i.uv);
				//// apply fog
				//UNITY_APPLY_FOG(i.fogCoord, col);
				//return col;
			}
		ENDCG
		}
	}
}
