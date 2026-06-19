namespace eCommerce.Model.Responses
{
    public class RewardRuleResponse
    {
        public int Id { get; set; }
        public int ActivityId { get; set; }
        public string RewardTitle { get; set; } = string.Empty;
        public int MaxDaysToComplete { get; set; }
        public int NumberOfPoints { get; set; }
    }
}
