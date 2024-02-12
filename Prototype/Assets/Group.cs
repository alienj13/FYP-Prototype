using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group
{
    private string Name { get; }
    // private Neuron[][] neurons;
    private int[,] test;
    private GameObject[,] cubes;
    private string id;
    private List<double> data = new List<double>();
    private List<Synapse> connections = new List<Synapse>();
    private int xcount;
    private int ycount;
    System.Random random = new System.Random();
    private int pos;

    private Vector3 centerPosition;


    public Group(string id, string name, int xcount, int ycount, int pos)
    {
        cubes = new GameObject[xcount, ycount];
        this.pos = pos;
        this.id = id;
        this.Name = name;
        // neurons = new Neuron[xcount][ycount];
        test = new int[xcount, ycount];
        this.xcount = xcount;
        this.ycount = ycount;

        for (int i = 0; i < xcount; i++)
        {
            for (int j = 0; j < ycount; j++)
            {
                cubes[i, j] = Main.Instance.create(pos, i, j, xcount,ycount);
            }
        }
          foreach (Synapse s in connections)
          {
            if(!s.connected){

            }
          }
    }

  


public string getID(){
    return id;
}

public int getPos(){
    return pos;
}

public string getName(){
    return Name;
}

    public void addData(double d)
    {
        data.Add(d);
    }

    public int getDataLength()
    {
        return data.Count;
    }

    public List<double> getData()
    {
        return data;
    }

 public void addConnection(Synapse s)
    {
        connections.Add(s);
    }

    public void Start()
    {
        Main.Instance.Turnoff(cubes, xcount, ycount);
        test = new int[xcount, ycount];

        double probability = data[0];
        data.RemoveAt(0);

        int count = (int)(test.Length * probability);

        Debug.Log("Number of neurons firing: " + count + "   " + id + "  " + probability);
        while (count > 0)
        {
            int rand = random.Next(test.GetLength(1));
            int rand2 = random.Next(test.GetLength(1));

            if (test[rand, rand2] == 0)
            {
                test[rand, rand2] = 1;
                Main.Instance.Turnon(cubes[rand, rand2]);
                count--;
            }
        }

    }
}
