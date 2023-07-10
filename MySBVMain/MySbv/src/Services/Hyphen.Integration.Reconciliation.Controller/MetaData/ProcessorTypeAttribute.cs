using System;
using System.ComponentModel.Composition;

namespace Hyphen.Integration.Reconciliation.Controller.MetaData
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ProcessorTypeAttribute : ExportAttribute
    {
        #region Constructor

        public ProcessorTypeAttribute()
            : base(typeof (IProcessorTypeMetadata))
        {
        }

        #endregion

        #region IProcessorMetadata Implementation

        public ProcessorType ProcessorType { get; set; }

        #endregion
    }
}