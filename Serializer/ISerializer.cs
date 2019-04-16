using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAMSS.Serializer
{
    public interface ISerializer<T> //where T : RecordBase, IList<RecordBase>, new()
    {
        void Serialize(T list);
        T Deserialize();
    }
}
