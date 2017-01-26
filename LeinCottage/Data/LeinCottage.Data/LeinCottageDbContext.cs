namespace LeinCottage.Data
{
    using System.Data.Entity;
    using Models;

    public class LeinCottageDbContext: DbContext
    {
        public LeinCottageDbContext()
            :base("LeinCottageDataBase")
        {
        }

        public virtual IDbSet<Photo> Photos { get; set; }
    }
}
