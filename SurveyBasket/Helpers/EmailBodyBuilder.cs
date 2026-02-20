namespace SurveyBasket.Helpers;

public static class EmailBodyBuilder
{
    public static string GenerateEmailBody(string template,Dictionary<string,string> templateModels)
    {
        var templatePath = $"{Directory.GetCurrentDirectory()}/Templates/{template}.html";
        var streamReader = new StreamReader(templatePath);
        var body = streamReader.ReadToEnd();
        streamReader.Close();
        foreach (var model in templateModels)
        {
            body = body.Replace(model.Key, model.Value);
        }
        return body;
    }
}
