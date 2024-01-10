using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Features.Rating.domain
{
    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public struct RatingData
    {
        public int position;
        public int rating;
        public string username;
        public bool isMine;
    }

    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public struct RatingDataResponse
    {
        public List<RatingData> items;
    }
}