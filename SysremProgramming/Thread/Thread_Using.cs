using System;
using System.Threading;
 

class Program
{
    static void Main ()
    {
        int[] numbers = { 4, -2, 9, 5, -7, 3 };

        ThreadState arifmetic1 = new ThreadState();
        arifmetic1.Array=numbers;
        arifmetic1.StartIndex=0;
        arifmetic1.EndIndex=3;

        Thread t1 = new Thread(ProcessData);
        t1.Start(arifmetic1);

        ThreadState arifmetic2= new ThreadState();
        arifmetic2.Array=numbers;
        arifmetic2.StartIndex=3;
        arifmetic2.EndIndex=numbers.Length;

        Thread t2 = new Thread(ProcessData);
        t2.Start(arifmetic2);

        t1.Join();
        t2.Join();

        Console.WriteLine(string.Join(",",numbers));
    }

    static void ProcessData(object obj)
    {
        var state=(ThreadState)obj;

        for (int i = state.StartIndex; i < state.EndIndex; i++)
        {
            if(state.Array[i]>0)
            {
                state.Array[i] = state.Array[i] * state.Array[i];
            }
        }
    }

    class ThreadState
    {
        public int[] Array { get;set; }
        public int StartIndex {get;set;}
        public int EndIndex {get;set;}

    }
}