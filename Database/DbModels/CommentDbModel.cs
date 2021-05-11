namespace Database.DbModels
{
    public class CommentDbModel
    {
        public int Id { get; set; }
        public UserDbModel User { get; set; }
        public FilmDbModel Film { get; set; }
        public string Text { get; set; }
    }
}