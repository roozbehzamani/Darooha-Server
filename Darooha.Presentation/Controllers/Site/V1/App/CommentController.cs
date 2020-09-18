using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Darooha.Data.DatabaseContext;
using Darooha.Data.Dtos.Site.App.Comment;
using Darooha.Data.Models;
using Darooha.Presentation.Helpers.Filters;
using Darooha.Presentation.Routes.V1;
using Darooha.Repo.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Darooha.Presentation.Controllers.Site.V1.App
{
    [AllowAnonymous]
    [ApiExplorerSettings(GroupName = "v1_Site_App")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IUnitOfWork<DaroohaDbContext> _db;
        private readonly IMapper _mapper;

        public CommentController(IUnitOfWork<DaroohaDbContext> dbContext, IMapper mapper)
        {
            _db = dbContext;
            _mapper = mapper;
        }

        [HttpGet(ApiV1Routes.Comment.GetCommentList)]
        public async Task<IActionResult> GetCommentList(string id)
        {
            var getComments = (await _db.CommentRepository.GetManyAsync(p => p.ProductId.Equals(id) && p.commentConfirm, l => l.OrderByDescending(x => x.DateCreated), ""));
            var allComments = _mapper.Map<IEnumerable<Tbl_Comment>, List<CommentForReturnDto>>(getComments);
            foreach (var item in allComments)
            {
                var user = await _db.UserRepository.GetByIdAsync(item.UserId);
                item.UserName = user.FirstName + " " + user.LastName;
                item.UserImageUrl = user.ImageURL;
            }
            return Ok(allComments);
        }

        [Authorize(Policy = "RequireUserRole")]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        [HttpPost(ApiV1Routes.Comment.AddComment)]
        public async Task<IActionResult> AddComment(string id, string userId, CommentForCreateDto commentForCreateDto)
        {
            var commentToCreate = new Tbl_Comment
            {
                UserId = userId,
                ProductId = id,
                CommentResponse = "none",
                commentConfirm = true,
            };
            var comment = _mapper.Map(commentForCreateDto, commentToCreate);
            await _db.CommentRepository.InsertAsync(comment);

            if (await _db.SaveAsync())
            {
                var findProduct = (await _db.ProductRepository.GetByIdAsync(id));
                findProduct.CommentCount++;
                findProduct.SumPoint += comment.CommentPoint;
                _db.ProductRepository.Update(findProduct);
                if (await _db.SaveAsync())
                {
                    var finalComment = _mapper.Map<CommentForReturnDto>(comment);
                    var user = await _db.UserRepository.GetByIdAsync(comment.UserId);
                    finalComment.UserName = user.FirstName + " " + user.LastName;
                    finalComment.UserImageUrl = user.ImageURL;
                    return Ok(finalComment);
                    //return CreatedAtRoute("GetComment", new { id = comment.ID, userId = userId }, comment);
                }
                else
                {
                    _db.CommentRepository.Delete(comment.ID);
                    if (await _db.SaveAsync())
                    {
                        return BadRequest("نظر ثبت نشد");
                    }
                    else
                    {
                        return BadRequest("عملیات نظر با خطا مواجه شد");
                    }
                }
            }
            else
            {
                return BadRequest("خطا در ذخیره اطلاعات");
            }
        }

        [Authorize(Policy = "RequireUserRole")]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        [HttpGet(ApiV1Routes.Comment.GetComment, Name = "GetComment")]
        public async Task<IActionResult> GetComment(string id, string userId)
        {
            var commentFromRepo = await _db.CommentRepository.GetByIdAsync(id);
            if (commentFromRepo != null)
            {
                if (commentFromRepo.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value)
                {
                    var comment = _mapper.Map<CommentForReturnDto>(commentFromRepo);
                    var user = await _db.UserRepository.GetByIdAsync(comment.UserId);
                    comment.UserName = user.FirstName + " " + user.LastName;
                    comment.UserImageUrl = user.ImageURL;
                    return Ok(comment);
                }
                else
                {
                    return BadRequest("شما اجازه دسترسی به کامنت دیگری را ندارید");
                }
            }
            else
            {
                return BadRequest("کامنتی با این مشخصات وجود ندارد");
            }
        }
    }
}