Shader "SeeingSoundwaves/DistanceLerp" {
	
	//Fallback "Standard"
	
	Properties {
		_mainTexture("Texture", 2D) = "white" {}
		_range("Range", Float) = 200
		_dropoff("Drop off", Float) = 0.1
		_strength("Strength", Float) = 150.0	
     }
     
     SubShader {
		Tags { "Queue" = "Transparent" "RenderType"="Transparent" } 
        // draw after all opaque geometry has been drawn				
       
		Pass {
			 ZWrite Off // don't write to depth buffer 
             // in order not to occlude other objects
 
			Blend SrcAlpha OneMinusSrcAlpha // use alpha blending
 
			CGPROGRAM

			#pragma vertex vert 
			#pragma fragment frag			
          
			#include "UnityCG.cginc" //wat doet dit
          
			float _range;
			float _dropoff;
			float _strength; 
			sampler2D _mainTexture;
          
			struct vertIn {	
				 float4 vertexPosition : POSITION;				
				 fixed4 color : COLOR;
				 float4 texCoord : TEXCOORD0;
			};
          
			struct v2f{
				 float4 pos : SV_POSITION;				 
				 fixed4 color : COLOR;
				 float4 tex : TEXCOORD1;
			};
  
			v2f vert(vertIn i, appdata_full v) 
			{
				v2f o;

				o.tex = i.texCoord;             
				o.color = v.color;
				
				float dist = distance(mul(_Object2World, v.vertex), _WorldSpaceCameraPos) / _range;
				if(dist < _dropoff) {					
					o.color.r = 1.0f;
					o.color.g = 1.0f;
					o.color.b = 1.0f;
				}
				else if (dist > _dropoff) {
					o.color.r = (1.0 - dist) * (_strength * 0.01); 
					o.color.g = (1.0 - dist) * (_strength * 0.01);
					o.color.b = (1.0 - dist) * (_strength * 0.01);
				}
				
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);

				return o;
			}
              
			struct fragOut {
				fixed4 color : COLOR;
			};
              
			fragOut frag(v2f i)
			{
				fragOut o;
				float4 textureColor = tex2D(_mainTexture, i.tex.xy);
				o.color = textureColor * i.color; 
				
				return o;
			}
  
			ENDCG  
		}
		
		Blend OneMinusDstColor OneMinusSrcAlpha
        BlendOp Add

		Pass {
			//Zwrite On
			ColorMask 0
		}
		
     }
}