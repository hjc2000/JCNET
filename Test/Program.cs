using JCNET.字符串处理;

string str = @"aaa 666 777 bbb 666a777";

str = str.ReplaceTwoWord("666", "777", "hahaha");
Console.WriteLine(str);
