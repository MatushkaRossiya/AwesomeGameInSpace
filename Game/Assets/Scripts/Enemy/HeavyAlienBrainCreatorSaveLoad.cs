using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Collections.Generic;

//Boring part

public partial class HeavyAlienBrainCreator : MonoSingleton<HeavyAlienBrainCreator>
{
    private string path = "brainz.dat";

    void Awake()
    {
        if (File.Exists(path)) Load();

        else Initialize();

    }


    void OnApplicationQuit()
    {
        Save();
    }

    private void Save()
    {
        SaveFormat save = new SaveFormat(populationSize, currentGeneration, next,
            hiddenLayersPerNetwork, neuronsPerHiddenLayer,
            inputs, outputs,
            minWeight, maxWeight,
            bias,
             currentPopulation);
        FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
        BinaryFormatter bin = new BinaryFormatter();
        try
        {
            bin.Serialize(fs, save);
        }
        catch (SerializationException e)
        {
            Debug.Log("[Brain Creator] Save fail!  " + e.Message);
            throw;
        }
        finally
        {
            fs.Close();
        }
    }

    private void Load()
    {
        FileStream fs = new FileStream(path, FileMode.Open);
        BinaryFormatter bin = new BinaryFormatter();
        SaveFormat load = (SaveFormat)bin.Deserialize(fs);
        if (load.hiddenlayers == HeavyAlienBrainCreator.instance.hiddenLayersPerNetwork && load.neuronsinlayer == HeavyAlienBrainCreator.instance.neuronsPerHiddenLayer
            && load.inputs == HeavyAlienBrainCreator.instance.inputs && load.outputs == HeavyAlienBrainCreator.instance.outputs
            && load.minW == HeavyAlienBrainCreator.instance.minWeight && load.maxW == HeavyAlienBrainCreator.instance.maxWeight
            && load.bias == HeavyAlienBrainCreator.instance.bias)
        {
            populationSize = load.populationsize;
            currentGeneration = load.epoch;
            next = load.next;
            currentPopulation = load.population;

        }
        else Initialize();
       
    }

    [Serializable]
    class SaveFormat
    {
        internal SaveFormat(int p, int e, int n, int h, int nl, int i, int o, float min, float max, float b, List<Genome> g)
        {
            populationsize = p;
            epoch = e;
            population = g;
            next = n;
            neuronsinlayer = nl;
            hiddenlayers = h;
            inputs = i;
            outputs = o;
            minW = min;
            maxW = max;
            bias = b;
        }
        internal int inputs;
        internal int outputs;
        internal int populationsize;
        internal int hiddenlayers;
        internal int neuronsinlayer;
        internal int epoch;
        internal int next;
        internal float minW;
        internal float maxW;
        internal float bias;
        internal List<Genome> population = new List<Genome>();
    }

  

}
