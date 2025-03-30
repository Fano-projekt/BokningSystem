using System.Text.Json.Serialization;

public class Patient
{
    public int Id { get; set; }  
    public string Personnummer { get; set; }
    public string Fornamn { get; set; }
    public string Efternamn { get; set; }

    [JsonIgnore]  
    public string Name => $"{Fornamn} {Efternamn}";
}
