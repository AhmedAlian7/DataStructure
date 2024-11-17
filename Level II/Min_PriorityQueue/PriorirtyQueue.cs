using System;
using System.Collections.Generic;

namespace Min_PriorityQueue
{
    public class Node<T>
    {
        public T Value { get; set; }
        public int Priority { get; set; }

        public Node(T value,int priority)
        {
            Value = value;
            Priority = priority;
        }
    }

    public class PriorirtyQueue<T>
    {
        private List<Node<T>> elements = new List<Node<T>>();

        public void Add(T value, int priority)
        {
            var node = new Node<T>(value, priority);
            elements.Add(node);
            HeapifyUp(elements.Count - 1);
        }
        private void HeapifyUp(int index)
        {
            while (index > 0)
            {
                int parentIndex = (index - 1) / 2;
                if (elements[index].Priority >= elements[parentIndex].Priority) break;

                // shorthand swap, tuple assignment 
                (elements[index], elements[parentIndex]) = (elements[parentIndex], elements[index]);

                index = parentIndex;
            }
        }
        public Node<T> Peek()
        {
            if (elements.Count == 0)
                throw new InvalidOperationException("Heap is empty.");

            return elements[0]; // The smallest element is at the root
        }

        public Node<T> ExtractMin()
        {
            var nim = elements[0];

            elements[0] = elements[elements.Count - 1];
            elements.RemoveAt(elements.Count - 1);
            HeapifyDown(0);

            return nim;
        }
        private void HeapifyDown(int index)
        {
            while (index < elements.Count)
            {
                int leftChildIndex = index * 2 + 1;
                int rightChildIndex = index * 2 + 2;

                int smallestIndex = index;
                // if the left child is exist and it less than the current index
                if (leftChildIndex < elements.Count && elements[leftChildIndex].Priority < elements[smallestIndex].Priority)
                    smallestIndex = leftChildIndex;
                // if the right child is exist and it less than the current index
                if (rightChildIndex < elements.Count && elements[rightChildIndex].Priority < elements[smallestIndex].Priority)
                    smallestIndex = rightChildIndex;

                if (smallestIndex == index) break;

                // swap 
                (elements[index], elements[smallestIndex]) = (elements[smallestIndex], elements[index]);

                index = smallestIndex;
            }
        }

    }
}
