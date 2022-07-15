using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
public class JsonUtilityTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void JsonUtilityTestSimplePasses()
    {

        Debug.Log(JsonUtility.ToJson(0.1f));
        Debug.Log(JsonUtility.ToJson(Vector2.one));
        Debug.Log(JsonUtility.ToJson(Vector3.one));
        Debug.Log(JsonUtility.ToJson(Color.blue));
        Debug.Log(JsonUtility.ToJson(Matrix4x4.identity));
        Debug.Log(JsonUtility.ToJson(Texture2D.whiteTexture));
    }
    [Test]
    public void JObjectAdd()
    {
        JObject jo = new JObject();
        jo.Add(new JProperty("x", 1));
        jo.Add(new JProperty("y", 2));
        jo.Add(new JProperty("z", 3));
        Debug.Log(jo.ToString());
    }
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator JsonUtilityTestWithEnumeratorPasses()
    {

        yield return null;
    }
}
