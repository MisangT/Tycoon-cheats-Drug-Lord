using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIC.LD.TrainingEvaluation.SP.Solution.Helper
{
    public class PostLearningInterventionEvaluationEmployeeObject
    {
        public int? PostInterventionID
        { get; set; }

        public int? PostInterventionEmployeeID
        { get; set; }

        public string LearningIntervention
        { get; set; }

        public string CourseWorthTimeandExpectations
        { get; set; }

        public string ApplySkillsLearned
        { get; set; }

        public string QualityOfPresenter
        { get; set; }

        public string LearningInterventionCoordination
        { get; set; }

        public string HighlightsOfTraining
        { get; set; }

        public int Submitted
        { get; set; }
    }
}
