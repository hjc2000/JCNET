Uri uri1 = new("https://fanyi.baidu.com/p/");
Uri uri2 = new("https://fanyi.baidu.com/p");
Console.WriteLine(uri1.IsBaseOf(uri2));
Console.WriteLine(uri1 == uri2);