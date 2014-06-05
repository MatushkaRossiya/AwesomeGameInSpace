using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


//fun part

public partial class HeavyAlienBrainCreator : MonoSingleton<HeavyAlienBrainCreator> {


    public float bias = -1.0f;
    public int inputs = 7;
    public int outputs = 1;

    private int elitism = 2;
    public int next = 0;
    public int populationSize = 20;
    private int currentGeneration = 1;

    private float crossoverRate = 0.85f;
    private float mutationRate = 0.08f;

    public int neuronsPerHiddenLayer = 4;
    public int hiddenLayersPerNetwork = 1;

    private List<Genome> currentPopulation = new List<Genome>();

    private float minWeight = -1.1f;
    private float maxWeight = 1.1f;



    void Initialize()
    {
        Debug.Log("[creator] initializing");
        int weightsInGenome = (inputs + 1) * neuronsPerHiddenLayer + (neuronsPerHiddenLayer + 1) * neuronsPerHiddenLayer * (hiddenLayersPerNetwork - 1) + outputs * (neuronsPerHiddenLayer + 1);
        for(int i=0;i<populationSize;++i)
        {
            Genome genome = new Genome();
            genome.fitness = null;

            for(int j=0; j < weightsInGenome; ++j)
            {
                genome.weights.Add(UnityEngine.Random.Range(minWeight, maxWeight));
            }
         
            currentPopulation.Add(genome);
          

        }
      

       }


   

    void Start()
    {
       
    }

    public void GiveWeights(ref NeuralNetwork net)
    {
       
        List<float> w = currentPopulation[next].weights;
        net.genomeIndex = next;
        net.PutWeights(w);
        ++next;
        if (next >= populationSize)
        {
            if (allFitnessed()) Epoch();
            else --next;
        }
       
    }

    public void Feedback(int index, float fitness)
    {
        currentPopulation[index].fitness = fitness;
    }

    [Serializable]
    public class Genome : IComparable<Genome>
    {
        internal List<float> weights = new List<float>();
        internal float? fitness;

        internal Genome() { }

        public static bool operator >(Genome g1, Genome g2)
        {
            return g1.fitness > g2.fitness;
        }

        public static bool operator <(Genome g1, Genome g2)
        {
            return g1.fitness < g2.fitness;
        }

        int IComparable<Genome>.CompareTo(Genome other)
        {
            if (other > this) return -1;
            else if (this > other) return 1;
            else return 0;
        }

    }

    private void Epoch()
    {
   //     Debug.Log("[creator] it's an epoch");
        List<Genome> nextGeneration = new List<Genome>();

        float avg = 0.0f;
        foreach (var g in currentPopulation) avg += (float)g.fitness;
        avg /= currentPopulation.Count;

        currentPopulation.Sort();
     //   Debug.Log("avg fit " + avg + "best: " + currentPopulation[currentPopulation.Count - 1].fitness);

       // Debug.Log("worst: " + currentPopulation[0].fitness);
     //   Debug.Log("best: " + currentPopulation[currentPopulation.Count - 1].fitness);
        for(int i=0;i<elitism;++i)
        {
            nextGeneration.Add(currentPopulation[currentPopulation.Count - 1]);
            currentPopulation.RemoveAt(currentPopulation.Count - 1);
        }

        while(nextGeneration.Count < populationSize)
        {
            int m = Selection();
            Genome mum = currentPopulation[m];
         //   Debug.Log("mum " + mum.fitness);
            currentPopulation.RemoveAt(m);
            int d = Selection();
            Genome dad = currentPopulation[d];
          //  Debug.Log("dad " + dad.fitness);
            currentPopulation.RemoveAt(d);
            Genome child1, child2;
            Crossover(mum, dad, out child1, out child2);
            nextGeneration.Add(child1);
            nextGeneration.Add(child2);
         }

        currentPopulation = nextGeneration;
        currentGeneration++;
        next = 0;
    }


    int Selection()
    {
        float totalFitness = 0.0f;
        foreach (Genome g in currentPopulation) totalFitness += (float)g.fitness;
        float slice = UnityEngine.Random.Range(0.0f, totalFitness);
        float total = 0.0f;
        int ret = 0;
        for(int i=0;i<currentPopulation.Count;++i)
        {
            total += (float)currentPopulation[i].fitness;
            if (total >= slice)
            {
                ret = i;
                break;
            }
        }
        return ret;
    }

   void Crossover(Genome mum, Genome dad, out Genome baby1, out Genome baby2)
    {

       if(UnityEngine.Random.Range(0.0f,1.0f) > crossoverRate)
       {
           baby1 = mum;
           baby2 = dad;
       }
       else
       {
           baby1 = new Genome();
           baby2 = new Genome();
           int crossPointNumber = UnityEngine.Random.Range(1, 6);
           List<int> crossPoints = new List<int>();
           for(int i =0;i<crossPointNumber;++i) crossPoints.Add(UnityEngine.Random.Range(1,mum.weights.Count-1));
           crossPoints.Add(0);
           crossPoints.Add(mum.weights.Count);
           crossPoints.Sort();


           baby1.fitness = null;
           baby2.fitness = null;

           for (int j = 1; j <= crossPointNumber+1; ++j )
           {

               if(j%2==0)
               {
                   baby1.weights.AddRange(mum.weights.GetRange(crossPoints[j - 1], crossPoints[j] - crossPoints[j - 1]));
                   baby2.weights.AddRange(dad.weights.GetRange(crossPoints[j - 1], crossPoints[j] - crossPoints[j - 1]));
               }
               else
               {
                   baby2.weights.AddRange(mum.weights.GetRange(crossPoints[j - 1], crossPoints[j] - crossPoints[j - 1]));
                   baby1.weights.AddRange(dad.weights.GetRange(crossPoints[j - 1], crossPoints[j] - crossPoints[j - 1]));
               }

           }

           if (UnityEngine.Random.Range(0.0f, 1.0f) <= mutationRate) Mutate(ref baby1);
           if (UnityEngine.Random.Range(0.0f, 1.0f) <= mutationRate) Mutate(ref baby2);            
       }

    }

    void Mutate(ref Genome genome)
   {
      // Debug.Log("x-men");
       int mutationPoint = UnityEngine.Random.Range(0, genome.weights.Count);
       genome.weights[mutationPoint] = UnityEngine.Random.Range(minWeight, maxWeight);
   }

 


    private bool allFitnessed()
    {
        bool r = true;
        foreach (Genome g in currentPopulation) r &= (g.fitness != null);
        return r;
    }
}
