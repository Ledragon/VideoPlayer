using VideoPlayer.Streaming.Models;

public static class HttpContextExtensions
{
    private const string SceneKey = "Scene";

    public static Scene GetScene(this HttpContext context)
    {
        return context.Items[SceneKey] as Scene 
            ?? throw new InvalidOperationException("Scene not found in context");
    }

    public static void SetScene(this HttpContext context, Scene scene)
    {
        context.Items[SceneKey] = scene;
    }
}