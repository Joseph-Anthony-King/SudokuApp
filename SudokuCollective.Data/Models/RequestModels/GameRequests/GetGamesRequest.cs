using SudokuCollective.Core.Interfaces.APIModels.PageModels;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Data.Models.PageModels;

namespace SudokuCollective.Data.Models.RequestModels
{
    public class GetGamesRequest : IGetGamesRequest
    {
        public string License { get; set; }
        public int RequestorId { get; set; }
        public int AppId { get; set; }
        public IPaginator Paginator { get; set; }
        public int UserId { get; set; }

        public GetGamesRequest() : base()
        {
            UserId = 0;

            if (Paginator == null)
            {
                Paginator = new Paginator();
            }
        }
    }
}
