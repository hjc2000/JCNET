using JCNET.字符串处理;

string str = "666-中间-666";
ReadOnlyMemory<char> result = str.GetBetween("666", "777");
Console.WriteLine(result);
