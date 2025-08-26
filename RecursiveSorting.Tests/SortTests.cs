using System.Windows.Markup;

namespace RecursiveSorting.Tests
{
    public static class TestUtils
    {
        public static string ListToCommaSeparatedString<T>(IEnumerable<T?> values)
        {
            return string.Join(",", values);
        }

        public class Mnemonic : IComparable<Mnemonic>
        {
            public int Value { get; set; }
            public int Order { get; set; }

            public int CompareTo(Mnemonic? other)
            {
                return Value.CompareTo(other?.Value);
            }
        }
    }



    public class SortsTests
    {
        [Theory]
        [InlineData(new int[] { })]
        [InlineData(new int[] { 2 })]
        [InlineData(new int[] { 2, 3, 5, 7, 11 })]
        [InlineData(new int[] { 11, 7, 5, 3, 2 })]
        [InlineData(new int[] { 11, 7, 5, 11, 2 })]
        [InlineData(new int[] { 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5 })]
        [InlineData(new int[] { -11, 7, -5, 3, -2 })]
        public void MergeSort_Sorts_Properly<T>(T[] vals) where T : IComparable<T>
        {
            List<T> list = vals.ToList();
            List<T> sortedList = Sorts<T>.MergeSort(list);
            list.Sort();
            bool result = list.SequenceEqual(sortedList);
            Assert.True(result, $"Sequences {TestUtils.ListToCommaSeparatedString(list)} and {TestUtils.ListToCommaSeparatedString(sortedList)} are not equal");
        }

        [Fact]
        public void MergeSort_IsNot_Destructive()
        {
            List<int> list = [1, 2, 5, 4, 3];
            List<int> identicalList = [1, 2, 5, 4, 3];
            List<int> sortedList = Sorts<int>.MergeSort(list);
            Assert.True(list.SequenceEqual(identicalList), "MergeSort modified input list!");
        }

        [Fact]
        public void MergeSort_Is_Stable()
        { 
            List<TestUtils.Mnemonic> mnemonics = new();
            mnemonics.Add(new() { Value = 2, Order = 1 });
            mnemonics.Add(new() { Value = 2, Order = 2 });
            mnemonics.Add(new() { Value = 2, Order = 3 });
            mnemonics.Add(new() { Value = 2, Order = 4 });
            List<TestUtils.Mnemonic> sortedMnemonics = Sorts<TestUtils.Mnemonic>.MergeSort(mnemonics);
            for (int i = 0; i < sortedMnemonics.Count - 1; i++)
            {
                Assert.True(sortedMnemonics[i].Order < sortedMnemonics[i + 1].Order, "Instability in MergeSort detected!");
            }
        }
    }
}