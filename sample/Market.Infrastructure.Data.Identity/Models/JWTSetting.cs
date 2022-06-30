namespace Market.Infrastructure.Data.Identity.Models;

public class JWTSetting
{
public string Key { get; set; }
public string Issuer { get; set; }
public string Audience { get; set; }
public double DurationInMinutes { get; set; }
}


