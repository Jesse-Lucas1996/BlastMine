namespace BlastMine.Configurations;

using System.Diagnostics;

public static class SpaConfiguration
{
    public static void ConfigureSpa(this WebApplication app)
    {
        var clientAppPath = Path.Combine(Directory.GetCurrentDirectory(), "clientApp");

        if (app.Environment.IsDevelopment())
        {
            if (!Directory.Exists(clientAppPath))
            {
                Console.WriteLine($"Client app directory not found at {clientAppPath}");
                return;
            }

            var packageJsonPath = Path.Combine(clientAppPath, "package.json");
            if (!File.Exists(packageJsonPath))
            {
                Console.WriteLine($"No package.json found at {packageJsonPath}");
                return;
            }

            try
            {
                var isWindows = OperatingSystem.IsWindows();
                var startInfo = new ProcessStartInfo
                {
                    WorkingDirectory = clientAppPath,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = false,
                };

                if (isWindows)
                {
                    startInfo.FileName = "cmd.exe";
                    startInfo.Arguments = "/c bun install && bun run build && bun run dev";
                }
                else
                {
                    startInfo.FileName = "/bin/bash";
                    startInfo.Arguments = "-lic \"bun install && bun run build && bun run dev\"";
                }

                var process = Process.Start(startInfo);
                if (process != null)
                {
                    Console.WriteLine("Started development server with Bun");
                    process.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
                    process.BeginOutputReadLine();
                    process.ErrorDataReceived += (sender, e) => Console.Error.WriteLine(e.Data);
                    process.BeginErrorReadLine();
                }
                else
                {
                    Console.WriteLine("Failed to start the development server.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to start development server with Bun: {ex.Message}");
            }

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = clientAppPath;
                spa.UseProxyToSpaDevelopmentServer("http://localhost:5173");
            });
        }
        else
        {
            app.UseStaticFiles();
            app.MapFallbackToFile("index.html");
        }
    }
}