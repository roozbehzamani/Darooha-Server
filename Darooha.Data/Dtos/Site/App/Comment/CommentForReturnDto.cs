using System;
using System.Collections.Generic;
using System.Text;

namespace Darooha.Data.Dtos.Site.App.Comment
{
    public class CommentForReturnDto
    {
        public string ID { get; set; }
        public string CommentText { get; set; }
        public int CommentPoint { get; set; }
        public string CommentResponse { get; set; }
        public int SumPoint { get; set; }
        public int CommentCount { get; set; }
        public string DateModified { get; set; }
        public string UserName { get; set; }
        public string UserImageUrl { get; set; }
        public string UserId { get; set; }
    }
}
