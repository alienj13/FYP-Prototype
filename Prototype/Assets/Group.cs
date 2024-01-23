using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group 
{
    private string Name;
   // private Neuron[][] neurons;
    private int[,] test;
    private GameObject[,] cubes;
    private string id;
    private List<double> data = new List<double>();
    private int xcount;
    private int ycount;
    System.Random random = new System.Random();
    private int pos;
    



    // Start is called before the first frame update
    public Group(string id, string name, int xcount, int ycount,int pos)
    {
        cubes = new GameObject[xcount, ycount];
        this.pos = pos;
        this.id = id;
        this.Name = name;
       // neurons = new Neuron[xcount][ycount];
        test = new int[xcount,ycount];
        this.xcount = xcount;
        this.ycount = ycount;

        for (int i = 0; i< xcount;i++) {
            for(int j = 0; j < ycount;j++) {
               cubes[i,j] =  Main.Instance.create(pos,i,j,xcount);
            }
        }
    
    }

    public override bool Equals(object obj)
    {
        // You can implement more complex logic depending on what defines equality for your class
        if (obj is Group other)
        {
            return this.id == other.getGroupId(); // Assuming 'Id' is a unique identifier of Group
        }
        return false;
    }

    public override int GetHashCode()
    {
        return this.id.GetHashCode(); // Again, assuming 'Id' is a unique identifier
    }

    public string getGroupName()
    {
        return Name;
    }

 
    public string getGroupId()
    {
        return id;
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

   

    public void Start()
    {
        Main.Instance.Turnoff(cubes,xcount,ycount);
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
                Main.Instance.Turnon(cubes[rand,rand2]);
                    count--;
                }
            }
        
	}
}
