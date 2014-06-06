using UnityEngine;
using System;
using System.Collections.Generic;


public class NeuralLayer
{
    private int numberOfInputs;

    public List<Neuron> neurons;
    public NeuralLayer(int NumberOfInputs, int NumberOfOutputs)
    {
        numberOfInputs = NumberOfInputs;
        neurons = new List<Neuron>();
        for (int i = 0; i < NumberOfOutputs; ++i) neurons.Add(new Neuron(numberOfInputs));
      //  Debug.Log("[layer] inputz " + numberOfInputs + "  " + NumberOfInputs);
    }

    public List<float> LayerResponse(List<float> inputs)
    {
        if (inputs.Count  != numberOfInputs)
        {
            Debug.Log("[Layer] Error! incorrect number of inputs expected: " + numberOfInputs + " got:  " + inputs.Count);
            return null;
        }

        List<float> outputs = new List<float>();

      //  Debug.Log("[layer] neurons " + neurons.Count);

        foreach (Neuron neuron in neurons)
        {
            float r = neuron.NeuronResponse(inputs);
           // Debug.Log("[layer] neu res " + r);
            outputs.Add(r);
        }
        return outputs;

    }

}

