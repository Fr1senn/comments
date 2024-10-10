using commentsAPI.Entities.DTOs;
using commentsAPI.Entities.Requests;
using commentsAPI.Entities.Shared;
using commentsAPI.Entities.Shared.Filters;
using commentsAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace commentsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;

        public CommentController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        [HttpGet("all")]
        public async Task<ApiResponse<PaginatedResult<IEnumerable<CommentDTO>>>> GetComments([FromQuery] PaginationFilter filter)
        {
            var paginatedComments = await _commentRepository.GetCommentsAsync(filter);
            return ApiResponse<PaginatedResult<IEnumerable<CommentDTO>>>.Succeed(HttpStatusCode.OK, paginatedComments);
        }


        [HttpGet("details")]
        public async Task<ApiResponse<CommentDetailsDTO>> GetCommentById([FromQuery] Guid id, [FromQuery] PaginationFilter filter)
        {
            var comment = await _commentRepository.GetCommentById(id, filter);
            return ApiResponse<CommentDetailsDTO>.Succeed(HttpStatusCode.OK, comment);
        }

        [HttpPost("create")]
        public async Task<ApiResponse> CreateComment([FromBody] CommentRequest request)
        {
            await _commentRepository.CreateCommentAsync(request);
            return ApiResponse.Succeed(HttpStatusCode.OK);
        }
    }
}
