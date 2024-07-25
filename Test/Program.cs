using JCNET.字符串处理;

RoutePath path1 = new(new Uri("https://fanyi.baidu.com/"));
RoutePath path2 = new(new Uri("https://fanyi.baidu.com/p"));

Console.WriteLine(path1);
Console.WriteLine(path2);
Console.WriteLine(path2 == path1);
Console.WriteLine(path2 < path1);
