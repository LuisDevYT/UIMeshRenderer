// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "UI/UIMeshRenderer"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
		_LightDirection ("Light Direction", Vector) = (0, -1, 0, 0)
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

		Cull off
        Lighting Off
        ZWrite On
        ZTest LEqual
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
				float3 normal   : NORMAL;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                half2  texcoord : TEXCOORD0;
				half3 worldNormal : NORMAL;
            };

            fixed4 _Color;
            fixed4 _TextureSampleAdd;
			half3 _LightDirection;
            float3 _LocalPosition;
            float3 _Scale;

            v2f vert(appdata_t IN)
            {
                v2f OUT;

                IN.vertex.xyz -= _LocalPosition;
                IN.vertex.xyz *= _Scale;
                IN.vertex.xyz += _LocalPosition;

                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color    = IN.color * _Color;
				OUT.worldNormal = normalize(IN.normal);

                return OUT;
            }

            sampler2D _MainTex;

            fixed4 frag(v2f IN) : SV_Target
            {
				fixed diffuse = saturate(dot(IN.worldNormal, _LightDirection));
				//return fixed4(diffuse, diffuse, diffuse, 1); 
				fixed4 texcol = tex2D(_MainTex, IN.texcoord);
				texcol.rgb *= diffuse + 0.5; // add some ambient light
                return (texcol + _TextureSampleAdd) * IN.color;
            }
        ENDCG
        }
    }
}
