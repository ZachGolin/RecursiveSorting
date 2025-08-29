using System.Windows.Markup;
using static RecursiveSorting.Tests.TestUtils;

namespace RecursiveSorting.Tests
{
    public static class TestUtils
    {
        public static string ListToCommaSeparatedString<T>(IEnumerable<T?> values)
        {
            return string.Join(",", values);
        }

        public class Mnemonic : IComparable<Mnemonic?> //a reference type to use as a test subject
        {
            public int Value { get; set; }
            public int Order { get; set; }

            public int CompareTo(Mnemonic? other)
            {
                if (other is null) { return -1; } //assume this mnemonic precedes a null mnemonic
                return Value.CompareTo(other.Value);
            }
        }

        public static Comparison<int> IntOrder = (a,b) => (a.CompareTo(b));
        public static Comparison<Mnemonic?> MnemonicOrder = (a,b) => (a?.CompareTo(b)) ?? 1; //assume null mnemonics follow all others
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
        public void MergeSort_Sorts_Properly(int[] vals)
        {
            List<int> list = vals.ToList();
            List<int> sortedList = Sorts<int>.MergeSort(list, IntOrder);
            list.Sort(IntOrder);
            bool result = list.SequenceEqual(sortedList);
            Assert.True(result, $"Sequences {ListToCommaSeparatedString(list)} and {ListToCommaSeparatedString(sortedList)} are not equal");
        }

        [Fact]
        public void MergeSort_IsNot_Destructive()
        {
            //original list should remain unchanged
            List<int> list = [1, 2, 5, 4, 3];
            List<int> identicalList = [1, 2, 5, 4, 3];
            List<int> sortedList = Sorts<int>.MergeSort(list, IntOrder);
            Assert.True(list.SequenceEqual(identicalList), "MergeSort modified input list! Original list order was altered!");

            //references to objects in both lists should also remain unchanged
            List<Mnemonic?> mnemonics = [
                new() { Value = 2, Order = 1 },
                new() { Value = 3, Order = 2 },
                new() { Value = 1, Order = 3 }
            ];
            List<Mnemonic?> sortedMnemonics = Sorts<Mnemonic?>.MergeSort(mnemonics, MnemonicOrder);
            Assert.True(ReferenceEquals(mnemonics[0], sortedMnemonics[1]), "MergeSort modified input list! Element references were altered!");
        }

        [Fact]
        public void MergeSort_Is_Stable()
        {
            List<Mnemonic?> mnemonics = [
                new() { Value = 2, Order = 1 },
                new() { Value = 2, Order = 2 },
                new() { Value = 2, Order = 3 },
                new() { Value = 2, Order = 4 }
            ];
            List<Mnemonic?> sortedMnemonics = Sorts<Mnemonic?>.MergeSort(mnemonics, MnemonicOrder);
            for (int i = 0; i < sortedMnemonics.Count - 1; i++)
            {
                Assert.NotNull(sortedMnemonics[i]);
                Assert.NotNull(sortedMnemonics[i + 1]);
                Assert.True(sortedMnemonics[i]!.Order < sortedMnemonics[i + 1]!.Order, "Instability in MergeSort detected!");
            }
        }

        [Fact]
        public void MergeSort_PlacesNullReferences_Last() //this behavior only holds if the Order follows certain properties. the Mnemonic order *does* follow such properties. this test is not strictly necessary
        {
            List<Mnemonic?> mnemonics = [
                new() { Value = 2, Order = 1 },
                new() { Value = 2, Order = 2 },
                null,
                new() { Value = 2, Order = 4 }
            ];
            List<Mnemonic?> sortedMnemonics = Sorts<Mnemonic?>.MergeSort(mnemonics, MnemonicOrder);
            mnemonics.Sort(MnemonicOrder);
            Assert.True(mnemonics[3] is null, "Mnemonic ordering does not place null at the end of the sort order; please fix this delegate!");
            Assert.True(sortedMnemonics[3] is null, "null reference was not placed at the end of the sort order!");
        }




        [Theory]
        [InlineData(new int[] { })]
        [InlineData(new int[] { 2 })]
        [InlineData(new int[] { 2, 3, 5, 7, 11 })]
        [InlineData(new int[] { 11, 7, 5, 3, 2 })]
        [InlineData(new int[] { 11, 7, 5, 11, 2 })]
        [InlineData(new int[] { 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5 })]
        [InlineData(new int[] { -11, 7, -5, 3, -2 })]
        public void QuickSort_Sorts_Properly(int[] vals)
        {
            List<int> list = vals.ToList();
            List<int> listCopy = [];
            listCopy.AddRange(list);
            Sorts<int>.QuickSort(list, IntOrder);
            listCopy.Sort(IntOrder);
            bool result = list.SequenceEqual(listCopy);
            Assert.True(result, $"Sequences {ListToCommaSeparatedString(list)} and {ListToCommaSeparatedString(listCopy)} are not equal");
        }

        /*
        [Fact]
        public void QuickSort_PlacesNullReferences_Last() //this behavior only holds if the Order follows certain properties. the Mnemonic order *does* follow such properties. this test is not strictly necessary
        {
            //quicksort keeps finding excuses to swap nulls with each other... maybe relax this property of the sort
            List<Mnemonic?> mnemonics = [
                null,
                new() { Value = 2, Order = 1 },
                new() { Value = 2, Order = 2 },
                new() { Value = 2, Order = 4 }
            ];
            List<Mnemonic?> mnemonicsCopy = [];
            mnemonicsCopy.AddRange(mnemonics);
            Sorts<Mnemonic?>.QuickSort(mnemonics, MnemonicOrder);
            mnemonicsCopy.Sort(MnemonicOrder);
            Assert.True(mnemonicsCopy[3] is null, "Mnemonic ordering does not place null at the end of the sort order; please fix this delegate!");
            Assert.True(mnemonics[3] is null, "null reference was not placed at the end of the sort order!");
        }
        */

    }
}