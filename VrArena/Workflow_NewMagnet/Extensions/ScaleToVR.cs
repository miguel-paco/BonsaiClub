using Bonsai;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

[Combinator]
[Description("")]
[WorkflowElementCategory(ElementCategory.Transform)]
public class ScaleToVR
{
    public IObservable<float> Process(IObservable<Tuple<float, float[][]>> source)
    {
        return source.Select(value => Scale(value));
    }

    float Scale(Tuple<float, float[][]> inputs)
    {
        float a = inputs.Item2[0][0];
        float b = inputs.Item2[0][1];
        float c = inputs.Item2[0][2];
        float d = inputs.Item2[1][0]; 
        float e = inputs.Item2[1][1];
        float f = inputs.Item2[1][2];
        float g = inputs.Item2[2][0];
        float h = inputs.Item2[2][1];
        float i = inputs.Item2[2][2];

        float scaleX = (a*1 + b*0 + c) / (g*1 + h*0 + i);
        float scaleY = (d*0 + e*1 + f) / (g*0 + h*1 + i);

        return  Math.Abs((scaleX+scaleY)/2) * inputs.Item1;
    }
}
