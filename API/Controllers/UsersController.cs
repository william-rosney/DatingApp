using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            return Ok(await _userRepository.GetMembersAsync());
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            return await _userRepository.GetMemberAsync(username);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {

            //Récupère le nom d'utilisateur à partir du token de login
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            //Get the current AppUser by using the username
            var user = await _userRepository.GetUserByUsernameAsync(username);

            //Map memberUpdateDto > user   
            _mapper.Map(memberUpdateDto, user);

            //Flag le user comme modifié par EntityFramework
            _userRepository.Update(user);

            //Sauve les changements
            if (await _userRepository.SaveAllAsync()) return NoContent();
            return BadRequest("Failed to update user");


        }

    }

}