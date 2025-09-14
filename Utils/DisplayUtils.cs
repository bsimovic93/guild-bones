using Godot;
using System;

public static class DisplayUtils
{
    public static Color BookTextColor = new Color(0.341f, 0.196f, 0.102f);
    public static Color DarkGray = new Color(0.412f, 0.412f, 0.412f);
    public static Color LightBrown = new Color(0.718f, 0.525f, 0.329f);

    /// <summary>
    ///  tweens the scale a control element to 0,8 then to 1 (bounce effect) 
    /// </summary>
    /// <param name="control">the control to apply the effect on</param>
    /// <param name="duration">total duration of the effect</param>
    public static void BounceEffect(Control control, float duration = 0.25f)
    {
        if (control == null) return;

        control.PivotOffset = new Vector2(control.Size.X, control.Size.Y) / 2;
        Tween tween = control.GetTree().CreateTween();
        tween.TweenProperty(control, "scale", new Vector2(0.8f, 0.8f), duration / 2).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.Out);
        tween.Chain().TweenProperty(control, "scale", new Vector2(1f, 1f), duration / 2).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.In);
    }

    /// <summary>
    /// fades in a control element, then fades it out   
    /// </summary>
    /// <param name="control"></param>
    /// <param name="duration"></param>
    public static void FadeInOut(Control control, float duration = 0.25f)
    {
        if (control == null) return;

        //We assume the control starts invisible (alpha = 0)
        control.Visible = true;
        Tween tween = control.GetTree().CreateTween();
        tween.TweenProperty(control, "color:a", 1.0f, duration / 2).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.Out);
        tween.Chain().TweenProperty(control, "color:a", 0.0f, duration / 2).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.In);
    }

    public static void FadeIn(Control control, float duration = 0.25f)
    {
        if (control == null) return;

        //We assume the control starts invisible (alpha = 0)
        control.Visible = true;
        Tween tween = control.GetTree().CreateTween();
        tween.TweenProperty(control, "color:a", 0.5f, duration).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.Out);
    }
    
    public static void FadeOut(Control control, float duration = 0.25f)
    {
        if (control == null) return;

        //We assume the control starts visible (alpha = 1)
        Tween tween = control.GetTree().CreateTween();
        tween.TweenProperty(control, "color:a", 0.0f, duration).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.In);
        tween.Chain().TweenCallback(Callable.From(() => control.Visible = false));
    }

}