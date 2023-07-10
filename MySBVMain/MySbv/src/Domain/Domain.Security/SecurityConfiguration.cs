namespace Domain.Security
{
    public class SecurityConfiguration
    {
        /// <summary>
        ///     Initialized the Web Security API
        /// </summary>
        /// <param name="security">An Instance of the Web Security Wrapper</param>
        /// <param name="connectionString">Db Connection String</param>
        /// <param name="userTableName">Name of the table where user infomation will be stored</param>
        /// <param name="userIdField">
        ///     Name of the primaryKey(userId) fild in the <userTable />
        /// </param>
        /// <param name="userNameField">
        ///     Username field in the <userTable />
        /// </param>
        /// <param name="autoCreateTables">Boolean flag for Auto Creation of tables</param>
        public void RegisterSecurity(ISecurity security, string connectionString, string userTableName,
            string userIdField, string userNameField, bool autoCreateTables = true)
        {
            security.InitializeDatabaseConnection(connectionString, userTableName, userIdField, userNameField,
                autoCreateTables);
        }
    }
}