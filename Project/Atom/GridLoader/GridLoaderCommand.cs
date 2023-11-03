namespace DoubleEngine.Atom
{
        internal enum GridLoaderCommand : byte
        {
            DoNothing = 0,
            //StopLoading = 1,
            PutInInt16Coords = 16,
            SetModel = 17,
            SetOrientation = 18,
            SetMaterial = 19
        }
}