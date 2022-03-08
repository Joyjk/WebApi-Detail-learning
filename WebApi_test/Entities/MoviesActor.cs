namespace WebApi_test.Entities
{
    public class MoviesActor
    {
        public int PersonId { get; set; }
        public int MovieId { get; set; }
        public Person Person { get; set; }
        public Movie Movie { get; set; }
        public string Character { get; set; }
        public int Order { get; set; }

    }
}
