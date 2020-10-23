using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.APIModels.PageModels;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;

namespace SudokuCollective.Data.Models.RequestModels
{
    public class UpdateDifficultyRequest : IUpdateDifficultyRequest
    {
        public string License { get; set; }
        public int RequestorId { get; set; }
        public int AppId { get; set; }
        public IPageListModel PageListModel { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public DifficultyLevel DifficultyLevel { get; set; }

        public UpdateDifficultyRequest() : base()
        {
            Id = 0;
            Name = string.Empty;
            DifficultyLevel = DifficultyLevel.NULL;
        }
    }
}
