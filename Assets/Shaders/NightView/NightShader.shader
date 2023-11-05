Shader "Unlit/Night Shader" {
 
	Properties {
		_MainTex ("Base (RGB)", RECT) = "white" {}
	}
 
	SubShader {
		Pass {
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
 
			CGPROGRAM
				#pragma vertex vert_img
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest 
				#include "UnityCG.cginc"
 
				// frag shaders data
				uniform sampler2D _MainTex;
				uniform float4 _Luminance;
				uniform float _LensRadius;
                uniform bool _IsTransitioning;
 
				// frag shader
                struct v2f {
                    float2 uv : TEXCOORD0;
                };
				float4 frag (v2f_img i) : COLOR
				{	
 
					float4 col = tex2D(_MainTex, i.uv);
 
                    //ADD NIGHT VIEW
					//obtain luminance value
					//col = dot(col, _Luminance);

					//add lens circle effect
					float dist = distance(i.uv, float2(0.5, 0.5));
					col *= smoothstep( _LensRadius,  _LensRadius - 0.3, dist);
 
					//add rb to the brightest pixels
                    if(_IsTransitioning == 1)
					    col.r = max (col.r - 0.45, 0) * 10;
 
					// return col pixel
                    
					return col;
				}
            //clip 0.25 of pixels
            /*
            struct v2f {
                float2 uv : TEXCOORD0;
            };
            fixed4 frag (v2f i, UNITY_VPOS_TYPE screenPos : VPOS) : SV_Target
            {
                // screenPos.xy will contain pixel integer coordinates.
                // use them to implement a checkerboard pattern that skips rendering
                // 4x4 blocks of pixels

                // checker value will be negative for 4x4 blocks of pixels
                // in a checkerboard pattern
                screenPos.xy = floor(screenPos.xy * 0.5) * 0.5;
                float checker = -frac(screenPos.r + screenPos.g);

                // clip HLSL instruction stops rendering a pixel if value is negative
                clip(checker);

                // for pixels that were kept, read the texture and output it
                fixed4 c = tex2D (_MainTex, i.uv);
                return c;
            }
            */
			ENDCG
 
		}
	}
 
	Fallback off
}