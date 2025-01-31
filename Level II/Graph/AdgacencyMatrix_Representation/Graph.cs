using System;
using System.Collections.Generic;

namespace AdgacencyMatrix_Representation
{
    public class Graph<T>
    {
        public enum enType { Direct, Indirect};

        private int[,] _vertecesMatrix;
        private Dictionary<T, int> _vertecesIndex;
        private int _numberOfVerteces;
        private enType _type;

        public Graph(List<T> verteces, enType type)
        {
            _type = type;
            _numberOfVerteces = verteces.Count;
            _vertecesMatrix = new int[_numberOfVerteces, _numberOfVerteces];
            _vertecesIndex = new Dictionary<T, int>();
            for (int i = 0; i < _numberOfVerteces; i++)
            {
                _vertecesIndex.Add(verteces[i], i);
            }
        }

        public void AddEdge(T sourse, T destination, int cost = 1)
        {
            if (!_vertecesIndex.ContainsKey(sourse) || !_vertecesIndex.ContainsKey(destination))
            {
                throw new EntryPointNotFoundException("Invaliad vetreces");
            }

            _vertecesMatrix[_vertecesIndex[sourse], _vertecesIndex[destination]] = cost;
            if (_type == enType.Indirect)
                _vertecesMatrix[_vertecesIndex[destination], _vertecesIndex[sourse]] = cost;
        }
        public void RemoveEdge(T sourse, T destination)
        {
            if (!_vertecesIndex.ContainsKey(sourse) || !_vertecesIndex.ContainsKey(destination))
            {
                throw new EntryPointNotFoundException("Invaliad vetreces");
            }

            _vertecesMatrix[_vertecesIndex[sourse], _vertecesIndex[destination]] = 0;
            if (_type == enType.Indirect)
                _vertecesMatrix[_vertecesIndex[destination], _vertecesIndex[sourse]] = 0;
        }
        public void Display(string Message = "Matrix Adjacency representation:")
        {
            Console.WriteLine($"\n{Message}\n");

            Console.Write("  ");
            foreach (var vetrex in _vertecesIndex.Keys)
            {
                Console.Write(vetrex + " ");
            }
            Console.WriteLine();

            foreach (var item in _vertecesIndex)
            {
                Console.Write(item.Key + " ");
                for (int i = 0; i < _numberOfVerteces; i++)
                {
                    Console.Write(_vertecesMatrix[item.Value, i] + " ");
                }
                Console.WriteLine();
            }
        }

        public bool IsEdgeBetween(T sourse, T destination)
        {
            if (!_vertecesIndex.ContainsKey(sourse) || !_vertecesIndex.ContainsKey(destination))
            {
                throw new EntryPointNotFoundException("Invaliad vetreces");
            }

            return (_vertecesMatrix[_vertecesIndex[sourse], _vertecesIndex[destination]] != 0);
        }
        public int Indegrees(T vetrex) // coloumns
        {
            int count = 0;
            for (int i = 0; i < _numberOfVerteces; i++)
            {
                if (_vertecesMatrix[i, _vertecesIndex[vetrex]] != 0)
                    count++;
            }
            return count;
        }
        public int Outdegrees(T vetrex) // coloumns
        {
            int count = 0;
            for (int i = 0; i < _numberOfVerteces; i++)
            {
                if (_vertecesMatrix[_vertecesIndex[vetrex], i] != 0)
                    count++;
            }
            return count;
        }

