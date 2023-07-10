namespace Vault.Integration.DataContracts
{
    public class ValidationResult
    {
        /// <summary>
        ///     Result of the validation
        /// </summary>
        public bool Result { get; set; }

        /// <summary>
        ///     Details of the validation
        /// </summary>
        public ValidationError ValidationError { get; set; }
    }
}