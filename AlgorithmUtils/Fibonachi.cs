namespace AlgorithmUtils;

public class Fibonachi
{
    public int _fibNumber;

    public Fibonachi(int fibNumber)
    {
        _fibNumber = fibNumber;
    }

    public static int GetByForLoop()
    {
        int pre1 = 1;
        int pre2 = 1;
        int result = 0;

        for (int i = 3; i <= 20; i++)
        {
            result = pre1 + pre2;
            pre2 = pre1;
            pre1 = result;
            Console.WriteLine($"result in {i} iteration is {result} \n pre1 {pre1} \n pre2 {pre2} ");
        }

        return result;
    }

    public static int GetByListWhileLoop()
    {
        var fib = new List<int> { 1, 1 };

        while (fib.Count < 20)
        {
            var pre = fib[^1];
            var next = fib[^2];

            fib.Add(pre + next);
        }

        Console.WriteLine(fib[^1]);
        return fib[^1];
    }
}
