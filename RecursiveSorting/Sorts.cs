namespace RecursiveSorting
{
    public static class Sorts<T> where T : IComparable<T>
    {
        public static List<T> MergeSort(List<T> vals)
        {
            List<T> elements = [];
            elements.AddRange(vals);
            if (elements.Count <= 1) { return elements; }

            //divide
            List<T> firsts = new();
            List<T> lasts = new();
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
            
            //recur
            firsts = MergeSort(firsts);
            lasts = MergeSort(lasts);


            //incorporate
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
                if (firsts[i].CompareTo(lasts[j]) <= 0) //must be <= to ensure sort stability
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
            return elements;
        }
    }
}
