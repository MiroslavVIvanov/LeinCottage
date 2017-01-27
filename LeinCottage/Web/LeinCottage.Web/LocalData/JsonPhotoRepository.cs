namespace LeinCottage.Web.LocalData
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Newtonsoft.Json;
    using Models;
    using Data;

    public class JsonPhotoRepository<T> : IRepository<T> where T : Photo
    {
        private static volatile JsonPhotoRepository<T> instance;
        private static object syncRoot = new object();
        public static JsonPhotoRepository<T> Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new JsonPhotoRepository<T>();
                    }
                }

                return instance;
            }
        }

        private static int idGenerator = 0;
        private const string LocalStorageFileName = "LocalJsonStorage.json";
        private readonly string filePath = 
            System.Web.Hosting.HostingEnvironment.MapPath(@"~/App_Data/" + LocalStorageFileName);

        public JsonPhotoRepository()
        {
            if (!File.Exists(filePath))
            {
                File.Create(filePath);
                this.AllObjects = new List<T>();
            }
            else
            {
                string alltext = string.Empty;
                using (StreamReader sr = new StreamReader(filePath))
                {
                    alltext = sr.ReadToEnd();
                }

                this.AllObjects = JsonConvert.DeserializeObject<List<T>>(alltext);
                if (this.AllObjects == null)
                {
                    this.AllObjects = new List<T>();
                }
            }
        }

        private List<T> AllObjects { get; set; }

        public void Add(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Can not add null to the json");
            }

            entity.Id = IdGenerator();

            this.AllObjects.Add(entity);
        }

        public IQueryable<T> All()
        {
            return this.AllObjects.AsQueryable<T>();
        }

        public T Attach(T entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(object id)
        {
            this.AllObjects.Remove(this.AllObjects.FirstOrDefault(p => p.Id == (int)id));
        }

        public void Delete(T entity)
        {
            this.AllObjects.Remove(entity);
        }

        public void Detach(T entity)
        {
            throw new NotImplementedException();
        }

        public T GetById(object id)
        {
            return this.AllObjects.FirstOrDefault(p => p.Id == (int)id);
        }

        public int SaveChanges()
        {
            File.WriteAllText(filePath, JsonConvert.SerializeObject(this.AllObjects));
            return 0;
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }

        private static int IdGenerator()
        {
            return idGenerator += 1;
        }
    }
}
