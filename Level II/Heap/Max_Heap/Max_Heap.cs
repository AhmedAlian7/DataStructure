using System;
using System.Collections.Generic;

namespace Heap
{
    internal class MaxHeap
    {
        private List<int> elements;
        public MaxHeap()
        {
            this.elements = new List<int>();
        }

        public void Add(int element)
        {
            this.elements.Add(element);
            HeapifyUp(elements.Count - 1);
        }
        private void HeapifyUp(int index)
        {
            while (index > 0)
            {
                int parentIndex = (index - 1) / 2;
                if (elements[index] <= elements[parentIndex]) break;

                // shorthand swap, tuple assignment 
                (elements[index], elements[parentIndex]) = (elements[parentIndex], elements[index]);

                index = parentIndex;
            }
        }

        public int Peek()
        {
            if (elements.Count == 0)
                throw new InvalidOperationException("Heap is empty.");

            return elements[0]; // The smallest element is at the root
        }

        public int ExtractMax()
        {
            int max = elements[0];

            elements[0] = elements[elements.Count - 1];
            elements.RemoveAt(elements.Count - 1);
            HeapifyDown(0);

            return max;
        }
        private void HeapifyDown(int index)
        {
            while (index < elements.Count)
            {
                int leftChildIndex = index * 2 + 1;
                int rightChildIndex = index * 2 + 2;

                int smallestIndex = index;
                // if the left child is exist and it less than the current index
                if (leftChildIndex < elements.Count && elements[leftChildIndex] > elements[smallestIndex])
                    smallestIndex = leftChildIndex;
                // if the right child is exist and it less than the current index
                if (rightChildIndex < elements.Count && elements[rightChildIndex] > elements[smallestIndex])
                    smallestIndex = rightChildIndex;

                if (smallestIndex == index) break;

                // swap 
                (elements[index], elements[smallestIndex]) = (elements[smallestIndex], elements[index]);

                index = smallestIndex;
            }
        }

        public override string ToString()
        {
            return string.Join(", ", elements);
        }
    }
}
