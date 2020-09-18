﻿using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Darooha.Data.DatabaseContext;
using Darooha.Data.Dtos.Site.Panel.Ticket;
using Darooha.Data.Models;
using Darooha.Presentation.Helpers.Filters;
using Darooha.Presentation.Routes.V1;
using Darooha.Repo.Infrastructure;
using Darooha.Services.Upload.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Darooha.Presentation.Controllers.Site.V1.User
{
    [ApiExplorerSettings(GroupName = "v1_Site_Panel")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly IUnitOfWork<DaroohaDbContext> _db;
        private readonly IMapper _mapper;
        private readonly ILogger<TicketsController> _logger;
        private readonly IUploadService _uploadService;
        private readonly IWebHostEnvironment _env;

        public TicketsController(IUnitOfWork<DaroohaDbContext> dbContext, IMapper mapper,
            ILogger<TicketsController> logger, IUploadService uploadService,
            IWebHostEnvironment env)
        {
            _db = dbContext;
            _mapper = mapper;
            _logger = logger;
            _uploadService = uploadService;
            _env = env;
        }


        [Authorize(Policy = "RequireUserRole")]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        [HttpGet(ApiV1Routes.Ticket.GetTickets)]
        public async Task<IActionResult> GetTickets(string userId, int page = 0)
        {
            var ticketsFromRepo = (await _db.TicketRepository
                .GetManyAsyncPaging(p => p.UserId == userId, s => s.OrderBy(x => x.Closed).ThenByDescending(x => x.DateModified), "",
                10, 0, page));

            return Ok(ticketsFromRepo);
        }

        [Authorize(Policy = "RequireUserRole")]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        [HttpGet(ApiV1Routes.Ticket.GetTicket, Name = "GetTicket")]
        public async Task<IActionResult> GetTicket(string id, string userId)
        {
            var ticketFromRepo = (await _db.TicketRepository.GetManyAsync(p => p.ID == id, null, "TicketContents"))
                .SingleOrDefault();
            if (ticketFromRepo != null)
            {
                if (ticketFromRepo.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value)
                {
                    ticketFromRepo.TicketContents.OrderByDescending(p => p.DateCreated);
                    return Ok(ticketFromRepo);
                }
                else
                {
                    _logger.LogError($"کاربر   {userId} قصد دسترسی به تیکت دیگری را دارد");

                    return BadRequest("شما اجازه دسترسی به تیکت کاربر دیگری را ندارید");
                }
            }
            else
            {
                return BadRequest("تیکتی وجود ندارد");
            }

        }

        [Authorize(Policy = "RequireUserRole")]
        [HttpPost(ApiV1Routes.Ticket.AddTicket)]
        public async Task<IActionResult> AddTicket(string userId, [FromForm]TicketForCreateDTO ticketForCreateDto)
        {
            var ticketFromRepo = await _db.TicketRepository
                .GetAsync(p => p.Title == ticketForCreateDto.Title && p.UserId == userId);

            if (ticketFromRepo == null)
            {
                var ticket = new Ticket()
                {
                    UserId = userId,
                    Closed = false,
                    IsAdminSide = false
                };

                _mapper.Map(ticketForCreateDto, ticket);

                await _db.TicketRepository.InsertAsync(ticket);

                if (await _db.SaveAsync())
                {
                    var ticketContent = new TicketContent()
                    {
                        TicketId = ticket.ID,
                        IsAdminSide = false,
                        Text = ticketForCreateDto.Text
                    };
                    if (ticketForCreateDto.File != null)
                    {
                        if (ticketForCreateDto.File.Length > 0)
                        {
                            var uploadRes = await _uploadService.UploadProfilePicToLocal(
                                ticketForCreateDto.File,
                                userId,
                                _env.WebRootPath,
                                $"{Request.Scheme ?? ""}://{Request.Host.Value ?? ""}{Request.PathBase.Value ?? ""}",
                                "Files\\TicketContent"
                            );
                            if (uploadRes.Status)
                            {
                                ticketContent.FileUrl = uploadRes.Url;
                            }
                            else
                            {
                                _db.TicketRepository.Delete(ticket.ID);
                                await _db.SaveAsync();
                                return BadRequest(uploadRes.Message);
                            }
                        }
                        else
                        {
                            ticketContent.FileUrl = "";
                        }
                    }
                    else
                    {
                        ticketContent.FileUrl = "";
                    }

                    await _db.TicketContentRepository.InsertAsync(ticketContent);

                    if (await _db.SaveAsync())
                    {
                        return CreatedAtRoute("GetTicket", new { id = ticket.ID, userId = userId },
                        ticket);
                    }
                    else
                    {
                        _db.TicketRepository.Delete(ticket.ID);
                        await _db.SaveAsync();
                        return BadRequest("خطا در ثبت اطلاعات ");
                    }
                }
                else
                {
                    return BadRequest("خطا در ثبت اطلاعات ");

                }
            }
            {
                return BadRequest("این تیکت قبلا ثبت شده است");
            }


        }

        //--------------------------------------------------------------------------------------------------------------------------------
        [Authorize(Policy = "RequireUserRole")]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        [HttpGet(ApiV1Routes.Ticket.GetTicketContent, Name = "GetTicketContent")]
        public async Task<IActionResult> GetTicketContent(string userId, string ticketId, string id)
        {
            var ticketFromRepo = await _db.TicketRepository.GetByIdAsync(ticketId);
            if (ticketFromRepo.UserId != User.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                _logger.LogError($"کاربر   {userId} قصد دسترسی به تیکت دیگری را دارد");

                return BadRequest("شما اجازه دسترسی به تیکت کاربر دیگری را ندارید");
            }

            var ticketContentFromRepo = await _db.TicketContentRepository.GetByIdAsync(id);
            if (ticketContentFromRepo != null)
            {
                return Ok(ticketContentFromRepo);
            }
            else
            {
                return BadRequest("تیکتی وجود ندارد");
            }

        }

        [Authorize(Policy = "RequireUserRole")]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        [HttpGet(ApiV1Routes.Ticket.GetTicketContents)]
        public async Task<IActionResult> GetTicketContents(string id, string userId)
        {
            var ticketFromRepo = await _db.TicketContentRepository.GetManyAsync(p => p.TicketId == id,
                s => s.OrderByDescending(x => x.DateCreated), "");
            return Ok(ticketFromRepo);
        }

        [Authorize(Policy = "RequireUserRole")]
        [HttpPost(ApiV1Routes.Ticket.AddTicketContent)]
        public async Task<IActionResult> AddTicketContent(string id, string userId, [FromForm]TicketContentForCreateDTO ticketContentForCreateDto)
        {
            var ticketContent = new TicketContent()
            {
                TicketId = id,
                IsAdminSide = false,
                Text = ticketContentForCreateDto.Text
            };
            if (ticketContentForCreateDto.File != null)
            {
                if (ticketContentForCreateDto.File.Length > 0)
                {
                    var uploadRes = await _uploadService.UploadProfilePicToLocal(
                        ticketContentForCreateDto.File,
                        userId,
                        _env.WebRootPath,
                        $"{Request.Scheme ?? ""}://{Request.Host.Value ?? ""}{Request.PathBase.Value ?? ""}",
                        "Files\\TicketContent"
                    );
                    if (uploadRes.Status)
                    {
                        ticketContent.FileUrl = uploadRes.Url;
                    }
                    else
                    {
                        return BadRequest(uploadRes.Message);
                    }
                }
                else
                {
                    ticketContent.FileUrl = "";
                }
            }
            else
            {
                ticketContent.FileUrl = "";
            }


            await _db.TicketContentRepository.InsertAsync(ticketContent);

            if (await _db.SaveAsync())
            {
                return CreatedAtRoute("GetTicketContent", new { userId = userId, ticketId = id, id = ticketContent.ID, },
                    ticketContent);
            }
            else
            {
                return BadRequest("خطا در ثبت اطلاعات ");

            }


        }
    }
}