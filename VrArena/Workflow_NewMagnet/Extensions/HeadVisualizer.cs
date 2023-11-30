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
public class HeadVisualizer
{
    public IObservable<IplImage> Process(IObservable<Tuple<IplImage, Point2f, Point2f, Point2f>> source)
    {
        return source.Select(value => 
        { 
            var image = value.Item1.Clone();
            var centroid = new Point((int)value.Item2.X,(int)value.Item2.Y);
            var head = new Point((int)value.Item3.X,(int)value.Item3.Y);
            var stim = new Point((int)value.Item4.X,(int)value.Item4.Y);
            CV.Circle(image, centroid, 1, Scalar.Rgb(255, 0, 0), -1);
            CV.Circle(image, head, 1, Scalar.Rgb(255, 255, 0), -1);
            CV.Circle(image, stim, 1, Scalar.Rgb(0, 255, 0), -1);
            return image;
        });
    }
}
