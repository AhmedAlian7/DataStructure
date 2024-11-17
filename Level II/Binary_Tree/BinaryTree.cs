using System;
using System.Collections.Generic;

namespace Binary_Tree
{
    public class BinaryNode<T>
    {
        public T Value { get; set; }
        public BinaryNode<T> Left { get; set; }
        public BinaryNode<T> Right { get; set; }

        public BinaryNode(T value)
        {
            Value = value;
            Left = null;
            Right = null;
        }
    }

    public class BinaryTree<T>
    {
        public BinaryNode<T> Root { get; private set; }

        public BinaryTree()
        {
            Root = null;
        }

        public void Insert(T value)
        {
            if (Root == null)
            {
                Root = new BinaryNode<T>(value);
                return;
            }

            Queue<BinaryNode<T>> queue = new Queue<BinaryNode<T>>();
            queue.Enqueue(Root);
            while (queue.Count > 0)
            {
                var currentBinaryNode = queue.Dequeue();

                if (currentBinaryNode.Left == null)
                {
                    currentBinaryNode.Left = new BinaryNode<T>(value);
                    break;
                }
                else
                {
                    queue.Enqueue(currentBinaryNode.Left);
                }

                if (currentBinaryNode.Right == null)
                {
                    currentBinaryNode.Right = new BinaryNode<T>(value);
                    break;
                }
                else
                {
                    queue.Enqueue(currentBinaryNode.Right);
                }
            }
        }

        public void Print(BinaryNode<T> startNode, int space = 0)
        {
            const int COUNT = 7;
            if (startNode == null) return;

            space += COUNT;

            // Process right child first
            Print(startNode.Right, space);

            // Print current node after padding spaces
            Console.WriteLine();
            for (int i = COUNT; i < space; i++)
                Console.Write(" ");
            Console.WriteLine(startNode.Value);

            // Process left child
            Print(startNode.Left, space);
        }

        public void Print()
        {
            Print(Root, 0);
        }

        private void PreOrderTraversal(BinaryNode<T> node)
        {
            if (node == null) return;   
            
            Console.Write(node.Value + " ");
            PreOrderTraversal(node.Left);
            PreOrderTraversal(node.Right);
            
        }
        public void PreOrderTraversal()
        {
            PreOrderTraversal(Root);
            Console.WriteLine();
        }
        private void PostOrderTraversal(BinaryNode<T> node)
        {
            if (node == null) return;

            PostOrderTraversal(node.Left);
            PostOrderTraversal(node.Right);
            Console.Write(node.Value + " ");
        }
        public void PostOrderTraversal()
        {
            PostOrderTraversal(Root);
            Console.WriteLine();
        }
        private void InOrderTraversal(BinaryNode<T> node)
        {
            if (node == null) return;

            InOrderTraversal(node.Left);
            Console.Write(node.Value + " ");
            InOrderTraversal(node.Right);
        }
        public void InOrderTraversal()
        {
            InOrderTraversal(Root);
            Console.WriteLine();
        }
    }


}
