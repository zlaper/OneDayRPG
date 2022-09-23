using System.Collections.Generic;

namespace zg.gramrpg.serialize
{
    public interface ISerializable
    {
        Dictionary<string, object> Serialize();
        void Deserialize(Dictionary<string, object> data);
    }
}
