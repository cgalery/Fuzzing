using FlatSharp.Attributes;
using SharpFuzz;
using System;
using System.Text;

namespace Fuzzing
{
    public class Program
    {
        [FlatBufferTable]
        public class Cat
        {
            [FlatBufferItem(0)]
            public int AgeInYears { get; set; }
            [FlatBufferItem(1)]
            public bool IsFluffy { get; set; }
            [FlatBufferItem(2)]
            public string Name { get; set; }
        }

        static void Main(string[] args)
        {
            Fuzzer.Run(stream =>
            {
                try
                {
                    FlatSharp.FlatBufferSerializer serializer = new();

                    byte[] data = Encoding.UTF8.GetBytes(stream);
                    serializer.Parse<object>(data);

                    //Cat c = new Cat
                    //{
                    //    AgeInYears = 2,
                    //    IsFluffy = true,
                    //    Name = "Pet"
                    //};

                    //var destination = new byte[serializer.GetMaxSize(c)];
                    //serializer.Serialize(c, destination);
                    //File.WriteAllBytes(@"C:\src\fuzzing\Fuzzing\TestCases.txt", destination);
                }
                catch (Exception) { }
            });
        }
    }
}
