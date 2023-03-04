namespace TechnoEgypt.DTOS
{
    public class CourseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string Image_URL{ get; set; }
        public bool IsAvailable { get; set; }
        public bool DataCollectionandAnalysis { get; set; }
        public bool CriticalThinking { get; set; }
        public bool Planning { get; set; }
        public bool MathematicalReasoning { get; set; }
        public bool Innovation { get; set; }
        public bool LogicalThinking { get; set; }
        public bool CognitiveAbilities { get; set; }
        public bool ProblemSolving { get; set; }
        public bool SocialLifeSkills { get; set; }
        public bool ScientificResearch { get; set; }
    }
}
