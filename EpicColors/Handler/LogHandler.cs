namespace EpicColors
{
    public static class Logger
    {
        public static void DebugLogger(object value)
        {
            // Will be used soon for debugging
            ColorsPlugin.Logger.LogMessage($"(THIS IS USED FOR DEBUGGING!) {value}.");
            return;
        }
        public static void ErrorLogger(object value, object ex)
        {
            ColorsPlugin.Logger.LogError($"It seems like there was an error when {value}. Here's some log that might be helpful.");
            ColorsPlugin.Logger.LogError($"======================");
            ColorsPlugin.Logger.LogError(ex);
            ColorsPlugin.Logger.LogError($"======================");

            return;
        }
    }
}