using Bonsai;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

[Combinator]
[Description("")]
[WorkflowElementCategory(ElementCategory.Transform)]
public class UnitTransform
{
    public IObservable<Tuple<float, float>> Process(IObservable<Tuple<Tuple<float, float>,float[][]>> source)
    {
        return source.Select(value => Convert(value));
    }

    Tuple<float, float> Convert(Tuple<Tuple<float, float>,float[][]> inputs)
    {
        float x = inputs.Item1.Item1;
        float y = inputs.Item1.Item2;

        float a = inputs.Item2[0][0];
        float b = inputs.Item2[0][1];
        float c = inputs.Item2[0][2];
        float d = inputs.Item2[1][0];
        float e = inputs.Item2[1][1];
        float f = inputs.Item2[1][2];
        float g = inputs.Item2[2][0];
        float h = inputs.Item2[2][1];
        float i = inputs.Item2[2][2];

        float X = (a*x + b*y + c) / (g*x + h*y + i);
        float Y = (d*x + e*y + f) / (g*x + h*y + i);

        return new Tuple<float, float>(X, Y);

    }
}
