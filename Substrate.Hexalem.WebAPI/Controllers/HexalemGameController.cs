using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Substrate.Hexalem.Engine;
using Substrate.Hexalem.WebAPI.Data;
using System;
using System.Collections.Generic;

namespace Substrate.Hexalem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HexalemGameController : ControllerBase
    {
        private const double BLOCKTIME_SEC = 6;

        private readonly ApiContext _context;

        private readonly Random _random;

        public HexalemGameController(ApiContext context)
        {
            _random = new Random();

            _context = context;

            _context.Configs.Add(new Config { Genesis = DateTime.Now });

            _context.Players.Add(new Player() { Name = "Alice" });

            _context.Players.Add(new Player() { Name = "Bob" });

            _context.SaveChanges();
        }

        [HttpGet("Genesis")]
        public JsonResult Genesis()
        {
            var config = _context.Configs.FirstOrDefault();
            if (config == null)
            {
                return new JsonResult(NotFound("No genesis block found!"));
            }

            return new JsonResult(Ok(config.Genesis));
        }

        [HttpGet("BlockNumber")]
        public JsonResult BlockNumber()
        {
            var config = _context.Configs.FirstOrDefault();
            if (config == null)
            {
                return new JsonResult(NotFound("No genesis block found!"));
            }

            return new JsonResult(Ok(CurrentBlockNumber(config.Genesis)));
        }

        [HttpGet("Player")]
        public JsonResult Player(int playerId)
        {
            var inDbPlayer = _context.Players
                .Include(p => p.Board) // Include the Board in the query
                .FirstOrDefault(p => p.Id == playerId);

            if (inDbPlayer == null)
            {
                return new JsonResult(NotFound("No player found!"));
            }

            return new JsonResult(Ok(inDbPlayer));
        }

        [HttpGet("Players")]
        public JsonResult Players()
        {
            var inDbPlayers = _context.Players;

            if (inDbPlayers == null)
            {
                return new JsonResult(NotFound("No players found!"));
            }

            return new JsonResult(Ok(inDbPlayers));
        }

        private uint CurrentBlockNumber(DateTime genesis)
        {
            DateTime now = DateTime.Now;
            var currentBlockNumber = Math.Floor(now.Subtract(genesis).TotalSeconds / BLOCKTIME_SEC);
            return Convert.ToUInt32(currentBlockNumber);
        }
    }
}