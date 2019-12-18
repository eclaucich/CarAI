using UnityEngine;

public class NeuralNetwork
{
    private int inputLayerSize = 8;       
    private int hiddenLayerSize = 5;    
    private int outputLayerSize = 3;  

    private float[] inputNeurons;
    private float[] hiddenNeurons;
    private float[] outputNeurons;

    private float[][] weightsIH;
    private float[][] weightsHO;

    private float initialMin = -5f;
    private float initialMax = 5f;


    public NeuralNetwork()
    {
        inputNeurons = new float[inputLayerSize];
        hiddenNeurons = new float[hiddenLayerSize];
        outputNeurons = new float[outputLayerSize];

        weightsIH = new float[inputLayerSize][];
        for (int i = 0; i < inputLayerSize; i++)
        {
            weightsIH[i] = new float[hiddenLayerSize];
        }

        weightsHO = new float[hiddenLayerSize][];
        for (int i = 0; i < hiddenLayerSize; i++)
        {
            weightsHO[i] = new float[outputLayerSize];
        }


        for (int i = 0; i < inputLayerSize; i++)
        {
            inputNeurons[i] = GetRandomValue();
        }

        Randomize(weightsIH);
        Randomize(weightsHO);
    }

    public NeuralNetwork(DNA dna)
    {
        weightsIH = dna.GetDNA()[0];    //LA LAYER Input-Hidden
        weightsHO = dna.GetDNA()[1];    //LA LAYER Hidden-Output
    }

    public float[] FeedForward(float[] inputs)
    {
        //Nuevos inputs
        inputNeurons = inputs; 
         
        //Calcular nodos Hidden
        for (int i = 0; i < hiddenLayerSize; i++)
        {
            float sum = 0f;
            for (int j = 0; j < inputLayerSize; j++)
            {
                sum += inputNeurons[j] * weightsIH[j][i];
            }
            hiddenNeurons[i] = Sigmoid(sum);
        }

        //Calcular nodos Output
        for (int i = 0; i < outputLayerSize; i++)
        {
            float sum = 0f;
            for (int j = 0; j < hiddenLayerSize; j++)
            {
                sum += hiddenNeurons[j] * weightsHO[j][i];
            }
            outputNeurons[i] = Sigmoid(sum);
        }

        return outputNeurons;
    }

    private float Sigmoid(float x)
    {
        return 1 / (1 + Mathf.Exp(-x));
    }

    private void Randomize(float[][] weights)
    {
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                weights[i][j] = Random.Range(initialMin, initialMax);
            }
        }
    }

    private float GetRandomValue()
    {
        return Random.Range(initialMin, initialMax);
    }
}
