using JCNET.容器;

List<int> list = [1, 2, 3, 4, 5, 6, 7, 8, 9];
await foreach (int item in list.AsIAsyncEnumerableEx())
{
	Console.WriteLine(item);
}