         public void BFS(T startVertex)
 {
     bool[] visited = new bool[_numberOfVerteces];
     var queue = new Queue<int>();

     int startIndex = _vertecesIndex[startVertex];
     visited[startIndex] = true;

     queue.Enqueue(startIndex);
     while (queue.Count > 0)
     {
         var current = queue.Dequeue();

         Console.Write($"{GetVertexValue(current)} ");

         for (var i = 0; i < _numberOfVerteces; i++)
         {

             if (_vertecesMatrix[current, i] != 0 && !visited[i]) // neighbor and unvisited
             {
                 queue.Enqueue(i);
                 visited[i] = true;
             }
         }
     }

 }
 public void DFS(T startVertex)
 {
     if (!_vertecesIndex.ContainsKey(startVertex))
     {
         Console.WriteLine("Invalid start vertex.");
         return;
     }

     bool[] visited = new bool[_numberOfVerteces];

     Stack<int> stack = new Stack<int>();

     int startIndex = _vertecesIndex[startVertex];
     stack.Push(startIndex);


     Console.WriteLine("\nDepth-First Search:");

     while (stack.Count > 0)
     {
         int currentVertex = stack.Pop();

         if (visited[currentVertex])
             continue;

         visited[currentVertex] = true;
         Console.Write($"{GetVertexValue(currentVertex)} ");

         // Add all unvisited neighbors to the stack
         for (int i = _numberOfVerteces - 1; i >= 0; i--) // Reverse order for stack-based traversal
         {
             if (_vertecesMatrix[currentVertex, i] > 0 && !visited[i])
             {
                 stack.Push(i);
             }
         }
     }

     Console.WriteLine();
 }
 private T GetVertexValue(int index)
 {
     KeyValuePair<T, int> pair1 = new KeyValuePair<T, int>();
     foreach (var pair in _vertecesIndex)
     {
         if (pair.Value == index)
             return pair.Key;
     }
     return pair1.Key;
 }


 // Dijkstra's Algorithm: Finds the shortest paths from a source vertex
 public void Dijkstra(T startVertex)
 {
     // Adjacency Matrix : O(V^2)

     if (!_vertecesIndex.ContainsKey(startVertex))
         throw new KeyNotFoundException("Vertex you entered does not exists");

     int startIndex = _vertecesIndex[startVertex];
     int[] distances = new int[_numberOfVerteces];
     bool[] visited = new bool[_numberOfVerteces];
     T[] predecessors = new T[_numberOfVerteces]; // Tracks the previous vertex

     for (int i = 0; i < _numberOfVerteces; i++)
     {
         distances[i] = int.MaxValue;
     }
     distances[startIndex] = 0;

     // Main loop: Process each vertex
     for (int count = 0; count < _numberOfVerteces - 1; count++)
     {
         int minVertex = GetMinDistanceVertex(distances, visited);
         visited[minVertex] = true;

         // Update distances for all neighbors of the current vertex
         for (int v = 0; v < _numberOfVerteces; v++)
         {
             // Update distance if:
             // 1. There is an edge.
             // 2. The vertex is unvisited.
             // 3. The new distance is shorter.
             if (!visited[v] && _vertecesMatrix[minVertex, v] > 0 &&
                 distances[minVertex] != int.MaxValue &&
                 distances[minVertex] + _vertecesMatrix[minVertex, v] < distances[v])
             {
                 distances[v] = distances[minVertex] + _vertecesMatrix[minVertex, v];
                 predecessors[v] = GetVertexValue(minVertex); // Record the predecessor, prev node.
             }
         }
     }

     // Display the shortest paths and their distances
     Console.WriteLine("\nShortest paths from vertex " + startVertex + ":");
     for (int i = 0; i < _numberOfVerteces; i++)
     {
         Console.WriteLine($"{startVertex} -> {GetVertexValue(i)}: Distance = {distances[i]}, Path = {GetPath(predecessors, i)}");
     }
 }
 public void Dijkstra(T startVertex, T endVertex)
 {
     // Adjacency Matrix : O(V^2)

     if (!_vertecesIndex.ContainsKey(startVertex))
         throw new KeyNotFoundException("Vertex you entered does not exists");

     int startIndex = _vertecesIndex[startVertex];
     int[] distances = new int[_numberOfVerteces];
     bool[] visited = new bool[_numberOfVerteces];
     T[] predecessors = new T[_numberOfVerteces]; // Tracks the previous vertex

     for (int i = 0; i < _numberOfVerteces; i++)
     {
         distances[i] = int.MaxValue;
     }
     distances[startIndex] = 0;

     // Main loop: Process each vertex
     for (int count = 0; count < _numberOfVerteces - 1; count++)
     {
         int minVertex = GetMinDistanceVertex(distances, visited);
         visited[minVertex] = true;

         // Update distances for all neighbors of the current vertex
         for (int v = 0; v < _numberOfVerteces; v++)
         {
             // Update distance if:
             // 1. There is an edge.
             // 2. The vertex is unvisited.
             // 3. The new distance is shorter.
             if (!visited[v] && _vertecesMatrix[minVertex, v] > 0 &&
                 distances[minVertex] != int.MaxValue &&
                 distances[minVertex] + _vertecesMatrix[minVertex, v] < distances[v])
             {
                 distances[v] = distances[minVertex] + _vertecesMatrix[minVertex, v];
                 predecessors[v] = GetVertexValue(minVertex); // Record the predecessor, prev node.
             }
         }
     }

     // Display the shortest paths and their distances
     Console.WriteLine("\nShortest paths from vertex " + startVertex + ":");
     for (int i = 0; i < _numberOfVerteces; i++)
     {
         Console.WriteLine($"{startVertex} -> {GetVertexValue(i)}: Distance = {distances[i]}, Path = {GetPath(predecessors, i)}");
     }

     int endIndex = _vertecesIndex[endVertex];
     Console.WriteLine($"\nShortest path from {startVertex} to {endVertex}:");
     if (distances[endIndex] == int.MaxValue)
         Console.WriteLine("No path exists.");
     else
     {
         string path = GetPath(predecessors, endIndex);
         Console.WriteLine($"Path: {path}");
         Console.WriteLine($"Distance: {distances[endIndex]}");
     }
 }

