namespace SudokuCollective.WebApi.Models.RequestModels.GameRequests {
    
    public class CreateGameRequest : BaseRequest {

        public int UserId { get; set; }
        public int DifficultyId { get; set; }
        public int AppId { get; set; }

        public CreateGameRequest() : base() {

            UserId = 0;
            DifficultyId = 0;
            AppId = 0;
        }
    }
}
