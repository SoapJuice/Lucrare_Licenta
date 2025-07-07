using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node
{
    public abstract string Evaluate();
}
public class DecisionBranch: Node
{
    
    public Func<bool> Condition { get;  }
    public Node Positive {  get; }
    public Node Negative { get; }

    public DecisionBranch(Func<bool> condition, Node positive, Node negative)
    {
        Condition = condition;
        Positive = positive;
        Negative = negative;
    }

    public override string Evaluate()
    {
        return Condition() ? Positive.Evaluate() : Negative.Evaluate();
    }
}

public class DecisionNode : Node
{
    private string difficulty;

    public DecisionNode(string difficulty)
    {
        this.difficulty = difficulty;
    }

    public override string Evaluate() => difficulty;
}