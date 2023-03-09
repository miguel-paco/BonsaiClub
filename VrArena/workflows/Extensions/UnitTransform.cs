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
    public IObservable<Tuple<double, double>> Process(IObservable<Tuple<double, double, float[][]>> source)
    {

        return source.Select(value => Miguel(value));
    }

    Tuple<double, double> Miguel(Tuple<double, double, float[][]> inputs)
    {
        double a = inputs.Item3[0][0];
        double b = inputs.Item3[0][1];
        double c = inputs.Item3[0][2];
        double d = inputs.Item3[1][0];
        double e = inputs.Item3[1][1];
        double f = inputs.Item3[1][2];
        double g = inputs.Item3[2][0];
        double h = inputs.Item3[2][1];
        double i = inputs.Item3[2][2];

        double x = inputs.Item1;
        double y = inputs.Item2;

        x = (a*x + b*y + c) / (g*x + h*y + i);
        y = (d*x + e*y + f) / (g*x + h*y + i);

        return new Tuple<double, double>(x ,y);

    }

}
