using JCNET;

StringBuilderLogWriter writer = new();
TextWriter origin_out = Console.Out;
Console.SetOut(writer);
Console.WriteLine("666666666666");
Console.SetOut(origin_out);

string writer_string = writer.ToString();
Console.WriteLine(writer_string);
