using System.Collections.Generic;

namespace Teleflow.Sync.Utils;

public class UpdateChangeSet<T>
{
    public IEnumerable<T> ChangeTo { get; set; }
    public IEnumerable<T> Original { get; set; }
}