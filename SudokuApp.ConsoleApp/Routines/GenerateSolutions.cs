﻿using System;
using SudokuApp.ConsoleApp.Classes;
using SudokuApp.Models;

namespace SudokuApp.ConsoleApp.Routines {

    internal static class GenerateSolutions {

        internal static void Run() {

            var result = string.Empty;
            var continueLoop = true;

            do {
                var matrix = new SudokuMatrix();
                matrix.SetDifficulty(Difficulty.TEST);
                matrix.GenerateSolution();

                DisplayScreens.DisplayMatix(matrix);

                Console.Write("\n\nWould you like to generate another solution (yes/no): ");

                result = Console.ReadLine();

                if (result.ToLower().Equals("no") || result.ToLower().Equals("n")) {

                    continueLoop = false;
                }

            } while (continueLoop);
        }
    }
}
