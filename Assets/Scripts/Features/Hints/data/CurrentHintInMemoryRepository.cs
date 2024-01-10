using System;
using Features.Hints.domain;

namespace Features.Hints.data
{
    public class CurrentHintInMemoryRepository: ICurrentHintRepository
    {
        private string hintText = String.Empty;
        
        public void SetHintText(string text) => hintText = text;

        public string GetHintText() => hintText;
    }
}