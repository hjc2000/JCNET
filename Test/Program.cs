using JCNET.字符串处理;

string s = "666.777.888";
CuttingMiddleResult result = s.CutMiddleWholeMatch("777");
Console.WriteLine(result);

result = s.CutMiddle("999");
Console.WriteLine(result);
