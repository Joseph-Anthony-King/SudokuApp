﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Data.Models;

namespace SudokuCollective.Data.Helpers
{
    public static class StaticApiHelpers
    {
        public async static Task AttachSudokuCells(
            this ISudokuMatrix matrix,
            DatabaseContext context)
        {

            var cells = await context.SudokuCells
                .Where(cell => cell.SudokuMatrixId == matrix.Id)
                .ToListAsync();

            matrix.SudokuCells = cells.ConvertAll(c => c as ISudokuCell);
        }
    }
}
