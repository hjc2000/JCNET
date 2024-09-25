using JCNET.字符串处理;

string s = "666777888";
CuttingMiddleResult result = s.CutMiddle("777");
Console.WriteLine(result);

result = s.CutMiddle("999");
Console.WriteLine(result);
