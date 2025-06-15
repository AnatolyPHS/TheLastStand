using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

public class ExampleTest
{
    //TODO: cover the managers with tests
    [Test]
    public void ExampleTestSimplePasses()
    {
        int value1 = 2;
        int value2 = 2;
        
        Assert.AreEqual(value1, value2, "Values are not equal");
    }
    
    [UnityTest]
    public IEnumerator ExampleTestWithEnumeratorPasses()
    {
        int value1 = 3;
        int value2 = 3;
        
        yield return null;
        
        Assert.AreEqual(value1, value2, "Values are not equal after a frame delay");
    }
}
