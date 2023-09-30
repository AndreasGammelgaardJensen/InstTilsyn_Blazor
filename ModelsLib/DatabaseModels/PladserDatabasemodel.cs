namespace ModelsLib.DatabaseModels
{
    public class PladserDatabasemodel
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastChangedAt { get; set; }
        public int VuggestuePladser { get; set; }
        public int BoernehavePladser { get; set; }
        public int DagplejePladser { get; set; }

    }
}