 private int GetMinDistanceVertex(int[] distances, bool[] visited)
 {
     int minDistance = int.MaxValue;
     int minIndex = -1;

     for (int i = 0; i < _numberOfVerteces; i++)
     {
         if (!visited[i] && distances[i] < minDistance)
         {
             minDistance = distances[i];
             minIndex = i;
         }
     }
     return minIndex;
 }

 private string GetPath(T[] predecessors, int currentIndex)
 {
     if (predecessors[currentIndex] == null)
         return GetVertexValue(currentIndex).ToString();

     return GetPath(predecessors, _vertecesIndex[predecessors[currentIndex]]) + " -> " + GetVertexValue(currentIndex).ToString();
 }


 // Dijkstra's Algorithm using Priority Queue (Min-Heap)
 public void Dijkstra_MinHeap(T startVertex)
 {
     if (!_vertecesIndex.ContainsKey(startVertex))
         throw new KeyNotFoundException("Vertex you entered does not exists");


     int startIndex = _vertecesIndex[startVertex];
     int[] distances = new int[_numberOfVerteces];
     bool[] visited = new bool[_numberOfVerteces];
     T[] predecessors = new T[_numberOfVerteces];

     for (int i = 0; i < _numberOfVerteces; i++)
     {
         distances[i] = int.MaxValue;
     }
     distances[startIndex] = 0; // Distance to the starting vertex is 0

     // Priority queue (Min-Heap) to store vertices with their distances
     var priorityQueue = new SortedSet<(int distance, int vertexIndex)>(
         Comparer<(int distance, int vertexIndex)>.Create((x, y) =>
             x.distance == y.distance ? x.vertexIndex.CompareTo(y.vertexIndex) : x.distance.CompareTo(y.distance))
     );


     // Add the starting vertex to the priority queue
     priorityQueue.Add((0, startIndex));


     // Process all vertices in the priority queue
     while (priorityQueue.Count > 0)
     {
         // Extract the vertex with the smallest distance
         var (currentDistance, currentIndex) = priorityQueue.Min;
         priorityQueue.Remove(priorityQueue.Min);


         // Skip the vertex if it's already visited
         if (visited[currentIndex]) continue;
         visited[currentIndex] = true; // Mark the vertex as visited


         // Update the distances for all neighbors of the current vertex
         for (int neighbor = 0; neighbor < _numberOfVerteces; neighbor++)
         {
             // Check if there is an edge and the neighbor is unvisited
             if (_vertecesMatrix[currentIndex, neighbor] > 0 && !visited[neighbor])
             {
                 // Calculate the new distance to the neighbor
                 int newDistance = distances[currentIndex] + _vertecesMatrix[currentIndex, neighbor];


                 // If the new distance is shorter, update it
                 if (newDistance < distances[neighbor])
                 {
                     priorityQueue.Remove((distances[neighbor], neighbor)); // Remove the old distance
                     distances[neighbor] = newDistance; // Update to the new distance
                     predecessors[neighbor] = GetVertexValue(currentIndex); // Update the predecessor
                     priorityQueue.Add((newDistance, neighbor)); // Add the updated distance to the queue
                 }
             }
         }
     }
 }
    }
}
