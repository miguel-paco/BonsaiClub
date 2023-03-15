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
    public IObservable<Tuple<float, float>> Process(IObservable<Tuple<float[][], Tuple<float, float>>> source)
    {

        return source.Select(value => Miguel(value));
    }

    Tuple<float, float> Miguel(Tuple<float[][], Tuple<float, float>> inputs)
    {
        float a = inputs.Item1[0][0];
        float b = inputs.Item1[0][1];
        float c = inputs.Item1[0][2];
        float d = inputs.Item1[1][0];
        float e = inputs.Item1[1][1];
        float f = inputs.Item1[1][2];
        float g = inputs.Item1[2][0];
        float h = inputs.Item1[2][1];
        float i = inputs.Item1[2][2];

        float x = inputs.Item2.Item1;
        float y = inputs.Item2.Item2;

        float X = (a*x + b*y + c) / (g*x + h*y + i);
        float Y = (d*x + e*y + f) / (g*x + h*y + i);

        return new Tuple<float, float>(X, Y);

    }

}
