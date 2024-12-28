using System;

namespace DS_Level2
{
    public class BinarySearchNode <T> where T : IComparable<T>
    {
        public T Value { get; set; }
        public BinarySearchNode<T> Left { get; set; }
        public BinarySearchNode<T> Right { get; set; }

        public BinarySearchNode(T value)
        {
            Value = value;
            Left = null;
            Right = null;
        }
    }

    public class BinarySearchTree <T> where T : IComparable<T>
    {
        public BinarySearchNode <T> Root { get; private set; }

        public BinarySearchTree()
        {
            Root = null;
        }

        private BinarySearchNode<T> Insert(BinarySearchNode<T> node, T value)
        {
            if (node is null)
                return new BinarySearchNode<T>(value);

            if (value.CompareTo(node.Value) < 0)
                node.Left = Insert(node.Left, value);
            else 
                node.Right = Insert(node.Right, value);

            return node;
        }
        public void Insert(T value)
        {
            Root = Insert(Root, value);
        }

        private void InOrderTraversal(BinarySearchNode<T> node)
        {
            if (node == null) return;

            InOrderTraversal(node.Left);
            Console.Write(node.Value + " ");
            InOrderTraversal(node.Right);
        } // should sort the elements ascending 
        public void InOrderTraversal()
        {
            InOrderTraversal(Root);
            Console.WriteLine();
        }

        public void Print(BinarySearchNode<T> startNode, int space = 0)
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

        private BinarySearchNode<T> Search(BinarySearchNode<T> node,T value)
        {
            while (node != null)
            {
                if (EqualityComparer<T>.Default.Equals(node.Value, value)) return node;

                if (node.Value.CompareTo(value) > 0) // if the current is greater than the value I search for
                    node = node.Left;
                else 
                    node = node.Right;
            }
            return null;
        }
        public bool Search(T value)
        {
            return Search(Root, value) != null;
        }
    }
}
