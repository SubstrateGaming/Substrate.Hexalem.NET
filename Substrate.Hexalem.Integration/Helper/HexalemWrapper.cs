﻿using Substrate.Hexalem.Engine;
using Substrate.Hexalem.Integration.Model;
using Substrate.NetApi;
using Substrate.NetApi.Model.Types.Primitive;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Substrate.Integration.Helper
{
    /// <summary>
    /// Hexalem Wrapper
    /// </summary>
    public static class HexalemWrapper
    {
        /// <summary>
        /// Get the hexa game
        /// </summary>
        /// <param name="game"></param>
        /// <param name="boards"></param>
        /// <returns></returns>
        public static HexaGame GetHexaGame(GameSharp game, BoardSharp[] boards)
        {
            var result = new HexaGame(game.GameId, new List<(HexaPlayer, HexaBoard)>())
            {
                HexBoardState = (HexBoardState)game.State + 1,
                HexBoardRound = game.Round,
                PlayerTurn = game.PlayerTurn,
                SelectBase = game.SelectionSize,
                UnboundTileOffers = game.Selection.Select(x => x).ToList(),
                PlayersCount = (byte)boards.Length,
                LastMove = BitConverter.GetBytes(game.LastBlock) 
            };

            foreach (var (board, playerAddress) in boards.Zip(game.Players, (b, p) => (b, p)))
            {
                var currentPlayer = new HexaPlayer(Utils.GetPublicKeyFrom(playerAddress), board.Resources);

                result.HexaTuples.Add((currentPlayer, GetHexaBoard(board)));
            }

            return result;
        }

        /// <summary>
        /// Get the hexa board
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public static HexaBoard GetHexaBoard(BoardSharp board)
        {
            return new HexaBoard(board.HexGrid.Select(x => x.Value).ToArray());
        }
    }
}