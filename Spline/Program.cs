using Spline;

var inputs =
    File.ReadAllLines("inputs.txt")
    .Select(
        line => line.Split(' ')
        .Select(x => double.Parse(x))
        .ToArray()
    ).ToArray();
var x = inputs.Select(x => x[0]).ToArray();
var y = inputs.Select(x => x[1]).ToArray();

var spline = new MySpline(x,y);
var plt = new ScottPlot.Plot(1000, 1000);
plt.SetAxisLimitsX(0.11, 0.145);
plt.SetAxisLimitsY(6.5, 9);
plt.AddFunction((double x) => spline.Interpolate(x), lineWidth: 3);
foreach (var d in inputs)
    plt.AddLine(d[0], d[1], d[0], d[1], color: System.Drawing.Color.Red, lineWidth: 7);

plt.SaveFig("plot.jpg");