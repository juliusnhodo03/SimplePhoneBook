using System;
using Domain.Data.Core;

namespace Domain.Data.Hyphen.Model
{
    public class HyphenScheduler : EntityBase
    {
        public int HyphenSchedulerId { get; set; }
        public int BatchNumber { get; set; }
        public string NumberOfDepositsSent { get; set; }
        public DateTime LastRan { get; set; }

        public override int Key
        {
            get { return HyphenSchedulerId; }
        }
    }
}