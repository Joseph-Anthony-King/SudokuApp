﻿namespace SudokuCollective.Core.Interfaces.APIModels.TokenModels
{
    public interface ITokenManagement
    {
        string Secret { get; set; }
        string Issuer { get; set; }
        string Audience { get; set; }
        int AccessExpiration { get; set; }
        int RefreshExpiration { get; set; }
    }
}
