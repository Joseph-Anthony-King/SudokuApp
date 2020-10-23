﻿using SudokuCollective.Core.Enums;

namespace SudokuCollective.Core.Interfaces.APIModels.RequestModels
{
    public interface IUpdateDifficultyRequest : IBaseRequest
    {
        int Id { get; set; }
        string Name { get; set; }
        DifficultyLevel DifficultyLevel { get; set; }
    }
}
