using FlatSharp;
using FlatSharp.Attributes;
using FlatSharp.Internal;
using Generated;
using SharpFuzz;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Fuzzing
{
    public class Program
    {
        [FlatBufferTable]
        public class Owner
        {
            [FlatBufferItem(0)]
            public string Name { get; set; }
            [FlatBufferItem(1)]
            public byte[] Document { get; set; }
            //[FlatBufferItem(2)]
            //public decimal CostOfAllCatsInTotal { get; set; }
            [FlatBufferItem(3)]
            //public Dictionary<string, Cat> Cats { get; set; }
            public Cat[] Cats { get; set; }
        }

        [FlatBufferEnum(typeof(int))]
        public enum CatBreed
        {
            Ragdoll,
            SavannahCat,
            BritishShorthair,
            BengalCat,
            PersianCat,
            MaineCoon,
            MunchkinCat,
            NorwegianForestcCat,
            EuropeanShorthair
        }

        [FlatBufferTable]
        public class Cat
        {
            [FlatBufferItem(7)]
            public string UniqueIdentfier { get; set; }
            [FlatBufferItem(0)]
            public uint AgeInYears { get; set; }
            [FlatBufferItem(1)]
            public bool IsFluffy { get; set; }
            [FlatBufferItem(2)]
            public string Name { get; set; }
            //[FlatBufferItem(3)]
            //public DateTime? NextVaccinationDate { get; set; }
            [FlatBufferItem(4)]
            public CatBreed Breed { get; set; }
            [FlatBufferItem(5)]
            public double GramsOfFoodPerDay { get; set; }
            [FlatBufferItem(6)]
            public Cat[] Children { get; set; }
            [FlatBufferItem(8)]
            public Owner Owner { get; set; }// -> this causes StackOverflow
            //[FlatBufferItem(9)]
            //public char Group { get; set; }
            [FlatBufferItem(10)]
            public long Whatever { get; set; }
            //[FlatBufferItem(11)]
            //public DateTime DateOfBirth { get; set; }
            //[FlatBufferItem(12)]
            //public Tuple<CatBreed, CatBreed[]> CompatibleCats { get; set; }
        }

        [FlatBufferTable]
        public class CatsAndOwners
        {
            [FlatBufferItem(0)]
            public Owner[] Owners { get; set; }
            [FlatBufferItem(1)]
            public Cat[] Cats { get; set; }
        }

        public struct Buffer : IInputBuffer, IInputBuffer2
        {
            private readonly byte[] memory;

            public Buffer(byte[] buffer)
            {
                this.memory = buffer;
            }

            public bool IsReadOnly => false;

            public int Length => this.memory.Length;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public byte ReadByte(int offset)
            {
                return ScalarSpanReader.ReadByte(this.memory.AsSpan().Slice(offset));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public sbyte ReadSByte(int offset)
            {
                return ScalarSpanReader.ReadSByte(this.memory.AsSpan().Slice(offset));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ushort ReadUShort(int offset)
            {
                this.CheckAlignment(offset, sizeof(ushort));
                return ScalarSpanReader.ReadUShort(this.memory.AsSpan().Slice(offset));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public short ReadShort(int offset)
            {
                this.CheckAlignment(offset, sizeof(short));
                return ScalarSpanReader.ReadShort(this.memory.AsSpan().Slice(offset));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public uint ReadUInt(int offset)
            {
                this.CheckAlignment(offset, sizeof(uint));
                return ScalarSpanReader.ReadUInt(this.memory.AsSpan().Slice(offset));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public int ReadInt(int offset)
            {
                this.CheckAlignment(offset, sizeof(int));
                return ScalarSpanReader.ReadInt(this.memory.AsSpan().Slice(offset));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ulong ReadULong(int offset)
            {
                this.CheckAlignment(offset, sizeof(ulong));
                return ScalarSpanReader.ReadULong(this.memory.AsSpan().Slice(offset));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public long ReadLong(int offset)
            {
                this.CheckAlignment(offset, sizeof(long));
                return ScalarSpanReader.ReadLong(this.memory.AsSpan().Slice(offset));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public float ReadFloat(int offset)
            {
                this.CheckAlignment(offset, sizeof(float));
                return ScalarSpanReader.ReadFloat(this.memory.AsSpan().Slice(offset));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public double ReadDouble(int offset)
            {
                this.CheckAlignment(offset, sizeof(double));
                return ScalarSpanReader.ReadDouble(this.memory.AsSpan().Slice(offset));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public string ReadString(int offset, int byteLength, Encoding encoding)
            {
                return ScalarSpanReader.ReadString(this.memory.AsSpan().Slice(offset, byteLength), encoding);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Memory<byte> GetByteMemory(int start, int length)
            {
                return new Memory<byte>(this.memory, start, length);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ReadOnlyMemory<byte> GetReadOnlyByteMemory(int start, int length)
            {
                return this.GetByteMemory(start, length);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ReadOnlySpan<byte> GetReadOnlySpan()
            {
                return this.memory;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Span<byte> GetSpan()
            {
                return this.memory;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Memory<byte> GetMemory()
            {
                return this.memory;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ReadOnlyMemory<byte> GetReadOnlyMemory()
            {
                return this.memory;
            }

            public T InvokeParse<T>(IGeneratedSerializer<T> serializer, int offset)
            {
                return serializer.Parse(this, offset);
            }
        }

        static void Main(string[] args)
        {
            Fuzzer.Run(stream =>
            {
                try
                {
                    //GeneratedSerializer serializer = new();
                    //byte[] data = Encoding.UTF8.GetBytes(stream);

                    //IInputBuffer buffer = new Buffer(data);

                    //serializer.Parse(buffer, 0);

                    //Generate assembly
                    //var data = File.ReadAllBytes(@"C:\src\fuzzing\Fuzzing\TestCases\TestCases.txt");
                    //FlatBufferSerializer serializer = new();
                    //var field = serializer.GetType().GetField("serializerCache", BindingFlags.Instance | BindingFlags.NonPublic);
                    //var cache = (Dictionary<Type, object>)field.GetValue(serializer);
                    //var obj = serializer.Parse<Cat>(data);
                    //Console.WriteLine(obj.Name);
                    //var wrapper = (ISerializer<Cat>)cache.Values.First();
                    //File.WriteAllBytes(@"C:\src\fuzzing\Fuzzing\Generated\GeneratedFlatSharp.dll", wrapper.AssemblyBytes);


                    //Get initial test case
                    FlatBufferSerializer serializer = new();

                    Owner owner1 = new()
                    {
                        Name = "John",
                        Document = File.ReadAllBytes(@"C:\src\fuzzing\Fuzzing\CatStory.txt"),
                        //CostOfAllCatsInTotal = 105629.0000999888m
                    };

                    Owner owner2 = new()
                    {
                        Name = "Alice",
                        Document = File.ReadAllBytes(@"C:\src\fuzzing\Fuzzing\CatStory.txt"),
                        //CostOfAllCatsInTotal = 165629.0000929888m
                    };

                    Cat[] cats = new Cat[9];

                    for (int i = 0; i < 9; i++)
                    {
                        Cat c = new()
                        {
                            AgeInYears = (uint)i * 2 + 1,
                            IsFluffy = i % 2 == 0,
                            Name = "Pet" + i,
                            Breed = (CatBreed)i,
                            Whatever = DateTime.UtcNow.Ticks,
                            //NextVaccinationDate = DateTime.UtcNow.AddDays(i),
                            //DateOfBirth = DateTime.UtcNow.AddYears(-i * 2 + 1),
                            GramsOfFoodPerDay = 33.6 * i + 100,
                            //Group = (char)i,
                            UniqueIdentfier = "Cat_Id_" + i,
                            //Owner = i % 2 == 0 ? owner1 : owner2
                        };
                        cats[i] = c;
                    }

                    //var ragdollsFriends = new Tuple<CatBreed, CatBreed[]>(CatBreed.Ragdoll, new CatBreed[] { CatBreed.Ragdoll, CatBreed.EuropeanShorthair });
                    //var norwegianForestCatsFriends = new Tuple<CatBreed, CatBreed[]>(CatBreed.NorwegianForestcCat, new CatBreed[] { CatBreed.EuropeanShorthair, CatBreed.BritishShorthair, CatBreed.MunchkinCat });
                    //var europeanShorthairsFriends = new Tuple<CatBreed, CatBreed[]>(CatBreed.EuropeanShorthair, new CatBreed[] { CatBreed.BengalCat, CatBreed.EuropeanShorthair });
                    //var maineCoonCatsFriends = new Tuple<CatBreed, CatBreed[]>(CatBreed.MaineCoon, new CatBreed[] { CatBreed.PersianCat, CatBreed.BritishShorthair });

                    //cats[0].CompatibleCats = ragdollsFriends;
                    //cats[5].CompatibleCats = maineCoonCatsFriends;
                    //cats[7].CompatibleCats = norwegianForestCatsFriends;
                    //cats[8].CompatibleCats = europeanShorthairsFriends;

                    cats[1].Children = new Cat[] { cats[3] };
                    cats[2].Children = new Cat[] { cats[4], cats[8] };

                    owner1.Cats = new Cat[5];
                    for (int i = 0; i < 5; i++)
                    {
                        owner1.Cats[i] = cats[i];
                    }

                    owner2.Cats = new Cat[4];
                    int j = 0;
                    for (int i = 5; i < 9; i++)
                    {
                        owner2.Cats[j++] = cats[i];
                    }

                    var mainObject = new CatsAndOwners
                    {
                        Owners = new Owner[] { owner1, owner2 },
                        Cats = cats
                    };

                    var destination = new byte[serializer.GetMaxSize(mainObject)];
                    Console.WriteLine("Destination " + destination);

                    serializer.Serialize(mainObject, destination);
                    //File.WriteAllBytes(@"C:\src\fuzzing\Fuzzing\TestCases.txt", destination);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });
        }
    }
}
