using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AdaptiveMapGenerator : MonoBehaviour
{
    public int width = 18;
    public int height = 11;

    public float explorationRate = 0.3f;
    public float learningRate = 0.1f;
    public float discountFactor = 0.9f;

    private DecisionTree difficultyTree;

    void Awake()
    {
        InitializeSystems();
    }

    void InitializeSystems()
    {
        difficultyTree = new DecisionTree()
            .AddNode("Health < 30%",
                new DecisionTree.Node("Easy Map"),
                new DecisionTree.Node("Check Time"))
            .AddNode("Time < 25%",
                new DecisionTree.Node("Very Easy Map"),
                new DecisionTree.Node("Standard Map"));

    }

}