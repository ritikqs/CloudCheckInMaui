namespace CloudCheckInMaui.Transitioning
{
    public static class TransitionConstants
    {
        public const string TransitionMessage = "TransitionMessage";
    }

    public enum TransitionType
    {
        None,
        Default,
        Fade,
        Flip,
        Scale,
        SlideFromBottom,
        SlideFromLeft,
        SlideFromRight,
        SlideFromTop,
        OpenMenu
    }
} 