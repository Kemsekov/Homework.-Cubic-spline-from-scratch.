using System;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Spline;
public class MySpline
{
    private double[] _x;
    private double[] _y;
    private double[] _b;
    private double[] _c;
    private double[] _d;

    public MySpline(double[] x,double[] y){
        Sorting.Sort(x,y);
        _x = x;
        _y = y;
        var mat = DenseMatrix.Create(x.Length-2,x.Length-2,0);
        var vec = DenseVector.Create(x.Length-2,0);
        var b = new double[x.Length];
        var d = new double[x.Length];
        for(int i = 1;i<x.Length-1;i++){
            if(i-2>=0)
            mat[i-1,i-2]=h(i-1);
            mat[i-1,i-1]=2*(h(i-1)+h(i));

            if(i<mat.ColumnCount)
                mat[i-1,i]=h(i);
            vec[i-1]=3*((y[i+1]-y[i])/h(i)-(y[i]-y[i-1])/h(i-1));
        }
        
        //can be improved using tridiagonal matrix algorithm
        var c = mat.Solve(vec).Append(0).Prepend(0).ToArray();
        for(int i = 0;i<x.Length-1;i++){
            var c_next = (i+1)<x.Length ? c[i+1] : 0;
            b[i]=(y[i+1]-y[i])/h(i)-(c_next+2*c[i])*h(i)/3;
            d[i]=(c_next-c[i])/3*h(i);
        }
        _b = b;
        _c = c;
        _d = d;

    }
    double h(int i){
        return _x[i+1]-_x[i];
    }
    int LeftSegmentIndex(double t)
    {
        int num = Array.BinarySearch(_x, t);
        if (num < 0)
        {
            num = ~num - 1;
        }
        return Math.Min(Math.Max(num, 0), _x.Length - 2);
    }
    public double Interpolate(double t)
    {
        int num = LeftSegmentIndex(t);
        double num2 = t - _x[num];
        return _y[num] + num2 * (_b[num] + num2 * (_c[num] + num2 * _d[num]));
    }
}
