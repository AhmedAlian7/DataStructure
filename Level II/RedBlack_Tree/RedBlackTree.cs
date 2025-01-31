using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS_Level2
{
    internal class Node
    {
        public enum enColor { Red, Black}

        public int Value;
        public Node Left, Right, Parent;
        public enColor Color = enColor.Red; // New nodes are red by default
        public bool IsRed { get { return this.Color == enColor.Red; } }
        public bool IsBlack { get { return this.Color == enColor.Black; } }

        public Node(int value)
        {
            Value = value;
        }
    }

    internal class RedBlackTree
    {
        private Node root;

        
        public void Insert(int newValue)
        {
            Node newNode = new Node(newValue);

            // Case when the tree is empty
            if (root == null)
            {
                root = newNode;
                root.Color = Node.enColor.Black; // The root must be black
                return;
            }


            // Standard Binary Search Tree insertion
            Node current = root;
            Node parent = null;
            while (current != null)
            {
                parent = current;
                if (newValue < current.Value)
                    current = current.Left;
                else
                    current = current.Right;
            }


            // Set the parent of the new node
            newNode.Parent = parent;
            if (newValue < parent.Value)
                parent.Left = newNode;
            else
                parent.Right = newNode;


            // Restore Red-Black properties that might have been violated during insertion
            FixInsert(newNode);
        }

        private void FixInsert(Node node)
        {
            Node parent = null;
            Node grandparent = null;


            while ((node != root) && (node.Color == Node.enColor.Red) && (node.Parent.Color == Node.enColor.Red))
            {
                parent = node.Parent;
                grandparent = parent.Parent;


                if (parent == grandparent.Left)
                {
                    Node uncle = grandparent.Right;

                    // Case 1: The uncle is red (recoloring)
                    if (uncle != null && (uncle.Color == Node.enColor.Red))
                    {
                        grandparent.Color = Node.enColor.Red;
                        parent.Color = Node.enColor.Black;
                        uncle.Color = Node.enColor.Black;

                        node = grandparent; // Move up the tree to continue fixing
                    }
                    else
                    {
                        // Case 2: Node is the right child of its parent (Triangle Case)
                        if (node == parent.Right)
                        {
                            RotateLeft(parent);
                            node = parent;
                            parent = node.Parent;
                        }


                        // Case 3: Node is now the left child of its parent (Line Case)
                        RotateRight(grandparent);

                        // Swap colors of parent and grandparent to maintain Red-Black properties
                        (parent.Color, grandparent.Color) = (grandparent.Color,  parent.Color);

                        node = parent;
                    }
                }
                else // Parent node is a right child
                {
                    Node uncle = grandparent.Left;


                    // Case 1: The uncle is red (recoloring)
                    if (uncle != null && uncle.Color == Node.enColor.Red)
                    {
                        grandparent.Color = Node.enColor.Red;
                        parent.Color = Node.enColor.Black;
                        uncle.Color = Node.enColor.Black;

                        node = grandparent; // Move up the tree to continue fixing
                    }
                    else
                    {
                        // Case 2: Node is the left child of its parent (Triangle Case)
                        if (node == parent.Left)
                        {
                            RotateRight(parent);
                            node = parent;
                            parent = node.Parent;
                        }


                        // Case 3: Node is now the right child of its parent (Line Case)
                        RotateLeft(grandparent);

                        // Swap colors of parent and grandparent to maintain Red-Black properties
                        (parent.Color, grandparent.Color) = (grandparent.Color,  parent.Color);

                        node = parent;

                    }
                }
            }

            root.Color = Node.enColor.Black; // Ensure the root remains black
        }


        private void RotateLeft(Node node)
        {
            Node right = node.Right;
            node.Right = right.Left; // Move the left subtree of right to the right subtree of node

            if (node.Right != null)
                node.Right.Parent = node;


            right.Parent = node.Parent; // Connect new root with the grandparent


            if (node.Parent == null) root = right;

            else if (node == node.Parent.Left) node.Parent.Left = right;

            else node.Parent.Right = right;


            right.Left = node;
            node.Parent = right; // Update parent of the original node
        }
        private void RotateRight(Node node)
        {
            Node left = node.Left;
            node.Left = left.Right; // Move the right subtree of left to the left subtree of node

            if (node.Left != null)
                node.Left.Parent = node;


            left.Parent = node.Parent; // Connect new root with the grandparent


            if (node.Parent == null) root = left;

            else if (node == node.Parent.Right) node.Parent.Right = left;

            else node.Parent.Left = left;


            left.Right = node;
            node.Parent = left; // Update parent of the original node
        }


        public void PrintTree()
        {
            PrintHelper(root, "", true);
        }
        private void PrintHelper(Node node, string indent, bool last)
        {
            if (node != null)
            {
                Console.Write(indent);
                if (last)
                {
                    Console.Write("R----");
                    indent += "   ";
                }
                else
                {
                    Console.Write("L----");
                    indent += "|  ";
                }
                var color = (node.Color == Node.enColor.Red) ? "RED" : "BLK";
                Console.WriteLine(node.Value + "(" + color + ")");
                PrintHelper(node.Left, indent, false);
                PrintHelper(node.Right, indent, true);
            }
        }

        private Node FindNode(Node SearchInNode, int value)
        {
            if (SearchInNode == null || value == SearchInNode.Value)
                return SearchInNode; // Return the node if found, or null if not found

            if (value < SearchInNode.Value)
                return FindNode(SearchInNode.Left, value); // Search in the left subtree

            else
                return FindNode(SearchInNode.Right, value); // Search in the right subtree
        }
        public Node Find(int value)
        {
            return FindNode(root, value);
        }


        // Public method to delete a value from the tree
        public bool Delete(int value)
        {
            Node nodeToDelete = FindNode(root, value);
            if (nodeToDelete == null)
                return false; // Node to delete not found

            DeleteNode(nodeToDelete);
            return true;
        }
        private void DeleteNode(Node nodeToDelete)
        {
            Node child = null;

            var originalColor = nodeToDelete.Color;

            // Case 1: The node to delete has no left child
            if (nodeToDelete.Left == null)
            {
                child = nodeToDelete.Right;
                Transplant(nodeToDelete, child);  // Replace nodeToDelete with its right child
            }

            // Case 2: The node to delete has no right child
            else if (nodeToDelete.Right == null)
            {
                child = nodeToDelete.Left;
                Transplant(nodeToDelete, child);  // Replace nodeToDelete with its left child
            }
            // Case 3: The node to delete has both left and right children
            else
            {
                // Find the in-order successor (smallest node in the right subtree)
                Node successor = Minimum(nodeToDelete.Right);
                originalColor = successor.Color;  // Store the original color of the successor
                child = successor.Right;  // The child is the right child of the successor

                // If the successor is the immediate child of the node to delete
                if (successor.Parent == nodeToDelete)
                {
                    if (child != null)
                        child.Parent = successor;  // Update the parent of the child
                }
                else
                {
                    // Replace the successor with its right child in its original position
                    Transplant(successor, successor.Right);
                    successor.Right = nodeToDelete.Right;  // Connect the right child of the node to delete to the successor
                    successor.Right.Parent = successor;  // Update the parent of the right child
                }

                // Replace the node to delete with the successor
                Transplant(nodeToDelete, successor);
                successor.Left = nodeToDelete.Left;  // Connect the left child of the node to delete to the successor
                successor.Left.Parent = successor;  // Update the parent of the left child
                successor.Color = nodeToDelete.Color;  // Maintain the original color of the node being deleted
            }

            // If the original color of the node was black, fix the Red-Black properties
            if ((originalColor == Node.enColor.Black) && child != null)
            {
                FixDelete(child);
            }
        }
        private void Transplant(Node target, Node with)
        {
            // If the target node is the root of the tree (i.e., it has no parent),
            // then the new subtree (with) becomes the new root of the tree.
            if (target.Parent == null)
                root = with;

            // If the target node is the left child of its parent,
            // then update the parent's left child to be the new subtree (with).
            else if (target == target.Parent.Left)
                target.Parent.Left = with;
            // If the target node is the right child of its parent,
            // then update the parent's right child to be the new subtree (with).
            else
                target.Parent.Right = with;

            // If the new subtree (with) is not null, 
            // update its parent to be the parent of the target node.
            if (with != null)
                with.Parent = target.Parent;
        }

        // Method to fix Red-Black properties after deletion
        private void FixDelete(Node node)
        {
            // Loop until the node is the root or until the node is red
            while (node != root && node.IsBlack)
            {
                if (node == node.Parent.Left)
                {
                    Node sibling = node.Parent.Right;

                    // Case 1: If the sibling is red, perform a rotation and recolor
                    if (sibling.IsRed)
                    {
                        sibling.Color = Node.enColor.Black; // Recolor sibling to black
                        node.Parent.Color = Node.enColor.Red; // Recolor parent to red
                        RotateLeft(node.Parent); // Rotate the parent to the left
                        sibling = node.Parent.Right; // Update sibling after rotation
                    }

                    // Case 2.1: If both of sibling's children are black
                    if (sibling.Left.IsBlack && sibling.Right.IsBlack)
                    {
                        sibling.Color = Node.enColor.Red; // Recolor sibling to red
                        node = node.Parent; // Move up the tree to continue fixing
                    }
                    else
                    {
                        // Case 2.2.2: If sibling's right child  is black and left child is red (Near child Red)
                        if (sibling.Right.IsBlack)
                        {
                            sibling.Left.Color = Node.enColor.Black; // Recolor sibling's left child to black
                            sibling.Color = Node.enColor.Red; // Recolor sibling to red
                            RotateRight(sibling); // Rotate sibling to the right
                            sibling = node.Parent.Right; // Update sibling after rotation
                        }

                        // Case 2.2.1: Sibling's right child is red (Far child Red)
                        sibling.Color = node.Parent.Color; // Recolor sibling with parent's color
                        node.Parent.Color = Node.enColor.Black; // Recolor parent to black
                        sibling.Right.Color = Node.enColor.Black; // Recolor sibling's right child to black
                        RotateLeft(node.Parent); // Rotate parent to the left
                        node = root; // Set node to root to break out of the loop
                    }
                }
                else // If the node is the right child of its parent
                {
                    // Get the sibling of the node
                    Node sibling = node.Parent.Left;

                    // Case 1: If the sibling is red, perform a rotation and recolor
                    if (sibling.IsRed)
                    {
                        sibling.Color = Node.enColor.Black; // Recolor sibling to black
                        node.Parent.Color = Node.enColor.Red; // Recolor parent to red
                        RotateRight(node.Parent); // Rotate the parent to the right
                        sibling = node.Parent.Left; // Update sibling after rotation
                    }

                    // Case 2.1: If both of sibling's children are black
                    if (!sibling.Left.IsRed && !sibling.Right.IsRed)
                    {
                        sibling.Color = Node.enColor.Red; // Recolor sibling to red
                        node = node.Parent; // Move up the tree to continue fixing
                    }
                    else
                    {
                        // Case 2.2.2: If sibling's left child is black and right child is red (Near Child is Red)
                        if (!sibling.Left.IsRed)
                        {
                            sibling.Right.Color = Node.enColor.Black; // Recolor sibling's right child to black
                            sibling.Color = Node.enColor.Red; // Recolor sibling to red
                            RotateLeft(sibling); // Rotate sibling to the left
                            sibling = node.Parent.Left; // Update sibling after rotation
                        }

                        // Case 2.2.1: Sibling's left child is red (Far Child is Red)
                        sibling.Color = node.Parent.Color; // Recolor sibling with parent's color
                        node.Parent.Color = Node.enColor.Black; // Recolor parent to black
                        sibling.Left.Color = Node.enColor.Black; // Recolor sibling's left child to black
                        RotateRight(node.Parent); // Rotate parent to the right
                        node = root; // Set node to root to break out of the loop
                    }
                }
            }
            node.Color = Node.enColor.Black; // Ensure the node is black before exiting
        }


        // Helper method to find the minimum value node in the tree
        private Node Minimum(Node node)
        {
            // Traverse down the left subtree until the leftmost node is reached
            while (node.Left != null)
                node = node.Left; // Move to the left child

            // Return the leftmost (minimum value) node
            return node;
        }

    }
}
