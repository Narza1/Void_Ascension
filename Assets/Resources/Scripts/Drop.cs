using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    // Start is called before the first frame update
    private string itemGui;
    private int quantity;
    private float probability;

    public Drop(string itemGui, int quantity, float probability)
    {
        this.itemGui = itemGui;
        this.quantity = quantity;
        this.probability = probability;
    }
}
