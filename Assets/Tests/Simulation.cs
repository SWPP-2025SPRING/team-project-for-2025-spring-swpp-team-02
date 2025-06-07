using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class Simulation
{
    [UnityTest]
    public IEnumerator SimulationTest()
    {

        yield return new WaitForSeconds(1);
    }
}
