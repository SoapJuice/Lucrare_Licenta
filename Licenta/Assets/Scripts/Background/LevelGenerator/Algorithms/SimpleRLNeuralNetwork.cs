using System;
using UnityEngine;

public class SimpleRLNeuralNetwork
{
    private int inputSize;
    private int hiddenSize;
    private int outputSize;

    private float[,] weightsInputHidden;
    private float[] biasHidden;

    private float[,] weightsHiddenOutput;
    private float[] biasOutput;

    private float learningRate = 0.05f;
    private System.Random rand = new System.Random();

    private float[] lastInput;
    private float[] lastOutput;

    public SimpleRLNeuralNetwork() : this(new int[] { 7, 10, 3 }) { }

    public SimpleRLNeuralNetwork(int[] layers)
    {
        if (layers.Length != 3)
            throw new ArgumentException("Only 3-layer networks supported: [input, hidden, output]");

        inputSize = layers[0];
        hiddenSize = layers[1];
        outputSize = layers[2];

        weightsInputHidden = new float[inputSize, hiddenSize];
        biasHidden = new float[hiddenSize];

        weightsHiddenOutput = new float[hiddenSize, outputSize];
        biasOutput = new float[outputSize];

        InitWeights(weightsInputHidden);
        InitWeights(weightsHiddenOutput);
        InitBias(biasHidden);
        InitBias(biasOutput);
    }

    void InitWeights(float[,] w)
    {
        for (int i = 0; i < w.GetLength(0); i++)
            for (int j = 0; j < w.GetLength(1); j++)
                w[i, j] = (float)(rand.NextDouble() * 2 - 1);
    }

    void InitBias(float[] b)
    {
        for (int i = 0; i < b.Length; i++)
            b[i] = 0f;
    }

    public float[] Predict(float[] input)
    {
        lastInput = input;
        float[] hidden = new float[hiddenSize];
        for (int j = 0; j < hiddenSize; j++)
        {
            float sum = biasHidden[j];
            for (int i = 0; i < inputSize; i++)
                sum += input[i] * weightsInputHidden[i, j];
            hidden[j] = ReLU(sum);
        }

        float[] output = new float[outputSize];
        for (int k = 0; k < outputSize; k++)
        {
            float sum = biasOutput[k];
            for (int j = 0; j < hiddenSize; j++)
                sum += hidden[j] * weightsHiddenOutput[j, k];
            output[k] = sum;
        }

        lastOutput = Softmax(output);
        return lastOutput;
    }

    public int ChooseAction(float[] input)
    {
        float[] probs = Predict(input);
        float randVal = (float)rand.NextDouble();
        float cumulative = 0f;
        for (int i = 0; i < probs.Length; i++)
        {
            cumulative += probs[i];
            if (randVal < cumulative)
                return i;
        }
        return probs.Length - 1;
    }

    public void TrainWithReward(float reward)
    {
        if (lastInput == null || lastOutput == null) return;

        float[] target = new float[outputSize];
        int maxIndex = 0;
        for (int i = 1; i < lastOutput.Length; i++)
            if (lastOutput[i] > lastOutput[maxIndex])
                maxIndex = i;

        target[maxIndex] = reward;

        Train(lastInput, target);
    }

    public void Train(float[] input, float[] target)
    {
        float[] hidden = new float[hiddenSize];
        float[] hiddenRaw = new float[hiddenSize];
        for (int j = 0; j < hiddenSize; j++)
        {
            float sum = biasHidden[j];
            for (int i = 0; i < inputSize; i++)
                sum += input[i] * weightsInputHidden[i, j];
            hiddenRaw[j] = sum;
            hidden[j] = ReLU(sum);
        }

        float[] output = new float[outputSize];
        for (int k = 0; k < outputSize; k++)
        {
            float sum = biasOutput[k];
            for (int j = 0; j < hiddenSize; j++)
                sum += hidden[j] * weightsHiddenOutput[j, k];
            output[k] = sum;
        }

        float[] predicted = Softmax(output);
        float[] dOutput = new float[outputSize];
        for (int k = 0; k < outputSize; k++)
            dOutput[k] = predicted[k] - target[k];

        for (int j = 0; j < hiddenSize; j++)
        {
            for (int k = 0; k < outputSize; k++)
                weightsHiddenOutput[j, k] -= learningRate * dOutput[k] * hidden[j];
        }

        for (int k = 0; k < outputSize; k++)
            biasOutput[k] -= learningRate * dOutput[k];

        float[] dHidden = new float[hiddenSize];
        for (int j = 0; j < hiddenSize; j++)
        {
            float grad = 0f;
            for (int k = 0; k < outputSize; k++)
                grad += dOutput[k] * weightsHiddenOutput[j, k];
            dHidden[j] = ReLU_Derivative(hiddenRaw[j]) * grad;
        }

        for (int i = 0; i < inputSize; i++)
        {
            for (int j = 0; j < hiddenSize; j++)
                weightsInputHidden[i, j] -= learningRate * dHidden[j] * input[i];
        }

        for (int j = 0; j < hiddenSize; j++)
            biasHidden[j] -= learningRate * dHidden[j];
    }

    float ReLU(float x) => Mathf.Max(0, x);
    float ReLU_Derivative(float x) => x > 0 ? 1f : 0f;

    float[] Softmax(float[] logits)
    {
        float max = Mathf.Max(logits);
        float sum = 0f;
        float[] result = new float[logits.Length];
        for (int i = 0; i < logits.Length; i++)
        {
            result[i] = Mathf.Exp(logits[i] - max);
            sum += result[i];
        }
        for (int i = 0; i < logits.Length; i++)
            result[i] /= sum;
        return result;
    }
}
