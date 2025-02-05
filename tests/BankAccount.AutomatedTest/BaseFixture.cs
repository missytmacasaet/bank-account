using OpenQA.Selenium.Remote;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;

namespace BankAccount.AutomatedTest;

public class BaseFixture : IDisposable
{

    private RemoteWebDriver? WebDriver;


    public RemoteWebDriver GetDriver(string platform, string profile)
    {
        // Get Configuration for correct profile
        string currentDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string path = Path.Combine(currentDirectory, "config.json");
        JObject config = JObject.Parse(File.ReadAllText(path));
        if (config is null)
            throw new Exception("Configuration not found!");

        // Get Platform specific capabilities
        JObject capabilities = config.GetValue("environments").Where(x => x is JObject y && x["browserName"].ToString().Equals(platform)).ToList()[0] as JObject;

        // Get Common Capabilities
        JObject commonCapabilities = config.GetValue("capabilities") as JObject;

        // Merge Capabilities
        capabilities.Merge(commonCapabilities);

        JObject ltOptions = capabilities["lt:options"] as JObject;

        // Get username and accesskey
        string? username = Environment.GetEnvironmentVariable("LT_USERNAME");
        if (username is null)
            username = config.GetValue("user").ToString();

        string? accessKey = Environment.GetEnvironmentVariable("LT_ACCESS_KEY");
        if (accessKey is null)
            accessKey = config.GetValue("key").ToString();

        ltOptions["userName"] = username;
        ltOptions["accessKey"] = accessKey;
        capabilities["lt:options"] = ltOptions;

        // Create Desired Cappabilities for WebDriver
        DriverOptions desiredCapabilities = getBrowserOption(capabilities.GetValue("browserName").ToString());
        capabilities.Remove("browserName");
        foreach (var x in capabilities)
        {
            if (x.Key.Equals("lt:options"))
                desiredCapabilities.AddAdditionalOption(x.Key, x.Value.ToObject<Dictionary<string, object>>());
            else
                desiredCapabilities.AddAdditionalOption(x.Key, x.Value);
        }

        // Create RemoteWebDriver instance
        WebDriver = new RemoteWebDriver(new Uri($"https://{config["server"]}/wd/hub"), desiredCapabilities);

        return WebDriver;
    }

    public void SetStatus(bool passed)
    {
        if(WebDriver is not null)
        {
            if (passed)
                ((IJavaScriptExecutor)WebDriver).ExecuteScript("lambda-status=passed");
            else
                ((IJavaScriptExecutor)WebDriver).ExecuteScript("lambda-status=failed");
        }
    }

    static DriverOptions getBrowserOption(String browser)
    {
        switch (browser)
        {
            case "chrome":
                return new OpenQA.Selenium.Chrome.ChromeOptions();
            case "firefox":
                return new OpenQA.Selenium.Firefox.FirefoxOptions();
            case "safari":
                return new OpenQA.Selenium.Safari.SafariOptions();
            case "edge":
                return new OpenQA.Selenium.Edge.EdgeOptions();
            default:
                return new OpenQA.Selenium.Chrome.ChromeOptions();
        }
    }

    public void Dispose()
    {
        if (WebDriver is not null)
        {
            WebDriver.Quit();
            WebDriver.Dispose();
        }
    }
}
