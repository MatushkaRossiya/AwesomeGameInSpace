using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class NeuralNetwork  {


    public int Inputs;
    public int Outputs;
    public int NeuronsInLayer;
    public int HiddenLayers;

    public List<NeuralLayer> hiddenLayers;
    public NeuralLayer outputLayer;
    public NeuralLayer firstHiddenLayer;


    public int genomeIndex;

    public NeuralNetwork()
    {       
    }

    public void IntializeNetwork()
    {
        Inputs = HeavyAlienBrainCreator.instance.inputs;
        Outputs = HeavyAlienBrainCreator.instance.outputs;
        NeuronsInLayer = HeavyAlienBrainCreator.instance.neuronsPerHiddenLayer;
        HiddenLayers = HeavyAlienBrainCreator.instance.hiddenLayersPerNetwork;
        hiddenLayers = new List<NeuralLayer>();
        firstHiddenLayer = new NeuralLayer(Inputs, NeuronsInLayer);
      //  Debug.Log("[network] neurons in layer" + NeuronsInLayer);
        outputLayer = new NeuralLayer(NeuronsInLayer, Outputs);
        for (int i = 0; i < HiddenLayers - 1; ++i) hiddenLayers.Add(new NeuralLayer(NeuronsInLayer, NeuronsInLayer));
    }

    public void Fitness(float t, float d, bool alive=false)
    {
        float fitness = 0.0f;
        fitness += (10.0f / (1.0f + Mathf.Exp(-d / 50.0f)));
        fitness += (1.0f / (1.0f + Mathf.Exp(-t * 0.1f)));
        if (alive) fitness += 5.0f;

        HeavyAlienBrainCreator.instance.Feedback(genomeIndex, fitness);
    }

    public void PutWeights(List<float> weights)
    {


        int expectedWeights = (Inputs + 1) * NeuronsInLayer + (NeuronsInLayer + 1) * NeuronsInLayer * (HiddenLayers - 1) + Outputs * (NeuronsInLayer + 1);
        if(weights.Count != expectedWeights)
        {
            Debug.Log("[Neural Network] putting weights error! incorrect number of weights. Excepted: " + expectedWeights + " got: " + weights.Count);
            return;
        }

        int w = 0;
        
        foreach(Neuron neuron in firstHiddenLayer.neurons)
        {
        //    Debug.Log("[network] neuron weights  " + neuron.weights.Count);
          //  Debug.Log("[network] 1st hidden layer neuron");
            for(int i=0;i<neuron.weights.Count;++i)
            {
              //  Debug.Log("[network] " + weights.Count + "  " + w);

                neuron.weights[i] = weights[w];
                ++w;
            }
        }

        foreach(NeuralLayer layer in hiddenLayers)
        {
         //   Debug.Log("[network] n hidden layer neuron");

            foreach (Neuron neuron in layer.neurons)
            {
                for (int i = 0; i < neuron.weights.Count; ++i)
                {
            //        Debug.Log("[network] " + weights.Count + "  " + w);

                    neuron.weights[i] = weights[w];
                    ++w;
                }
            }
        }

        foreach (Neuron neuron in outputLayer.neurons)
        {
        //    Debug.Log("[network] out layer neuron");

            for (int i = 0; i < neuron.weights.Count; ++i)
            {
        //        Debug.Log("[network] " + weights.Count + "  " + w);
                neuron.weights[i] = weights[w];
                ++w;
            }
        }

    }

    public List<float> NetworkResponse(List<float> inputs)
    {
        //    Debug.Log("[network] inputs " + inputs.Count);
            List<float> previousResponse = firstHiddenLayer.LayerResponse(inputs);
        //   Debug.Log("[network] response first hidden " + previousResponse.Count);
            for(int i=0; i < HiddenLayers-1; ++i)
            {
                List<float> tmp = hiddenLayers[i].LayerResponse(previousResponse);
                previousResponse = tmp;
            }

            return outputLayer.LayerResponse(previousResponse);

        }
    

}
