using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectGeneration;

public class TestGenerate : MonoBehaviour
{
    private void Start()
    {
        ObjectGeneratorMain.instance.SetupObjectGeneration();
    }
}
