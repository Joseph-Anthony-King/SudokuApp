﻿using System.Threading.Tasks;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;

namespace SudokuCollective.Core.Interfaces.Services
{
    public interface ISolutionsService
    {
        Task<ISolutionResult> GetSolution(int id, bool fullRecord = false);
        Task<ISolutionsResult> GetSolutions(IBaseRequest request, bool fullRecord = false);
        Task<ISolutionResult> Solve(ISolveRequest solveRequestsRO);
        Task<ISolutionResult> Generate();
        Task<IBaseResult> AddSolutions(int limit);
    }
}
