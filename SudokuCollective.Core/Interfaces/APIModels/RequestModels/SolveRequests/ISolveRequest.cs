﻿using System.Collections.Generic;

namespace SudokuCollective.Core.Interfaces.APIModels.RequestModels
{
    public interface ISolveRequest
    {
        List<int> FirstRow { get; set; }
        List<int> SecondRow { get; set; }
        List<int> ThirdRow { get; set; }
        List<int> FourthRow { get; set; }
        List<int> FifthRow { get; set; }
        List<int> SixthRow { get; set; }
        List<int> SeventhRow { get; set; }
        List<int> EighthRow { get; set; }
        List<int> NinthRow { get; set; }
    }
}
