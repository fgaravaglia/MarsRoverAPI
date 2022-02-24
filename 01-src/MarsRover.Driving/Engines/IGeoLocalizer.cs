namespace MarsRover.Driving.Engines
{
    /// <summary>
    /// Interface to model the behavior for locailiser: it manage the actual position
    /// </summary>
    internal interface IGeoLocalizer
    {
         Coordinates GetPosition();

         void SavePosition(Coordinates point);
    }
}