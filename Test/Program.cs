using JCNET.字符串处理;

UrlPath path1 = new("/p/");
UrlPath path2 = new("/p/p1/");

Console.WriteLine(path1);
Console.WriteLine(path2);
Console.WriteLine(path2 == path1);
Console.WriteLine(path2 < path1);
