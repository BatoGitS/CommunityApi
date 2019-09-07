namespace CommunityAPI.Contracts.v1.Response
{
    public class UserTechnologyScoreResponse
    {
        public TechnologyResponse Technology { get; set; }
        public double AvgScore { get; set; }
    }
}
