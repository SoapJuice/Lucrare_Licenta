using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionTree
{
    public class Node
    {
        public string query;  // For decision nodes (null for leaf nodes)
        public Node positive;  // Only for decision nodes
        public Node negative;  // Only for decision nodes
        public string result;  // For leaf nodes (null for decision nodes)

        public Node(string result)
        {
            this.result = result;
            this.query = null;
            this.positive = null;
            this.negative = null;
        }

        public Node(string query, Node positive, Node negative)
        {
            this.query = query;
            this.positive = positive;
            this.negative = negative;
            this.result = null;
        }

        public bool IsLeafNode => result != null;
    }

    private Node root;

    public DecisionTree AddNode(string query, Node positive, Node negative)
    {
        if (root == null)
        {
            root = new Node(query, positive, negative);
        }
        return this;
    }

    public string Evaluate(Stats stats)
    {
        if (root == null) return "Default Map";
        return EvaluateNode(root, stats);
    }

    private string EvaluateNode(Node node, Stats stats)
    {
        Debug.Log($"Evaluating node: {(node.IsLeafNode ? $"LEAF: {node.result}" : node.query)}");

        if (node.IsLeafNode)
        {
            Debug.Log($"Reached leaf node: {node.result}");
            return node.result;
        }

        bool conditionMet = node.query switch
        {
            "Health < 30%" => stats.remainingPlayerHealth < 0.3f,
            "Time < 25%" => stats.remainingTime < 0.25f,
            "Kills > 10" => stats.enemyKills > 10,
            "Health > 50%" => stats.remainingPlayerHealth > 0.5f,
            "Time > 75%" => stats.remainingTime > 0.75f,
            "Kills > 5" => stats.enemyKills > 5,
            _ => false
        };

        Debug.Log($"Condition '{node.query}': {conditionMet}");

        Node nextNode = conditionMet ? node.positive : node.negative;
        return EvaluateNode(nextNode, stats);
    }
}