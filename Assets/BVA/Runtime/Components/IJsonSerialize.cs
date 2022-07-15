using Newtonsoft.Json.Linq;
using System;

public interface IJsonSerialize
{
    JObject Serialize();
    void Deserialize();
}
