using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Synapse
{
    private string id;
    private string type;
    private Group target;
    private Group source;

    public bool connected;

    public GameObject connection;


    public Synapse(string id, string type, Group source, Group target)
    {
        this.id = id;
        this.type = type;
        this.target = target;
        this.source = source;
        connected = false;
        connection = Main.Instance.CreateConnection(source.getPos(), target.getPos());
    }

public GameObject getConnection(){
    return connection;
}

}
