using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Text.RegularExpressions;

public class Program
{
    public static async Task Main(string[] args)
    {

     
        if (args[0] == null)
        {
            throw new ArgumentNullException(nameof(args));
        }
       ;
        if (!Uri.IsWellFormedUriString(args[0], UriKind.Absolute))
        {
            throw new ArgumentException("nie jest to prawidłowy url");
        }

        string websiteUrl = args[0];
        HttpClient httpClient = new HttpClient();
        HttpResponseMessage response = await httpClient.GetAsync(websiteUrl);
        httpClient.Dispose();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("storna nie może zostać pobrana");
        }
        
        string text = await response.Content.ReadAsStringAsync();

        string pattern = @"(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@" + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\." + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|" + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})";
        Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
        Match matcher = regex.Match(text);
        // ma zapewnic unikalność adresów
        HashSet<string> adresses = new HashSet<string>();
        while (matcher.Success)
        {
            adresses.Add(matcher.Groups[0].Value);
            matcher = matcher.NextMatch();
              
        }
        if (!adresses.Any())
        {
            throw new Exception("nie ma żadnych aresów email na tej stronie");
        }
        else
        {
            Console.WriteLine("Adresy Mailowe na stronie");
            Console.WriteLine(String.Join(", ", adresses));

        }



    
   
    }
}