namespace LeinCottage.Data
{
    using Models;
    using System.Data.Entity;
    public class LeinCottageDbContext: DbContext
    {
        public virtual IDbSet<Photo> Photos { get; set; }
    }
}
