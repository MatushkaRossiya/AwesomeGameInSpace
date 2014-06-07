using UnityEngine;
using System;
using System.Collections.Generic;

public class Neuron
{
    public List<float> weights;
    private int numberOfInputs;

    public Neuron(int NumberOfInputs)
    {
        numberOfInputs = NumberOfInputs;
        weights = new List<float>();
        for (int i = 0; i < numberOfInputs + 1; ++i)
            weights.Add(new float());
    }

    public float NeuronResponse(List<float> inputs)
    {
        if (inputs.Count != numberOfInputs)
        {
            Debug.Log("[Neuron] Error! incorrect number of inputs");
            return 0.0f;
        }

        float response = 0.0f;

        // Debug.Log("[neuron] w" + weights.Count + "  inp " + inputs.Count);
        for (int i =0; i<numberOfInputs; ++i)
        {
            // Debug.Log("[neuron]  " + i);
            //   Debug.Log("[neuron]  " + inputs[i]);
            //   Debug.Log("[neuron]  " + weights[i]);
            response += inputs [i] * weights [i];
        }

        response += HeavyAlienBrainCreator.instance.bias * weights [numberOfInputs];
        // Debug.Log("[neuron] response " + response);
        return response;
    }
}

