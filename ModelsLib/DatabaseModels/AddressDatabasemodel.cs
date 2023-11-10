namespace ModelsLib.DatabaseModels
{
    public class AddressDatabasemodel
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastChangedAt { get; set; }
        public string Vej { get; set; }
        public string City { get; set; }
        public int Zip_code { get; set; }
        public string Number { get; set; }
    }
}