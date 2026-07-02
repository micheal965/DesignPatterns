using System;
using System.Collections.Generic;
using System.Text;

namespace AdapterPattern
{
    public interface IFrenchSpeaker
    {
        string AskQuestion(string Words);
        string AnswerFortheQuestion(string Words);
    }
}
