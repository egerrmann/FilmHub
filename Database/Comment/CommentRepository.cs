using System;
using System.Collections.Generic;
using System.Linq;
using Database.DbModels;
using Database.User;
using Microsoft.EntityFrameworkCore;


namespace Database.Comment
{
    public class CommentRepository : ICommentRepository
    {
        private readonly IAppContext _dbContext;
        public CommentRepository(IAppContext data)
        {
            _dbContext = data;
        }
        
        public void ToLikeAComment(int commentId)
        {
            CommentDbModel currentComment = _dbContext.Comments.Find(commentId);
            currentComment.Likes += 1;
            _dbContext.SaveChanges();
        }
    }
}