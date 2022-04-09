using System;
using System.Collections.Generic;

namespace simulationStarter
{
    public interface IContainer<T>
    {
        void MakeEmpty();  // Reset an instance to empty
        bool Empty();      // Test if an instance is empty
        int Size();        // Return the number of items in an instance
    }

    //-----------------------------------------------------------------------------

    public interface IPriorityQueue<T> : IContainer<T> where T : IComparable
    {
        void Add(T item);  // Add an item to a priority queue
        void Remove();     // Remove the item with the highest priority
        T Front();         // Return the item with the highest priority
    }

    //-------------------------------------------------------------------------

    // Priority Queue
    // Implementation:  Binary heap

    public class PriorityQueue<T> : IPriorityQueue<T> where T : IComparable
    {
        private int capacity;  // Maximum number of items in a priority queue
        private T[] A;         // Array of items
        private int count;     // Number of items in a priority queue

        // Constructor 1
        // Create a priority with maximum capacity of size
        // Time complexity:  O(1)

        public int Count
        {
            get { return count; }
        }

        public PriorityQueue(int size)
        {
            capacity = size;
            A = new T[size + 1];  // Indexing begins at 1
            count = 0;
        }

        // PercolateUp
        // Percolate up an item from position i in a priority queue
        // Time complexity:  O(log n)

        private void PercolateUp(int i)
        {
            int child = i, parent;

            // As long as child is not the root (i.e. has a parent)
            while (child > 1)
            {
                parent = child / 2;
                if (A[child].CompareTo(A[parent]) > 0)
                // If child has a higher priority than parent
                {
                    // Swap parent and child
                    T item = A[child];
                    A[child] = A[parent];
                    A[parent] = item;
                    child = parent;  // Move up child index to parent index
                }
                else
                    // Item is in its proper position
                    return;
            }
        }

        // Add
        // Add an item into the priority queue
        // Time complexity:  O(log n)

        public void Add(T item)
        {
            if (count < capacity)
            {
                A[++count] = item;  // Place item at the next available position
                PercolateUp(count);
            }
        }

        // PercolateDown
        // Percolate down from position i in a priority queue
        // Time complexity:  O(log n)

        private void PercolateDown(int i)
        {
            int parent = i, child;

            // while parent has at least one child
            while (2 * parent <= count)
            {
                // Select the child with the highest priority
                child = 2 * parent;    // Left child index
                if (child < count)  // Right child also exists
                    if (A[child + 1].CompareTo(A[child]) > 0)
                        // Right child has a higher priority than left child
                        child++;

                // If child has a higher priority than parent
                if (A[child].CompareTo(A[parent]) > 0)
                {
                    // Swap parent and child
                    T item = A[child];
                    A[child] = A[parent];
                    A[parent] = item;
                    parent = child;  // Move down parent index to child index
                }
                else
                    // Item is in its proper place
                    return;
            }
        }

        // Remove
        // Remove an item from the priority queue
        // Time complexity:  O(log n)

        public void Remove()
        {
            if (!Empty())
            {
                // Remove item with highest priority (root) and
                // replace it with the last item
                A[1] = A[count--];

                // Percolate down the new root item
                PercolateDown(1);
            }
        }

        // Remove
        // Remove the item at position i in the priority queue
        // Time complexity:  O(log n)

        public void RemoveAt(int i)
        {
            if (i >= 1 && i <= count)  // if i is valid
            {
                A[i] = A[count];      //replace the element at position i in array with the last child
                count--;              //reduce count 
                PercolateUp(i);       //tries both percolating up and down. If it is in the correct position, then both of these methods will do nothing. If it need repositioning in the heap, then either the percolate up 
                                      //OR percolate down method will work.   It cannot satisfy both conditions.
                PercolateDown(i);
            }
        }

        // Peek
        // Returns the item at position i in the priority queue
        // Time complexity:  O(1)

        public T Peek(int i)
        {
            return A[i]; //return element at index i in priority queue array
        }

        // Front
        // Return the item with the highest priority
        // Time complexity:  O(1)

        public T Front()
        {
            if (!Empty())
            {
                return A[1];  // Return the root item (highest priority)
            }
            else
                return default(T);
        }

        // MakeEmpty
        // Reset a priority queue to empty
        // Time complexity:  O(1)

        public void MakeEmpty()
        {
            count = 0;
        }

        // Empty
        // Return true if the priority is empty; false otherwise
        // Time complexity:  O(1)

        public bool Empty()
        {
            return count == 0;
        }

        // Size
        // Return the number of items in the priority queue
        // Time complexity:  O(1)

        public int Size()
        {
            return count;
        }
    }
}