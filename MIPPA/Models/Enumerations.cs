using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mippa.Models
{
    public enum Format
    {
        EightBall,
        NineBall,
        EightBallNineBall,
        EightBall_Double
    }

    public enum ScorecardState
    {
        Initial,
        InProgress,
        Completed,
        Finalized
    }

    public enum MatchupType
    {
        ThreeOnThree,
        FourOnFour,
        FiveOnFive,
        FiveOnFour
    }

    public enum ResetPreference
    {
        Text,
        Phone,
        InPerson
    }
}
