Shader "Unlit/AntiOclusion"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader
	{
		Pass {
			Material {
				Diffuse(1,1,1,1)
			}
			Lighting On
			SetTexture[_MainTex] {
				Combine Primary * Texture
			}
		}

		Pass{
			Color (1,0,1,1)
			Cull Front
		}
	}
}
