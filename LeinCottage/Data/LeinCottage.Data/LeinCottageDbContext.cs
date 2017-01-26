namespace LeinCottage.Data
{
    using Models;
    using System.Data.Entity;
    public class LeinCottageDbContext: DbContext
    {
        public LeinCottageDbContext()
            : base("LeinCottageDataBase")
        {
        }

        public virtual IDbSet<Photo> Photos { get; set; }
    }
}
