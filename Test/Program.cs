using JCNET.字符串处理;

string str = "function Servo.SetAbsolutePositionAndRun()";
str = str.ReplaceTwoWord("function", "Servo.SetAbsolutePositionAndRun", "G[0] = function");
Console.WriteLine(str);
Console.WriteLine('0'.IsWordSeperation());
