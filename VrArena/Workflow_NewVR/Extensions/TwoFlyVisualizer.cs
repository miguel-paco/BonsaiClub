using Bonsai;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using OpenCV.Net;

[Combinator]
[Description("")]
[WorkflowElementCategory(ElementCategory.Transform)]
public class TwoFlyVisualizer
{
    public IObservable<IplImage> Process(IObservable<Tuple<IplImage, Tuple<Point2f, Point2f>, Tuple<Point2f, Point2f>>> source)
    {
        return source.Select(value => 
        { 
            var image = value.Item1.Clone();
            var fly1centroid = new Point((int)value.Item2.Item1.X,(int)value.Item2.Item1.Y);
            var fly1head = new Point((int)value.Item2.Item2.X,(int)value.Item2.Item2.Y);
            var fly2centroid = new Point((int)value.Item3.Item1.X,(int)value.Item3.Item1.Y);
            var fly2head = new Point((int)value.Item3.Item2.X,(int)value.Item3.Item2.Y);
            CV.Circle(image, fly1centroid, 1, Scalar.Rgb(255, 0, 0), -1);
            CV.Circle(image, fly1head, 1, Scalar.Rgb(255, 255, 0), -1);
            CV.Circle(image, fly2centroid, 1, Scalar.Rgb(0, 0, 255), -1);
            CV.Circle(image, fly2head, 1, Scalar.Rgb(0, 255, 255), -1);
            return image;
        });
    }
}


