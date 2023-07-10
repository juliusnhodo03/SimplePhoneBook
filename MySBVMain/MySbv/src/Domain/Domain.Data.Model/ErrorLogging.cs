using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Data.Model
{
    [Table("ErrorLogging")]
    public class ErrorLogging
    {
        #region Mapped

        public int ErrorLoggingId { get; set; }

        [MaxLength(20)]
        public string Host { get; set; }

        [MaxLength(20)]
        public string Version { get; set; }

        public string Exception { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }

        [MaxLength(10)]
        public string Level { get; set; }

        [MaxLength(150)]
        public string Logger { get; set; }

        #endregion
    }
}