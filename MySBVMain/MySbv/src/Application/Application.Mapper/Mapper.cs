using System.ComponentModel.Composition;

namespace Application.Mapper
{
    [Export(typeof(IMapper))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class Mapper : IMapper
    {
        public TDestination Map<TSource, TDestination>(TSource entity)
        {
            return AutoMapper.Mapper.Map<TSource, TDestination>(entity);
        }
    }
}
