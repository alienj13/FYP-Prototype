using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Synapse
{
    private string id;
    private string type;
    private Group target;
    private Group source;


    public Synapse(string id, string type, Group source, Group target)
    {
        this.id = id;
        this.type = type;
        this.target = target;
        this.source = source;
    }

}
