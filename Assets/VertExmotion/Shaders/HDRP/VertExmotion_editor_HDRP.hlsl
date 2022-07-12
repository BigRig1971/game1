#define NB_SENSORS 20


uniform float4 KVMEditor_SensorPosition[NB_SENSORS];
uniform float4 KVMEditor_RadiusCentripetalTorque[NB_SENSORS];
uniform float KVMEditor_Power[NB_SENSORS];
uniform float KVMEditor_Links[NB_SENSORS];

uniform float4 KVM_SensorPosition[NB_SENSORS];
uniform float4 KVM_MotionDirection[NB_SENSORS];
uniform float4 KVM_MotionAxis[NB_SENSORS];
uniform float4 KVM_RadiusCentripetalTorque[NB_SENSORS];
uniform float4 KVM_SquashStretch[NB_SENSORS];
uniform float4 KVM_Speed[NB_SENSORS];
uniform float4 KVM_AxisXScale[NB_SENSORS];
uniform float4 KVM_AxisYScale[NB_SENSORS];
uniform float4 KVM_AxisZScale[NB_SENSORS];
uniform float KVM_Link[NB_SENSORS];
float KVM_NormalCorrection;
float KVM_NormalSmooth;
float KVM_NbSensors;


sampler2D _MainTex;
int _SensorId;
int _LayerId;
float _gradientFactor = .5;


void Editor_float( float3 wrldPos, float4 vertexColor, float2 uv,  out float3 Color)
{
    if (_SensorId > -1)//id==-1 -> show paint info
    {

			
        Color = float3(0, 0, 0);
			
		//paint info	
			
        for (int i = 0; i < NB_SENSORS; ++i)
        {
            if (i < KVM_NbSensors)
            {
                _LayerId = KVMEditor_SensorPosition[i].w;
                float3 layerColor = float3(0, 0, 0);
                if (_LayerId == 0)
                    layerColor = float3(1, 1, 1) * max(max(vertexColor.r, vertexColor.g), vertexColor.b);
                if (_LayerId == 1)
                    layerColor = vertexColor.rrr;
                if (_LayerId == 2)
                    layerColor = vertexColor.ggg;
                if (_LayerId == 3)
                    layerColor = vertexColor.bbb;
					

                int idNext = clamp(i + 1, 0, NB_SENSORS - 1);

                float lrp = 0;

					//KVMEditor_Links[i] = 1;
					
                if (KVMEditor_Links[i] == 1)
                {
                    float3 v = KVMEditor_SensorPosition[i].xyz - KVMEditor_SensorPosition[idNext].xyz;
                    float3 nv = normalize(v);
                    float p0 = dot(nv, normalize(wrldPos.xyz - KVMEditor_SensorPosition[idNext].xyz));
                    float p1 = dot(nv, wrldPos.xyz - KVMEditor_SensorPosition[idNext].xyz);
                    lrp = p0 <= 0.0 ? 1.0 : (p1 >= length(v) ? 0.0 : 1.0 - (p1 / length(v)));
                }
					

					//---------------------------		
                float4 SensorPosition = lerp(KVMEditor_SensorPosition[i], KVMEditor_SensorPosition[idNext], lrp);
                float4 Power = lerp(KVMEditor_Power[i], KVMEditor_Power[idNext], lrp);
                float4 RadiusCentripetalTorque = lerp(KVMEditor_RadiusCentripetalTorque[i], KVMEditor_RadiusCentripetalTorque[idNext], lrp);

                float dist = distance(wrldPos.xyz, SensorPosition.xyz);

                if (dist < RadiusCentripetalTorque.x && KVMEditor_Links[i] >= 0)
                {
                    float f = 1 - dist / (RadiusCentripetalTorque.x + .0000001f);
                    f = pow(abs(f), Power);




//#ifdef VERTEXMOTION_GRADIENT_ON	
                    Color.rgb = max(float3(f, f, f) * layerColor, Color.rgb);
/*
#else						
						//o.color.rgb = lerp(float3(0, 1, 0), o.color.rgb,1-f);
						o.color.rgb = lerp(float3(0, 1, 0)*layerColor.x, o.color.rgb, 1 - f);
#endif							
	*/					



                }
            }
		
        }
			
    }
    else
    {
			//paint info
        if (_LayerId == 0)
            Color.rgb = float3(1, 1, 1) * max(max(vertexColor.r, vertexColor.g), vertexColor.b);
        if (_LayerId == 1)
            Color.rgb = vertexColor.rrr;
        if (_LayerId == 2)
            Color.rgb = vertexColor.ggg;
        if (_LayerId == 3)
            Color.rgb = vertexColor.bbb;

        //Color.a = 1;
    }
    
    
    
    //gradient
    float3 red = float3(1, 0, 0);
    float3 yellow = float3(1, 1, 0);
    float3 green = float3(0, 1, 0);
    float3 blue = float3(0, 0, 4);

    float f = Color.r;
    float3 c1;
    c1 = lerp(yellow, red, (f - .6) * 3.3333);
    c1 = f < .6 ? lerp(green, yellow, (f - .3) * 3.3333) : c1;
    c1 = f < .3 ? lerp(blue, green, f * 3.3333) : c1;


    
    float3 c2;
    c2 = tex2D(_MainTex, uv).rgb;
    c2.x = c2.y = c2.z = (c2.x + c2.y + c2.z) / 3;
    c2 = lerp(Color.rgb, c2, .4);
	

    Color =  lerp(c2, c1, _gradientFactor);
    
    
}
