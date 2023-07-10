namespace Domain.Serializer
{
    public interface ISerializer
    {
        /// <summary>
        ///     Serialize and object in to type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectToSerialize"></param>
        /// <returns></returns>
        string Serialize<T>(T objectToSerialize) where T : class;

        /// <summary>
        ///     Desirialize from object to a type of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectToDesirialize"></param>
        /// <returns></returns>
        T Deserialize<T>(object objectToDesirialize) where T : class;
    }
}