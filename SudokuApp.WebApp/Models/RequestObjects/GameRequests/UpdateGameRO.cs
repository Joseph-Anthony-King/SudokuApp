﻿using SudokuApp.Models;
using System.Collections.Generic;

namespace SudokuApp.WebApp.Models.RequestObjects.GameRequests
{

    public class UpdateGameRO {

        public int GameId { get; set; }
        public List<SudokuCell> SudokuCells { get; set; }

    }
}