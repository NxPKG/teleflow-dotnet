using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Teleflow.DTO.Workflows;

namespace Teleflow.Sync.Comparers;

public class WorkflowDetailsComparer : IEqualityComparer<Workflow>
{
    public bool Equals(Workflow x, Workflow y)
    {
        if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
        {
            return false;
        }

        return x.Active == y.Active &&
               x.Description == y.Description &&
               // inefficient but needing to check if the payloads are different
               // TODO: may want to inject serialisation setting to be consistent
               JsonConvert.SerializeObject(x.PreferenceSettings, TeleflowClient.DefaultSerializerSettings)
                   .Equals(JsonConvert.SerializeObject(y.PreferenceSettings, TeleflowClient.DefaultSerializerSettings)) &&
               JsonConvert.SerializeObject(x.Steps, TeleflowClient.DefaultSerializerSettings)
                   .Equals(JsonConvert.SerializeObject(y.Steps, TeleflowClient.DefaultSerializerSettings));
    }

    public int GetHashCode(Workflow obj)
    {
        var hashCode = new HashCode();
        hashCode.Add(obj.Name);
        return hashCode.ToHashCode();
    }
}