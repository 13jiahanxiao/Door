Shader "Unlit/Shape"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_ShapeLineWidth("ShapeWidth",float) = 0.1
		_ShapeColor("ShapeColor",COLOR) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

	  Pass
			{

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag


				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					fixed4 col = tex2D(_MainTex, i.uv);
					return col;
				}
				ENDCG
			}

			Pass
			{
				ColorMask 0
				ZTest Off
				Stencil
				{
					Ref 1
					Comp Always
					Pass Replace
				}
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag


				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
				};

				struct v2f
				{
					float4 vertex : SV_POSITION;
				};


				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					return fixed4(1,1,1,1);
				}
				ENDCG
			}

			Pass
			{
				Stencil
				{
					Ref 0
					Comp Equal
					Pass Keep
				}
				ZWrite Off
				ZTest Off
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag


				#include "UnityCG.cginc"

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
				};


				float _ShapeLineWidth;
				fixed4 _ShapeColor;

				v2f vert(appdata_base v)
				{
					v2f o;
					v.vertex.xyz += v.normal * _ShapeLineWidth;
					o.vertex = UnityObjectToClipPos(v.vertex);
					return o;
				}

				[earlyDepthStencil]
				fixed4 frag(v2f i) : SV_Target
				{
					fixed4 col = _ShapeColor;
					return col;
				}
					ENDCG
				}
    }
}
