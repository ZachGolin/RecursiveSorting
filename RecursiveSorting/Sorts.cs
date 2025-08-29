using System.Windows.Markup;

namespace RecursiveSorting
{
    public static class Sorts<T>
    {
        public static List<T?> MergeSort(List<T?> vals, Comparison<T?> Order)
        {
            List<T?> elements = [];
            elements.AddRange(vals); //doing this to avoid referencing input list - causes the sort to be destructive otherwise
            if (elements.Count <= 1) { return elements; } //in this case, elements are already sorted as there is nothing else to sort with

            //divide
            List<T?> firsts = new();
            List<T?> lasts = new();
            int split = (elements.Count / 2);
            for (int l = 0; l < elements.Count; l++)
            {
                if (l < split)
                {
                    firsts.Add(elements[l]);
                }
                else
                { 
                    lasts.Add(elements[l]);
                }
            }
            
            //recur - assume firsts and lasts are now ordered
            firsts = MergeSort(firsts, Order);
            lasts = MergeSort(lasts, Order);


            //incorporate - interleave lists, maintaining sort order
            int i = 0;
            int j = 0;
            for (int k = 0; k < elements.Count; k++)
            {
                if (i >= firsts.Count)
                {
                    elements[k] = lasts[j];
                    j++;
                    continue;
                }
                else if (j >= lasts.Count)
                {
                    elements[k] = firsts[i];
                    i++;
                    continue;
                }
                if (Order(firsts[i], lasts[j]) <= 0) //must be <= to maintain stability
                {
                    elements[k] = firsts[i];
                    i++;
                }
                else
                {
                    elements[k] = lasts[j];
                    j++;
                }
            }
            return elements; //list is now ordered - return
        }



        public static bool QuickSort(List<T?> vals, Comparison<T?> Order) //convention: returns whether or not any sorting occurred this call
        {
            if (vals.Count <= 1) { return false; }
            return QuickSort(vals, Order, 0, vals.Count - 1);
        }

        private static bool QuickSort(List<T?> vals, Comparison<T?> Order, int left, int right)
        {
            if (left >= right) { return false; }
            T? pivot = vals[left];
            int L = left - 1;
            int R = right + 1;

            //only add if absolutely necessary
            //int i = left + 1;
            //while (pivot is null && i < right)
            //{
            //    pivot = vals[i];
            //    i++;
            //}

            while (true)
            {
                do
                {
                    L++;
                } while (Order(vals[L], pivot) < 0);

                do
                {
                    R--;
                } while (Order(vals[R], pivot) > 0);

                if (L >= R)
                { 
                    break;
                }

                T? temp = vals[L];
                vals[L] = vals[R];
                vals[R] = temp;
            }

            QuickSort(vals, Order, left, R); //sort preceding section
            QuickSort(vals, Order, R + 1, right); //sort following section

            return true;
        }
    }
}
