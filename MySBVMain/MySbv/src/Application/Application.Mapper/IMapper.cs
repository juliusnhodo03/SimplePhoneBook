namespace Application.Mapper
{
    public interface IMapper
    {
        /// <summary>
        ///     Map from type TSource to Type TDestination
        /// </summary>
        /// <typeparam name="TSource">Source Type to map from</typeparam>
        /// <typeparam name="TDestination">Destination type to map to</typeparam>
        /// <param name="entity">the entity with values to map</param>
        /// <returns>A mapped object</returns>
        TDestination Map<TSource, TDestination>(TSource entity);
    }
}