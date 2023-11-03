namespace DoubleEngine.Atom
{
    [System.Serializable]
    public record GridSide
    {
        public readonly int sideIndex;
        public readonly FlatNodeTransform flatTransform;
        public GridSide(int gridElementFacesRemoveId, FlatNodeTransform sourceNode)
        {
            this.sideIndex = gridElementFacesRemoveId;
            this.flatTransform = sourceNode;
        }
        public GridSide InvertX() => new(sideIndex, flatTransform.InvertX());
        public GridSide InvertY() => new(sideIndex, flatTransform.InvertY());
        public GridSide Rotate(PerpendicularAngle angle) => new(sideIndex, flatTransform.Rotate(angle));
    }




}

