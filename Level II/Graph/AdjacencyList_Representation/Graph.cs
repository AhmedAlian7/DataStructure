using System;
using System.Collections.Generic;

namespace AdjacencyList_Representation
{
    public class Graph<T>
    {
        public enum enType { Direct, Indirect };

        private Dictionary<T, List<Tuple<T, int>>> _AdjacencyList;
        private Dictionary<T, int> _vertecesIndex;
        private int _numberOfVerteces;
        private enType _type;

        public Graph(List<T> verteces, enType type)
        {
            _type = type;
            _numberOfVerteces = verteces.Count;
            _AdjacencyList = new Dictionary<T, List<Tuple<T, int>>>();  
            _vertecesIndex = new Dictionary<T, int>();
            for (int i = 0; i < _numberOfVerteces; i++)
            {
                _vertecesIndex.Add(verteces[i], i);
                _AdjacencyList[verteces[i]] = new List<Tuple<T, int>>();
            }
        }

        public void AddEdge(T sourse, T destination, int cost = 1)
        {
            if (!_vertecesIndex.ContainsKey(sourse) || !_vertecesIndex.ContainsKey(destination))
            {
                throw new EntryPointNotFoundException("Invaliad vetreces");
            }

            _AdjacencyList[sourse].Add(new Tuple<T, int>(destination, cost));
            if (_type == enType.Indirect)
                _AdjacencyList[destination].Add(new Tuple<T, int>(sourse, cost));
        }
        public void RemoveEdge(T sourse, T destination)
        {
            if (!_vertecesIndex.ContainsKey(sourse) || !_vertecesIndex.ContainsKey(destination))
            {
                throw new EntryPointNotFoundException("Invaliad vetreces");
            }

            _AdjacencyList[sourse].RemoveAll(t => EqualityComparer<T>.Default.Equals(t.Item1, destination));
            if (_type == enType.Indirect)
            _AdjacencyList[destination].RemoveAll(t => EqualityComparer<T>.Default.Equals(t.Item1, sourse));

        }
        public void Display(string Message = "Matrix Adjacency representation:")
        {
            Console.WriteLine($"\n{Message}\n");

            foreach (var vetrex in _AdjacencyList)
            {
                Console.Write($"{vetrex.Key} -> ");
                foreach (var edge in vetrex.Value)
                {
                    Console.Write($"({edge.Item1}, {edge.Item2}), ");
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

            foreach (var edge in _AdjacencyList[sourse])
            {
                if (EqualityComparer<T>.Default.Equals(edge.Item1, destination))
                    return true;
            }
            return false;
        }
        public int Indegrees(T vetrex) // coloumns
        {
            if (!_vertecesIndex.ContainsKey(vetrex))
                throw new EntryPointNotFoundException("Invaliad vetrex");

            int count = 0;
            foreach (var item in _AdjacencyList)
            {
                foreach (var edge in item.Value)
                {
                    if (EqualityComparer<T>.Default.Equals(edge.Item1, vetrex))
                        count++;
                }
            }
            return count;
        }
        public int Outdegrees(T vetrex) // coloumns
        {
            if (!_vertecesIndex.ContainsKey(vetrex))
                throw new EntryPointNotFoundException("Invaliad vetrex");

            return _AdjacencyList[vetrex].Count;
        }
    }
}
