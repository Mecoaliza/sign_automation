public static class InitAllSettings
{
    public static Dictionary<string, string> Run(string configPath)
    {
        var config = new Dictionary<string, string>();

        if (File.Exists(configPath))
        {
            foreach (var line in File.ReadLines(configPath))
            {
                var parts = line.Split('=');
                if (parts.Length == 2)
                    config[parts[0].Trim()] = parts[1].Trim();
            }
        }

        return config;
    }
}