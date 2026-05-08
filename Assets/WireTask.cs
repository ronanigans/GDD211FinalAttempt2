using UnityEngine;
using System.Collections.Generic;

public class WireTask : MonoBehaviour
{
    public List<Wire> leftWires;
    public List<Wire> rightWires;
    
    private int connectedWires = 0;

    // Call this when a pair is correctly connected
    public void RegisterConnection()
    {
        connectedWires++;
        if (connectedWires >= leftWires.Count)
        {
            Debug.Log("All wires connected! Task Complete.");
            // Trigger win state
        }
    }
} 