using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace General_Tree
{
    public class Node<T>
    {
        public T Value { get; set; }
        public List<Node<T>> Children { get; set; }

        public Node(T value)
        {
            this.Value = value;
            this.Children = new List<Node<T>>();
        }

        public void AddChild(Node<T> node)
        { this.Children.Add(node); }

        public Node<T> Find(T value)
        {
            if (EqualityComparer<T>.Default.Equals(this.Value, value)) return this;
            foreach (var child in this.Children)
            {
                var falg = child.Find(value);
                if (falg != null) return falg;
            }
            return null;
        }
    }

    public class Tree<T>
    {
        public Node<T> Root { get;}
        public Tree(T rootvalue)
        {
            Root = new Node<T>(rootvalue);
        }

        public void Print(Node<T> startnode, string indent = "  ")
        {
            Console.WriteLine(indent + startnode.Value);
            foreach (var child in startnode.Children)
            {
                this.Print(child, indent + indent);
            }
        }
        public void Print(string indent = "  ")
        {
            Print(Root, indent);
        }

        public Node<T> Find(T value)
        {
            return Root?.Find(value);
        }
        public Node<T> Find(Node<T> startnode,T value)
        {
            return startnode?.Find(value);
        }
    }
}
