using System;
using System.Collections;

namespace Domain.Data.Core
{
    public interface IEntity
    {
        int Key { get; }
        bool IsNotDeleted { get; set; }
        int? LastChangedById { get; set; }
        DateTime LastChangedDate { get; set; }
        int? CreatedById { get; set; }
        DateTime? CreateDate { get; set; }
        State EntityState { get; set; }

        void WalkObjectGraph(Func<IEntity, bool> snippetForObject, Action<IList> snippetForCollection,
            params string[] exemptProperties);
    }
}