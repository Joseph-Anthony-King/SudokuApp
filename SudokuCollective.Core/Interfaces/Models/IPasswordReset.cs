﻿using System;

namespace SudokuCollective.Core.Interfaces.Models
{
    public interface IPasswordReset : IEntityBase
    {
        int UserId { get; set; }
        int AppId { get; set; }
        string Token { get; set; }
        DateTime DateCreated { get; set; }
    }
}
