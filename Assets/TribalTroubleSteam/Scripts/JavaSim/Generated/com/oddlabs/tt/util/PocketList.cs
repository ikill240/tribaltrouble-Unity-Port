// Translated from PocketList.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.util
{
    // Java: 9
    public class PocketList : java.lang.Object
    {
        public java.util.List[] pockets;
        public int min_list_index;
        public int max_list_index;
        public int size_f;
        // Java: 21
        public void add(int cost, object obj)
        {
            if (cost >= pockets.Length)
            {
                cost = pockets.Length - 1;
            }
            pockets[cost].add(obj);
            if (cost < min_list_index)
            {
                min_list_index = cost;
            }
            if (cost > max_list_index)
            {
                max_list_index = cost;
            }
            size_f++;
        }
        // Java: 32
        public object removeBest()
        {
            java.util.List current_pocket = pockets[min_list_index];
            while (current_pocket.isEmpty())
            {
                min_list_index++;
                current_pocket = pockets[min_list_index];
            }
            object node = current_pocket.removeLast();
            size_f--;
            return node;
        }
        // Java: 44
        public void clear()
        {
            for (int i = min_list_index; i <= max_list_index; i++)
            {
                pockets[i].clear();
            }
            reset();
        }
        // Java: 58
        public bool isEmpty()
        {
            return size_f == 0;
        }
        // Java: 62
        public int size()
        {
            return size_f;
        }
        // Java: 66
        public void reset()
        {
            min_list_index = pockets.Length;
            max_list_index = 0;
            size_f = 0;
        }
        public PocketList(int num_pockets) : base()
        {
            pockets = ((java.util.List[])java.util.stream.IntStream_.rangeClosed(0, num_pockets).mapToObj(new java.util.function.IntFunction_L((int unused) =>
            {
                return new java.util.ArrayList();
            })).toArray(new java.util.function.IntFunction_L((int a0) => { return new java.util.List[a0]; })));
            reset();
        }
    }
}
