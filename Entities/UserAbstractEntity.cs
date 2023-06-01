namespace ArtisanELearningSystem.Entities
{
    public class UserAbstractEntity
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }

        public DateTime DateCreated = DateTime.Now;
    }
}
