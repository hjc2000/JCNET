using JCNET.字符串处理;

string lua = @"require(""Detector.AccelerationDetector"")";
Console.WriteLine(lua.Cut(@"require(""", @""")"));
