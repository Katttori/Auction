using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WEB.Models;
using AutoMapper;
using BLL.DTOs;
using BLL.Exceptions;
using System.Threading.Tasks;

namespace WEB.Controllers
{
    [RoutePrefix("api/lots")]
    public class LotsController : ApiController
    {
        private readonly ILotService lotService;
        private readonly IUserService userService;

        public LotsController(ILotService lotService, IUserService userService)
        {
            this.lotService = lotService;
            this.userService = userService;
        }
        
        [HttpGet]
        [Route("get")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult Get()
        {
            return Ok(Mapper.Map<IEnumerable<LotDTO>, List<LotModel>>(lotService.GetAllLots()));
        }

        [HttpGet]
        [Route("get/active")]
        public IHttpActionResult GetActive()
        {
            return Ok(Mapper.Map<IEnumerable<LotDTO>, List<LotModel>>(lotService.GetActiveLots()));
        }
       
        [HttpGet]
        [Route("get/{id}")]
        public IHttpActionResult Get(int id)
        {
            try
            {
                var lot = lotService.GetLot(id);
                return Ok(Mapper.Map<LotDTO, LotModel>(lot));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("get/category/{id}")]
        public IHttpActionResult GetWithCategory(int id)
        {
            try
            {
                var lots = lotService.GetLotsWithCategory(id);
                return Ok(Mapper.Map<IEnumerable<LotDTO>, List<LotModel>>(lots));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("get/user")]
        [Authorize]
        public IHttpActionResult GetWonLots()
        {
            var id = userService.GetUserByName(User.Identity.Name).Id;
            try
            {
                var lots = lotService.GetWonLots(id);
                return Ok(Mapper.Map<IEnumerable<LotDTO>, List<LotModel>>(lots));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("create/{id}")]
        [Authorize]
        public IHttpActionResult CreateLot(int id)
        {
            try
            {
                lotService.CreateLot(id);
                return Ok("Lot created successfuly");
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        [Route("update")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult UpdateLot( [FromBody] LotModel lot)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid input");
            var newLot = Mapper.Map<LotModel, LotDTO>(lot);
            try
            {
                lotService.EditLot(newLot);
                return Ok("Lot updated successfuly");
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentNullException)
            {
                return BadRequest("You should give lot");
            }
        }

        [HttpPut]
        [Route("end/{id}")]
        [Authorize(Roles = "Admin, Moderator")]
        public IHttpActionResult EndBidding(int id)
        {
            try
            {
                lotService.EndBidding(id);
                return Ok("Bidding finished successfuly");
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        [Route("timer/start/{id}")]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IHttpActionResult> EndBiddingWhenExpired(int id)
        {
            try
            {
                await lotService.EndBiddingWhenTimeExpired(id);
                return Ok("Bidding end");
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut]
        [Route("bet/{lotId}/{bet}")]
        [Authorize]
        public IHttpActionResult MakeBet(int lotId, decimal bet)
        {
            var userId = userService.GetUserByName(User.Identity.Name).Id;
            try
            {
                lotService.MakeBet(userId, lotId, bet);
                return Ok("Bet successful");
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                lotService.RemoveLot(id);
                return Ok("Lot removed successfuly");
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
    }
}