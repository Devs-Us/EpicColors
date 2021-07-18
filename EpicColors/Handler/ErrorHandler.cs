namespace EpicColors
{
    public static class Logger {
        public static void ErrorLogger(object value, System.Exception ex) {
            ColorsPlugin.Logger.LogError($"It seems like there was an error when {value}. Here's some log that might be helpful.");
            ColorsPlugin.Logger.LogError($"======================");
            ColorsPlugin.Logger.LogError(ex);
            ColorsPlugin.Logger.LogError($"======================");
            
            return;
        }
    }
}