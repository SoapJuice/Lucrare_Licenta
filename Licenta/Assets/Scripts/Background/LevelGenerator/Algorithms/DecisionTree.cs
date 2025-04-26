using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionTree : MonoBehaviour
{
    public class Node
    {
        public string query;
        public Node positive;
        public Node negative;
        public string result;

        public Node(string result) => this.result = result;
    }

    private Node root;

    public DecisionTree AddNode(string query, Node positive, Node negative)
    {
        if (root == null) root = new Node(query) { positive = positive, negative = negative };
        return this;
    }

    public string Evaluate(Stats stats)
    {
        return EvaluateNode(root, stats);
    }

    private string EvaluateNode(Node node, Stats stats)
    {
        if (node.result != null) return node.result;

        bool conditionMet = node.query switch
        {
            "Health < 30%" => stats.remainingPlayerHealth < 0.3f,
            "Time < 25%" => stats.remainingTime < 0.25f,
            _ => false
        };

        if (conditionMet)
        {
            return EvaluateNode(node.positive, stats);
        }
        else
        {
            return EvaluateNode(node.negative, stats);
        }
    }
}
