using Microsoft.EntityFrameworkCore;

namespace EticaretWebCoreEntity.Infrastructure
{
    public interface IBaseEntity
    {
        void Build(ModelBuilder builder);
    }
}