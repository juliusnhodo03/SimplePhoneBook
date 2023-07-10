using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace Domain.Data.Core
{
    [XmlRoot("MySBV")]
    public abstract class EntityBase : IEntity
    {
        #region Constructor

        protected EntityBase()
        {
            EntityState = State.Unchanged;
        }

        #endregion

        #region DB Properties

        public bool IsNotDeleted { get; set; }
        public int? LastChangedById { get; set; }
        public DateTime LastChangedDate { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreateDate { get; set; }

        #endregion

        #region Not Mapped

        [NotMapped]
        public abstract int Key { get; }

        [NotMapped]
        public State EntityState { get; set; }

        #endregion

        #region Methods

        void IEntity.WalkObjectGraph(Func<IEntity, bool> snippetForObject, Action<IList> snippetForCollection, params string[] exemptProperties)
        {
            var visited = new List<IEntity>();
            Action<IEntity> walk = null;

            var exemptions = new List<string>();
            if (exemptProperties != null)
                exemptions = exemptProperties.ToList();

            walk = o =>
            {
                if (o != null && !visited.Contains(o))
                {
                    visited.Add(o);

                    bool exitWalk = snippetForObject.Invoke(o);

                    if (!exitWalk)
                    {
                        PropertyInfo[] properties = o.GetBrowsableProperties();
                        foreach (PropertyInfo property in properties)
                        {
                            if (!exemptions.Contains(property.Name))
                            {
                                if (property.PropertyType.IsSubclassOf(typeof (IEntity)))
                                {
                                    var obj = (IEntity) (property.GetValue(o, null));
                                    walk(obj);
                                }
                                else
                                {
                                    var coll = property.GetValue(o, null) as IList;
                                    if (coll != null)
                                    {
                                        snippetForCollection.Invoke(coll);

                                        foreach (object item in coll)
                                        {
                                            if (item is IEntity)
                                                walk((IEntity) item);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            walk(this);
        }

        #endregion
    }

}