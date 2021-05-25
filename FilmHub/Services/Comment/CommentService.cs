using Database.Comment;

namespace FilmHub.Services.Comment
{
    public class CommentService : ICommentService
    {
        
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public void ToLikeAComment(int commentId)
        {
            _commentRepository.ToLikeAComment(commentId);
        }
    }
}