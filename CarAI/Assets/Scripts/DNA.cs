using UnityEngine;
using System.Collections.Generic;

public class DNA 
{
    private List<float[][]> dna;
    private float mutationRate = 0.05f;

    public DNA()
    {
        dna = new List<float[][]>();

        for (int k = 0; k < 2; k++)
        {
            float[][] weights = new float[8][];

            for (int i = 0; i < 8; i++)
            {
                weights[i] = new float[5];
                for (int j = 0; j < 5; j++)
                {
                    weights[i][j] = Random.Range(0f, 1f);
                }
            }

            dna.Add(weights);
        }
    }

    public DNA(DNA newDNA)
    {
        dna = newDNA.GetDNA();
    }

    public DNA(List<float[][]> weights)
    {
        dna = weights;
    }

    public DNA Mutate()
    {
        DNA newDNA = new DNA();

        for (int k = 0; k < dna.Count; k++)
        {
            for (int i = 0; i < dna[k].Length; i++)
            {
                for (int j = 0; j < dna[k][i].Length; j++)
                {
                    float random = Random.Range(0f, 1f);
                    float newValue = random <= mutationRate ? Random.Range(-1f, 1f) : dna[k][i][j];
                    newDNA.SetDNA(k, i, j, newValue);
                }
            }
        }

        return new DNA(newDNA);
    }

    public DNA CrossOver(DNA otherParent)
    {
        DNA newDNA = new DNA();

        for (int k = 0; k < dna.Count; k++)             //INDICE DE LAYER (Input-Hidden  o  Hidden-Output)
        {
            for (int i = 0; i < dna[k].Length; i++)     //INDICE DE LA PRIMERA LAYER
            {
                for (int j = 0; j < dna[k][i].Length; j++)  //INDICE DE LA SEGUNDA LAYER
                {
                    float random = Random.Range(0f, 1f);
                    float newValue = random >= 0.5f ? otherParent.GetDNA()[k][i][j] : dna[k][i][j];
                    newDNA.SetDNA(k, i, j, newValue);   //CON [k][i][j], accedo al elemento [i][j] de la layer "k"
                }
            }
        }

        return new DNA(newDNA);
    }

    public void SetDNA(int layer, int i, int j, float value)
    {
        dna[layer][i][j] = value;
    }

    public List<float[][]> GetDNA()
    {
        return dna;
    }
}
