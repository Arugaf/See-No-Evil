
#include "ClassicNoise3D.hlsl" 

#define ITER_COUNT 16

void SimpleRaymarch_float(float fogScale, float3 start, float3 end, out float Out)
{
    Out = 0;
    float3 delta = (end - start) / float(ITER_COUNT - 1);
    float magnitude = length(delta);
    for (int i = 0; i < ITER_COUNT; i++)
    {
        Out += (0.3 + cnoise(start * fogScale) * 0.7) * magnitude;
        start += delta;
    }
}

// END JIMMY'S MODIFICATIONS
