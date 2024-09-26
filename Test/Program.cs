using JCNET.字符串处理;

string str = @"666 777 分隔 666 777 分隔 666 777 666连起来777 666 777";

str = str.ReplaceTwoWord("666", "777", "hahaha");
Console.WriteLine(str);
