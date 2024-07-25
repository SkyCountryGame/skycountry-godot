using Godot;
using System.Collections.Generic;
using System.Collections;
using System;

//TODO implement
public class Graph<T> : ICollection<T>
{
    public int n { get;  }
    public int Count => n;
    public bool IsReadOnly => false;

    private bool directed = true; 
    public Dictionary<Node, List<Node>> adjacencyList;
    private HashSet<Node> nodes;
    public Node root;

    public Graph()
    {
        adjacencyList = new Dictionary<Node, List<Node>>();
        nodes = new HashSet<Node>();
    }
    public Graph(dynamic rootNodeValue)
    {
        adjacencyList = new Dictionary<Node, List<Node>>();
        nodes = new HashSet<Node>();
        if (rootNodeValue != null){
            Node n = new Node(rootNodeValue);
            nodes.Add(n);
            root = n;
            this.n = 1;
        }
        
    }

    public void Add(T item)
    {

    }

    public void Clear()
    {
    }

    public bool Contains(T item)
    {
        throw new System.NotImplementedException();
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        throw new System.NotImplementedException();
    }

    public bool Remove(T item)
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator<T> GetEnumerator()
    {
        throw new System.NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new System.NotImplementedException();
    }

    public class Node
    {
        public T value;
        public HashSet<Node> neighboors;
        public Node() { 
            neighboors = new HashSet<Node>();
        }
        public Node(T val)
        {
            neighboors = new HashSet<Node>();
            this.value = val;
        }

        public void attach(Node n)
        {
            neighboors.Add(n);
        }
    }
}

