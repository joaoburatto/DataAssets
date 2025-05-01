#if ENABLE_DATA_ASSET_SAMPLE
using System.Collections.Generic;

namespace DataAssetsPackage.Sample
{
    public class DefaultNamesDataAsset : DataAsset
    {
        public List<string> Names = new List<string>
        {
            "John",
            "Jane",
            "Doe",
            "Alice",
            "Bob"
        };
    }
}
#endif