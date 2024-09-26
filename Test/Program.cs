using JCNET.字符串处理;

string s = "666.888.666777";
s = s.ReplaceWholeMatch("666", "aaa");
Console.WriteLine(s);
